using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public class HaltInfo
    {
        /// <summary>
        /// The reason why this halt info exists.
        /// Can be null.
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// The full content related to this halt info.
        /// Can be null.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// The line this halt info refers to.
        /// If not available, expected to be -1.
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// The column this halt info refers to.
        /// If not available, expected to be -1.
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// The arma path this halt info refers to.
        /// Required
        /// </summary>
        public string File { get; }
        /// <summary>
        /// Creates a new <see cref="HaltInfo"/> instance.
        /// </summary>
        /// <param name="armaPath">The arma path this halt info refers to. Will set the <see cref="File"/> Property.</param>
        public HaltInfo(string armaPath)
        {
            this.File = armaPath;
        }
    }
}
