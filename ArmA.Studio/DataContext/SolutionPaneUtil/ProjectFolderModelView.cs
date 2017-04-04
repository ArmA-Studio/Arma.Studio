using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio.DataContext.SolutionPaneUtil
{
    public class ProjectFolderModelView : INotifyPropertyChanged, IList<object>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        private readonly IList<object> InnerList;

        public IEnumerator<object> GetEnumerator() => InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public int Count => this.InnerList.Count;
        public bool IsReadOnly => this.InnerList.IsReadOnly;
        public object this[int index] { get { return this.InnerList[index]; } set { this.InnerList[index] = value; } }
        public int IndexOf(object item) => this.InnerList.IndexOf(item);
        public void Insert(int index, object item) => this.InnerList.Insert(index, item);
        public void RemoveAt(int index) => this.InnerList.RemoveAt(index);
        public void Add(object item) => this.InnerList.Add(item);
        public void Clear() => this.InnerList.Clear();
        public bool Contains(object item) => this.InnerList.Contains(item);
        public void CopyTo(object[] array, int arrayIndex) => this.InnerList.CopyTo(array, arrayIndex);
        public bool Remove(object item) => this.InnerList.Remove(item);

        //ToDo: Make folder actually be renamed when this changes
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                RaisePropertyChanged();
            }
        }
        private string _Name;

        public bool IsExpaned { get { return this._IsExpaned; } set { this._IsExpaned = value; RaisePropertyChanged(); } }
        private bool _IsExpaned;

        public bool IsInRenameMode { get { return this._IsInRenameMode; } set { this._IsInRenameMode = value; RaisePropertyChanged(); } }
        private bool _IsInRenameMode;

        public object Parent { get; private set; }

        public ProjectFolderModelView(string name, object parent)
        {
            this.Name = name;
            this.InnerList = new List<object>();
            this.Parent = parent;
        }
    }
}
