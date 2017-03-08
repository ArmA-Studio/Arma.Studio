using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.UI.ViewModel
{
    public interface IUserInteractable
    {
        event EventHandler<UserInteractionEventArgs> OnUserInteraction;
    }
}
