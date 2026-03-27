```markdown
# 🚀 Sprint 4: Quản lý Dữ liệu Nền tảng (Master Data Management)

## 📌 Tổng quan
Sprint 4 tập trung vào việc xây dựng nền móng dữ liệu từ điển (Master Data) cho hệ thống Quản lý Thu/Chi. Hai module cốt lõi được hoàn thiện trong Sprint này là **Quản lý Đối tác (Partners)** và **Quản lý Danh mục Thu/Chi (Transaction Categories)**. 

Dữ liệu từ hai module này sẽ là tiền đề bắt buộc để thực hiện các nghiệp vụ tạo phiếu thu/chi ở các Sprint tiếp theo.

## ✨ Chức năng cốt lõi (Features)
* **Quản lý Đối tác (`ucPartner`):** Thêm, sửa, xóa, tìm kiếm khách hàng và nhà cung cấp. Phân loại hiển thị trực quan.
* **Quản lý Danh mục (`ucTransactionCategory`):** Thiết lập các khoản thu (IN) và khoản chi (OUT) cố định hoặc phát sinh.

## 🛠 Điểm nhấn Kỹ thuật (Technical Highlights)
Dự án áp dụng mô hình 3 lớp (3-Tier Architecture) với Entity Framework Core, tập trung mạnh vào bảo mật và trải nghiệm người dùng (UX):

1. **Kiến trúc Multi-tenancy (Cô lập dữ liệu):**
   * Tích hợp `TenantId` thông qua `SessionManager` vào mọi thao tác CRUD.
   * Ngăn chặn triệt để lỗ hổng IDOR/BOLA: Dữ liệu chi nhánh này được bảo mật hoàn toàn khỏi chi nhánh khác (`.Where(c => c.TenantId == tenantId)`).

2. **Xóa mềm an toàn (Soft Delete):**
   * Thay vì xóa vĩnh viễn (Hard Delete), hệ thống sử dụng cờ `IsActive = false`. 
   * Bảo toàn tính toàn vẹn của dữ liệu lịch sử và các ràng buộc khóa ngoại (Foreign Keys).

3. **Tối ưu hóa Trải nghiệm Người dùng (UI/UX):**
   * **Auto-Edit on Select:** Click vào dòng dữ liệu trên Grid để tự động mở khóa (Enable) các ô nhập liệu, giảm bớt thao tác bấm nút "Sửa".
   * **State Matrix Logic:** Quản lý chặt chẽ trạng thái các nút bấm (`btnSave`, `btnDelete`) để ngăn chặn lỗi logic và chống nảy phím (bounce) khi thao tác nhanh.
   * **Color Coding:** Tự động định dạng màu sắc dữ liệu trên Grid (Xanh cho Khoản Thu, Đỏ cho Khoản Chi).
   * **Chuẩn hóa Dữ liệu (Data Normalization):** Tách biệt tầng hiển thị ("Khoản Thu") và tầng lưu trữ (Lưu mã "IN"/"OUT" vào Database để tối ưu hiệu suất và đa ngôn ngữ).

4. **Tìm kiếm Tiếng Việt thông minh (Fuzzy Search):**
   * Tích hợp `TextUtility.RemoveVietnameseAccents()` giúp người dùng tìm kiếm không phân biệt dấu (VD: gõ "rau cu" vẫn match với "Rau Củ").

## 📂 Cấu trúc File chính
* `BLL/Services/PartnerService.cs` & `TransactionCategoryService.cs`: Chứa logic nghiệp vụ và truy xuất DB.
* `GUI/ucPartner.cs` & `ucTransactionCategory.cs`: Giao diện người dùng (User Controls).
* `Entities/Partner.cs` & `TransactionCategory.cs`: Các Model Entity Framework.

## ⚙️ Hướng dẫn Kiểm thử (Testing Setup)
Để kiểm thử trọn vẹn Sprint 4, cần đảm bảo Database đã được đồng bộ cờ `IsActive`. Chạy lệnh SQL sau nếu dữ liệu cũ không hiển thị:
```sql
UPDATE Partners SET IsActive = 1;
UPDATE TransactionCategories SET IsActive = 1;
```

## ⏭️ Bước tiếp theo (Next Steps)
* Hoàn tất Sprint 4.
* **Tiến hành Sprint 5:** Phát triển phân hệ Lập Phiếu Thu/Chi (Transactions), kết nối dữ liệu từ Đối tác và Danh mục.
```