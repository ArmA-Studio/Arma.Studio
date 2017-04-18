using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;

namespace ArmA.Studio.Dialogs
{
    public class WorkspaceSelectorDialogDataContext : IDialogContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string CurrentPath { get { return this._CurrentPath; } set { this._CurrentPath = value; this.OKButtonEnabled = !string.IsNullOrWhiteSpace(value); this.RaisePropertyChanged(); } }
        private string _CurrentPath;

        public List<String> WorkSpaceListBox { get { return this._WorkSpaceListBox;  } set { this._WorkSpaceListBox = value; this.RaisePropertyChanged(); } }
        private List<String> _WorkSpaceListBox;

        public ICommand CmdBrowse => new RelayCommand(Cmd_Browse);
        public ICommand CmdOKButtonPressed => new UI.Commands.RelayCommand(Cmd_Ok);

        public bool? DialogResult { get { return this._DialogResult; } set { this._DialogResult = value; this.RaisePropertyChanged(); } }
        private bool? _DialogResult;

        public string WindowHeader { get { return Properties.Localization.WorkspaceSelectorDialog_Header; } }

        public string OKButtonText { get { return Properties.Localization.OK; } }

        public bool OKButtonEnabled { get { return this._OKButtonEnabled; } set { this._OKButtonEnabled = value; this.RaisePropertyChanged(); } }
        private bool _OKButtonEnabled;

        public WorkspaceSelectorDialogDataContext()
        {
            this.CmdBrowse = new UI.Commands.RelayCommand(Cmd_Browse);
            this.CmdOKButtonPressed = new UI.Commands.RelayCommand(Cmd_Ok);

            this.WorkSpaceListBox = ConfigHost.App.PrevWorkspacePath;   
        }

        public void Cmd_Ok(object param)
        {

            //Save new Workspace to Prev Workspace List
            List<string> prevWorkSpaces = ConfigHost.App.PrevWorkspacePath;
            if (prevWorkSpaces.Contains(this.CurrentPath))
            {
                prevWorkSpaces.Remove(this.CurrentPath);
            }
            prevWorkSpaces.Insert(0, this.CurrentPath);
            int numSpaces = 5;
            if (prevWorkSpaces.Count > numSpaces)
            {
                prevWorkSpaces.RemoveRange(numSpaces, (prevWorkSpaces.Count - numSpaces));
            }
            ConfigHost.App.PrevWorkspacePath = prevWorkSpaces;

            this.DialogResult = true;

        }

        public void Cmd_Browse(object param)
        {
            var cofd = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false,
                Title = Properties.Localization.WorkspaceSelectorDialog_Description
            };
            if (!string.IsNullOrWhiteSpace(this.CurrentPath))
            {
                cofd.InitialDirectory = CurrentPath;
            }
            var dlgResult = cofd.ShowDialog();
            if(dlgResult == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                this.CurrentPath = cofd.FileName;
            }
        }
    }
}
