using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Log
{
    public interface ILogger
    {
        /// <summary>
        /// The name of this logtarget.
        /// </summary>
        string TargetName { get; }
        /// <summary>
        /// Method that receives the logger.
        /// Will be called before <see cref="IPlugin.Initialize(string, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="logger">The logger specifically for this very <see cref="IPlugin"/>.</param>
        void SetLogger(Logger logger);
    }
}
