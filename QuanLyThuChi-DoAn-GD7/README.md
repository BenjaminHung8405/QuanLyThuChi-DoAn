# 4.7 — Giai đoạn 7: Quản lý Phát sinh Công nợ và Nợ đầu kỳ

## 4.7.1 Minh chứng giai đoạn

- (Chưa có tài liệu minh chứng cụ thể trong file này)

## 4.7.2 Mô tả

### Mục tiêu

- Xây dựng tính năng ghi nhận nợ phát sinh không đi kèm dòng tiền (Accrual Accounting) như: công nợ đầu kỳ, mua/bán chịu.
- Hoàn thiện quy trình kiểm soát nội bộ (Maker — Checker) và cung cấp báo cáo tổng quan cho quản lý/giám đốc.

### Kiến trúc dữ liệu (BLL)

- `AddDebt`: Tạo khoản nợ mới với các giá trị khởi tạo an toàn (`PaidAmount = 0`, `Status = "NEW"`, `CreatedDate = DateTime.Now`) và liên kết chặt chẽ với `BranchId`/`TenantId`.
- `GetDebtsAsync(tenantId, branchId)`: Lấy danh sách công nợ theo ngữ cảnh Tenant/Branch. Khi `branchId == 0` sẽ bỏ qua bộ lọc chi nhánh (chế độ "Tất cả chi nhánh"). Sử dụng `async/await` trực tiếp (không dùng `Task.Run`).
- `ApproveDebtAsync`: Quy trình duyệt nợ, kiểm tra trạng thái gốc (`NEW`) trước khi chuyển sang `PENDING`.

### Giao diện nhập liệu & Tác nghiệp (UI)

- Form `frmAddDebt`: Dialog cố định (FixedDialog) kích thước phù hợp, hỗ trợ điều hướng bằng phím Tab và định dạng tiền (`N0`).
- Control `ucDebt`: Cập nhật cơ chế phân quyền và trạng thái nút thao tác thông qua `TogglePayButtonState()` — vô hiệu hóa thao tác khi ở chế độ "Tất cả chi nhánh" và chỉ mở khóa các nút tương ứng theo `RawStatus`/`Status`.

### Tích hợp hệ thống

- Nạp động danh sách chi nhánh, thêm item `BranchId = 0` cho chế độ xem toàn bộ tenant (dành cho SuperAdmin/TenantAdmin).
- Đồng bộ sự kiện `SelectedIndexChanged` từ `frmMain` để gọi `LoadDebtDataAsync()` cập nhật lưới ngay lập tức.

### Đồng bộ trạng thái hiển thị

- Ánh xạ 2 chiều giữa mã trạng thái DB (`NEW`, `PENDING`, `PARTIALLY_PAID`, `PAID`) và hiển thị tiếng Việt trên DataGridView (`Mới tạo`, `Chưa thanh toán`, `Thanh toán một phần`, `Đã thanh toán`).

## 4.7.3 Nhận xét

- Tách biệt rõ ràng giữa kế toán dồn tích và kế toán tiền mặt; thao tác thêm nợ không sinh giao dịch tiền mặt nên an toàn về tồn quỹ.
- Cơ chế duyệt công nợ (Maker/Checker) đã giảm rủi ro thao tác trái ngữ cảnh giữa các chi nhánh.
- UX được cải thiện: màu trạng thái rõ ràng, validations client-side, và hiệu năng tốt hơn nhờ sử dụng `async/await` đúng chỗ.

## 4.7.4 Khó khăn & Giải pháp

1. **Khó khăn:** Rủi ro lệch loại nợ do mã hệ thống (`RECEIVABLE`, `PAYABLE`) gây nhầm lẫn cho kế toán.
	**Giải pháp:** Sử dụng đối tượng ánh xạ hiển thị (anonymous object) trong ComboBox để trình bày tiếng Việt dễ hiểu (`"Khách nợ mình"`, `"Mình nợ đối tác"`) trong khi vẫn lưu mã chuẩn vào DB.

2. **Khó khăn:** Danh sách đối tác có thể chứa dữ liệu không thuộc chi nhánh hiện tại hoặc đối tác đã khóa.
	**Giải pháp:** Tái sử dụng `GetAllActive()` hoặc các API có ràng buộc `SessionManager.CurrentBranchId` để đảm bảo tính toàn vẹn dữ liệu.

3. **Khó khăn:** Xung đột trạng thái UI khi người dùng chọn chế độ "Tất cả chi nhánh" (BranchId = 0) làm nút hành động bị kẹt hoặc cho phép thao tác không hợp lệ.
	**Giải pháp:** Tập trung kiểm tra chế độ "view-all" và vô hiệu hóa các nút hành động (Thêm / Duyệt / Thanh toán) cho đến khi người dùng chọn chi nhánh cụ thể.

4. **Khó khăn:** Đồng bộ trạng thái giữa UI (hiển thị tiếng Việt) và DB (mã tiếng Anh) dễ gây lỗi khi logic kiểm tra dựa trên chuỗi.
	**Giải pháp:** Bổ sung thuộc tính `RawStatus` khi binding dữ liệu, đồng thời xây dựng `GetCurrentDebtStatus()` + `NormalizeDebtStatus()` để làm việc với mã gốc trước khi đưa ra quyết định UI.

---

Cập nhật: 2026-04-04

