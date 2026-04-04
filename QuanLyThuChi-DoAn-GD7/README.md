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

Mục tiêu: Thêm lựa chọn “Tất cả chi nhánh” cho SuperAdmin/TenantAdmin khi chọn chi nhánh.
Task List: Chèn item branch id 0 vào danh sách; cập nhật logic chọn mặc định để nhận giá trị 0.
Thu hoạch cho Báo cáo: Cần xử lý “branch id = 0” như một ngữ cảnh hợp lệ cho đa chi nhánh.
Mô tả: Trong LoadBranchesIntoComboBox, thêm item “--- Tất cả chi nhánh ---” và ưu tiên chọn 0 khi không có branch hiện tại.
Nhận xét: UI nhất quán hơn cho vai trò quản trị; không ảnh hưởng đến role bị khóa chi nhánh.
Khó khăn & Giải pháp: Tránh xung đột với logic chọn mặc định bằng cách chỉ gán 0 cho SuperAdmin/TenantAdmin.
Mục tiêu: Thêm phương thức async GetDebtsAsync xử lý chế độ "Tất cả chi nhánh" (BranchId == 0) đúng cách, thay thế wrapping sync method trong Task.Run.

Task List:

Thêm GetDebtsAsync(int tenantId, int? branchId) với logic lọc BranchId có điều kiện
Cập nhật ucDebt.LoadDebtDataAsync() để gọi phương thức async mới
Làm rõ nhận xét lọc BranchId trong GetDebts hiện tại
Thu hoạch cho Báo cáo: Phương thức async giúp tránh Task.Run wrapping và cho phép lọc cơ sở dữ liệu trực tiếp. BranchId == 0 giờ được xử lý nhất quán: bỏ qua lọc chi nhánh để lấy toàn bộ tenant.

Mô tả:

DebtService: Thêm GetDebtsAsync() với điều kiện if (branchId > 0) xác định có lọc chi nhánh hay không
ucDebt: Gọi GetDebtsAsync(tenantId, branchId) thay vì Task.Run(() => GetDebts(...))
Logic: Khi BranchId == 0, query trả về toàn bộ công nợ của Tenant
Nhận xét: Code rõ ràng hơn, hiệu suất tốt hơn (async thực sự, không Task.Run), phù hợp kiến trúc 3-tier.

Khó khăn & Giải pháp: Ban đầu có thể nhầm lẫn khi BranchId == 0; giải pháp là thêm comment rõ ràng và kiểm tra branchId.Value > 0 trước khi thêm WHERE clause.

Mục tiêu: Sửa lỗi dgvDebts không lọc theo chi nhánh khi đổi cbBranchs.
Task List: Trace flow đổi branch; kiểm tra refresh hiện tại; mở public hàm reload debt; bổ sung nhánh refresh cho ucDebt.
Thu hoạch cho Báo cáo: Context (TenantId/BranchId) đổi nhưng UI không tự lọc nếu control hiện tại không được đưa vào RefreshCurrentView.
Mô tả: cbBranchs_SelectedIndexChanged vẫn cập nhật session đúng, nhưng RefreshCurrentView thiếu case ucDebt; đã thêm await ucDebtView.LoadDebtDataAsync() để reload dữ liệu ngay theo branch.
Nhận xét: Luồng đồng bộ context giữa frmMain và ucDebt giờ nhất quán với các màn hình khác.
Khó khăn & Giải pháp: Lỗi không nằm ở query service mà ở lớp GUI orchestration; giải quyết bằng cách nối lại luồng refresh thay vì sửa query.
Mục tiêu: Ổn định flow enable/disable nút trên màn hình công nợ theo ngữ cảnh chi nhánh, tránh kẹt trạng thái khi ở Tất cả chi nhánh.
Task List: Thêm hàm xác định view-all mode; thêm hàm áp trạng thái nút; cập nhật TogglePayButtonState; thêm guard cho Add/Pay; build xác nhận.
Thu hoạch cho Báo cáo: Khi có nhiều nơi can thiệp trạng thái nút, cần một hàm trung tâm để tránh ghi đè lẫn nhau.
Mô tả: ucDebt nay coi null/0 là chế độ tổng hợp, tự khóa nút ghi dữ liệu và hiển thị thông báo yêu cầu chọn chi nhánh cụ thể trước khi thao tác.
Nhận xét: UI nhất quán hơn, giảm lỗi thao tác sai ngữ cảnh branch.
Khó khăn & Giải pháp: Nút bị re-enable lại ở finally do TogglePayButtonState; đã thêm check view-all trực tiếp trong Toggle để triệt tiêu xung đột logic.