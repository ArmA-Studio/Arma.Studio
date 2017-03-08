using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace RealVirtuality.Config.Parser
{
    public class ConfigEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public ConfigEntry ConfigEntryParent
        {
            get
            {
                ConfigEntry parent;
                if(this._ConfigEntryParent.TryGetTarget(out parent))
                {
                    return parent;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    this._ConfigEntryParent = null;
                }
                else
                {
                    this._ConfigEntryParent = new WeakReference<ConfigEntry>(value);
                }
            }
        }
        private WeakReference<ConfigEntry> _ConfigEntryParent;
        public int ConfigEntryParentsCount { get { var parent = this.ConfigEntryParent; return parent != null ? parent.ConfigEntryParentsCount + 1 : 0; } }

        public ConfigEntry(ConfigEntry parent)
        {
            this.Children = new ObservableCollection<ConfigEntry>();
            parent?.Children.Add(this);
            this._ConfigEntryParent = new WeakReference<ConfigEntry>(parent);
        }
        private object _Value;
        public object Value { get { return _Value; } set {  this._Value = value; this.RaisePropertyChanged(); } }
        public ObservableCollection<ConfigEntry> Children { get { return (ObservableCollection<ConfigEntry>)_Value; } set { this._Value = value; this.RaisePropertyChanged(); this.RaisePropertyChanged("Value"); } }

        public string Name
        {
            get { return this.NameStart == null || this.NameEnd == null ? null : new TextRange(this.NameStart, this.NameEnd).Text; }
            set { if (this.IsDummy) this.Create(); new TextRange(this.NameStart, this.NameEnd).Text = value; this.RaisePropertyChanged(); }
        }
        public string Parent
        {
            get { return this.ParentStart == null || this.ParentEnd == null ? null : new TextRange(this.ParentStart, this.ParentEnd).Text; }
            set
            {
                if (this.IsDummy)
                    this.Create();
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (this.ParentStart.GetOffsetToPosition(this.ParentEnd) == 0)
                    {
                        //Add parent ':'
                        new TextRange(this.NameEnd.GetPositionAtOffset(0, LogicalDirection.Forward), this.ParentStart.GetPositionAtOffset(0, LogicalDirection.Backward)).Text = " : ";
                    }
                }
                else
                {
                    if (this.ParentStart.GetOffsetToPosition(this.ParentEnd) != 0)
                    {
                        //Remove parent ':'
                        new TextRange(this.NameEnd.GetPositionAtOffset(0, LogicalDirection.Forward), this.ParentStart.GetPositionAtOffset(0, LogicalDirection.Backward)).Text = string.Empty;
                    }
                }
                new TextRange(this.ParentStart, this.ParentEnd).Text = value;
                this.RaisePropertyChanged();
            }
        }
        public string Content
        {
            get { return this.ContentStart == null || this.ContentEnd == null ? null : new TextRange(this.ContentStart, this.ContentEnd).Text; }
            set { if (this.IsDummy) this.Create(); new TextRange(this.ContentStart, this.ContentEnd).Text = value; this.RaisePropertyChanged(); }
        }

        public string FullEntry
        {
            get { return this.FullStart == null || this.FullEnd == null ? null : new TextRange(this.FullStart, this.FullEnd).Text; }
            set { new TextRange(this.FullStart, this.FullEnd).Text = value; this.RaisePropertyChanged(); }
        }

        public bool IsField { get; internal set; }
        public bool IsDummy { get { return this.FullStart.GetOffsetToPosition(this.FullEnd) == 0; } }


        public TextPointer FullEnd { get; internal set; }
        public TextPointer FullStart { get; internal set; }
        public TextPointer ContentEnd { get; internal set; }
        public TextPointer ContentStart { get; internal set; }
        public TextPointer NameEnd { get; internal set; }
        public TextPointer NameStart { get; internal set; }
        public TextPointer ParentEnd { get; internal set; }
        public TextPointer ParentStart { get; internal set; }

        public ConfigEntry this[string key]
        {
            get
            {
                foreach(var it in this.Children)
                {
                    if (it.Name == key)
                        return it;
                }
                if(this.Parent != null)
                {
                    var queue = new Queue<string>();
                    queue.Enqueue(key);
                    return this.TraverseParents(queue);
                }
                return null;
            }
        }
        private ConfigEntry TraverseParents(Queue<string> keyqueue)
        {
            //ToDo: Check function for correctness
            if (keyqueue.Count == 0)
                return this;

            //Check current childrens for given key
            var key = keyqueue.Dequeue();
            foreach (var it in this.Children)
            {
                if (it.Name == key)
                {
                    var traverseResult = it.TraverseParents(keyqueue);
                    if(traverseResult != null)
                    {
                        //Return the traverse result as valid entry
                        return traverseResult;
                    }
                    else
                    {
                        //we had our match ... we will not encounter another one in here
                        break;
                    }
                }
            }
            //Current children do not contain key so reenqueue current key
            keyqueue.Enqueue(key);


            //Check if this has a parent class
            if (this.Parent != null)
            {
                //Search for parent class
                var cur = this.ConfigEntryParent;
                ConfigEntry parent = null;
                do
                {
                    foreach(var it in cur.Children)
                    {
                        if(it.Name == this.Parent)
                        {
                            //Parent class was found
                            parent = it;
                            break;
                        }
                    }
                    if(parent != null)
                    {
                        break;
                    }
                    else
                    {
                        cur = cur.ConfigEntryParent;
                    }
                } while (cur != null);
                if(parent != null)
                {
                    //Correct parent was found, search in there for the key
                    return parent.TraverseParents(keyqueue);
                }
            }
            //Field is unknown
            return null;
        }
        private void Create()
        {
            if (this.ConfigEntryParent == null)
                throw new InvalidOperationException("Not applicable on root node");
            var range = new TextRange(this.FullStart, this.FullEnd);
            var builder = new StringBuilder();
            if(this.IsField)
            {
                builder.Append(new string('\t', ConfigEntryParentsCount));
                var fName = this.ConfigEntryParent.GetUniqueChildName();
                builder.Append(fName);
                builder.Append(" = 0;");
                range.Text = builder.ToString();
                this.NameStart = this.FullStart.GetPositionAtOffset(ConfigEntryParentsCount, LogicalDirection.Forward);
                this.NameEnd = this.NameStart.GetPositionAtOffset(fName.Length, LogicalDirection.Backward);
                this.ContentStart = this.NameEnd.GetPositionAtOffset(3, LogicalDirection.Forward);
                this.ContentEnd = this.ContentStart.GetPositionAtOffset(1, LogicalDirection.Backward);
            }
            else
            {
                builder.Append(new string('\t', ConfigEntryParentsCount));
                var fName = this.ConfigEntryParent.GetUniqueChildName();
                builder.Append(fName);
                builder.Append(" { };");
                range.Text = builder.ToString();
                this.NameStart = this.FullStart.GetPositionAtOffset(ConfigEntryParentsCount, LogicalDirection.Forward);
                this.NameEnd = this.NameStart.GetPositionAtOffset(fName.Length, LogicalDirection.Backward);
                this.ParentStart = this.NameEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
                this.ParentEnd = this.NameEnd.GetPositionAtOffset(0, LogicalDirection.Backward);
                this.ContentStart = this.NameEnd.GetPositionAtOffset(2, LogicalDirection.Forward);
                this.ContentEnd = this.ContentStart.GetPositionAtOffset(1, LogicalDirection.Backward);
            }
        }
        private string GetUniqueChildName()
        {
            var attempt = 0;
            string name;
            do
            {
                name = string.Concat('n', attempt++);
            } while (this.Children.FirstOrDefault((it) => it.Name == name) != null);
            return name;
        }
    }
}