using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.DataContext.SolutionPaneUtil;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.WindowsAPICodePack.Dialogs;
using Utility.Collections;

namespace ArmA.Studio.DataContext
{
    public class SolutionPane : PanelBase
    {
        public override string Title => Properties.Localization.PanelDisplayName_Solution;

        public IEnumerable<ProjectModelView> ProjectModels { get { return this._ProjectModels; } set { this._ProjectModels = value; RaisePropertyChanged(); } }
        private IEnumerable<ProjectModelView> _ProjectModels;


        public ICommand CmdCreateProject => new RelayCommand((p) =>
        {
            var dlgdc = new Dialogs.CreateNewProjectDialogDataContext();
            var dlg = new Dialogs.CreateNewProjectDialog(dlgdc);
            var dlgresult = dlg.ShowDialog();
            if (dlgresult.HasValue && dlgresult.Value)
            {
                Workspace.Instance.Solution.AddProject(dlgdc.SelectedName, dlgdc.FinalType);
            }
        });
        public ICommand CmdAddProject => new RelayCommand((p) =>
        {
            var cofd = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false,
                Title = Properties.Localization.AddExistingProject,
                InitialDirectory = Path.GetDirectoryName(Workspace.Instance.Solution.FileUri.AbsolutePath)
            };
            var dlgResult = cofd.ShowDialog();
            if (dlgResult == CommonFileDialogResult.Ok)
            {
                Workspace.Instance.Solution.AddProject(Path.GetFileName(cofd.FileName), Path.GetDirectoryName(Path.GetFileName(cofd.FileName)).Any((c) => c == '.') ? EProjectType.Mission : EProjectType.Addon, cofd.FileName);
            }
        });

        public void RebuildTree(Solution s)
        {
            var projectModelsList = new List<ProjectModelView>();

            foreach(var p in s.Projects)
            {
                projectModelsList.Add(Create(p));
            }
            //ToDo: Reapply selectedItem & isExtended property settings
            this.ProjectModels = projectModelsList;
        }

        private static ProjectModelView Create(Project p)
        {
            var pmv = new ProjectModelView(p);
            foreach(var pff in p)
            {
                Create(pmv, pff);
            }
            return pmv;
        }

        private static void Create(IList<object> projectModels, ProjectFile pff)
        {
            var pfmv = new ProjectFileModelView(pff);
            var splittedPath = pff.ProjectRelativePath.Split('/');
            var index = 0;
            var current = projectModels;
            var func = new Func<object, bool>((it) =>
            {
                var folder = it as ProjectFolderModelView;
                if (folder == null)
                    return false;
                return folder.Name.Equals(splittedPath[index], StringComparison.InvariantCultureIgnoreCase);
            });
            while (splittedPath.Length - 1 > index)
            {
                if (string.IsNullOrWhiteSpace(splittedPath[index]))
                {
                    index++;
                    continue;
                }
                if (current.Any(func))
                {
                    current = (current.First(func) as ProjectFolderModelView);
                }
                else
                {
                    var tmp = new ProjectFolderModelView(splittedPath[index], current);
                    current.Add(tmp);
                    Sort(current);
                    current = tmp;
                }
                index++;
            }
            current.Add(pfmv);
            Sort(current);
        }
        private static void Sort(IList<object> projectmodellist)
        {
            for (int i = 0; i < projectmodellist.Count; i++)
            {
                for (int j = i + 1; j < projectmodellist.Count; j++)
                {
                    if((projectmodellist[i] is ProjectFileModelView) && !(projectmodellist[j] is ProjectFileModelView))
                    {
                        var tmp = projectmodellist[i];
                        projectmodellist[i] = projectmodellist[j];
                        projectmodellist[j] = tmp;
                    }
                }
            }
        }

        public SolutionPane()
        {
            Task.Run(() =>
            {
                SpinWait.SpinUntil(() => Workspace.Instance.Solution != null && Workspace.Instance.Solution.Projects != null);
                Workspace.Instance.Solution.Projects.CollectionChanged += Projects_CollectionChanged;
                App.Current.Dispatcher.Invoke(() => this.RebuildTree(Workspace.Instance.Solution));
            });
        }

        private void Projects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => this.RebuildTree(Workspace.Instance.Solution));
        }
    }
}