using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RealVirtuality.SQF
{
    [XmlRoot]
    public partial struct SqfDefinition
    {
        [XmlAttribute]
        public EKind Kind;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string LArgType;
        [XmlAttribute]
        public string RArgType;
        [XmlAttribute]
        public string ReturnType;
        [XmlElement]
        public string Description;
        [XmlElement]
        public List<string> Examples;
    }
}
