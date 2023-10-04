using System;

namespace qcs_product.API.Helpers
{
    public class DateHelper
    {
        public static DateTime Now()
        {
            return DateTime.UtcNow.AddHours(7);
        }
        
        public static string ToStr(DateTime? date, string format = null)
        {
            if (!date.HasValue)
            {
                return "";
            }
            
            try
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = "dd MMM yyyy HH:mm:ss";
                }
                return date.Value.ToString(format);
            }
            catch (Exception)
            {
                // ignored
            }

            return "";
        }
    }
}