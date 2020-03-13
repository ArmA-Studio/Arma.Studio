using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arma.Studio.Data.UI
{
    public interface IKeyInteractible
    {
        bool KeyDown(KeyEventArgs keyEventArgs);
    }
}
