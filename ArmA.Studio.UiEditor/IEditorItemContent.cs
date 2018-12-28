using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor
{
    public interface IEditorItemContent
    {
        double PositionX { get; set; }
        double PositionY { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        bool IsSelected { get; set; }
    }
}
