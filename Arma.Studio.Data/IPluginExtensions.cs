using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public static class IPluginExtensions
    {
        public static IApp GetApplication(this IPlugin plugin) => (IApp)System.Windows.Application.Current;
    }
}
