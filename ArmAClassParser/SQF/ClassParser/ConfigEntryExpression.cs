using System.ComponentModel;
using RealVirtuality.Config.Parser;

namespace RealVirtuality.Config
{
    public class ConfigEntryExpression : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        private ConfigEntry ConfigBase;
        private string ExpressionPath;

        public string Value { get; set; }

        public ConfigEntryExpression(ConfigEntry it, string path)
        {
            this.ConfigBase = it;
            this.ExpressionPath = path;
        }
    }
}