using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Log
{
    public interface ILogHandler
    {
        void SetLogCollection(LoggerCollection loggers);
    }
}
