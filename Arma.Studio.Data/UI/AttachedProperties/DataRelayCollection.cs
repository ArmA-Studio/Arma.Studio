using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data.UI.AttachedProperties
{
    /// <summary>
    /// Simple <see cref="FreezableCollection{T}"/> for <see cref="DataRelay"/>s only.
    /// Class itself is empty besides the base-class definition.
    /// </summary>
    public class DataRelayCollection : FreezableCollection<DataRelay> { }
}
