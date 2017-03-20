using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace ArmA.Studio.Data.Configuration
{
    public class ConfigEntry : INotifyPropertyChanged
    {
        public enum EEditTemplate
        {
            String,
            Color
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                    var old = this._Value;
                    this._Value = this.Converter != null ? this.Converter.ConvertFrom(value) : value;
                    this.OnValueChanged(old, this._Value);
                }
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
            }
        }
        private object _Value;

        private readonly Func<object, object, bool> ValueCondition;
        private readonly TypeConverter Converter;

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"{this.Name}: {this.Value.ToString()}";

        public ConfigEntry(string root, string option) : this(root, option, null, EEditTemplate.String, (s1, s2) => true, null) { }
        public ConfigEntry(string root, string option, object value) : this(root, option, value, EEditTemplate.String, (s1, s2) => true, null) { }
        public ConfigEntry(string root, string option, object value, EEditTemplate editTemplate) : this(root, option, value, editTemplate, (s1, s2) => true, null) { }
        public ConfigEntry(string root, string option, object value, EEditTemplate editTemplate, TypeConverter converter) : this(root, option, value, editTemplate, (s1, s2) => true, converter) { }
        public ConfigEntry(string root, string option, object value, EEditTemplate editTemplate, Func<object, object, bool> valueCondition) : this(root, option, value, editTemplate, valueCondition, null) { }
        public ConfigEntry(string root, string option, object value, EEditTemplate editTemplate, Func<object, object, bool> valueCondition, TypeConverter converter)
        {
            this.Root = root;
            this.Option = option;
            this.ValueCondition = valueCondition;
            this._Value = value;
            this.Converter = converter;
            this.EditTemplate = editTemplate;
        }

        public override bool Equals(object obj)
        {
            var entry = obj as ConfigEntry;
            if (entry == null)
                return base.Equals(obj);
            return entry.Name.Equals(this.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public virtual void OnValueChanged(object oldValue, object newValue) { }
    }
}