## Documentation Summary

1. **Mục tiêu**
- Xây dựng form `frmAddDebt` (Popup Thêm nợ) theo chuẩn ERP giúp kế toán nhập liệu nhanh, logic và an toàn.

2. **Task List**
- Tạo hàm `DebtService.AddDebt(...)` (BLL) trong `DebtService.cs`.
- Thiết kế GUI `frmAddDebt` theo layout 3 phần Header/Main/Footer.
- Gán thuộc tính chuẩn form: FixedDialog, CenterParent, không resize (MaximizeBox/MinimizeBox false), kích thước 600x800.
- Tạo các control cần thiết: `cboPartner`, `cboDebtType`, `txtTotalAmount`, `dtpDueDate`, `txtNotes`, `btnSave`, `btnCancel`.
- Đảm bảo TabIndex: `cboPartner(0)`, `cboDebtType(1)`, `txtTotalAmount(2)`, `dtpDueDate(3)`, `txtNotes(4)`, `btnSave(5)`.
- Logic đổi màu header theo loại nợ và tự format số tiền ở `txtTotalAmount`.
- Valid input trước khi gọi `DebtService.AddDebt(...)`.

3. **Thu hoạch cho Báo cáo**
- Kỹ năng thiết kế UX form nhập liệu theo chiều dọc (Top-Down) cho phép thao tác bằng phím Tab mượt.
- Áp dụng chuẩn hóa dữ liệu tiền tệ (N0) ngay tại UI, giảm lỗi trước khi gọi BLL.
- Phân tách rõ ràng trách nhiệm UI/BLL và tái dùng service hiện có.

4. **Mô tả**
- File BLL: `Business Logic Layer/Services/DebtService.cs` đã có hàm `AddDebt` theo yêu cầu.
- File GUI: `frmAddDebt.Designer.cs` + `frmAddDebt.cs` triển khai đầy đủ Form và hành vi.
- Đối tác load từ `PartnerService.GetPartnersByTenant` theo Tenant context.
- Dữ liệu công nợ ghi vào DB thông qua `DebtService.AddDebt`, cập nhật `Debts` table.

5. **Nhận xét**
- Dữ liệu logic và UI đúng với hướng dẫn (màu sắc, font, tab, dịch vụ dữ liệu).
- Dễ tích hợp vào `ucDebt` bằng popup modal và refresh lưới khi thành công.

6. **Khó khăn & Giải pháp**
- Khó khăn: frmAddDebt ban đầu rỗng không có controls, cần tự dựng lại toàn bộ.
- Giải pháp: Thiết kế toàn bộ layout thủ công trong Designer, kết hợp event và service để đảm bảo tính năng hoàn chỉnh.
