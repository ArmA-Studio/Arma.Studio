using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public interface IApp
    {
        IMainWindow MainWindow { get; }
        void DisplayOperationFailed(Exception ex);
        void DisplayOperationFailed(Exception ex, string body);
    }
}
