using QuanLyThuChi_DoAn.Data_Access_Layer;

namespace QuanLyThuChi_DoAn
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Helpers.AppCulture.ApplyConfiguredCulture();

            try
            {
                DatabaseSchemaInitializer.EnsureDatabaseReady();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Khong the dong bo schema CSDL: {ex.Message}\n\nUng dung van tiep tuc chay, nhung mot so man hinh bao cao co the bi loi neu DB dang thieu cot.",
                    "Canh bao CSDL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogin());
        }
    }
}