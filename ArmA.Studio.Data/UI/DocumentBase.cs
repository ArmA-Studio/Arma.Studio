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
        public sealed class DocumentDescribor
        {
            public Uri IconSource { get; private set; }
            public string Name { get; private set; }
            public IEnumerable<FileType> SupportedFileTypes { get; private set; }

            public DocumentDescribor(IEnumerable<FileType> supportedFileTypes, string name) : this(supportedFileTypes, name, null as Uri) { }
            public DocumentDescribor(IEnumerable<FileType> supportedFileTypes, string name, string iconSource) : this(supportedFileTypes, name, new Uri(iconSource, UriKind.RelativeOrAbsolute)) { }
            public DocumentDescribor(IEnumerable<FileType> supportedFileTypes, string name, Uri iconSource)
            {
                this.SupportedFileTypes = supportedFileTypes;
                this.Name = name;
                this.IconSource = iconSource;
            }
        }



        public event EventHandler OnDocumentClosing;
        protected static DataTemplate GetDataTemplateFromAssemblyRes(string path) => GetDataTemplateFromAssemblyRes(path, System.Reflection.Assembly.GetCallingAssembly());
        protected static DataTemplate GetDataTemplateFromAssemblyRes(string path, System.Reflection.Assembly ass)
        {
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

        public override string ContentId { get { return this.FileReference.FilePath; } set { throw new NotImplementedException(); } }
        public abstract DataTemplate Template { get; }
        public ICommand CmdClosing => new Commands.RelayCommand((p) =>
        {
            if (HasChanges)
            {
                var msgResult = MessageBox.Show(Properties.Localization.DocumentBase_CloseSaveConfirmation_Body, Properties.Localization.DocumentBase_CloseSaveConfirmation_Caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        SaveDocument(this.FileReference.FilePath);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            this.OnDocumentClosing?.Invoke(this, new EventArgs());
        });
        public abstract bool HasChanges { get; }

        public ProjectFile FileReference { get { ProjectFile v; this.WeakFileReference.TryGetTarget(out v); return v; } set { this.WeakFileReference.SetTarget(value); } }
        private WeakReference<ProjectFile> WeakFileReference;

        public bool IsTemporary { get { return this._IsTemporary; } set { if (this._IsTemporary == value) return; this._IsTemporary = value; RaisePropertyChanged(); } }
        private bool _IsTemporary;

        public abstract void SaveDocument();
        public abstract void SaveDocument(string path);
        public abstract void LoadDocument();

        public DocumentBase(ProjectFile fileRef)
        {
            this.WeakFileReference = new WeakReference<ProjectFile>(fileRef);
        }

        public abstract void RefreshVisuals();
    }
}
