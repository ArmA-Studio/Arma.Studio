using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Markup;

namespace ArmA.Studio.Data.UI
{
    public abstract class DocumentBase : DockableBase
    {
        public event EventHandler OnDocumentClosing;
        protected static DataTemplate GetDataTemplateFromAssemblyRes(string path)
        {
            var ass = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = ass.GetManifestResourceStream(path))
            {
                try
                {
                    var obj = XamlReader.Load(stream);
                    if (!(obj is DataTemplate))
                    {
                        return null;
                    }
                    return obj as DataTemplate;
                }
                catch
                {
                    return null;
                }
            }
        }

        public override string ContentId { get { return this.FilePath; } set { throw new NotImplementedException(); } }
        public abstract string FilePath { get; }
        public abstract DataTemplate Template { get; }
        public ICommand CmdClosing => new Commands.RelayCommand((p) =>
        {
            if (HasChanges)
            {
                var msgResult = MessageBox.Show(Properties.Localization.DocumentBase_CloseSaveConfirmation_Body, Properties.Localization.DocumentBase_CloseSaveConfirmation_Caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        SaveDocument(FilePath);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            this.OnDocumentClosing?.Invoke(this, new EventArgs());
        });
        public bool HasChanges { get; protected set; }

        public ProjectFileFolder.File FileReference { get { ProjectFileFolder.File v; this.WeakFileReference.TryGetTarget(out v); return v; } set { this.WeakFileReference.SetTarget(value); } }
        private WeakReference<ProjectFileFolder.File> WeakFileReference;
        public abstract void SaveDocument(string path);
        public abstract void OpenDocument(string path);
        public abstract void ReloadDocument();

        public DocumentBase(ProjectFileFolder.File fileRef)
        {
            this.WeakFileReference = new WeakReference<ProjectFileFolder.File>(fileRef);
        }
    }
}
