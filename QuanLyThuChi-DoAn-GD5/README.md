
# Sprint 5: Hoàn thiện Kiến trúc Multi-tenant, Quản lý Quỹ tiền và Nghiệp vụ Kế toán lõi

## 1. Mục tiêu của Sprint
- **Kiến trúc:** Chuyển hệ thống sang mô hình SaaS (Multi-tenant) hoàn chỉnh, đảm bảo dữ liệu tách biệt giữa Công ty (Tenant) và Chi nhánh (Branch).
- **Nghiệp vụ:** Hoàn thiện module Sổ Quỹ (Cash Funds) và Sổ Giao Dịch (Transaction Ledger); số hóa nghiệp vụ "Chuyển quỹ nội bộ" theo nguyên tắc kế toán kép.
- **UX/UI:** Nâng cấp `frmMain`, tối ưu trạng thái chờ (Loading) và phân quyền hiển thị động.

## 2. Danh sách công việc đã thực hiện (Task List)
- Tái cấu trúc menu chính: chuyển từ "Thu/Chi rời rạc" sang "Sổ Giao Dịch" tập trung; thêm control chọn ngữ cảnh (`cbTenants`, `cbBranches`).
- Hoàn thiện RBAC & Context: tích hợp `SessionManager` giới hạn truy cập theo 4 cấp: SuperAdmin, Admin (Tenant Manager), Branch Manager, Staff.
- Xây dựng `ucCashFund`: hiển thị danh sách két/tài khoản, đối soát tự động và tính toán số dư.
- Xây dựng `ucTransaction`: hiển thị lịch sử dòng tiền, hỗ trợ lọc động (Dynamic Query) theo Danh mục và Đối tác.
- Phát triển `frmFundTransfer`: logic chuyển quỹ nội bộ nhân đôi bút toán (Phiếu Thu IN + Phiếu Chi OUT) đảm bảo toàn vẹn dữ liệu.
- Cải thiện UX: thêm `toolStripProgressBar1` và `panelLoadingOverlay` để chặn tương tác khi truy vấn nặng.
- Tách Service Layer: `TenantService`, `BranchService` để phân tách trách nhiệm.

## 3. Mô tả kỹ thuật
- Dữ liệu Multi-tenant: cho phép `TenantId` và `BranchId` nullable để định nghĩa tài khoản SuperAdmin (quản lý chéo).
- Truy vấn động: sử dụng `IQueryable` trong Entity Framework để nối chuỗi `Where` chỉ khi filter được chọn (Dynamic Query), tối ưu hiệu năng.
- Toàn vẹn giao dịch: Chuyển quỹ nội bộ tạo đồng thời 2 record liên quan (IN + OUT), liên kết qua `TransferRefId` và thực thi atomically bằng một `_context.SaveChanges()`.
- Tách tầng BLL/GUI: mọi logic lọc theo Tenant/Branch và kiểm tra quyền nằm trong BLL; GUI chỉ hiển thị và gọi service.

## 4. Thu hoạch (Key Takeaways)
- Hiểu sâu thiết kế CSDL Multi-tenant: thiết lập khóa ngoại và điều hướng dữ liệu theo Context thay vì chỉ dựa vào quyền User.
- Nắm vững nguyên lý Kế toán Kép (Double-entry): áp dụng vào chuyển quỹ nội bộ để không ảnh hưởng sai lệch báo cáo Doanh thu/Chi phí (CategoryId tách riêng: 98 = OUT, 99 = IN).
- Thông thạo EF & LINQ: quản lý state object, dùng Data Annotations (`[ForeignKey]`, `[Table]`) và xử lý lỗi ràng buộc.
- Tư duy UI/UX thực dụng: áp dụng Loading Overlay và kiểm tra thụ động để UX mượt mà.

## 5. Khó khăn & Cách khắc phục
- SqlNullValueException cho SuperAdmin: cho phép `TenantId/BranchId` NULL ở DB và dùng `int?` trong model; kiểm tra `HasValue` ở logic.
- Xung đột FK khi seed dữ liệu: áp dụng quy trình "Xóa ngược — Nạp xuôi" và rà soát dependency giữa bảng trước khi seed.
- Nhầm lẫn Category khi chuyển quỹ: tách 2 danh mục đối ứng (ID 98: OUT, ID 99: IN) để loại trừ khỏi tính toán lợi nhuận.

## 6. Minh chứng (Evidence / Deliverables)
- Giao diện: `frmMain` thay đổi toolbar/ComboBox theo quyền; Loading Bar và overlay hoạt động khi truy vấn nặng.
- Mã nguồn: Service Layer (`TenantService.cs`, `BranchService.cs`) tách biệt logic.
- Database: Giao dịch chuyển quỹ nội bộ tạo 2 dòng `Transactions` cùng `TransferRefId`, `IsActive = 1`; số dư 2 quỹ trong `CashFunds` thay đổi đối nghịch chính xác.

## 7. Nhận xét (Self-Evaluation)
- Tiến độ: Hoàn thành 100% mục tiêu Sprint 5 — nền tảng xử lý tiền tệ, multi-tenant và nghiệp vụ chuyển quỹ đã ổn định.
- Bước tiếp theo: Dùng dữ liệu giao dịch hiện có để triển khai Dashboard trực quan, thống kê và xuất báo cáo (Sprint tiếp theo).

---

Nếu bạn muốn, tôi có thể commit thay đổi này vào Git hoặc rà soát lại ngôn từ/bố cục tiếp.

