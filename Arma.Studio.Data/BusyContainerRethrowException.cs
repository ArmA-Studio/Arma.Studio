using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    /// <summary>
    /// Simple <see cref="Exception"/> container that allows
    /// catching all exceptions but this.
    /// Will only ever be used in <see cref="BusyContainerManager.Run"/>.
    /// </summary>
    [Serializable]
    public class BusyContainerRethrowException : Exception
    {
        public BusyContainerRethrowException()
        {
        }

        public BusyContainerRethrowException(string message) : base(message)
        {
        }

        public BusyContainerRethrowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusyContainerRethrowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
