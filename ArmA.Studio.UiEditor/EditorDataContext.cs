using ArmA.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArmA.Studio.UiEditor
{
    public class EditorDataContext :
        DockableBase,
        Data.UI.AttachedProperties.IOnDragEnter,
        Data.UI.AttachedProperties.IOnDragLeave,
        Data.UI.AttachedProperties.IOnDragOver,
        Data.UI.AttachedProperties.IOnDrop
    {
        public ObservableCollection<EditorItem> Items { get => this._Items; set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorItem> _Items;

        public ObservableCollection<EditorItem> SelectedItems { get => this._SelectedItems; set { this._SelectedItems = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorItem> _SelectedItems;

        public EditorDataContext()
        {
            this._Items = new ObservableCollection<EditorItem>
            {
                new EditorItem(this, new Control()
                {
                    PositionX = 100,
                    PositionY = 100,
                    Width = 50,
                    Height = 50
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 200,
                    PositionY = 200,
                    Width = 50,
                    Height = 50
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 100,
                    PositionY = 200,
                    Width = 50,
                    Height = 50
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 200,
                    PositionY = 100,
                    Width = 50,
                    Height = 50
                })
            };
            this.SelectedItems = new ObservableCollection<EditorItem>();
        }


        public void OnDragEnter(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        public void OnDragOver(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        public void OnDrop(UIElement sender, DragEventArgs e)
        {
            var data = e.GetInfo().GetDataOrDefault<EditorToolboxItem>();
            if (data == null)
            {
                return;
            }
            
            var position = e.GetPosition(sender);
            var editorItem = new EditorItem(this, new Control()
            {
                PositionX = position.X - 25,
                PositionY = position.Y - 25,
                Width = 50,
                Height = 50,
                Type = data.Type
            });
            this.Items.Add(editorItem);
            e.Handled = true;
        }

        public void OnDragLeave(UIElement sender, DragEventArgs e)
        {
            if (e.GetInfo().HasData<EditorToolboxItem>())
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}
