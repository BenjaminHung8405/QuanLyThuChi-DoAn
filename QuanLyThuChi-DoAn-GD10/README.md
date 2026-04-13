# 📊 Hệ thống Quản lý Dòng tiền (Cash Flow Management)
## Tài liệu hướng dẫn giai đoạn hoàn thiện (GD10 - GD11 - GD12)

Dự án đã hoàn tất các mốc quan trọng từ Giai đoạn 10 đến Giai đoạn 12, tập trung vào bảo mật đa tầng, quản trị thuế, lưu vết hệ thống và chuẩn hóa các nghiệp vụ kế toán nâng cao. Dưới đây là mô tả chi tiết các thay đổi trong repo này.

---

### 4.10 Giai đoạn 10: Phân cấp quyền Đa Tenant và UX Drill-down chuyên sâu

#### 4.10.1 Minh chứng giai đoạn
*   **Giao diện Quản lý Người dùng & Chi nhánh:** Ảnh chụp màn hình `ucUserManagement` và `ucBranchManagement` hiển thị danh sách người dùng/chi nhánh tương ứng với Tenant hiện tại. Ảnh chụp Form thêm/sửa người dùng (`frmAddUser`, `frmEditUser`) với các tuỳ chọn chức vụ (Role) và chi nhánh (Branch) được lọc động theo cấp bậc quyền (PriorityLevel).
*   **Giao diện Báo cáo & Công nợ (UX nâng cao):** Ảnh chụp màn hình `ucCashbookReport` với chế độ xem Sổ quỹ chi tiết (có dòng "SỐ DƯ ĐẦU KỲ" và cột Lũy kế - Running Balance) và chế độ xem Tổng hợp. Ảnh chụp màn hình `ucDebt` thể hiện tính năng "Toggle View" (chuyển đổi xem Từng phiếu / Tổng đối tác) và nút "Bỏ lọc nhanh".
*   **Cơ sở dữ liệu:** Ảnh chụp bảng `Roles` có thêm các cột `RoleCode`, `PriorityLevel`. Bảng `Branch` có thêm trường `CreatedDate`.

#### 4.10.2 Mô tả
*   **Mục tiêu:** Triển khai phân hệ con người (Nhân sự & Chi nhánh) theo hướng bảo mật và kiến trúc đa khách hàng (Multi-tenant). Nâng cấp trải nghiệm người dùng (UX) cho phân hệ Công nợ và Báo cáo Sổ quỹ.
*   **Kiến trúc Bảo mật:** 
    *   Tầng BLL áp dụng cơ chế xác thực nghiệp vụ (tenant scope, branch scope) trước mọi thao tác để ngăn chặn UI bypass. 
    *   Bảng `Roles` được mở rộng với `RoleCode` và `PriorityLevel` để phân quyền linh hoạt (VD: Giám đốc chi nhánh không thể xóa tài khoản của Admin).
*   **Giao diện tương tác (UI):** Thiết kế lại các UserControl hoạt động theo dạng Card-based. Bổ sung cơ chế Drill-down (bấm đúp từ view tổng hợp sang view chi tiết) và liên kết reset filter 1 chạm.

#### 4.10.3 Khó khăn và Giải pháp
*   **Khó khăn:** Đồng bộ ngữ cảnh Chi nhánh giữa Form cha (`frmMain`) và UserControl con khi có thay đổi dữ liệu chi nhánh.
*   **Giải pháp:** Xây dựng cơ chế giao tiếp qua Event (`BranchChangedEventArgs`). Khi lưu thành công ở UserControl, `frmMain` bắt tín hiệu và refresh lại danh sách chi nhánh bất đồng bộ.

---

### 4.11 Giai đoạn 11: Quản trị Thuế và Lưu vết hệ thống (Audit Logging)

#### 4.11.1 Minh chứng giai đoạn
*   **Giao diện Nhật ký hệ thống:** Màn hình `ucAuditLogViewer` hiển thị chi tiết các hành động Sửa/Xóa/Hủy, bao gồm giá trị cũ (Old Data) và giá trị mới (New Data) dưới dạng JSON.
*   **Tính năng Hủy phiếu:** Form `frmInputReason` xuất hiện khi Quản lý yêu cầu hủy giao dịch, bắt buộc nhập lý do để lưu vào Audit Log.
*   **Giao diện Thuế:** Danh mục thuế (`ucTaxManagement`) cho phép cấu hình các mức thuế suất (8%, 10%...) áp dụng cho từng giao dịch.

#### 4.11.2 Mô tả
*   **Lưu vết (Audit Logging):** Xây dựng `AuditLogService` ghi lại mọi biến động dữ liệu quan trọng. Mọi hành động nhạy cảm đều được định danh bởi `UserId` và `ActionDate`.
*   **Quản lý Thuế:** Chuẩn hóa DAL/BLL để bóc tách `SubTotal` (tiền trước thuế) và `TaxAmount` (tiền thuế). `Amount` hiện là tổng tiền thanh toán cuối cùng, giúp báo cáo tài chính minh bạch hơn.
*   **Hủy giao dịch (Voiding):** Thay vì xóa vật lý, hệ thống triển khai logic hủy phiếu (Status = `CANCELLED`). Khi hủy, số dư quỹ sẽ được hoàn lại tự động và ghi log chi tiết lý do.

#### 4.11.3 Khó khăn và Giải pháp
*   **Khó khăn:** Dữ liệu cũ chỉ có `Amount` nên khi nâng cấp lên cấu trúc có Thuế sẽ bị lệch số liệu báo cáo.
*   **Giải pháp:** Viết script Migration tự động backfill: `SubTotal = Amount` và `TaxAmount = 0` cho toàn bộ dữ liệu lịch sử.

---

### 4.12 Giai đoạn 12: Chuyển quỹ 2 bước & Snapshot chống trôi lịch sử

#### 4.12.1 Minh chứng giai đoạn
*   **Giao diện Chuyển quỹ:** Form `frmFundTransfer` hỗ trợ nhập phí ngân hàng. Giao dịch tại quỹ đích hiển thị trạng thái `PENDING` (Chờ xác nhận).
*   **Giao diện Xác nhận:** Nút "Xác nhận nhận tiền" xuất hiện trên grid `ucTransaction` đối với các phiếu chờ nhận.
*   **Báo cáo Nhất quán:** Grid hiển thị tên Danh mục/Đối tác tại thời điểm lập phiếu (Snapshot) ngay cả khi dữ liệu gốc đã bị đổi tên hoặc xóa mềm.

#### 4.12.2 Mô tả
*   **Quy trình Chuyển quỹ nội bộ 2 bước:**
    *   **Bước 1:** Tạo phiếu OUT (`COMPLETED`) ở quỹ nguồn (trừ tiền ngay). Đồng thời tạo phiếu IN (`PENDING`) ở quỹ đích (chưa cộng tiền).
    *   **Bước 2:** Quản lý quỹ đích nhấn "Xác nhận" để chuyển trạng thái sang `COMPLETED`, lúc này tiền mới chính thức vào quỹ đích.
*   **Snapshot dữ liệu:** Bổ sung `CategoryNameSnapshot` và `PartnerNameSnapshot` vào bảng `Transactions`. Điều này đảm bảo báo cáo quá khứ luôn chính xác theo đúng thông tin tại thời điểm phát sinh giao dịch.
*   **Xóa mềm (Soft Delete):** Áp dụng triệt để `IsActive = false` cho Branch và Partner để bảo toàn tính toàn vẹn tham chiếu (Referential Integrity).

#### 4.12.3 Khó khăn và Giải pháp
*   **Khó khăn:** Tính toán số dư quỹ và báo cáo bị sai nếu bao gồm cả các phiếu `PENDING` hoặc phiếu đã hủy.
*   **Giải pháp:** Cấu trúc lại toàn bộ các hàm lọc và Summary trong `TransactionService` và `ReportService` để chỉ tính toán trên các giao dịch có `Status = 'COMPLETED'`.

---

### 🛠 Cập nhật Kỹ thuật (DAL - BLL - GUI)

#### Tầng Dữ liệu (DAL)
*   **Taxes:** Bảng mới quản lý mức thuế suất theo Tenant.
*   **AuditLogs:** Bảng lưu vết hành động người dùng.
*   **Transactions:** Mở rộng các trường `SubTotal`, `TaxAmount`, `TaxId`, `Status`, `TransferRefId`, `CategoryNameSnapshot`, `PartnerNameSnapshot`.

#### Tầng Nghiệp vụ (BLL)
*   **TaxService:** Xử lý logic tính toán thuế suất động.
*   **AuditLogService:** Tự động hóa việc ghi nhật ký thay đổi dữ liệu.
*   **TransactionService:** Triển khai logic Chuyển quỹ 2 bước có hỗ trợ phí ngân hàng và cơ chế Hủy phiếu an toàn.

#### Tầng Giao diện (GUI)
*   **Visual Enhancements:** Sử dụng `TableLayoutPanel` cho các form báo cáo để đảm bảo giao diện co giãn tốt (Responsive).
*   **UX Improvements:** Cột trạng thái có màu sắc phân biệt (Xác nhận/Chờ/Hủy), định dạng số tiền `N0` đồng nhất, hỗ trợ phím tắt và reset bộ lọc nhanh.