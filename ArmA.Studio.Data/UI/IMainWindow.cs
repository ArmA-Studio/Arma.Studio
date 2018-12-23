using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data.UI
{
    public interface IMainWindow
    {
        Window OwningWindow { get; }
        DockableBase SelectedDockable { get; set; }
        void SetStatusLabel(string s);
        DockableBase FirstDocumentOrDefault(Func<DockableBase, bool> predicate);
        DockableBase FirstAnchorableOrDefault(Func<DockableBase, bool> predicate);
        void AddDocument(DockableBase dockableBase);
        void AddAnchorable(DockableBase dockableBase);
        IEnumerable<T> GetDocuments<T>() where T : DockableBase;
        T GetAnchorable<T>() where T : DockableBase;
    }
}
