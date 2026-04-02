using System;
using System.Globalization;
using System.Text;

namespace QuanLyThuChi_DoAn.BLL.Common
{
    /// <summary>
    /// Utility class for text manipulation operations
    /// </summary>
    public static class TextUtility
    {
        /// <summary>
        /// Remove Vietnamese accents from text for search and comparison
        /// </summary>
        /// <param name="text">Text input with Vietnamese accents</param>
        /// <returns>Text without accents (normalized)</returns>
        public static string RemoveVietnameseAccents(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string text = input.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString()
                     .Normalize(NormalizationForm.FormC)
                     .Replace('đ', 'd')
                     .Replace('Đ', 'D');
        }
    }
}
