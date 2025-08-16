using System.Globalization;
using System.Text;

namespace Bases.WorkView.ApiRouter.ApiService.Utilities
{
    public static class Utility_Extensions
    {
        public static string SanitizeUrl(this string strThis)
        {
            if (strThis == null)
                return null;
            strThis = strThis.Trim().Replace(" ","");           

            var sb = new StringBuilder();

            foreach (char c in strThis.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
