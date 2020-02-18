using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Log
{
    public class Logger
    {
        public IPlugin RelatedPlugin { get; }

        public Logger(ILogger relatedLogger)
        {
            // ToDo: Localize
            Contract.Assert(relatedLogger is IPlugin, "ILogger has to also implement IPlugin.");
            this.RelatedPlugin = relatedLogger as IPlugin;
        }

        public event EventHandler<LogEventArgs> OnLog;

        public void Log(ESeverity severity, string message) => this.Log(severity, message, default);
        public void Log(ESeverity severity, string message, Exception exception)
        {
            this.OnLog?.Invoke(this.RelatedPlugin, new LogEventArgs(severity, message, exception));
        }

        public void Log(ESeverity severity, StringBuilder stringBuilder) => this.Log(severity, stringBuilder.ToString(), default);
        public void Log(ESeverity severity, StringBuilder stringBuilder, Exception exception) => this.Log(severity, stringBuilder.ToString(), exception);

        public void Diagnostic(string message) => this.Log(ESeverity.Diagnostic, message, default);
        public void Diagnostic(StringBuilder stringBuilder) => this.Log(ESeverity.Diagnostic, stringBuilder.ToString(), default);
        public void Trace(string message) => this.Log(ESeverity.Trace, message, default);
        public void Trace(StringBuilder stringBuilder) => this.Log(ESeverity.Trace, stringBuilder.ToString(), default);
        public void Info(string message) => this.Log(ESeverity.Info, message, default);
        public void Info(StringBuilder stringBuilder) => this.Log(ESeverity.Info, stringBuilder.ToString(), default);
        public void Warning(string message) => this.Log(ESeverity.Warning, message, default);
        public void Warning(StringBuilder stringBuilder) => this.Log(ESeverity.Warning, stringBuilder.ToString(), default);
        public void Error(string message) => this.Log(ESeverity.Error, message, default);
        public void Error(StringBuilder stringBuilder) => this.Log(ESeverity.Error, stringBuilder.ToString(), default);
        public void Error(string message, Exception exception) => this.Log(ESeverity.Error, message, exception);
        public void Error(StringBuilder stringBuilder, Exception exception) => this.Log(ESeverity.Error, stringBuilder.ToString(), exception);
    }
}
