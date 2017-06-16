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
        /// <summary>
        /// Loads given type <typeparamref name="T"/> from provided <paramref name="path"/>.
        /// Will throw <see cref="System.IO.FileNotFoundException"/> in case of the <paramref name="path"/> being not existing.
        /// Will throw <see cref="InvalidCastException"/> in case of the item behind <paramref name="path"/> is not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="class"/> type which will be loaded from <paramref name="assembly"/>.</typeparam>
        /// <param name="assembly">The <see cref="System.Reflection.Assembly"/> where to look the <paramref name="path"/> up.</param>
        /// <param name="path">Path to the xaml file. 'Example: ArmA.Studio.Data.Configuration.StringItem.xaml' </param>
        /// <returns></returns>
        protected static T LoadFromEmbeddedResource<T>(System.Reflection.Assembly assembly, string path) where T : class
        {
            var resNames = from name in assembly.GetManifestResourceNames() where name.EndsWith(".xaml") where name.Equals(path) select name;
            foreach (var res in resNames)
            {
                var resSplit = res.Split('.');
                var header = resSplit[resSplit.Count() - 2];
                using (var stream = assembly.GetManifestResourceStream(res))
                {
                    try
                    {
                        var obj = XamlReader.Load(stream);
                        if (!(obj is T))
                        {
                            throw new InvalidCastException();
                        }
                        return obj as T;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            throw new System.IO.FileNotFoundException();
        }

        public override string ContentId { get { return this.FileReference?.FilePath; } set { throw new NotImplementedException(); } }
        public ICommand CmdClosing => new Commands.RelayCommand((p) =>
        {
            if (this.HasChanges)
            {
                var msgResult = MessageBox.Show(Properties.Localization.DocumentBase_CloseSaveConfirmation_Body, Properties.Localization.DocumentBase_CloseSaveConfirmation_Caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        this.SaveDocument(this.FileReference.FilePath);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            this.OnDocumentClosing?.Invoke(this, new EventArgs());
        });
        public abstract bool HasChanges { get; }
        public IKeyManager KeyManager { get; set; }

        public ProjectFile FileReference { get { ProjectFile v; this.WeakFileReference.TryGetTarget(out v); return v; } set { this.WeakFileReference.SetTarget(value); } }
        private WeakReference<ProjectFile> WeakFileReference;

        public bool IsTemporary { get { return this._IsTemporary; } set { if (this._IsTemporary == value) return; this._IsTemporary = value;
            this.RaisePropertyChanged(); } }
        private bool _IsTemporary;

        public string TemporaryIdentifier { get { return this._TemporaryIdentifier; } set { if (this._TemporaryIdentifier == value) return; this._TemporaryIdentifier = value;
            this.RaisePropertyChanged(); } }
        private string _TemporaryIdentifier;

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
