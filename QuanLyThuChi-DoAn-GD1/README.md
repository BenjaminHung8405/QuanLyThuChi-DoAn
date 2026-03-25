```markdown
# 📊 Đồ án: Hệ thống Quản lý Dòng tiền (Cash Flow Management)

**Mô hình:** Multi-Tenant & Multi-Branch (Hỗ trợ quản lý chuỗi cơ sở)  
**Giai đoạn hiện tại:** Hoàn thành Sprint 1 (Khởi tạo Kiến trúc & Thiết kế Database)

## 🎯 Tổng quan Dự án
Hệ thống Quản lý Dòng tiền là một ứng dụng Desktop được thiết kế chuyên sâu cho các mô hình kinh doanh dạng chuỗi (Ví dụ: hệ thống chuỗi nhà hàng chay gia đình, cửa hàng bán lẻ đa cơ sở...). 

Điểm nổi bật của hệ thống là khả năng quản lý tài chính độc lập cho từng chi nhánh (Branch) và từng quỹ tiền (Tiền mặt, Ngân hàng), đồng thời hỗ trợ theo dõi chi tiết công nợ khách hàng/nhà cung cấp và lưu vết mọi biến động tài chính.

## 🛠 Công nghệ & Kiến trúc
* **Nền tảng:** C# .NET (Windows Forms Application)
* **Cơ sở dữ liệu:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First Approach)
* **Kiến trúc:** Logical 3-Tier Architecture (Chia 3 lớp vật lý trong cùng 1 Project)
  * `DAL` (Data Access Layer): Chứa `AppDbContext` và 13 class Entities.
  * `BLL` (Business Logic Layer): Xử lý nghiệp vụ, tính toán tiền tệ (Sắp triển khai).
  * `GUI` (Graphical User Interface): Giao diện hiển thị với người dùng (Sắp triển khai).

## 🗄 Cấu trúc Cơ sở dữ liệu (Database Schema)
Hệ thống bao gồm 13 bảng cốt lõi, được chuẩn hóa theo tiêu chuẩn kế toán:
1. **Tenants & Branches:** Quản lý thông tin hệ thống và các chi nhánh trực thuộc.
2. **Roles, Permissions, RolePermissions, Users:** Phân quyền linh hoạt (SuperAdmin, BranchManager, Staff...).
3. **CashFunds:** Quản lý các nguồn tiền thực tế (Két sắt, Tài khoản ngân hàng).
4. **Partners & Debts:** Sổ quản lý Đối tác và chi tiết Công nợ (Phải thu / Phải trả).
5. **TransactionCategories & Transactions:** Lõi của hệ thống, ghi nhận dòng tiền Thu/Chi thực tế. Liên kết chặt chẽ với sổ công nợ.
6. **TransactionAttachments:** Lưu trữ hình ảnh hóa đơn, chứng từ.
7. **AuditLogs:** Lưu vết mọi thao tác Thêm/Sửa/Xóa của người dùng để đối soát.

## 🚀 Hướng dẫn Cài đặt & Khởi chạy (Dành cho Lập trình viên)

### Yêu cầu hệ thống:
* Visual Studio 2022 (hoặc tương đương) có hỗ trợ .NET desktop development.
* SQL Server Management Studio (SSMS) / SQL Server Express.

### Các bước khởi tạo Database:
**Bước 1:** Clone project về máy và mở file Solution (`CashFlowManagement.sln`).

**Bước 2:** Cấu hình chuỗi kết nối (Connection String)
* Mở file `DAL/AppDbContext.cs`.
* Tìm đến phương thức `OnConfiguring` và cập nhật tên Server SQL của bạn:
  ```csharp
  string connectionString = @"Server=.\SQLEXPRESS;Database=CashFlowDB;Trusted_Connection=True;TrustServerCertificate=True;";
  ```

**Bước 3:** Chạy lệnh Migration để sinh Database
* Mở cửa sổ **Package Manager Console** trong Visual Studio (`View` -> `Other Windows` -> `Package Manager Console`).
* Chạy lần lượt 2 lệnh sau:
  ```powershell
  Add-Migration InitialCreate
  Update-Database
  ```
* Kiểm tra SQL Server, database `CashFlowDB` với 13 bảng đã sẵn sàng để sử dụng.

## ⚠️ Known Issues & Troubleshooting (Giải quyết sự cố)
* **Lỗi "Multiple cascade paths" khi Update-Database:** Lỗi này xảy ra do bảng `Transactions` và `Debts` có quá nhiều khóa ngoại tham chiếu chéo. Đã được khắc phục triệt để bằng cách sử dụng Fluent API trong `OnModelCreating` để thiết lập `DeleteBehavior.Restrict` (Chặn xóa dây chuyền), đảm bảo an toàn tuyệt đối cho dữ liệu tài chính.
* **Lỗi Exception "OAProject is not marked as serializable":** Gây ra do xung đột phiên bản Entity Framework cũ. Xử lý bằng cách gỡ bỏ package `EntityFramework` cũ và cài đặt bộ 3 thư viện chuẩn của `Microsoft.EntityFrameworkCore` (SqlServer, Tools, Design).
```