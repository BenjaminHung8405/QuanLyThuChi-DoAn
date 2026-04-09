### 4.10 Giai đoạn 10: Phân cấp quyền Đa Tenant, Quản lý Hệ thống và Nâng cấp Trải nghiệm Báo cáo (UX/UI)

#### 4.10.1 Minh chứng giai đoạn
*   **Giao diện Quản lý Người dùng & Chi nhánh:** Ảnh chụp màn hình `ucUserManagement` và `ucBranchManagement` hiển thị danh sách người dùng/chi nhánh tương ứng với Tenant hiện tại. Ảnh chụp Form thêm/sửa người dùng (`frmAddUser`, `frmEditUser`) với các tuỳ chọn chức vụ (Role) và chi nhánh (Branch) được lọc động theo cấp bậc quyền (PriorityLevel).
*   **Giao diện Báo cáo & Công nợ (UX nâng cao):** Ảnh chụp màn hình `ucCashbookReport` với chế độ xem Sổ quỹ chi tiết (có dòng "SỐ DƯ ĐẦU KỲ" và cột Lũy kế - Running Balance) và chế độ xem Tổng hợp. Ảnh chụp màn hình `ucDebt` thể hiện tính năng "Toggle View" (chuyển đổi xem Từng phiếu / Tổng đối tác) và nút "Bỏ lọc nhanh".
*   **Cơ sở dữ liệu:** Ảnh chụp bảng `Roles` có thêm các cột `RoleCode`, `PriorityLevel`. Bảng `Branch` có thêm trường `CreatedDate`.

#### 4.10.2 Mô tả
*   **Mục tiêu:** Triển khai phân hệ con người (Nhân sự & Chi nhánh) theo hướng bảo mật và kiến trúc đa khách hàng (Multi-tenant). Cho phép Giám đốc tạo tài khoản, phân quyền theo mức độ ưu tiên (Priority Level), và quản lý chi nhánh. Nâng cấp trải nghiệm người dùng (UX) cho phân hệ Công nợ (Drill-down, Toggle view) và Báo cáo Sổ quỹ (tính số dư lũy kế, chia mode chi tiết/tổng hợp).
*   **Kiến trúc Bảo mật & Dữ liệu:** 
    *   Tầng BLL (Services) áp dụng cơ chế xác thực nghiệp vụ (tenant scope, branch scope) trước mọi thao tác để ngăn chặn UI bypass. 
    *   Bảng `Roles` được mở rộng linh hoạt với mã quyền kỹ thuật (`RoleCode`) và cấp độ quyền (`PriorityLevel`), thoát khỏi việc hard-code kiểm tra bằng tên chức vụ.
    *   Truyền tải dữ liệu sử dụng các DTO (`UserDTO`, `CashbookDetailDTO`, `CashbookSummaryDTO`, `DebtSummaryDTO`) nhằm che giấu thông tin nhạy cảm (như `PasswordHash`) và phục vụ riêng biệt cho từng ngữ cảnh hiển thị.
*   **Giao diện tương tác (UI):** Thiết kế lại các UserControl hoạt động theo dạng Card-based; áp dụng mẫu Form dùng chung cho Add/Edit để đồng bộ validation. Bổ sung các tính năng nâng cao như cơ chế Drill-down (bấm đúp từ view tổng hợp sang view chi tiết), tự động đồng bộ Combobox chi nhánh ở form chính thông qua Event (BranchChanged), và liên kết (LinkLabel) reset filter 1 chạm.

#### 4.10.3 Nhận xét
*   **An toàn & Nhất quán nghiệp vụ:** Cơ chế bảo mật được thắt chặt. Phân quyền hiển thị (như ẩn/hiện Menu `mnuReports`, `mnuManageUsers`) và giới hạn đối tượng thao tác được kiểm soát theo hàm `SessionManager.CurrentPriorityLevel` và `CurrentRoleCode`. Quản lý cấp thấp không thể tạo hay sửa tài khoản của quản lý cấp cao.
*   **Kiến trúc UI linh hoạt (Smart Grid):** Các DataGridView hiện tại có khả năng cấu hình động thông qua việc "Toggle Mode". Cùng một lưới dữ liệu có thể chuyển đổi mượt mà giữa "Chế độ chi tiết" và "Chế độ tổng hợp", đồng thời UI tự cập nhật trạng thái khóa các nút bấm (Duyệt/Thanh toán) để bảo vệ dữ liệu tổng hợp không bị thao tác nhầm.
*   **Độ hoàn thiện UX trực quan:** Việc nhấn mạnh Running Balance bằng màu sắc, phân biệt trạng thái tài khoản bằng Text/Màu thay vì Checkbox, bổ sung "SỐ DƯ ĐẦU KỲ" cho báo cáo giúp người đọc số liệu và thao tác vận hành hàng ngày giảm tải sai sót đáng kể.

#### 4.10.4 Khó khăn và Giải pháp
*   **Khó khăn 1 (Rủi ro lệch phiên bản cấu trúc CSDL - Schema Drift khi refactor Roles):** Việc gỡ bỏ/thêm cột vào bảng `Roles` và loại bỏ `TenantId` khỏi Roles dễ gây văng lỗi do các Index không tồn tại hoặc cột chưa được cập nhật kịp thời trong cùng một batch migration.
    *   **Giải pháp:** Can thiệp lệnh SQL có điều kiện (`IF EXISTS`) bằng metadata hệ thống, tách batch update sau khi cột đã được tạo để thực hiện xử lý theo hướng Upsert an toàn, không gián đoạn môi trường đang chạy thật.
*   **Khó khăn 2 (Đồng bộ ngữ cảnh Chi nhánh giữa Form cha và UserControl con):** Khi Thêm/Sửa/Xóa chi nhánh từ bên trong `ucBranchManagement`, hộp thoại chọn chi nhánh tổng ở `frmMain` bị lỗi thời (stale data), đòi hỏi người dùng phải tắt ứng dụng mở lại mới thấy chi nhánh mới.
    *   **Giải pháp:** Xây dựng cơ chế giao tiếp UserControl -> Form cha bằng Event (`BranchChangedEventArgs`). Khi lưu thành công, UserControl ném Event kèm `BranchId` vừa làm việc, `frmMain` bắt tín hiệu và nạp tải bất đồng bộ (refresh async) lại danh sách chi nhánh mà không gây treo giao diện.
*   **Khó khăn 3 (Rủi ro thao tác nghiệp vụ trên dữ liệu đã cấu trúc - GroupBy):** Khi màn hình Công nợ chuyển sang "Tổng hợp đối tác", bộ dữ liệu trên Grid là số tổng giả lập, việc kích hoạt "Duyệt" hay "Thanh toán" sẽ gây lỗi cấu trúc mảng và sai dòng tiền.
    *   **Giải pháp:** Thiết lập cờ `IsSummaryViewMode`. Khi cờ này kích hoạt, bộ lọc (Filter) bên trên tự tinh giản, các lệnh nghiệp vụ được vô hiệu hóa tạm thời (Khóa Nút + Chặn dòng Event Click) và hành động Double-click được rẽ nhánh thành "Drill-down" (lọc chi tiết vào từng phiếu của chính đối tác đó) thay vì gọi Pop-up Thanh toán Từng Phiếu.

Code DAL (Entity -> BLL -> GUI)

Entity: thêm bảng Thuế ở Tax.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuChi_DoAn
{
[Table("Taxes")]
public class Tax
{
[Key]
public int TaxId { get; set; }

public int TenantId { get; set; }
[ForeignKey("TenantId")]
public virtual Tenant Tenant { get; set; }

[Required]
[StringLength(100)]
public string TaxName { get; set; }

[Column(TypeName = "decimal(5, 2)")]
public decimal Rate { get; set; }

public bool IsActive { get; set; } = true;

public virtual ICollection<Transaction> Transactions { get; set; }
}
}

Entity: nâng cấp Transaction ở Transaction.cs

[Column(TypeName = "decimal(18, 0)")]
public decimal SubTotal { get; set; }

public int? TaxId { get; set; }
[ForeignKey("TaxId")]
public virtual Tax Tax { get; set; }

[Column(TypeName = "decimal(18, 0)")]
public decimal TaxAmount { get; set; }

[Column(TypeName = "decimal(18, 0)")]
public decimal Amount { get; set; }

DAL Context: đăng ký DbSet Tax ở AppDbContext.cs

public DbSet<Tax> Taxes { get; set; }

Migration DAL:

Tạo migration mới: 20260408184426_AddTaxesAndTransactionTaxBreakdown.cs
Auto update snapshot/designer:
AppDbContextModelSnapshot.cs
20260408184426_AddTaxesAndTransactionTaxBreakdown.Designer.cs
Có bổ sung migrate dữ liệu cũ trong Up:
SubTotal = Amount
TaxAmount = 0
BLL: chưa thay đổi trong scope yêu cầu DAL.

GUI: chưa thay đổi trong scope yêu cầu DAL.

Kết quả chạy

Đã chạy migrations add thành công.
Đã chạy database update thành công, migration 20260408184426 đã được apply.
Build thành công, có cảnh báo nullability cũ của dự án (không phải lỗi mới chặn build).
File DB nhị phân đã thay đổi sau khi apply migration: CashFlowDB.mdf, CashFlowDB_log.ldf.
Documentation Summary

Mục tiêu:
Nâng cấp tầng DAL để quản lý thuế linh hoạt theo danh mục thuế và bóc tách số tiền giao dịch thành tiền trước thuế, tiền thuế và tổng thanh toán.

Task List:

Tạo entity Tax và ánh xạ bảng Taxes.
Bổ sung các cột SubTotal, TaxId, TaxAmount vào entity Transaction.
Thiết lập quan hệ khóa ngoại Transaction -> Tax.
Cập nhật AppDbContext với DbSet Taxes.
Tạo migration AddTaxesAndTransactionTaxBreakdown.
Bổ sung script chuyển đổi dữ liệu cũ trong migration (SubTotal lấy từ Amount).
Apply migration lên database bằng EF Core.
Thu hoạch cho Báo cáo:
Thiết kế danh mục thuế giúp tránh hardcode 8%/10%, mở rộng linh hoạt theo chính sách mới.
Chuẩn hóa dữ liệu Transaction theo cấu trúc kế toán rõ ràng hơn.
Đảm bảo tương thích dữ liệu lịch sử nhờ bước backfill khi migration.
Mô tả:
Bảng Taxes gồm TaxId, TenantId, TaxName, Rate, IsActive; có FK về Tenants.
Bảng Transactions thêm SubTotal (decimal 18,0), TaxAmount (decimal 18,0), TaxId (nullable FK sang Taxes).
Amount vẫn giữ vai trò tổng tiền thanh toán cuối cùng.
Migration tạo index cho TaxId ở Transactions và TenantId ở Taxes; thêm FK restrict theo convention hiện tại của hệ thống.
Nhận xét:
DAL đã tuân thủ mô hình multi-tenant của dự án (bảng mới có TenantId).
Thiết kế mới phù hợp nghiệp vụ ERP, sẵn sàng cho các mức thuế khác nhau theo thời gian.
Không phát sinh lỗi build; ảnh hưởng hiện tại tập trung ở tầng schema/data model.
Khó khăn & Giải pháp:
Khó khăn: dữ liệu Transactions cũ chỉ có Amount nên khi thêm SubTotal có nguy cơ mất nghĩa dữ liệu lịch sử.
Giải pháp: thêm câu lệnh SQL trong migration để backfill SubTotal = Amount và TaxAmount = 0 cho dữ liệu hiện hữu trước khi đưa vào vận hành logic thuế mới.