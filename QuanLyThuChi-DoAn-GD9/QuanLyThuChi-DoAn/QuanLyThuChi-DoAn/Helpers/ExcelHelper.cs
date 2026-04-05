using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace QuanLyThuChi_DoAn.Helpers
{
    public static class ExcelHelper
    {
        /// <summary>
        /// Hàm Generic xuất List bất kỳ ra file Excel với định dạng đẹp mắt
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của List</typeparam>
        /// <param name="data">Danh sách dữ liệu cần xuất</param>
        /// <param name="sheetName">Tên Sheet (vd: "Bao Cao")</param>
        /// <param name="defaultFileName">Tên file mặc định khi lưu</param>
        /// <param name="reportTitle">Tiêu đề in to ở đầu trang (Tùy chọn)</param>
        public static void ExportListToExcel<T>(List<T> data, string sheetName, string defaultFileName, string reportTitle = "")
        {
            // 1. Kiểm tra dữ liệu đầu vào
            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Mở hộp thoại cho phép người dùng chọn nơi lưu file
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = defaultFileName;
                sfd.Title = "Lưu báo cáo Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Khởi tạo thư viện ClosedXML
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add(sheetName);
                            int currentRow = 1;

                            // 3. Xử lý in Tiêu đề báo cáo (Nếu có)
                            if (!string.IsNullOrEmpty(reportTitle))
                            {
                                var titleCell = worksheet.Cell(currentRow, 1);
                                titleCell.Value = reportTitle.ToUpper(); // Viết hoa toàn bộ
                                titleCell.Style.Font.Bold = true;
                                titleCell.Style.Font.FontSize = 16;
                                titleCell.Style.Font.FontColor = XLColor.DarkBlue;

                                // Merge (Gộp) 5 cột đầu tiên lại để tiêu đề nằm rộng ra
                                worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                                currentRow += 2; // Cách xuống 2 dòng cho thoáng trước khi vẽ bảng
                            }

                            // 4. Tuyệt chiêu InsertTable: Tự động đổ dữ liệu và tạo bảng Excel
                            var dataCell = worksheet.Cell(currentRow, 1);
                            var table = dataCell.InsertTable(data);
                            table.Name = "ReportTable";

                            // Áp dụng Theme Excel có sẵn (TableStyleMedium2 là viền xanh dương xen kẽ trắng cực đẹp)
                            table.Theme = XLTableTheme.TableStyleMedium2;

                            // 5. Căn chỉnh độ rộng cột tự động (Auto-fit) cho vừa khít với chữ
                            worksheet.Columns().AdjustToContents();

                            // 6. Lưu file vào đường dẫn người dùng đã chọn
                            workbook.SaveAs(sfd.FileName);
                        }

                        // 7. Trải nghiệm UX: Hỏi xem có muốn mở file lên xem ngay không
                        var result = MessageBox.Show("Xuất báo cáo Excel thành công!\nBạn có muốn mở file ngay bây giờ không?",
                                                     "Hoàn tất", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            // Lệnh gọi Windows mở file bằng ứng dụng mặc định (MS Excel)
                            Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                        }
                    }
                    catch (IOException) // Bắt đúng lỗi file đang bị khóa
                    {
                        MessageBox.Show("Không thể lưu file!\nNguyên nhân: File này đang được mở bởi một chương trình khác (có thể là Excel).\nVui lòng đóng file đó lại và thử xuất lại.",
                                        "Lỗi File Đang Mở", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi hệ thống khi xuất Excel: " + ex.Message,
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
