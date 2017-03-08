using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RealVirtuality.Config.Control.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPathDescriptor : Attribute
    {

        public string Path { get; set; }

        public Type Converter;

        public ConfigPathDescriptor(string path)
        {
            this.Path = path;
        }
        public static ConfigPathDescriptor GetAttribute(object obj)
        {
            if (obj == null)
                return null;
            var info = obj.GetType().GetMember(obj.ToString());
            if (info != null && info.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(info.First(), typeof(ConfigPathDescriptor)) as ConfigPathDescriptor;
                if (attr != null)
                {
                    return attr;
                }
            }
            return null;
        }
    }
}
