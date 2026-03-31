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

