using System;

namespace qcs_product.API.Helpers
{
    public class StringHelper
    {
        public static string GetMonthRomawi(DateTime date)
        {
            var onDate = date.Month;
            var romawi = "I";

            if (onDate == 2)
            {
                romawi = "II";
            }
            else if (onDate == 3)
            {
                romawi = "III";
            }
            else if (onDate == 4)
            {
                romawi = "IV";
            }
            else if (onDate == 5)
            {
                romawi = "V";
            }
            else if (onDate == 6)
            {
                romawi = "VI";
            }
            else if (onDate == 7)
            {
                romawi = "VII";
            }
            else if (onDate == 8)
            {
                romawi = "VIII";
            }
            else if (onDate == 9)
            {
                romawi = "IX";
            }
            else if (onDate == 10)
            {
                romawi = "X";
            }
            else if (onDate == 11)
            {
                romawi = "XI";
            }
            else if (onDate == 12)
            {
                romawi = "XII";
            }

            return romawi;
        }
    }
}