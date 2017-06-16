using System;
using System.Windows;

namespace ArmA.Studio.Dialogs
{
    public partial class WorkspaceSelectorDialog
    {
        /// <summary>
        /// Creates a new WorkspaceSelectorDialog to allow the user to select a workspace.
        /// </summary>
        /// <returns>new workspace path or empty string</returns>
        public static string GetWorkspacePath(string selectedPath = null)
        {
            string workspace = string.Empty;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dlgDc = new WorkspaceSelectorDialogDataContext();
                if (!string.IsNullOrWhiteSpace(selectedPath))
                {
                    dlgDc.CurrentPath = selectedPath.Replace('/', '\\');
                }
                else
                {
                    dlgDc.CurrentPath = App.DefaultWorkspacepath;
                }
                var dlg = new WorkspaceSelectorDialog(dlgDc);
                var dlgResult = dlg.ShowDialog();
                if (dlgResult.HasValue && dlgResult.Value && !dlgDc.CurrentPath.Equals(selectedPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    workspace = dlgDc.CurrentPath;
                }
            });
            return workspace;
        }
    }
}
