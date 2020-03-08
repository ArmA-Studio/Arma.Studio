using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UiEditor.Data
{
    public interface IControlElement : INotifyPropertyChanged, IPropertyHost
    {
        int ZIndex { get; set; }
        bool IsSelected { get; set; }
        double Left { get; set; }
        double Top { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}
