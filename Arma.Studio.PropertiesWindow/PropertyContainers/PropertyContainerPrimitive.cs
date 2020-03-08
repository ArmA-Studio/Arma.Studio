using System;
using System.Reflection;
namespace Arma.Studio.PropertiesWindow.PropertyContainers
{
    public class PropertyContainerByte : PropertyContainerBase
    {
		public PropertyContainerByte(string title, string tooltip, object data, string propertyName, Func<object, Byte> getFunc, Action<object, Byte> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Byte)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerByte"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerByte Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerByte(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Byte)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerSByte : PropertyContainerBase
    {
		public PropertyContainerSByte(string title, string tooltip, object data, string propertyName, Func<object, SByte> getFunc, Action<object, SByte> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (SByte)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerSByte"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerSByte Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerSByte(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (SByte)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerInt32 : PropertyContainerBase
    {
		public PropertyContainerInt32(string title, string tooltip, object data, string propertyName, Func<object, Int32> getFunc, Action<object, Int32> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Int32)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerInt32"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerInt32 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerInt32(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Int32)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerUInt32 : PropertyContainerBase
    {
		public PropertyContainerUInt32(string title, string tooltip, object data, string propertyName, Func<object, UInt32> getFunc, Action<object, UInt32> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (UInt32)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerUInt32"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerUInt32 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerUInt32(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (UInt32)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerInt16 : PropertyContainerBase
    {
		public PropertyContainerInt16(string title, string tooltip, object data, string propertyName, Func<object, Int16> getFunc, Action<object, Int16> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Int16)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerInt16"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerInt16 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerInt16(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Int16)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerUInt16 : PropertyContainerBase
    {
		public PropertyContainerUInt16(string title, string tooltip, object data, string propertyName, Func<object, UInt16> getFunc, Action<object, UInt16> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (UInt16)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerUInt16"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerUInt16 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerUInt16(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (UInt16)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerInt64 : PropertyContainerBase
    {
		public PropertyContainerInt64(string title, string tooltip, object data, string propertyName, Func<object, Int64> getFunc, Action<object, Int64> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Int64)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerInt64"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerInt64 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerInt64(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Int64)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerUInt64 : PropertyContainerBase
    {
		public PropertyContainerUInt64(string title, string tooltip, object data, string propertyName, Func<object, UInt64> getFunc, Action<object, UInt64> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (UInt64)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerUInt64"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerUInt64 Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerUInt64(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (UInt64)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerSingle : PropertyContainerBase
    {
		public PropertyContainerSingle(string title, string tooltip, object data, string propertyName, Func<object, Single> getFunc, Action<object, Single> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Single)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerSingle"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerSingle Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerSingle(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Single)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerDouble : PropertyContainerBase
    {
		public PropertyContainerDouble(string title, string tooltip, object data, string propertyName, Func<object, Double> getFunc, Action<object, Double> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Double)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerDouble"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerDouble Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerDouble(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Double)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerChar : PropertyContainerBase
    {
		public PropertyContainerChar(string title, string tooltip, object data, string propertyName, Func<object, Char> getFunc, Action<object, Char> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Char)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerChar"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerChar Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerChar(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Char)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerBoolean : PropertyContainerBase
    {
		public PropertyContainerBoolean(string title, string tooltip, object data, string propertyName, Func<object, Boolean> getFunc, Action<object, Boolean> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Boolean)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerBoolean"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerBoolean Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerBoolean(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Boolean)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerString : PropertyContainerBase
    {
		public PropertyContainerString(string title, string tooltip, object data, string propertyName, Func<object, String> getFunc, Action<object, String> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (String)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerString"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerString Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerString(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (String)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
    public class PropertyContainerDecimal : PropertyContainerBase
    {
		public PropertyContainerDecimal(string title, string tooltip, object data, string propertyName, Func<object, Decimal> getFunc, Action<object, Decimal> setFunc) : 
            base(title, tooltip, data, propertyName, (obj) => getFunc(obj), (obj, val) => setFunc(obj, (Decimal)val)) {}

        /// <summary>
        /// Creates a new <see cref="PropertyContainerDecimal"/>.
        /// </summary>
        /// <param name="data">The object that relates to the <paramref name="propertyInfo"/>.</param>
        /// <param name="propertyInfo">The property that is supposed to be represented.</param>
        /// <returns></returns>
		public static PropertyContainerDecimal Create(object data, PropertyInfo propertyInfo)
		{
            var attribute = (Data.UI.PropertyAttribute)propertyInfo.GetCustomAttribute(typeof(Data.UI.PropertyAttribute), true);
            if (attribute is null)
            {
                throw new ArgumentException("Missing Arma.Studio.Data.UI.PropertyAttribute.", nameof(propertyInfo));
            }
            return new PropertyContainerDecimal(attribute.Title, attribute.Description, data, propertyInfo.Name, (obj) => (Decimal)propertyInfo.GetValue(obj, null), (obj, val) => propertyInfo.SetValue(obj, val, null));
		}
    }
}