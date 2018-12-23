using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmA.Studio.UiEditor
{
    public class PluginMain : IPlugin
    {
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public IEnumerable<DockableInfo> GetAnchorables()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DockableInfo> GetDocuments()
        {
            throw new NotImplementedException();
        }

        public Task Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
