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
        public class ScaleComboboxItem
        {
            public double Value { get; }
            public string Display => $"{this.Value * 100:N0} %";
            public ScaleComboboxItem(double scaleFactor)
            {
                this.Value = scaleFactor;
            }
        }
        public ObservableCollection<EditorItem> Items { get => this._Items; set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorItem> _Items;

        public ObservableCollection<EditorItem> SelectedItems { get => this._SelectedItems; set { this._SelectedItems = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<EditorItem> _SelectedItems;

        public IEnumerable<ScaleComboboxItem> ScaleSource { get; } = new ScaleComboboxItem[] {
            new ScaleComboboxItem(0.2),
            new ScaleComboboxItem(0.4),
            new ScaleComboboxItem(0.6),
            new ScaleComboboxItem(0.8),
            new ScaleComboboxItem(1.0),
            new ScaleComboboxItem(1.2),
            new ScaleComboboxItem(1.4),
            new ScaleComboboxItem(1.6),
            new ScaleComboboxItem(1.8),
            new ScaleComboboxItem(2.0)
        };


        public bool SnapToGrid { get => this._SnapToGrid; set { this._SnapToGrid = value; this.RaisePropertyChanged(); } }
        private bool _SnapToGrid;

        public Rect GridRect => new Rect(0, 0, this.GridSize, this.GridSize); 
        public double GridSize { get => this._GridSize; set { this._GridSize = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.GridRect)); } }
        private double _GridSize;

        public ScaleComboboxItem SelectedScale { get => this.ScaleSource.FirstOrDefault((sci) => sci.Value == this.ScaleFactor); set => this.ScaleFactor = value?.Value ?? this.ScaleFactor; }
        public double ScaleFactor
        {
            get => this._ScaleFactor;
            set
            {
                this._ScaleFactor = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.SelectedScale));
            }
        }
        private double _ScaleFactor;

        public EditorDataContext()
        {
            this._Items = new ObservableCollection<EditorItem>
            {
                new EditorItem(this, new Control()
                {
                    PositionX = 100,
                    PositionY = 100,
                    Width = 75,
                    Height = 75
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 200,
                    PositionY = 200,
                    Width = 75,
                    Height = 75
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 100,
                    PositionY = 200,
                    Width = 75,
                    Height = 75
                }),
                new EditorItem(this, new Control()
                {
                    PositionX = 200,
                    PositionY = 100,
                    Width = 75,
                    Height = 75
                })
            };
            this.SelectedItems = new ObservableCollection<EditorItem>();
            this._ScaleFactor = 1.0;
            this._GridSize = 20;
            this._SnapToGrid = true;
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
