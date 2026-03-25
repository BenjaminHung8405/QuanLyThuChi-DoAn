📂 Sprint 2: Xây dựng Repository & Logic nền tảng (BLL)
Trạng thái: ✅ Hoàn thành

Trọng tâm: Thiết lập tầng nghiệp vụ, quản lý phiên làm việc và bảo mật dữ liệu người dùng.

🎯 Mục tiêu của Sprint
Chia tách hoàn toàn logic xử lý dữ liệu khỏi giao diện người dùng.
Triển khai mẫu thiết kế Repository Pattern để tối ưu hóa code CRUD.
Xây dựng cơ chế Session Management để hỗ trợ mô hình Multi-tenant.
Đảm bảo an toàn thông tin bằng thuật toán băm mật khẩu hiện đại.

🏗 Các thành phần chính đã triển khai
1. Data Access Layer (DAL) - Nâng cao
Generic Repository: Triển khai IBaseRepository<T> và BaseRepository<T>.
Giúp giảm thiểu việc lặp lại code SQL/LINQ cho 13 thực thể.
Hỗ trợ các thao tác: GetAll, Find (với Expression), Add, Update, Delete.
Quản lý tập trung lệnh SaveChanges() để đảm bảo tính toàn vẹn dữ liệu.

2. Business Logic Layer (BLL) - Core Logic
SessionManager (Static Class): 
* Lưu trữ thông tin người dùng hiện tại: UserId, TenantId, BranchId, Role.
Là "chìa khóa" để thực hiện lọc dữ liệu Multi-tenant tự động.

UserService & Security:
Tích hợp thư viện BCrypt.Net-Next.
Logic Password Hashing: Không lưu mật khẩu thuần, chỉ lưu chuỗi băm 1 chiều.
Hàm Authenticate: Kiểm tra thông tin đăng nhập và khởi tạo Session.

Danh mục Services:
BranchService: Quản lý danh sách chi nhánh theo từng công ty.
PartnerService: Quản lý Khách hàng/Nhà cung cấp, kiểm tra ràng buộc trước khi xóa.
CategoryService: Phân loại khoản thu/chi (Hạch toán).

🛠 Thư viện mới đã sử dụng (NuGet)
BCrypt.Net-Next: Thư viện mã hóa mật khẩu tiêu chuẩn.

🚀 Cách thức hoạt động của Logic
Đăng nhập: Người dùng nhập thông tin -> UserService lấy chuỗi Hash từ DB -> Dùng BCrypt so khớp -> Nếu đúng, nạp thông tin vào SessionManager.
Truy vấn: Khi tầng GUI yêu cầu dữ liệu -> Tầng Service (BLL) gọi BaseRepository -> Tự động chèn thêm điều kiện WHERE TenantId = SessionManager.TenantId.
Lưu dữ liệu: Tầng Service tự động gán TenantId từ Session vào Object trước khi lưu, tránh việc người dùng sửa đổi ID trái phép.

📝 Bài học kinh nghiệm & Khó khăn
Khó khăn: Lỗi "Multiple cascade paths" từ Sprint 1 tiếp tục ảnh hưởng khi thực hiện các lệnh xóa (Delete) ở tầng Repository.
Giải pháp: Đã xử lý triệt để bằng cách kết hợp DeleteBehavior.Restrict trong EF Core và kiểm tra logic nghiệp vụ (ví dụ: không cho xóa Đối tác nếu đang có nợ) tại tầng Service.
Nhận xét: Việc dành thời gian xây dựng Generic Repository giúp tốc độ code các tính năng sau này nhanh hơn gấp 2-3 lần.

Tiến độ dự án: 
- [x] Sprint 1: Database & Entities
- [x] Sprint 2: Repository & BLL Logic
- [ ] Next: Sprint 3: Thiết kế Giao diện (GUI) - Login & Main Dashboard