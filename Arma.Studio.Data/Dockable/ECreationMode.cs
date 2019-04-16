using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Dockable
{
    [Flags]
    public enum ECreationMode
    {
        Nothing = 0b00,
        Document = 0b01,
        Anchorable = 0b10,
        Both = Document | Anchorable
    }
}
