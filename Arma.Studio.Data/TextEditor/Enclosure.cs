using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class Enclosure
    {
        public Color Color { get; }
        public string Open { get; }
        public string Close { get; }

        public Enclosure(Color color, string open, string close = "")
        {
            this.Color = color;
            this.Open = open;
            this.Close = close;
        }
    }
}
