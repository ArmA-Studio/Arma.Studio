using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    public interface IInteractionSave
    {
        Task Save(System.Threading.CancellationToken cancellationToken);
    }
}
