using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Collections;

namespace ArmA.Studio.Data
{
    public class Project : IComparable, INotifyPropertyChanged, IEnumerable<ProjectFileFolder>
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string Name { get { return this._Name; } set { if (this._Name == value) return; this._Name = value; RaisePropertyChanged(); } }
        private string _Name;

        public EProjectType ProjectType { get { return this._ProjectType; } set { if (this._ProjectType == value) return; this._ProjectType = value; RaisePropertyChanged(); } }
        private EProjectType _ProjectType;

        public ObservableSortedCollection<ProjectFileFolder> Children { get; private set; }

        public Solution OwningSolution { get { Solution v; this.WeakOwningSolution.TryGetTarget(out v); return v; } set { this.WeakOwningSolution.SetTarget(value); } }
        private WeakReference<Solution> WeakOwningSolution;

        public Project()
        {
            this.Children = new ObservableSortedCollection<ProjectFileFolder>();
            this.WeakOwningSolution = new WeakReference<Solution>(null);
            this.Name = string.Empty;
        }
        

        public int CompareTo(object obj)
        {
            return this.Name.CompareTo(obj);
        }

        public IEnumerator<ProjectFileFolder> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator().Cast();
        }
    }
}