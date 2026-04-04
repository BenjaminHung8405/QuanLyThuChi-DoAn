4.8 Giai đoạn 8: Thống kê & Báo cáo cấp Chi nhánh (Dashboard)
4.8.1 Minh chứng giai đoạn
Giao diện: Ảnh chụp màn hình ucDashboard hiển thị theo phong cách Card-based hiện đại, chia làm 3 vùng rõ rệt: Thanh lọc thời gian (Filter), Thẻ chỉ số tổng quan (KPI Cards), và Vùng phân tích (Biểu đồ Doughnut và ListView chi tiết). Các chỉ số tiền tệ được tự động định dạng chuẩn N0 VNĐ (ví dụ: 1,000,000 VNĐ).

Cơ sở dữ liệu: Không có thay đổi về Schema hay phát sinh Migration mới trong giai đoạn này, đảm bảo an toàn tuyệt đối cho kiến trúc Database hiện tại. Việc thống kê hoàn toàn dựa trên các lệnh truy vấn đọc (Read-only).

4.8.2 Mô tả
Mục tiêu: Xây dựng phân hệ Thống kê & Báo cáo chuẩn ERP nhằm cung cấp góc nhìn tổng quan, trực quan về dòng tiền (Thu/Chi), công nợ và cơ cấu chi tiêu theo thời gian thực cho cấp Quản lý và Giám đốc.

Kiến trúc dữ liệu (BLL & DTOs): Áp dụng tư duy Clean Architecture để tách biệt tầng DAL và GUI thông qua bộ 3 DTO chuyên biệt (DashboardOverviewDTO, CategoryStatisticDTO, CashFlowTrendDTO). Xây dựng ReportService tích hợp các thuộc tính tính toán động (Computed properties) như NetWorth và NetCashFlow.

Truy vấn hiệu năng cao: Sử dụng kỹ thuật thực thi trì hoãn (Deferred Execution) với IQueryable và AsNoTracking(). Toàn bộ các tác vụ tính toán nặng (GroupBy, Sum) được đẩy trực tiếp xuống SQL Server thay vì kéo dữ liệu thô lên bộ nhớ RAM.

Giao diện và Tích hợp (UI): Thiết kế ucDashboard sử dụng cơ chế bất đồng bộ async/await kết hợp trạng thái "Đang tải dữ liệu...", giúp UI không bị treo khi xử lý khối lượng dữ liệu lớn. Tích hợp liền mạch màn hình này vào sự kiện RefreshCurrentView() của frmMain, cho phép tự động cập nhật biểu đồ và KPI ngay khi người dùng thay đổi ngữ cảnh BranchId trên thanh công cụ.

4.8.3 Nhận xét
Tối ưu hiệu năng & Tài nguyên: Việc sử dụng DTO giúp hạn chế tối đa việc rò rỉ dữ liệu dư thừa từ Entity ra tầng GUI. Tốc độ xuất báo cáo nhanh và tối ưu băng thông nhờ triệt tiêu hoàn toàn rủi ro lỗi "N+1 Query".

Bảo vệ tính chính xác của dữ liệu: Logic truy vấn áp dụng nghiêm ngặt bộ lọc đa ngữ cảnh (TenantId, BranchId). Đặc biệt, việc xử lý biên độ thời gian (cộng 1 ngày và trừ 1 tick để lấy chuẩn mốc 23:59:59 của ngày kết thúc) đảm bảo số liệu không bị thất thoát.

Trải nghiệm người dùng (UX): UI được thiết kế đồng bộ với các màn hình nghiệp vụ khác, giảm thiểu cảm giác "WinForms cổ điển". Biểu đồ Doughnut được cấu hình thông minh: tự động hiển thị vòng tròn xám (fallback) khi khoảng thời gian lọc không có phát sinh giao dịch, tránh để lại các vùng trống gây hiểu nhầm cho người dùng.

4.8.4 Khó khăn và Giải pháp
Khó khăn 1 (Rủi ro văng ứng dụng do dữ liệu rỗng): Trong quá trình dùng LINQ để SumAsync các giao dịch, nếu khoảng thời gian lọc không có bản ghi nào, hệ thống sẽ quăng Exception (Lỗi Null Reference).

Giải pháp: Sử dụng cơ chế ép kiểu an toàn (decimal?) ngay trong biểu thức Sum, kết hợp với toán tử ?? 0 để trả kết quả về 0 nếu dữ liệu gốc bị Null. Đồng thời thiết lập string.Empty cho các trường chuỗi trong DTO để tránh cảnh báo Nullability.

Khó khăn 2 (Xung đột Layout và Thư viện biểu đồ): Môi trường .NET mới (net10.0-windows) gặp lỗi thiếu namespace cho control Chart. Thêm vào đó, việc dùng Dock = Fill cho ListView và Chart trực tiếp vào Card gây ra hiện tượng đè chữ và vỡ bố cục khi thay đổi kích thước màn hình.

Giải pháp: Import PackageReference bản prerelease tương thích của DataVisualization để kích hoạt biểu đồ. Tái cấu trúc giao diện theo mô hình Host Panel: tạo các Panel bọc ngoài (Host) để neo giữ nội dung, tách biệt phần tiêu đề (Title/Subtitle) khỏi phần hiển thị dữ liệu. Bổ sung tính năng Auto-resize cột cho ListView để hiển thị hoàn hảo trên mọi độ phân giải.

Khó khăn 3 (Mâu thuẫn Naming Convention): Tên trường trong thiết kế mẫu không đồng nhất với cấu trúc schema thực tế (ví dụ: TransactionDate vs TransDate, biến thể TotalIncome vs Income).

Giải pháp: Rà soát và ánh xạ (Mapping) lại toàn bộ tên biến và thuộc tính LINQ khớp 100% với schema vật lý của Database, đồng bộ hóa danh pháp (naming) từ DTO cho đến BLL và GUI để code trở nên nhất quán.