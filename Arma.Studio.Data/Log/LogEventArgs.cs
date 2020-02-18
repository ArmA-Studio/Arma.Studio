using System;

namespace Arma.Studio.Data.Log
{
    public class LogEventArgs : EventArgs
    {
        public ESeverity Severity { get; }
        public string Message { get; }
        public Exception Exception { get; }

        internal LogEventArgs(ESeverity severity, string message, Exception exception)
        {
            this.Severity = severity;
            this.Message = message;
            this.Exception = exception;
        }
    }
}