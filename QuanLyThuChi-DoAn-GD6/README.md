## Mục tiêu
- Thiết kế giao diện `ucDebt` đồng bộ với `ucTransaction` và cung cấp chức năng lọc, tìm kiếm, hiển thị danh sách khoản nợ, và khởi tạo quá trình thanh toán.

## Task List
- Tạo UI: thanh công cụ lọc (tìm kiếm, loại nợ, trạng thái) và nút `Thêm nợ`, `Thanh toán`.
- Cấu hình `DataGridView` (`dgvDebts`) với các cột cố định: Đối tác, Loại nợ, Tổng nợ, Đã trả, Còn nợ, Hạn trả, Trạng thái.
- Thực hiện `LoadDebtDataAsync()` để gọi `DebtService.GetDebts(...)` trên background thread và hiển thị `toolStripProgressBar1` tại `frmMain` trong khi load.
- Tính toán trường `Remaining` (Tổng nợ - Đã trả) trên client trước khi bind dữ liệu.
- Xử lý sự kiện `btnPayDebt` để mở `frmDebtPayment(debtId)` và reload grid khi thanh toán thành công.

## Thu hoạch cho Báo cáo
- Áp dụng pattern load bất đồng bộ và hiển thị tiến trình (progress) để tránh UI freeze.
- Đảm bảo null-safety cho quan hệ `Partner` khi bind dữ liệu.
- Định dạng tiền tệ `#,##0` và ngày `dd/MM/yyyy` cho hiển thị thân thiện.

## Mô tả

## Nhận xét

## Khó khăn & Giải pháp

---
Các file đã chỉnh sửa: `Graphical User Interface/ucDebt.cs`, `Graphical User Interface/ucDebt.Designer.cs`.
## Cập nhật giao diện ucDebt

- Thêm `pnlTop` chứa `FlowLayoutPanel` cho bộ lọc và hai `Label` thống kê ở bên phải.
- Thêm `DataGridView dgvDebts` với cột: `Đối tác`, `Loại nợ`, `Tổng nợ`, `Đã trả`, `Còn nợ`, `Trạng thái`.
### Thu hoạch cho Báo cáo
- Giao diện sử dụng panel/flow layout giúp co giãn tốt khi phóng to/thu nhỏ.
- `ucDebt.Designer.cs` được cập nhật để tạo layout: `pnlTop` (dock top) và bảng `dgvDebts` (dock fill). Các cột tiền được cấu hình format `#,##0`.

- Giữ phần xử lý dữ liệu và wiring event cho bước tiếp theo (BLL / frmDebtPayment).

- Không sửa logic nghiệp vụ ở bước này; chỉ implement UI để đảm bảo đồng bộ kiểu dáng.

## Sprint: Branch-scope & Migration (2026-04-01)

### **Mục tiêu**
- Cứng hóa branch-scope ở mức schema cho `Partner` và `TransactionCategory` (thêm `BranchId` + FK).
- Đảm bảo mọi thao tác ghi/đọc master data bắt buộc resolve branch từ `SessionManager.CurrentBranchId` (không tin input từ UI).

### **Task List**
- Thêm `BranchId` và FK `Branch` cho `Partner` và `TransactionCategory`.
- Cập nhật BLL services (`PartnerService`, `CategoryService`, `TransactionCategoryService`, `TransactionService`) để resolve scope read/write.
- Cập nhật UI (`ucPartner`, `ucTransactionCategory`) để bắt buộc chọn chi nhánh khi tạo/cập nhật và gán `BranchId`.
- Tạo migration có backfill dữ liệu cũ (lấy từ `Debts`/`Transactions`), rồi đặt NOT NULL + FK.
- Xử lý drift migration cũ (nếu cột đã tồn tại) bằng cách harden migration với checks (`IF COL_LENGTH` / `IF EXISTS`).
- Apply migrations lên DB local và regenerate idempotent script `Migrations/branch_scope_upgrade.sql`.

### **Thu hoạch cho Báo cáo**
- Branch isolation giờ được cưỡng chế từ DB → BLL → UI, giảm rủi ro rò dữ liệu giữa các chi nhánh.
- Cách resolve scope tập trung trong `SessionManager` giúp giữ nhất quán RBAC giữa GUI và service.
- Khi DB đã có drift (cột tồn tại nhưng migration chưa applied), harden migration bằng điều kiện giúp update an toàn.

### **Mô tả**
- Backfill: Partner.BranchId được populate từ `Debts` (nếu có); TransactionCategory.BranchId populate từ `Transactions` (nếu có); nếu không có nguồn thì fallback lấy branch đầu tiên trong tenant.
- Migrations: thêm SQL an toàn trước khi thay đổi schema (ví dụ: `IF COL_LENGTH('Transactions','BranchId') IS NULL ALTER TABLE ...`).
- BLL: thêm helper resolver `ResolveBranchScopeForRead/Write` trong mỗi service để buộc branch-scope theo session.
- UI: chặn tạo/cập nhật khi `SessionManager.CurrentBranchId` chưa xác định và gán `BranchId = SessionManager.CurrentBranchId.Value` khi lưu.

### **Nhận xét**
- Thiết kế hiện tại phù hợp nhu cầu multi-tenant + multi-branch; mọi kiểm tra quan trọng nằm ở BLL chứ không chỉ UI.
- Cần duy trì quy ước: mọi migration liên quan đến khóa ngoại hoặc cột mới có thể cần backfill an toàn.

### **Khó khăn & Giải pháp**
- Khó: DB local có drift (cột đã tồn tại) gây lỗi duplicate-column khi chạy `dotnet ef database update`.
- Giải pháp: chỉnh các migration pending để kiểm tra tồn tại (IF COL_LENGTH / IF EXISTS) trước khi tạo column/index/fk, chạy `dotnet ef database update`, sau đó regenerate script idempotent.

### **Các file đã chỉnh sửa (chính)**
- Business Logic Layer/Services/PartnerService.cs
- Business Logic Layer/Services/CategoryService.cs
- Business Logic Layer/Services/TransactionCategoryService.cs
- Business Logic Layer/Services/TransactionService.cs
- Data Access Layer/Entities/Partner.cs
- Data Access Layer/Entities/TransactionCategory.cs
- Graphical User Interface/ucPartner.cs
- Graphical User Interface/ucTransactionCategory.cs
- Migrations/20260330065025_AddBranchIdToTransaction.cs (hardened)
- Migrations/20260330074507_AddIsActiveToCashFund.cs (hardened)
- Migrations/20260401020943_AddBranchIdToPartnerAndTransactionCategory.cs
- Migrations/branch_scope_upgrade.sql

---
Tiếp theo: nếu bạn muốn mình tách thay đổi `Users.TenantId` ra migration riêng để giữ migration feature "sạch" thì mình có thể làm tiếp.

