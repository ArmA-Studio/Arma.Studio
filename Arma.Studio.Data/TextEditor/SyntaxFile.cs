using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class SyntaxFile
    {
        public Color DigitsColor { get; set; }
        public List<KeywordCollection> Keywords { get; }
        public List<Enclosure> Enclosures { get; }
        public bool IgnoreCase { get; }
        public string Delimeters { get; }
        public SyntaxFile(bool ignoreCase, string delimeters)
        {
            this.Keywords = new List<KeywordCollection>();
            this.Enclosures = new List<Enclosure>();
            this.DigitsColor = Colors.Black;
            this.Delimeters = delimeters;
            this.IgnoreCase = ignoreCase;
        }
    }
}
