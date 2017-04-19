
//ToDo: Fix floating point number TextBox input

using System;
using System.Reflection;
using System.Windows;
using System.Linq;
namespace ArmA.Studio.Data.Configuration
{
    public class ByteItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(ByteItem).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Byte, bool> IsValidFunction;

        public ByteItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public ByteItem(string name, string path, object propertyOwner, Func<Byte, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public ByteItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public ByteItem(string name, PropertyInfo property, object propertyOwner, Func<Byte, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public ByteItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public ByteItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<Byte, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Byte && this.IsValidFunction((Byte)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Byte)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Byte)
                return true;
            if (value is string)
            {
                Byte v;
                return Byte.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Byte)
                return value;
            return Byte.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class SByteItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(SByteItem).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<SByte, bool> IsValidFunction;

        public SByteItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public SByteItem(string name, string path, object propertyOwner, Func<SByte, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public SByteItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public SByteItem(string name, PropertyInfo property, object propertyOwner, Func<SByte, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public SByteItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public SByteItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<SByte, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is SByte && this.IsValidFunction((SByte)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((SByte)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is SByte)
                return true;
            if (value is string)
            {
                SByte v;
                return SByte.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is SByte)
                return value;
            return SByte.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class Int16Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(Int16Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Int16, bool> IsValidFunction;

        public Int16Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public Int16Item(string name, string path, object propertyOwner, Func<Int16, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public Int16Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public Int16Item(string name, PropertyInfo property, object propertyOwner, Func<Int16, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public Int16Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public Int16Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<Int16, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Int16 && this.IsValidFunction((Int16)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Int16)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Int16)
                return true;
            if (value is string)
            {
                Int16 v;
                return Int16.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Int16)
                return value;
            return Int16.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class Int32Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(Int32Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Int32, bool> IsValidFunction;

        public Int32Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public Int32Item(string name, string path, object propertyOwner, Func<Int32, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public Int32Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public Int32Item(string name, PropertyInfo property, object propertyOwner, Func<Int32, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public Int32Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public Int32Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<Int32, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Int32 && this.IsValidFunction((Int32)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Int32)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Int32)
                return true;
            if (value is string)
            {
                Int32 v;
                return Int32.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Int32)
                return value;
            return Int32.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class Int64Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(Int64Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Int64, bool> IsValidFunction;

        public Int64Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public Int64Item(string name, string path, object propertyOwner, Func<Int64, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public Int64Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public Int64Item(string name, PropertyInfo property, object propertyOwner, Func<Int64, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public Int64Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public Int64Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<Int64, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Int64 && this.IsValidFunction((Int64)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Int64)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Int64)
                return true;
            if (value is string)
            {
                Int64 v;
                return Int64.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Int64)
                return value;
            return Int64.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class UInt16Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(UInt16Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<UInt16, bool> IsValidFunction;

        public UInt16Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public UInt16Item(string name, string path, object propertyOwner, Func<UInt16, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public UInt16Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public UInt16Item(string name, PropertyInfo property, object propertyOwner, Func<UInt16, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public UInt16Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public UInt16Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<UInt16, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is UInt16 && this.IsValidFunction((UInt16)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((UInt16)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is UInt16)
                return true;
            if (value is string)
            {
                UInt16 v;
                return UInt16.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is UInt16)
                return value;
            return UInt16.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class UInt32Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(UInt32Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<UInt32, bool> IsValidFunction;

        public UInt32Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public UInt32Item(string name, string path, object propertyOwner, Func<UInt32, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public UInt32Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public UInt32Item(string name, PropertyInfo property, object propertyOwner, Func<UInt32, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public UInt32Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public UInt32Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<UInt32, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is UInt32 && this.IsValidFunction((UInt32)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((UInt32)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is UInt32)
                return true;
            if (value is string)
            {
                UInt32 v;
                return UInt32.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is UInt32)
                return value;
            return UInt32.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class UInt64Item : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(UInt64Item).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<UInt64, bool> IsValidFunction;

        public UInt64Item(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public UInt64Item(string name, string path, object propertyOwner, Func<UInt64, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public UInt64Item(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public UInt64Item(string name, PropertyInfo property, object propertyOwner, Func<UInt64, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public UInt64Item(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public UInt64Item(string name, string icon, PropertyInfo property, object propertyOwner, Func<UInt64, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is UInt64 && this.IsValidFunction((UInt64)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((UInt64)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is UInt64)
                return true;
            if (value is string)
            {
                UInt64 v;
                return UInt64.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is UInt64)
                return value;
            return UInt64.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class DecimalItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(DecimalItem).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Decimal, bool> IsValidFunction;

        public DecimalItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public DecimalItem(string name, string path, object propertyOwner, Func<Decimal, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public DecimalItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public DecimalItem(string name, PropertyInfo property, object propertyOwner, Func<Decimal, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public DecimalItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public DecimalItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<Decimal, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Decimal && this.IsValidFunction((Decimal)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Decimal)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Decimal)
                return true;
            if (value is string)
            {
                Decimal v;
                return Decimal.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Decimal)
                return value;
            return Decimal.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class DoubleItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(DoubleItem).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Double, bool> IsValidFunction;

        public DoubleItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public DoubleItem(string name, string path, object propertyOwner, Func<Double, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public DoubleItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public DoubleItem(string name, PropertyInfo property, object propertyOwner, Func<Double, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public DoubleItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public DoubleItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<Double, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Double && this.IsValidFunction((Double)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Double)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Double)
                return true;
            if (value is string)
            {
                Double v;
                return Double.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Double)
                return value;
            return Double.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    public class SingleItem : PropertyItem
    {
        private readonly static DataTemplate ThisDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(SingleItem).Assembly, "ArmA.Studio.Data.Configuration.NumericItem.xaml");
		private readonly Func<Single, bool> IsValidFunction;

        public SingleItem(string name, string path, object propertyOwner) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
        public SingleItem(string name, string path, object propertyOwner, Func<Single, bool> isValid) : this(name, string.Empty, propertyOwner.GetType().GetProperty(path), propertyOwner, isValid) { }
        public SingleItem(string name, PropertyInfo property, object propertyOwner) : this(name, string.Empty, property, propertyOwner, (v) => true) { }
        public SingleItem(string name, PropertyInfo property, object propertyOwner, Func<Single, bool> isValid) : this(name, string.Empty, property, propertyOwner, isValid) { }
		public SingleItem(string name, string icon, string path, object propertyOwner) : this(name, icon, propertyOwner.GetType().GetProperty(path), propertyOwner, (v) => true) { }
		public SingleItem(string name, string icon, PropertyInfo property, object propertyOwner, Func<Single, bool> isValid) : base(ThisDataTemplate, property, propertyOwner)
        {
            this.ImageSource = icon;
            this.Name = name;
			this.IsValidFunction = isValid;
        }
		
		public override bool IsValidValue(object value)
		{
            if (value is Single && this.IsValidFunction((Single)value))
                return true;
            if (this.CanConvert(value) && this.IsValidFunction.Invoke((Single)this.DoConversion(value)))
				return true;
            return false;
		}

        protected override bool CanConvert(object value)
        {
            if (value is Single)
                return true;
            if (value is string)
            {
                Single v;
                return Single.TryParse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
            }
            return false;
        }
        protected override object DoConversion(object value)
        {
            if (value is Single)
                return value;
            return Single.Parse(value as string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}