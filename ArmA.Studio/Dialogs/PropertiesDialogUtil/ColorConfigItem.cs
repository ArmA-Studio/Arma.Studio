using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ArmA.Studio.Data.Configuration;

namespace ArmA.Studio.Dialogs.PropertiesDialogUtil
{
    public class ColorConfigItem : PropertyItem
    {
        private readonly static DataTemplate ColorConfigItemDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(ColorConfigItem).Assembly, "ArmA.Studio.Dialogs.PropertiesDialogUtil.ColorConfigItem.xaml");
        public ColorConfigItem(string name, PropertyInfo property) : base(ColorConfigItemDataTemplate, property, null)
        {
            this.Name = name;
        }
    }
}
