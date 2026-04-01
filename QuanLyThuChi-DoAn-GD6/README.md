# 4.6 Giai đoạn 6: Phân hệ Sổ Công nợ và Cô lập dữ liệu cấp Chi nhánh (Branch‑Scope)

## 4.6.1 Minh chứng giai đoạn

- **Giao diện:** ảnh chụp màn hình ucDebt hiển thị danh sách công nợ với các cột tính toán động (Còn nợ), định dạng tiền tệ chuẩn Việt Nam (#,##0), định dạng ngày (`dd/MM/yyyy`) và bộ lọc dữ liệu đa chiều. Ảnh chụp cửa sổ frmDebtPayment khi thực hiện thanh toán.
- **Mã nguồn & CSDL:** các migration đã được "cứng hóa" (ví dụ `Migrations/branch_scope_upgrade.sql`) theo kiểu Idempotent Scripts (kiểm tra tồn tại bằng `IF COL_LENGTH` / `IF EXISTS`) để tránh Schema Drift.

## 4.6.2 Mô tả

- **Mục tiêu:** Hoàn thiện phân hệ Quản lý Công nợ và nâng cấp kiến trúc dữ liệu để cô lập dữ liệu ở mức Chi nhánh (Branch‑scope) cho các bảng danh mục.

- **Giao diện & trải nghiệm (UI/UX):**
	- `ucDebt` thiết kế responsive với `pnlTop` (bộ lọc) và `dgvDebts` (lưới dữ liệu).
	- Tải dữ liệu bất đồng bộ thông qua `LoadDebtDataAsync()` chạy trên luồng nền; hiển thị tiến trình trên `toolStripProgressBar1` của `frmMain` để tránh treo UI.
	- Trường "Còn nợ" được tính trên client trước khi bind: $Remaining = TotalDebt - PaidAmount$ (tức $Còn\;nợ = Tổng\;nợ - Đã\;trả$) và hiển thị theo `#,##0`.

- **Nâng cấp Kiến trúc Dữ liệu (DAL):**
	- Thêm trường `BranchId` và FK cho các bảng danh mục quan trọng (Partner, TransactionCategory).
	- Chuẩn hóa kiểu tiền tệ thành `decimal(18,0)` cho VND.

- **Cập nhật Logic Nghiệp vụ (BLL):**
	- Tái cấu trúc service (PartnerService, CategoryService, TransactionService, TransactionCategoryService) để mọi thao tác đọc/ghi tuân thủ `SessionManager.CurrentBranchId` (ResolveBranchScopeForRead/Write).
	- Các thao tác phát sinh giao dịch/thu/chi khi thanh toán nợ phải thực hiện trong `IDbContextTransaction` để đảm bảo tính nguyên tử (atomic).

- **Luồng Thanh toán Nợ:**
	- `btnPayDebt` mở `frmDebtPayment(debtId)`; khi thanh toán thành công cập nhật `Debts.PaidAmount` và sinh Transaction/Receipt trong cùng một transaction DB.

## 4.6.3 Nhận xét

- **Bảo mật dữ liệu:** Cô lập dữ liệu theo chi nhánh ngăn chặn rò rỉ và chồng chéo dữ liệu giữa các chi nhánh trong môi trường multi‑tenant.
- **Hiệu năng:** Tải dữ liệu bất đồng bộ kết hợp tính toán "Còn nợ" trên client giúp giảm tải cho SQL Server và giữ được trải nghiệm mượt mà khi dữ liệu lớn.
- **Quản trị DB chuyên nghiệp:** Các migration idempotent và các kiểm tra tồn tại trước khi thay đổi schema cho phép triển khai an toàn trên môi trường có drift.

## 4.6.4 Khó khăn & Giải pháp

1. **Schema Drift (cột/constraint đã tồn tại):**
	 - Vấn đề: Thêm `BranchId` trên bảng đã có dữ liệu dễ gây lỗi "column already exists" hoặc vi phạm ràng buộc.
	 - Giải pháp: Viết migration hardened bằng kiểm tra tồn tại (`IF COL_LENGTH`, `IF EXISTS`) và thực hiện backfill dữ liệu an toàn từ `Debts`/`Transactions`; chỉ sau khi backfill mới đặt NOT NULL + FK.

2. **Đảm bảo an toàn khi nhập liệu danh mục:**
	 - Vấn đề: Các form quản lý danh mục cũ có thể lưu `BranchId` rỗng, làm vỡ logic phân quyền.
	 - Giải pháp: Thêm guard clauses ở UI + BLL; buộc gán `BranchId = SessionManager.CurrentBranchId.Value` khi lưu; vô hiệu hóa thao tác khi `CurrentBranchId` chưa xác định.

3. **Null‑Reference khi binding dữ liệu:**
	 - Vấn đề: Một số khoản nợ không liên kết Partner (Partner = null) dẫn đến lỗi khi hiển thị `Partner.PartnerName`.
	 - Giải pháp: Áp dụng null‑safety khi binding (toán tử `?.`), kiểm tra null trước khi truy xuất thuộc tính, và hiển thị placeholder khi thiếu dữ liệu.

---

**Ghi chú:** muốn mình tách thay đổi `Users.TenantId` ra migration riêng để giữ các migration feature sạch hơn không? Nếu có, mình sẽ tạo migration idempotent tách riêng và cập nhật script deploy.


