using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Data;

namespace Arma.Studio.Data.UI.Converters
{
    /// <summary>
    /// Converts the passed <see cref="Enum"/> value to the corresponding <see cref="String"/> representing its name.
    /// Requires the parameter to be the <see cref="Type"/> of the enum to convert.
    /// Can convert backwards.
    /// </summary>
    public class EnumNameConverter : IValueConverter
    {
        public static EnumNameConverter Instance { get => _Instance; set => _Instance = value; }
        private static EnumNameConverter _Instance;
        public static Dictionary<Assembly, ResourceManager> ResourceManagerCache { get; set; }

        static EnumNameConverter()
        {
            _Instance = new EnumNameConverter();
            ResourceManagerCache = new Dictionary<Assembly, ResourceManager>();
        }
        public string Convert(Type enumType, object enumValue)
        {
            return this.Convert(enumType, enumValue, CultureInfo.CurrentUICulture);
        }
        public string Convert(Type enumType, object enumValue, CultureInfo cultureInfo)
        {
            return (string)this.Convert(enumValue, typeof(string), enumType, cultureInfo);
        }
        /// <summary>
        /// Converts a value
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is Type t))
            {
                if (value == null)
                {
                    return String.Empty;
                }
                t = value.GetType();
            }
            if (t.IsEnum)
            {
                string name = Enum.GetName(t, value);
                var fieldInfo = t.GetField(name);
                if (fieldInfo == null)
                {
                    return name;
                }
                var atts = fieldInfo.GetCustomAttributes(typeof(EnumNameAttribute), false).Cast<EnumNameAttribute>();
                var att = atts.FirstOrDefault();
                if (att == null)
                {
                    return name;
                }
                else if (String.IsNullOrWhiteSpace(att.ResourceName))
                {
                    return att.FallbackName;
                }
                else
                {
                    if (!ResourceManagerCache.TryGetValue(t.Assembly, out var resourceManager))
                    {
                        string[] resourceNames = t.Assembly.GetManifestResourceNames();
                        foreach (var it in resourceNames
                            .Where((it) => it.EndsWith(".resources"))
                            .Select((it) => new ResourceManager(it.Substring(0, it.Length - ".resources".Length), t.Assembly)))
                        {
                            if (!String.IsNullOrWhiteSpace(it.GetString(att.ResourceName, culture)))
                            {
                                resourceManager = it;
                                break;
                            }
                            it.ReleaseAllResources();
                        }
                        if (resourceManager == null)
                        {
                            return att.FallbackName;
                        }
                        ResourceManagerCache[t.Assembly] = resourceManager;
                    }
                    string res = resourceManager.GetString(att.ResourceName, culture);
                    return String.IsNullOrWhiteSpace(res) ? att.FallbackName : res;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is Type t))
            {
                t = targetType;
            }
            if (t.IsEnum && value is string name)
            {
                foreach (var fieldInfo in t.GetFields())
                {
                    if (fieldInfo == null)
                    {
                        continue;
                    }
                    var atts = fieldInfo.GetCustomAttributes(typeof(EnumNameAttribute), false).Cast<EnumNameAttribute>();
                    if (atts.FirstOrDefault()?.FallbackName == name)
                    {
                        return fieldInfo.GetValue(null);
                    }
                }
                // ToDo: Implement reverse lookup of names
                return Enum.Parse(t, name);
            }
            else
            {
                return null;
            }
        }
    }
}
