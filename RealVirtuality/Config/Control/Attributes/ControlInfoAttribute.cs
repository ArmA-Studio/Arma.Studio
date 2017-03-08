using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RealVirtuality.Config.Control.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ControlInfoAttribute : Attribute
    {

        public Type Type { get; set; }

        public ControlInfoAttribute(Type t)
        {
            this.Type = t;
        }
        public static ControlInfoAttribute GetAttribute(object obj)
        {
            if (obj == null)
                return null;
            var info = obj.GetType().GetMember(obj.ToString());
            if (info != null && info.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(info.First(), typeof(ControlInfoAttribute)) as ControlInfoAttribute;
                if (attr != null)
                {
                    return attr;
                }
            }
            return null;
        }
    }
}
