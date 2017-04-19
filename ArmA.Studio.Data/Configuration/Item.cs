using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.Configuration
{
    public abstract class Item : ConfigurationBaseClass
    {
        /// <summary>
        /// The ToolTip to display when hovering the <see cref="Item"/>
        /// </summary>
        public string ToolTip { get; set; }
        /// <summary>
        /// The <see cref="DataTemplate"/> to use to display and edit <see cref="Value"/>.
        /// </summary>
        public DataTemplate Template { get; private set; }

        /// <summary>
        /// Value copy of this <see cref="Item"/>
        /// </summary>
        public object Value
        {
            get { return this._Value; }
            set
            {
                if (this._Value != null && this._Value.Equals(value))
                    return;
                if (this.IsValidValue(value))
                    this._Value = this.OnValueChange(this._Value, value);
                this.NotifyPropertyChanged();
            }
        }
        private object _Value;

        /// <summary>
        /// Creates a new <see cref="Item"/> instance.
        /// </summary>
        /// <param name="dt">The <see cref="DataTemplate"/> this <see cref="Item"/> should use.</param>
        /// <param name="value">The initial value of this <see cref="Item"/>.</param>
        public Item(DataTemplate dt, object value)
        {
            this.Template = dt;
            this._Value = value;
        }

        /// <summary>
        /// Callen whenever validation of a new value has to be done.
        /// Never represents current value.
        /// </summary>
        /// <param name="value">A value that has to be checked for validity.</param>
        /// <returns><see cref="true"/> if value is valid for this <see cref="Item"/>, false in any other case.</returns>
        public abstract bool IsValidValue(object value);
        /// <summary>
        /// Callback to provide new value for value change.
        /// Will be callen after value was validated.
        /// Determines the final value.
        /// </summary>
        /// <param name="oldValue">The old value that was used.</param>
        /// <param name="newValue">The new value that was passed.</param>
        /// <returns>The new value that will be set.</returns>
        public abstract object OnValueChange(object oldValue, object newValue);

        /// <summary>
        /// Loads given type <typeparamref name="T"/> from provided <paramref name="path"/>.
        /// Will throw <see cref="System.IO.FileNotFoundException"/> in case of the <paramref name="path"/> being not existing.
        /// Will throw <see cref="InvalidCastException"/> in case of the item behind <paramref name="path"/> is not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="class"/> type which will be loaded from <paramref name="assembly"/>.</typeparam>
        /// <param name="assembly">The <see cref="System.Reflection.Assembly"/> where to look the <paramref name="path"/> up.</param>
        /// <param name="path">Path to the xaml file. 'Example: ArmA.Studio.Data.Configuration.StringItem.xaml' </param>
        /// <returns></returns>
        protected static T LoadFromEmbeddedResource<T>(System.Reflection.Assembly assembly, string path) where T : class
        {
            var resNames = from name in assembly.GetManifestResourceNames() where name.EndsWith(".xaml") where name.Equals(path) select name;
            foreach (var res in resNames)
            {
                var resSplit = res.Split('.');
                var header = resSplit[resSplit.Count() - 2];
                using (var stream = assembly.GetManifestResourceStream(res))
                {
                    try
                    {
                        var obj = System.Windows.Markup.XamlReader.Load(stream);
                        if (!(obj is T))
                        {
                            throw new InvalidCastException();
                        }
                        return obj as T;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            throw new System.IO.FileNotFoundException();
        }
    }
}
