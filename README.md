# 📊 CashFlow ERP: Hệ thống Quản lý Dòng tiền Đa chi nhánh
## Giải pháp Quản trị Tài chính Multi-Tenant chuyên sâu cho Chuỗi cửa hàng

[![Platform](https://img.shields.io/badge/.NET-10.0-blueviolet)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/SQL%20Server-2022-red)](https://www.microsoft.com/sql-server)
[![Architecture](https://img.shields.io/badge/Architecture-3--Tier-green)](https://en.wikipedia.org/wiki/Multitier_architecture)
[![Security](https://img.shields.io/badge/Security-RBAC%20%2B%20Audit-orange)](https://en.wikipedia.org/wiki/Role-based_access_control)

### 🌟 Tổng quan dự án
**CashFlow ERP** là một hệ thống quản lý tài chính tập trung hỗ trợ mô hình **Multi-Tenant** (Đa doanh nghiệp) và **Multi-Branch** (Đa chi nhánh). Ứng dụng giúp kiểm soát dòng tiền, công nợ, quỹ tiền mặt và nhật ký giao dịch một cách minh bạch, an toàn và có khả năng truy vết tuyệt đối.

Hệ thống được thiết kế theo tư duy **SaaS (Software as a Service)**, cho phép cô lập hoàn toàn dữ liệu giữa các khách hàng (Tenant) và các cơ sở trong cùng hệ thống.

---

### 🏗 Kiến trúc & Công nghệ
| Thành phần | Công nghệ sử dụng | Ý nghĩa |
| :--- | :--- | :--- |
| **Logic 3-Tier** | C# .NET Windows Forms | Tách biệt rạch ròi DAL (Dữ liệu) - BLL (Nghiệp vụ) - GUI (Giao diện). |
| **ORM / Database** | EF Core (Code First) + SQL Server | Quản lý schema linh hoạt, hỗ trợ Migration và bảo mật dữ liệu cấp dòng. |
| **Security** | BCrypt + RBAC + Audit Logging | Mã hóa mật khẩu chuẩn quốc tế, phân quyền theo mức ưu tiên và lưu vết mọi thay đổi. |
| **Reporting** | ClosedXML + LINQ Aggregation | Xuất báo cáo Excel tốc độ cao và tính toán số dư lũy kế trực tiếp trên RAM. |

---

### 🚀 Lộ trình Phát triển 12 Giai đoạn (Sprints)

#### 🛡 Hệ thống & Bảo mật (GD1 - GD5)
*   **GD1 - GD2:** Thiết lập Data Model Multi-branch và Generic Repository. Sửa lỗi "Multiple cascade paths" bằng Fluent API.
*   **GD3 - GD4:** Module Đăng nhập & Master Data. Triển khai xóa mềm (IsActive) và Fuzzy Search tiếng Việt.
*   **GD5:** Số hóa nghiệp vụ Kế toán kép cho "Chuyển quỹ nội bộ". Xây dựng SessionManager để cô lập Tenant ngầm định.

#### 📈 Công nợ & Báo cáo (GD6 - GD9)
*   **GD6 - GD7:** Quản lý Công nợ chi tiết theo Branch. Hỗ trợ Nợ đầu kỳ và quy trình Maker/Checker (Duyệt nợ).
*   **GD8 - GD9:** Dashboard trực quan (Charts) và đối soát chuỗi chi nhánh. Tích hợp xuất báo cáo Excel "1 giây" bằng ClosedXML.

#### 💎 Nâng cấp Chuyên sâu (GD10 - GD12)
*   **GD10:** Phân cấp quyền nâng cao (`PriorityLevel`) và UX Drill-down (Bấm đúp xem chi tiết phiếu).
*   **GD11:** Hệ thống **Audit Logging** (Lưu vết Sửa/Xóa/Hủy) và **Quản lý Thuế** chuyên nghiệp.
*   **GD12:** **Chuyển quỹ 2 bước** (PENDING/COMPLETED) và cơ chế **Snapshot dữ liệu** (Giữ tên danh mục/đối tác lịch sử ngay cả khi đã bị xóa).

---

### ✨ Tính năng Nổi bật (Core Features)
1.  **Bảo mật Đa tầng:** Chặn truy cập trái phép ở cả tầng UI (ẩn menu) và tầng BLL (lọc SQL theo Tenant/Branch ID).
2.  **Snapshot Lịch sử:** Đảm bảo báo cáo năm 2024 vẫn giữ đúng tên đối tác mặc dù năm 2025 đối tác đã đổi tên hoặc ngừng hoạt động.
3.  **Running Balance:** Báo cáo sổ quỹ tự động tính số dư lũy kế từng dòng, hỗ trợ xem mode Chi tiết hoặc Tổng hợp.
4.  **Audit Trail:** Truy vết từng hành động nhạy cảm (Ai hủy phiếu? Hủy lúc nào? Lý do gì? Giá trị trước khi hủy là bao nhiêu?).
5.  **Chuyển quỹ An toàn:** Luồng chuyển tiền liên chi nhánh bắt buộc quỹ nhận phải nhấn "Xác nhận" mới được tăng số dư, tránh thất thoát dòng tiền.

---

### 📁 Cấu trúc Thư mục Dự án
*   `QuanLyThuChi-DoAn-GD[X]`: Các phiên bản lưu trữ theo từng giai đoạn phát triển.
*   `Database`: Chứa tệp tin CSDL SQL Server (`.mdf`, `.ldf`).
*   `QuanLyThuChi-DoAn/Business Logic Layer`: Chứa logic xử lý nghiệp vụ và các Services.
*   `QuanLyThuChi-DoAn/Data Access Layer`: Chứa Entity, AppDbContext và cấu hình Fluent API.
*   `QuanLyThuChi-DoAn/Graphical User Interface`: Chứa các UserControl và Form giao diện.

---

### 🛠 Hướng dẫn Cài đặt
1.  **Yêu cầu:** .NET 10 SDK, SQL Server Express.
2.  **Database:** Nếu chạy lần đầu, ứng dụng tự động khởi tạo Schema qua `DatabaseSchemaInitializer`.
3.  **Tài khoản Admin mặc định:** Admin cấp 1 (Tenant 1) thường được mặc định trong code khởi tạo để Test.

---
*Cập nhật tự động: 2026-04-13 — Trình trạng: Ổn định (Production Ready)*
