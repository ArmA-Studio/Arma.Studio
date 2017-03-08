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

namespace ArmA.Studio
{
    public abstract class DocumentBase : DockableBase
    {
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
        public abstract string[] SupportedFileExtensions { get; }
        public ICommand CmdClosing { get { return new UI.Commands.RelayCommand(OnClosing); } }
        public bool HasChanges { get; protected set; }

        protected virtual void OnClosing(object param)
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
            try
            {
                Workspace.CurrentWorkspace.DocumentsDisplayed.Remove(this);
            }
            catch (NullReferenceException) { } //AvalonDock NRE catch as avalondock handles background docs invalidly
        }
        public abstract void SaveDocument(string path);
        public abstract void OpenDocument(string path);
        public abstract void ReloadDocument();
    }
}
