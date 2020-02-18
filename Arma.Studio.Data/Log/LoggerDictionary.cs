using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data.Log
{
    public class LoggerCollection : ReadOnlyCollection<Logger>
    {
        public LoggerCollection(IList<Logger> list) : base(list)
        {
        }
    }
}
