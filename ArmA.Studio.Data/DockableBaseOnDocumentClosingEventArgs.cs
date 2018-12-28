using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public class DockableBaseOnDocumentClosingEventArgs
    {
        public bool Cancel { get; set; }
        public DockableBaseOnDocumentClosingEventArgs()
        {
            this.Cancel = false;
        }
    }
}
