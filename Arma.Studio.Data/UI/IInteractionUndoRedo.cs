using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public interface IInteractionUndoRedo
    {
        bool UndoAvailable { get; }
        Task Undo(System.Threading.CancellationToken cancellationToken);
        bool RedoAvailable { get; }
        Task Redo(System.Threading.CancellationToken cancellationToken);
    }
}
