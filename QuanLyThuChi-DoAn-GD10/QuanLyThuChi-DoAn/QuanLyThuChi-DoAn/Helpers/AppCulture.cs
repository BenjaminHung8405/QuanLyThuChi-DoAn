using System;
using System.Configuration;
using System.Globalization;
using System.Threading;

namespace QuanLyThuChi_DoAn.Helpers
{
    public static class AppCulture
    {
        private const string AppCultureKey = "AppCulture";
        private const string DefaultCultureName = "vi-VN";
        private static readonly object SyncRoot = new object();
        private static CultureInfo? _configuredCulture;

        public static CultureInfo GetConfiguredCulture()
        {
            if (_configuredCulture != null)
            {
                return _configuredCulture;
            }

            lock (SyncRoot)
            {
                if (_configuredCulture == null)
                {
                    _configuredCulture = ResolveCultureFromConfig();
                }

                return _configuredCulture;
            }
        }

        public static void ApplyConfiguredCulture()
        {
            var culture = GetConfiguredCulture();

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private static CultureInfo ResolveCultureFromConfig()
        {
            try
            {
                string? configuredName = ConfigurationManager.AppSettings[AppCultureKey];
                if (!string.IsNullOrWhiteSpace(configuredName))
                {
                    return CultureInfo.GetCultureInfo(configuredName.Trim());
                }
            }
            catch (Exception)
            {
                // Fallback to default culture when config is missing/invalid.
            }

            return CultureInfo.GetCultureInfo(DefaultCultureName);
        }
    }
}
