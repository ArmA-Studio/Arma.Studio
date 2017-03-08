using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Utility
{
    public static class StringExtensionMethods
    {
        #region IsNumeric
        public static bool IsNumeric(this string val)
        {
            double dummy;
            return double.TryParse(val, NumberStyles.Number, CultureInfo.InvariantCulture, out dummy);
        }
        public static bool IsNumeric(this string val, CultureInfo culture)
        {
            double dummy;
            return double.TryParse(val, NumberStyles.Number, culture, out dummy);
        }
        public static bool IsNumeric(this string val, CultureInfo culture, NumberStyles style)
        {
            double dummy;
            return double.TryParse(val, style, culture, out dummy);
        }

        public static bool IsInteger(this string val)
        {
            int dummy;
            return int.TryParse(val, NumberStyles.Number, CultureInfo.InvariantCulture, out dummy);
        }
        public static bool IsInteger(this string val, CultureInfo culture)
        {
            int dummy;
            return int.TryParse(val, NumberStyles.Number, culture, out dummy);
        }
        public static bool IsInteger(this string val, CultureInfo culture, NumberStyles style)
        {
            int dummy;
            return int.TryParse(val, style, culture, out dummy);
        }
        #endregion
    }
}
