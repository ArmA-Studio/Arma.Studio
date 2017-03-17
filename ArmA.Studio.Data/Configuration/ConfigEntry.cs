using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace ArmA.Studio.Data.Configuration
{
    public sealed class ConfigEntry : INotifyPropertyChanged
    {
        public enum EEditTemplate
        {
            String,
            Color
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Root { get; private set; }
        public string Option { get; private set; }
        public string Name => $"{this.Root}.{this.Option}";

        public EEditTemplate EditTemplate { get; private set; }

        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                if (this.Converter != null ? (this.Converter.CanConvertFrom(value.GetType()) && this.ValueCondition(this._Value, value)) : this.ValueCondition(this._Value, value))
                {
                    this._Value = this.Converter != null ? this.Converter.ConvertFrom(value) : value;
                }
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
            }
        }
        private object _Value;

        private readonly Func<object, object, bool> ValueCondition;
        private readonly TypeConverter Converter;

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"{this.Name}: {this.Value.ToString()}";

        public ConfigEntry(string root, string option) : this(root, option, (s1, s2) => true, null, EEditTemplate.String, null) { }
        public ConfigEntry(string root, string option, string value) : this(root, option, (s1, s2) => true, value, EEditTemplate.String, null) { }
        public ConfigEntry(string root, string option, string value, EEditTemplate editTemplate) : this(root, option, (s1, s2) => true, value, editTemplate, null) { }
        public ConfigEntry(string root, string option, string value, EEditTemplate editTemplate, TypeConverter converter) : this(root, option, (s1, s2) => true, value, editTemplate, converter) { }
        public ConfigEntry(string root, string option, Func<object, object, bool> valueCondition, string value, EEditTemplate editTemplate) : this(root, option, valueCondition, value, editTemplate, null) { }
        public ConfigEntry(string root, string option, Func<object, object, bool> valueCondition, string value, EEditTemplate editTemplate, TypeConverter converter)
        {
            this.Root = root;
            this.Option = option;
            this.ValueCondition = valueCondition;
            this.Value = value;
            this.Converter = converter;
            this.EditTemplate = editTemplate;
        }

        public override bool Equals(object obj)
        {
            var entry = obj as ConfigEntry;
            if(entry == null)
                return base.Equals(obj);
            return entry == this;
        }
    }
}