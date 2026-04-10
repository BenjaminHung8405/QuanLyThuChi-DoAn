# 📊 Hệ thống Quản lý Dòng tiền (Cash Flow Management)

**Mô hình:** Multi-Tenant & Multi-Branch (Hỗ trợ quản lý chuỗi cơ sở)  
**Phân tầng Kiến trúc:** Logical 3-Tier Architecture (Chia 3 lớp vật lý trên cùng 1 Project: DAL, BLL, GUI)

**Công nghệ sử dụng:**
* **Nền tảng:** C# .NET (Windows Forms Application)
* **Cơ sở dữ liệu:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First Approach)
* **Bảo mật:** BCrypt (mã hóa mật khẩu), Role-Based Access Control (RBAC).

Dự án này là một ứng dụng Desktop chuyên sâu dành cho mô hình kinh doanh chuỗi (chuỗi cửa hàng, nhà hàng, hệ thống bán lẻ...). Hệ thống sở hữu khả năng quản lý tài chính độc lập cho từng chi nhánh và quỹ tiền, giám sát chặt chẽ công nợ theo thời gian thực và lưu vết biến động (Audit). Mọi luồng dữ liệu đều được cách ly chặt chẽ theo `TenantId` và `BranchId`.

Dưới đây là tiến trình hoàn thiện qua **12 Giai đoạn (Sprints)** của dự án.

---

## 🚀 Lộ trình 12 Giai đoạn Phát triển (Sprints)

### 🎯 Giai đoạn 1: Khởi tạo Kiến trúc & Thiết kế Database
* **Mục tiêu:** Xây dựng cơ sở dữ liệu cốt lõi hỗ trợ mô hình Multi-tenant & Multi-branch.
* **Chi tiết:** Khởi tạo 13 bảng cốt lõi (Tenants, Branches, Roles, Users, CashFunds, Partners, Debts, Transactions, Attachments, AuditLogs...). Chuẩn hóa CSDL theo tiêu chuẩn kế toán và cấu trúc ORM linh hoạt.
* **Khắc phục:** Sửa chữa triệt để lỗi "Multiple cascade paths" bằng việc tích hợp Fluent API để thiết lập `DeleteBehavior.Restrict`, đảm bảo an toàn cho dữ liệu tài chính.

### 🎯 Giai đoạn 2: Xây dựng Repository & Logic nền tảng (BLL)
* **Mục tiêu:** Tách biệt logic dữ liệu khỏi UI và chuẩn bị sẵn mô hình bảo mật.
* **Chi tiết:** Triển khai Generic Repository Pattern tối ưu code CRUD. Tạo lõi SessionManager tĩnh xử lý Multi-tenant ngầm ở tầng BLL, tự động giới hạn và chặn lọc dữ liệu trái phép. Mã hóa mật khẩu an toàn theo băm BCrypt.Net.

### 🎯 Giai đoạn 3: Phân hệ Đăng nhập & Điều hướng (Authentication & Navigation)
* **Mục tiêu:** Xây dựng cửa ngõ bảo mật đầu vào, khung Dashboard tĩnh tại và phân quyền UI.
* **Chi tiết:** Form Đăng nhập chuyên nghiệp (`frmLogin`), hỗ trợ "Remember Me". Trang chính (`frmMain`) dùng cơ chế nạp động UserControl thân thiện để giảm tải nháy/treo. Phân cấp hệ thống quyền quyết định việc ẩn/hiện Module một cách linh động qua RBAC.

### 🎯 Giai đoạn 4: Quản lý Dữ liệu Nền tảng (Master Data Management)
* **Mục tiêu:** Xây dựng nền móng Master Data quản lý Đối tác và chuẩn hóa danh mục Thu/Chi.
* **Chi tiết:** Thực thi quy tắc xóa mềm (Soft Delete) qua cờ `IsActive` thay vì xóa vật lý. Tư duy UX sâu sắc với cơ chế Auto-Edit trên Grid, chuyển màu sắc theo bản chất nguồn thu/chi. Hỗ trợ Fuzzy Search tiếng Việt (tìm không dấu). Phân mảnh quyền sở hữu rạch ròi theo Tenant.

### 🎯 Giai đoạn 5: Cấu trúc SaaS Multi-tenant, Sổ Quỹ và Kế toán kép
* **Mục tiêu:** Số hóa nghiệp vụ Kế toán cốt lõi, Quỹ tiền & Giao dịch chuyển quỹ.
* **Chi tiết:** Số hóa triệt để "Chuyển quỹ nội bộ" qua nguyên lý kế toán kép (Tạo song song Record Thu và Chi liên kết bằng `TransferRefId`). Áp dụng UI Loading Overlay và các truy vấn động (`IQueryable`) chạy nhẹ nhàng. Mọi Logic kiểm tra hoàn toàn chôn trong BLL. Khởi đầu mô hình Multi-tenant ròng.

### 🎯 Giai đoạn 6: Phân hệ Sổ Công nợ và Cô lập dữ liệu cấp Chi nhánh
* **Mục tiêu:** Hoàn thiện sơ đồ Quản lý Công nợ chi tiết ở cấp Branch-scope.
* **Chi tiết:** Xử lý bất đồng bộ Background Worker, với cột tính toán `Còn Nợ` tự động giảm trừ qua mỗi lần trả. Quá trình trả nợ đặt an toàn tuyệt đối bên trong `IDbContextTransaction` (atomic rollback). Viết logic Migration chịu lỗi "Idempotent" để di chuyển Schema của môi trường đang có cấu trúc dở dang mà không mất dữ liệu.

### 🎯 Giai đoạn 7: Phân hệ Phát sinh Công nợ đột xuất và Nợ đầu kỳ
* **Mục tiêu:** Module dồn tích tài khoản để xử lý khoản công nợ khởi tạo (không dính giao dịch tiền).
* **Chi tiết:** Đưa quy trình Duyệt (Maker/Checker) vào nợ phát sinh. Giải quyết bài toán BranchId linh hoạt (BranchId = 0 cho xem toàn cụm chi nhánh nếu có quyền Admin). Đồng bộ mapping tên tiếng Việt/Tiếng Anh trực quan nhưng vẫn lưu mã CSDL rạch ròi (`RECEIVABLE`, `PAYABLE`).

### 🎯 Giai đoạn 8: Thống kê & Báo cáo cấp Chi nhánh (Dashboard)
* **Mục tiêu:** Màn hình Dashboard trực quan thể hiện bức tranh tài chính.
* **Chi tiết:** Sử dụng DTO làm proxy vận chuyển dữ liệu cho Card UI và Data Visualization Chart. Xử lý "N+1 Query" bằng Deferred Execution LINQ ép thực thi tận ở SQL Server. Giải quyết lỗi khi khoảng thời gian rỗng, tự động căn chỉnh và format tiền VND tiêu chuẩn ERP.

### 🎯 Giai đoạn 9: Đối soát Chuỗi & Xuất báo cáo Excel
* **Mục tiêu:** Báo cáo chi tiết luồng tiền liên-chi-nhánh và tích hợp thao tác xuất Offline.
* **Chi tiết:** Truy vấn chéo lấy tổng quát toàn hệ thống dựa vào TenantId và trả báo cáo số liệu phân vùng. Cache lại Record trên giao diện để đổ file `.xlsx` (sử dụng ClosedXML) mượt "như giật điện 1 giây", ngăn ngừa SQL bottleneck. Chuẩn mực định dạng kế toán.

### 🎯 Giai đoạn 10: Phân cấp quyền nâng cao Đa Tenant và UX Drill-down chuyên sâu
* **Mục tiêu:** Chống drift Schema khi sửa cấu trúc User/Roles, hoàn thiện các "cách xem" Sổ quy chuẩn.
* **Chi tiết:** Cơ chế Drill-down và "Toggle View": Bấm mở chi tiết từng phiếu, chuyển đổi giữa kiểu danh sách và loại dòng tiền tổng đối tác, có Running Balance Tính số dư lũy kế trực tiếp trên giao diện Report. Cơ chế phát sự kiện Event giữa `UserControl` và `Form` đè. Thay cấu hình Roles hard-code thành cấu trúc CSDL cấp ưu tiên (`PriorityLevel`). Năng lực SuperAdmin tùy chỉnh Tenant Admin tuyệt mật.

### 🎯 Giai đoạn 11: Quản trị Thuế, Lưu vết hệ thống (Audit Logging) & Cải tiến Dashboard
* **Mục tiêu:** Kiểm soát và truy vết mọi thay đổi dữ liệu (Audit), bổ sung module Thuế & Tối ưu Dashboard.
* **Chi tiết:** 
  * **Hệ thống Lưu vết (Audit Logging):** Xây dựng `AuditLogService` và giao diện `ucAuditLogViewer` để ghi nhận mọi hành động (thêm, sửa, xóa, hủy giao dịch) theo `UserId`, giúp theo dõi biến động dữ liệu theo thời gian thực.
  * **Hủy Giao dịch & Ghi nhận lý do:** Bổ sung tính năng hủy giao dịch thay vì xóa cứng (`frmInputReason`). Lưu lý do hủy vào chi tiết giao dịch và Audit.
  * **Quản lý Thuế:** Phát triển module thuế hoàn chỉnh (`TaxService`, `ucTaxManagement`, `frmAddEditTax`), hỗ trợ tính toán thuế áp dụng trên các khoản thu/chi.
  * **Cải tiến Dashboard:** Bổ sung các chỉ số tổng quan (Financial overview metrics) trên `ucDashboard`, cung cấp cái nhìn toàn diện hơn về dòng tiền hiện tại.

### 🎯 Giai đoạn 12: Chuyển quỹ 2 bước, Snapshot dữ liệu lịch sử & Chuẩn hóa báo cáo
* **Mục tiêu:** Tăng độ an toàn khi chuyển quỹ liên chi nhánh và bảo toàn tính nhất quán báo cáo khi dữ liệu danh mục/đối tác thay đổi theo thời gian.
* **Chi tiết:**
  * **Chuyển quỹ nội bộ 2 bước:** Tại `TransactionService`, lệnh chuyển quỹ tạo đồng thời phiếu OUT trạng thái `COMPLETED` (trừ quỹ nguồn ngay) và phiếu IN trạng thái `PENDING` (chờ xác nhận ở quỹ đích). Bổ sung luồng xác nhận nhận tiền (`ConfirmPendingInboundTransferAsync`) và liên kết chứng từ qua `TransferRefId`/`TransferRefNo`.
  * **Hỗ trợ phí giao dịch ngân hàng:** Luồng chuyển quỹ cho phép nhập phí và tự động tạo thêm giao dịch OUT phí ngân hàng, vẫn đảm bảo tính toàn vẹn bằng transaction DB.
  * **Nâng cấp UX giao dịch:** `ucTransaction` hiển thị cột trạng thái (Chờ xác nhận/Hoàn tất/Đã hủy), khóa chỉnh sửa phiếu chờ xác nhận và cho phép người có quyền xác nhận trực tiếp trên màn hình. `frmFundTransfer` cập nhật thông báo kết quả theo mô hình 2 bước và định dạng nhập tiền theo chuẩn văn hóa cấu hình.
  * **Snapshot dữ liệu chống trôi lịch sử:** Bổ sung `CategoryNameSnapshot`, `PartnerNameSnapshot` cho `Transactions` và `PartnerNameSnapshot` cho `Debts`; migration `AddTransactionAndDebtSnapshots` tự động backfill dữ liệu cũ để báo cáo không bị ảnh hưởng khi đổi tên danh mục hoặc đối tác.
  * **Chuẩn hóa tính toán theo trạng thái:** Báo cáo/số dư quỹ chỉ tính giao dịch `COMPLETED`, loại trừ phiếu `PENDING` và phiếu đã hủy khỏi tổng hợp tài chính.
  * **Nhất quán xóa mềm dữ liệu nền:** Chuẩn hóa xóa mềm cho Branch/Partner (`IsActive = false`) nhằm bảo toàn liên kết lịch sử giao dịch và công nợ.

---
*Cập nhật tự động nội dung tổng hợp từ 12 giai đoạn phát triển.*

## Cập nhật (từ commit 4e3ddc511d046a14659534b7c6733fc0e7160dd9 → HEAD)
- **2026-04-10 — feat(transactions): enhance transaction list with details** (08bc579): Mở rộng giao diện danh sách giao dịch hiển thị thông tin chi tiết, cập nhật `TransactionService` và `ucTransaction` để hỗ trợ hiển thị thêm cột và hành vi lọc/chi tiết.
- **2026-04-10 — feat: disallow editing of established transactions** (8917710): Khóa khả năng chỉnh sửa các giao dịch đã được xác nhận/hoàn tất; thay đổi trong `TransactionService` và `ucTransaction` để ngăn sửa phiếu đã thiết lập trạng thái.
- **Thêm** `Data Access Layer/DatabaseSchemaInitializer.cs`: tiện ích khởi tạo/cập nhật schema cơ sở dữ liệu tại khởi động (được thêm để hỗ trợ môi trường phát triển cục bộ).
- **Cập nhật** `Business Logic Layer/Services/DebtService.cs`, `Business Logic Layer/Services/TransactionService.cs`, `Graphical User Interface/ucDebt.cs`, `Graphical User Interface/ucTransaction.cs`, và `Program.cs`.
- **Cập nhật cơ sở dữ liệu cục bộ**: `Database/CashFlowDB.mdf` và `Database/CashFlowDB_log.ldf` (tập tin DB đã thay đổi — lưu ý khi đồng bộ môi trường).

Hướng dẫn ngắn:
- Rebuild solution và chạy ứng dụng để kiểm tra thay đổi UI.
- Nếu chạy lần đầu sau pull, kiểm tra `DatabaseSchemaInitializer` để đảm bảo schema đã được áp dụng hoặc chạy migration tương ứng.

-- Cập nhật tự động được sinh từ lịch sử commit phạm vi yêu cầu.
