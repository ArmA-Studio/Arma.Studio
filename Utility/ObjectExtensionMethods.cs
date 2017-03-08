using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Utility
{
    public static class ObjectExtensionMethods
    {
        public const string ERR_XMLSERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE = "XmlSerialize requires XmlRoot to be set.";
        public const string ERR_XMLDESERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE = "XmlDeserialize requires XmlRoot to be set.";
        #region XmlSerialize
        public static void XmlSerialize(this object val, string path)
        {
            var att = Attribute.GetCustomAttribute(val.GetType(), typeof(XmlRootAttribute));
            if (att == null && !(val is IList))
            {
                throw new InvalidOperationException(ERR_XMLSERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE);
            }
            using (var stream = File.Open(path, FileMode.Create))
            {
                var serializer = new XmlSerializer(val.GetType());
                serializer.Serialize(stream, val);
            }
        }
        public static void XmlSerialize(this object val, Stream stream)
        {
            var att = Attribute.GetCustomAttribute(val.GetType(), typeof(XmlRootAttribute));
            if (att == null && !(val is IList))
            {
                throw new InvalidOperationException(ERR_XMLSERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE);
            }
            var serializer = new XmlSerializer(val.GetType());
            serializer.Serialize(stream, val);
        }
        #endregion

        #region XmlDeserialize<T>
        public static T XmlDeserialize<T>(this string path) where T : class
        {
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(XmlRootAttribute));
            if (att == null && !typeof(IList).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException(ERR_XMLDESERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE);
            }
            using (var stream = File.OpenRead(path))
            {
                var serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(stream) as T;
            }
        }
        public static T XmlDeserialize<T>(this Stream stream) where T : class
        {
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(XmlRootAttribute));
            if (att == null && !typeof(IList).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException(ERR_XMLDESERIALIZE_REQUIRES_XMLROOT_ATTRIBUTE);
            }
            
            var serializer = new XmlSerializer(typeof(T));
            return serializer.Deserialize(stream) as T;
        }
        #endregion
    }
}
