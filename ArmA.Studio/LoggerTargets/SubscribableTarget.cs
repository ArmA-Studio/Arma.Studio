using System;
using NLog;
using NLog.Targets;

namespace ArmA.Studio.LoggerTargets
{
    [Target("SubscribableTarget")]
    public sealed class SubscribableTarget : TargetWithLayout
    {
        public sealed class OnLogEventArgs : EventArgs
        {
            public string Logger { get; private set; }
            public string Severity { get; private set; }
            public string Message { get; private set; }
            public OnLogEventArgs(string logger, string severity, string message)
            {
                this.Logger = logger;
                this.Severity = severity;
                this.Message = message;
            }
        }

        public SubscribableTarget()
        {
            this.Name = "SubscribableTarget";
            this.Layout = "${logger}|${pad:padding=5:inner=${level:uppercase=true}} ${message}";
        }
        public event EventHandler<OnLogEventArgs> OnLog;
        protected override void Write(LogEventInfo logEvent)
        {
            if (this.OnLog == null)
                return;
            if (logEvent.Level < LogLevel.Info)
                return;
            // this.Layout.Render(logEvent)
            this.OnLog(this, new OnLogEventArgs(logEvent.LoggerName, logEvent.Level.Name, logEvent.Message));
        }

    }
}