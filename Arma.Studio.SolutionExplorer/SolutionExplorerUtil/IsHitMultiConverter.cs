using Arma.Studio.Data.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arma.Studio.SolutionExplorer.SolutionExplorerUtil
{
    public class IsHitMultiConverter : IMultiValueConverter
    {
        private static bool RecursiveCheck(FileFolderBase ffb, string search)
        {
            if (ffb.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                return true;
            }
            else if (ffb is Folder folder)
            {
                return folder.Any((child) => RecursiveCheck(child, search));
            }
            else if (ffb is PBO pbo)
            {
                return pbo.Any((child) => RecursiveCheck(child, search));
            }
            else
            {
                return false;
            }
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values == null || values.Length != 2 ||
                !(values[0] is string search) ||
                String.IsNullOrWhiteSpace(search) ||
                !(values[1] is FileFolderBase ffb) ||
                RecursiveCheck(ffb, search);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
