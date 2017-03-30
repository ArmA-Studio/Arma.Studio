using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Utility.Collections;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Data.UI.Commands;
using ArmA.Studio.DataContext.SolutionPaneUtil;
using ArmA.Studio.Data;
using System.Threading;

namespace ArmA.Studio.DataContext
{
    public class SolutionPane : PanelBase
    {
        public override string Title => Properties.Localization.PanelDisplayName_Solution;

        public IEnumerable<ProjectModelView> ProjectModels { get { return this._ProjectModels; } set { this._ProjectModels = value; RaisePropertyChanged(); } }
        private IEnumerable<ProjectModelView> _ProjectModels;

        public void RebuildTree(Solution s)
        {
            var projectModelsList = new List<ProjectModelView>();

            foreach(var p in s.Projects)
            {
                projectModelsList.Add(Create(p));
            }
            this.ProjectModels = projectModelsList;
        }

        private static ProjectModelView Create(Project p)
        {
            var pmv = new ProjectModelView(p);
            foreach(var pff in p)
            {
                Create(pmv.Children, pff);
            }
            return pmv;
        }

        private static void Create(List<ProjectFileFolderModelView> projectModels, ProjectFileFolder pff)
        {
            var pffmv = new ProjectFileFolderModelView(pff);
            var splittedPath = pff.ProjectRelativePath.Split('/');
            var index = 0;
            var current = projectModels;
            while (splittedPath.Length - 1 > index)
            {
                if (current.Any((it) => it.Name.Equals(splittedPath[index], StringComparison.InvariantCultureIgnoreCase)))
                {
                    current = current.First((it) => it.Name.Equals(splittedPath[index], StringComparison.InvariantCultureIgnoreCase)).Children;
                }
                else
                {
                    var tmp = new ProjectFileFolderModelView(splittedPath[index]);
                    current.Add(tmp);
                    current = tmp.Children;
                }
                index++;
            }
            current.Add(pffmv);
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