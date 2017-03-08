using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RealVirtuality.Config.Control.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StyleInfoAttribute : Attribute
    {

        public string Name { get; set; }
        public Type Type { get; set; }

        public StyleInfoAttribute()
        {

        }
        public static StyleInfoAttribute GetAttribute(object obj)
        {
            if (obj == null)
                return null;
            var info = obj.GetType().GetMember(obj.ToString());
            if (info != null && info.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(info.First(), typeof(StyleInfoAttribute)) as StyleInfoAttribute;
                if (attr != null)
                {
                    return attr;
                }
            }
            return null;
        }
    }
}
