using Arma.Studio.Data.Debugging;
using Arma.Studio.Data.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data.UI
{
    public interface IMainWindow : INotifyPropertyChanged
    {
        event EventHandler<EventArgs> DebuggerStateChanged;
        IDebugger Debugger { get; }
        Window OwningWindow { get; }
        IFileManagement FileManagement { get; }
        IBreakpointManager BreakpointManager { get; }
        BusyContainerManager BusyContainerManager { get; }
        DockableBase ActiveDockable { get; set; }
        void SetStatusLabel(string s);
        DockableBase FirstDocumentOrDefault(Func<DockableBase, bool> predicate);
        DockableBase FirstAnchorableOrDefault(Func<DockableBase, bool> predicate);
        void AddDocument(DockableBase dockableBase);
        void AddAnchorable(DockableBase dockableBase);
        IEnumerable<T> GetDocuments<T>() where T : DockableBase;
        T GetAnchorable<T>() where T : DockableBase;
        Task<Data.UI.IEditorDocument> OpenFile(File file);
        IEnumerable<EditorInfo> EditorInfos { get; }
        IEnumerable<Data.TextEditor.TextEditorInfo> TextEditorInfos { get; }
        IPropertyHost PropertyHost { get; set; }
    }
}
