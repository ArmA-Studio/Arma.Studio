using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Arma.Studio.Data
{
    public static class IAppExtensions
    {
        public static Dispatcher GetDispatcher(this IApp app) => ((Application)app).Dispatcher;
    }
}
