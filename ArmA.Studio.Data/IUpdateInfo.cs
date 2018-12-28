using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public interface IUpdateInfo
    {
        bool Available { get; }
        Task DoUpdate(string pluginLocation, string tempDirectory);
    }
}
