using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyThuChi_DoAn.BLL.Common;
using QuanLyThuChi_DoAn.Data_Access_Layer;
using QuanLyThuChi_DoAn.DTOs;
using QuanLyThuChi_DoAn.Helpers;

namespace QuanLyThuChi_DoAn.BLL.Services
{
    public class AuditLogService
    {
        private const string ActionAll = "ALL";
        private const int DefaultMaxRows = 500;
        private const int AbsoluteMaxRows = 2000;

        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLogUserOptionDTO>> GetFilterUsersAsync()
        {
            EnsureCanViewAuditLog();
            int tenantId = ResolveTenantScope();

            List<AuditLogUserOptionDTO> users = await _context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .Where(u => u.TenantId == tenantId)
                .OrderBy(u => u.FullName)
                .ThenBy(u => u.Username)
                .Select(u => new AuditLogUserOptionDTO
                {
                    UserId = u.UserId,
                    DisplayName = BuildUserFilterDisplay(u.FullName, u.Username, u.Role.RoleCode, u.Role.RoleName)
                })
                .ToListAsync()
                .ConfigureAwait(false);

            users.Insert(0, new AuditLogUserOptionDTO
            {
                UserId = 0,
                DisplayName = "Tất cả nhân viên"
            });

            return users;
        }

        public async Task<List<AuditLogDTO>> GetLogsAsync(
            DateTime fromDate,
            DateTime toDate,
            string? actionType,
            int? userId,
            int maxRows = DefaultMaxRows)
        {
            EnsureCanViewAuditLog();
            int tenantId = ResolveTenantScope();

            DateTime fromDateOnly = fromDate.Date;
            DateTime toDateOnly = toDate.Date;
            if (fromDateOnly > toDateOnly)
            {
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }

            int safeLimit = Math.Clamp(maxRows, 1, AbsoluteMaxRows);
            string normalizedActionCode = NormalizeActionCode(actionType);

            IQueryable<AuditLog> query = _context.AuditLogs
                .AsNoTracking()
                .Where(log => log.TenantId == tenantId
                           && log.ActionDate >= fromDateOnly
                           && log.ActionDate < toDateOnly.AddDays(1));

            if (normalizedActionCode != ActionAll)
            {
                query = query.Where(log => (log.ActionType ?? string.Empty).ToUpper() == normalizedActionCode);
            }

            if (userId.HasValue && userId.Value > 0)
            {
                query = query.Where(log => log.UserId == userId.Value);
            }

            List<AuditLog> rawLogs = await query
                .OrderByDescending(log => log.ActionDate)
                .Take(safeLimit)
                .ToListAsync()
                .ConfigureAwait(false);

            List<int> userIds = rawLogs
                .Where(log => log.UserId.HasValue && log.UserId.Value > 0)
                .Select(log => log.UserId!.Value)
                .Distinct()
                .ToList();

            Dictionary<int, User> usersById = userIds.Count == 0
                ? new Dictionary<int, User>()
                : await _context.Users
                    .AsNoTracking()
                    .Include(u => u.Role)
                    .Where(u => userIds.Contains(u.UserId))
                    .ToDictionaryAsync(u => u.UserId)
                    .ConfigureAwait(false);

            List<AuditLogDTO> result = new List<AuditLogDTO>(rawLogs.Count);
            foreach (AuditLog log in rawLogs)
            {
                User? actor = null;
                if (log.UserId.HasValue && log.UserId.Value > 0)
                {
                    usersById.TryGetValue(log.UserId.Value, out actor);
                }

                string actionCode = NormalizeActionCode(log.ActionType);
                string tableName = log.TableName ?? string.Empty;
                string recordId = log.RecordId ?? string.Empty;

                result.Add(new AuditLogDTO
                {
                    LogId = log.LogId,
                    ActionDate = log.ActionDate,
                    ActorDisplay = BuildActorDisplay(log, actor),
                    ActionCode = actionCode,
                    ActionTypeDisplay = TranslateActionType(actionCode),
                    ModuleDisplay = TranslateTableName(tableName),
                    ReferenceCode = BuildReferenceCode(tableName, recordId),
                    Details = BuildDetails(actionCode, tableName, recordId, log.NewValues, log.OldValues)
                });
            }

            return result;
        }

        private static void EnsureCanViewAuditLog()
        {
            if (!SessionManager.IsSuperAdmin && 
               (!SessionManager.IsTenantAdmin || SessionManager.CurrentPriorityLevel < 80))
            {
                throw new UnauthorizedAccessException("Chỉ có Ban Giám đốc (Priority >= 80) mới có quyền truy cập Dấu vết Hệ thống.");
            }
        }

        private static int ResolveTenantScope()
        {
            int tenantId = SessionManager.CurrentTenantId ?? SessionManager.TenantId ?? 0;
            if (tenantId <= 0)
            {
                throw new InvalidOperationException("Không xác định được tenant hiện tại. Vui lòng đăng nhập lại hoặc chọn tenant.");
            }

            return tenantId;
        }

        private static string NormalizeActionCode(string? actionType)
        {
            if (string.IsNullOrWhiteSpace(actionType))
            {
                return ActionAll;
            }

            return actionType.Trim().ToUpperInvariant();
        }

        private static string TranslateActionType(string actionCode)
        {
            return actionCode switch
            {
                "VOID_TRANSACTION" => "Hủy Phiếu Giao Dịch",
                "CREATE" => "Thêm mới",
                "UPDATE" => "Cập nhật",
                "DELETE" => "Xóa",
                _ => string.IsNullOrWhiteSpace(actionCode) || actionCode == ActionAll ? "Không xác định" : actionCode
            };
        }

        private static string TranslateTableName(string tableName)
        {
            string code = (tableName ?? string.Empty).Trim();
            return code switch
            {
                "Transactions" => "Phiếu giao dịch",
                "Users" => "Người dùng",
                "Branches" => "Chi nhánh",
                "Debts" => "Công nợ",
                "Partners" => "Đối tác",
                "CashFunds" => "Quỹ tiền",
                "TransactionCategories" => "Danh mục thu chi",
                "Taxes" => "Thuế suất",
                _ => string.IsNullOrWhiteSpace(code) ? "N/A" : code
            };
        }

        private static string TranslateRoleName(string? roleCode, string? roleName)
        {
            string code = (roleCode ?? string.Empty).Trim().ToUpperInvariant();
            return code switch
            {
                "SUPERADMIN" => "Quản trị hệ thống",
                "TENANTADMIN" => "Giám đốc",
                "BRANCHMANAGER" => "Quản lý chi nhánh",
                "STAFF" => "Nhân viên",
                _ => string.IsNullOrWhiteSpace(roleName) ? "N/A" : roleName
            };
        }

        private static string BuildUserFilterDisplay(string? fullName, string? username, string? roleCode, string? roleName)
        {
            string displayName = !string.IsNullOrWhiteSpace(fullName)
                ? fullName.Trim()
                : (!string.IsNullOrWhiteSpace(username) ? username.Trim() : "N/A");

            string translatedRole = TranslateRoleName(roleCode, roleName);
            return string.IsNullOrWhiteSpace(translatedRole) || translatedRole == "N/A"
                ? displayName
                : $"{displayName} ({translatedRole})";
        }

        private static string BuildActorDisplay(AuditLog log, User? actor)
        {
            if (actor != null)
            {
                return BuildUserFilterDisplay(actor.FullName, actor.Username, actor.Role?.RoleCode, actor.Role?.RoleName);
            }

            if (!string.IsNullOrWhiteSpace(log.UserName))
            {
                return log.UserName.Trim();
            }

            return "Hệ thống";
        }

        private static string BuildReferenceCode(string tableName, string recordId)
        {
            string cleanRecordId = string.IsNullOrWhiteSpace(recordId) ? "N/A" : recordId.Trim();
            string tableCode = (tableName ?? string.Empty).Trim();

            return tableCode switch
            {
                "Transactions" => $"Phiếu số #{cleanRecordId}",
                "Debts" => $"Khoản nợ #{cleanRecordId}",
                "Users" => $"Người dùng #{cleanRecordId}",
                "Branches" => $"Chi nhánh #{cleanRecordId}",
                _ => $"Bản ghi #{cleanRecordId}"
            };
        }

        private static string BuildDetails(string actionCode, string tableName, string recordId, string? newValues, string? oldValues)
        {
            string reason = TryReadJsonString(newValues, "Reason", "CancelReason", "LyDoHuy", "ReasonNote");
            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = TryReadJsonString(oldValues, "Reason", "CancelReason", "LyDoHuy", "ReasonNote");
            }

            decimal? amount = TryReadJsonDecimal(newValues, "Amount", "SubTotal", "TotalAmount");
            amount ??= TryReadJsonDecimal(oldValues, "Amount", "SubTotal", "TotalAmount");

            if (actionCode == "VOID_TRANSACTION")
            {
                if (!string.IsNullOrWhiteSpace(reason) && amount.HasValue)
                {
                    return $"Lý do hủy: {reason}. Số tiền liên quan: {FormatVnd(amount.Value)}";
                }

                if (!string.IsNullOrWhiteSpace(reason))
                {
                    return $"Lý do hủy: {reason}";
                }

                return "Hủy phiếu giao dịch.";
            }

            if (actionCode == "DELETE")
            {
                return string.IsNullOrWhiteSpace(reason)
                    ? "Xóa dữ liệu (hoặc xóa mềm) theo nghiệp vụ."
                    : $"Xóa dữ liệu. Lý do: {reason}";
            }

            if (actionCode == "UPDATE")
            {
                string diffText = BuildDiffSummary(oldValues, newValues);
                return string.IsNullOrWhiteSpace(diffText) ? "Cập nhật dữ liệu." : diffText;
            }

            if (actionCode == "CREATE")
            {
                string summary = BuildJsonSummary(newValues);
                return string.IsNullOrWhiteSpace(summary) ? "Thêm mới dữ liệu." : summary;
            }

            string fallback = BuildJsonSummary(newValues);
            if (!string.IsNullOrWhiteSpace(fallback))
            {
                return fallback;
            }

            fallback = BuildJsonSummary(oldValues);
            if (!string.IsNullOrWhiteSpace(fallback))
            {
                return fallback;
            }

            string cleanTableName = TranslateTableName(tableName);
            string cleanRecordId = string.IsNullOrWhiteSpace(recordId) ? "N/A" : recordId.Trim();
            return $"Tác động lên {cleanTableName} - Mã #{cleanRecordId}.";
        }

        private static string BuildJsonSummary(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return string.Empty;
                }

                List<string> parts = new List<string>();
                foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                {
                    string valueText = ConvertJsonValue(property.Value);
                    if (string.IsNullOrWhiteSpace(valueText))
                    {
                        continue;
                    }

                    parts.Add($"{TranslateJsonKey(property.Name)}: {valueText}");
                    if (parts.Count >= 3)
                    {
                        break;
                    }
                }

                return string.Join("; ", parts);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string BuildDiffSummary(string? oldValues, string? newValues)
        {
            Dictionary<string, string> oldDict = ParseJsonObjectAsText(oldValues);
            Dictionary<string, string> newDict = ParseJsonObjectAsText(newValues);

            if (oldDict.Count == 0 && newDict.Count == 0)
            {
                return string.Empty;
            }

            List<string> diffs = new List<string>();
            List<string> allKeys = oldDict.Keys
                .Union(newDict.Keys)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (string key in allKeys)
            {
                oldDict.TryGetValue(key, out string? oldText);
                newDict.TryGetValue(key, out string? newText);

                if (string.Equals(oldText, newText, StringComparison.Ordinal))
                {
                    continue;
                }

                string label = TranslateJsonKey(key);
                if (string.IsNullOrWhiteSpace(oldText))
                {
                    diffs.Add($"{label}: thêm {newText}");
                }
                else if (string.IsNullOrWhiteSpace(newText))
                {
                    diffs.Add($"{label}: xóa");
                }
                else
                {
                    diffs.Add($"{label}: {oldText} -> {newText}");
                }

                if (diffs.Count >= 3)
                {
                    break;
                }
            }

            return diffs.Count == 0 ? string.Empty : string.Join("; ", diffs);
        }

        private static Dictionary<string, string> ParseJsonObjectAsText(string? json)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(json))
            {
                return result;
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return result;
                }

                foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                {
                    result[property.Name] = ConvertJsonValue(property.Value);
                }
            }
            catch
            {
                // Dữ liệu json không hợp lệ thì bỏ qua, không văng lỗi ra UI.
            }

            return result;
        }

        private static string ConvertJsonValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString() ?? string.Empty,
                JsonValueKind.Number => TryFormatDecimal(element),
                JsonValueKind.True => "Có",
                JsonValueKind.False => "Không",
                JsonValueKind.Null => string.Empty,
                JsonValueKind.Undefined => string.Empty,
                JsonValueKind.Object => "{...}",
                JsonValueKind.Array => "[...]",
                _ => element.ToString()
            };
        }

        private static string TryFormatDecimal(JsonElement element)
        {
            if (element.TryGetDecimal(out decimal decimalValue))
            {
                return FormatVnd(decimalValue);
            }

            return element.ToString();
        }

        private static string TryReadJsonString(string? json, params string[] keys)
        {
            if (string.IsNullOrWhiteSpace(json) || keys == null || keys.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);

                foreach (string key in keys)
                {
                    if (TryFindPropertyRecursive(doc.RootElement, key, out JsonElement foundValue))
                    {
                        string text = ConvertJsonValue(foundValue).Trim();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            return text;
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private static decimal? TryReadJsonDecimal(string? json, params string[] keys)
        {
            if (string.IsNullOrWhiteSpace(json) || keys == null || keys.Length == 0)
            {
                return null;
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);

                foreach (string key in keys)
                {
                    if (!TryFindPropertyRecursive(doc.RootElement, key, out JsonElement foundValue))
                    {
                        continue;
                    }

                    if (foundValue.ValueKind == JsonValueKind.Number && foundValue.TryGetDecimal(out decimal numericValue))
                    {
                        return numericValue;
                    }

                    if (foundValue.ValueKind == JsonValueKind.String)
                    {
                        string? rawText = foundValue.GetString();
                        if (TryParseDecimal(rawText, out decimal parsedValue))
                        {
                            return parsedValue;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static bool TryParseDecimal(string? rawText, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(rawText))
            {
                return false;
            }

            string normalized = rawText.Trim();
            CultureInfo configuredCulture = AppCulture.GetConfiguredCulture();

            if (decimal.TryParse(normalized, NumberStyles.Any, configuredCulture, out value))
            {
                return true;
            }

            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.GetCultureInfo("vi-VN"), out value))
            {
                return true;
            }

            normalized = normalized.Replace(",", string.Empty).Replace(".", string.Empty);
            return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryFindPropertyRecursive(JsonElement element, string propertyName, out JsonElement foundValue)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        foundValue = property.Value;
                        return true;
                    }

                    if (TryFindPropertyRecursive(property.Value, propertyName, out foundValue))
                    {
                        return true;
                    }
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement child in element.EnumerateArray())
                {
                    if (TryFindPropertyRecursive(child, propertyName, out foundValue))
                    {
                        return true;
                    }
                }
            }

            foundValue = default;
            return false;
        }

        private static string TranslateJsonKey(string rawKey)
        {
            string key = (rawKey ?? string.Empty).Trim();
            return key switch
            {
                "Reason" => "Lý do",
                "CancelReason" => "Lý do hủy",
                "ReasonNote" => "Ghi chú",
                "Status" => "Trạng thái",
                "IsActive" => "Kích hoạt",
                "Amount" => "Số tiền",
                "SubTotal" => "Tiền trước thuế",
                "TaxAmount" => "Tiền thuế",
                "TotalAmount" => "Tổng tiền",
                "Description" => "Diễn giải",
                "RefNo" => "Mã chứng từ",
                "TransDate" => "Ngày giao dịch",
                "CategoryName" => "Loại thu chi",
                "BranchId" => "Chi nhánh",
                "PartnerId" => "Đối tác",
                _ => key
            };
        }

        private static string FormatVnd(decimal value)
        {
            return value.ToString("N0", AppCulture.GetConfiguredCulture());
        }
    }
}