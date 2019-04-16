using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Arma.Studio.SqfEditor
{
    public class SqfDefinitionsFile
    {
        public interface Concated
        {
            string Name { get; set; }
            string Group { get; set; }
            bool GroupSpecified { get; }
        }
        [XmlRoot("binary")]
        public class Binary : Concated
        {
            [XmlText]
            public string Name { get; set; }
            [XmlAttribute("left")]
            public string Left { get; set; }
            [XmlAttribute("right")]
            public string Right { get; set; }

            [XmlAttribute("group")]
            public string Group { get; set; }
            public bool GroupSpecified => !String.IsNullOrWhiteSpace(this.Group);

            [XmlAttribute("precedence")]
            public int Precedence { get; set; } = 4;
        }
        [XmlRoot("unary")]
        public class Unary : Concated
        {
            [XmlText]
            public string Name { get; set; }
            [XmlAttribute("right")]
            public string Right { get; set; }

            [XmlAttribute("group")]
            public string Group { get; set; }
            public bool GroupSpecified => !String.IsNullOrWhiteSpace(this.Group);
        }
        [XmlRoot("nular")]
        public class Nular : Concated
        {
            [XmlText]
            public string Name { get; set; }

            [XmlAttribute("group")]
            public string Group { get; set; }
            public bool GroupSpecified => !String.IsNullOrWhiteSpace(this.Group);
        }

        public class Group
        {
            [XmlText]
            public string Name { get; set; }

            [XmlAttribute("red")]
            public byte Red { get; set; }

            [XmlAttribute("green")]
            public byte Green { get; set; }

            [XmlAttribute("blue")]
            public byte Blue { get; set; }

            [XmlAttribute("bold")]
            public bool IsBold { get; set; }
        }

        [XmlElement("binary")]
        public List<Binary> Binaries { get; set; }
        [XmlElement("unary")]
        public List<Unary> Unaries { get; set; }
        [XmlElement("nular")]
        public List<Nular> Nulars { get; set; }
        [XmlElement("group")]
        public List<Group> Groups { get; set; }

        public IEnumerable<Concated> ConcatAll()
        {
            foreach (var it in this.Nulars)
            {
                yield return it;
            }
            foreach (var it in this.Unaries)
            {
                yield return it;
            }
            foreach (var it in this.Binaries)
            {
                yield return it;
            }
        }

        public SqfDefinitionsFile()
        {
            this.Binaries = new List<Binary>();
            this.Unaries = new List<Unary>();
            this.Nulars = new List<Nular>();
            this.Groups = new List<Group>();
        }
    }
}
