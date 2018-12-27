using ArmA.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArmA.Studio.UiEditor
{
    public class EditorDataContext :
        DockableBase,
        Data.UI.AttachedProperties.IOnDragEnter,
        Data.UI.AttachedProperties.IOnDragLeave,
        Data.UI.AttachedProperties.IOnDragOver,
        Data.UI.AttachedProperties.IOnDrop,
        Data.UI.AttachedProperties.IOnMouseMove,
        Data.UI.AttachedProperties.IOnMouseLeftButtonDown,
        Data.UI.AttachedProperties.IOnMouseLeftButtonUp,
        Data.UI.AttachedProperties.IOnMouseLeave
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
        public ObservableCollection<IEditorItemContent> Items { get => this._Items; set { this._Items = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<IEditorItemContent> _Items;

        public ObservableCollection<IEditorItemContent> SelectedItems { get => this._SelectedItems; set { this._SelectedItems = value; this.RaisePropertyChanged(); } }
        private ObservableCollection<IEditorItemContent> _SelectedItems;

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
            this._Items = new ObservableCollection<IEditorItemContent>
            {
                new Control()
                {
                    PositionX = 100,
                    PositionY = 100,
                    Width = 75,
                    Height = 75
                },
                new Control()
                {
                    PositionX = 200,
                    PositionY = 200,
                    Width = 75,
                    Height = 75
                },
                new Control()
                {
                    PositionX = 100,
                    PositionY = 200,
                    Width = 75,
                    Height = 75
                },
                new Control()
                {
                    PositionX = 200,
                    PositionY = 100,
                    Width = 75,
                    Height = 75
                }
            };
            this.SelectedItems = new ObservableCollection<IEditorItemContent>();
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
            this.Items.Add(new Control()
            {
                PositionX = position.X - 25,
                PositionY = position.Y - 25,
                Width = 50,
                Height = 50,
                Type = data.Type
            });
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

        public bool MouseLeftButtonDown
        {
            get => this._MouseLeftButtonDown;
            set
            {
                if (this._MouseLeftButtonDown == value)
                {
                    return;
                }
                this._MouseLeftButtonDown = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _MouseLeftButtonDown;
        public bool MouseDrag
        {
            get => this._MouseDrag;
            set
            {
                if (this._MouseDrag == value)
                {
                    return;
                }
                this._MouseDrag = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _MouseDrag;

        private double MouseDownX;
        private double MouseDownY;

        public EEditorMouseMode MouseMode
        {
            get => this._MouseMode;
            set
            {
                if (this._MouseMode == value)
                {
                    return;
                }
                this._MouseMode = value;
                this.RaisePropertyChanged();
            }
        }
        private EEditorMouseMode _MouseMode;

        public void OnMouseLeftButtonDown(UIElement sender, MouseButtonEventArgs e)
        {
            this.MouseLeftButtonDown = true;
            var pos = e.GetPosition(sender as IInputElement);
            this.MouseDownX = pos.X;
            this.MouseDownY = pos.Y;
        }
        public void OnMouseLeftButtonUp(UIElement sender, MouseButtonEventArgs e)
        {
            
            if (!this.MouseDrag)
            {
                var itemBelow = (sender as Canvas).GetChildBelowCursor();
                if (itemBelow == null)
                {
                    foreach (var it in this.SelectedItems)
                    {
                        it.IsSelected = false;
                    }
                    this.SelectedItems.Clear();
                }
                else
                {
                    var context = itemBelow.DataContext as IEditorItemContent;
                    var ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                    if (context.IsSelected)
                    {
                        if (this.SelectedItems.Count > 1 && !ctrl)
                        {
                            foreach (var it in this.SelectedItems.ToArray())
                            {
                                if (it == itemBelow)
                                {
                                    continue;
                                }
                                it.IsSelected = false;
                                this.SelectedItems.Remove(it);
                            }
                        }
                        else
                        {
                            context.IsSelected = false;
                            this.SelectedItems.Remove(context);
                        }
                    }
                    else
                    {
                        if (!ctrl)
                        {
                            foreach (var it in this.SelectedItems)
                            {
                                it.IsSelected = false;
                            }
                            this.SelectedItems.Clear();
                        }
                        context.IsSelected = true;
                        this.SelectedItems.Add(context);
                    }
                }
            }

            this.MouseLeftButtonDown = false;
            this.MouseDrag = false;
        }
        public void OnMouseMove(UIElement sender, MouseEventArgs e)
        {
            const int DRAGDIST = 2;
            var belowCursor = (sender as Canvas).GetChildBelowCursor();
            if (!this.MouseDrag)
            {
                if (belowCursor == null)
                {
                    this.MouseMode = EEditorMouseMode.NA;
                }
                else if (belowCursor.DataContext is IEditorItemContent belowCursorContext)
                {
                    int EDGE = (int)(8 * (1 / this.ScaleFactor));
                    var local = e.GetPosition(belowCursor);
                    var leftEdge = local.X < EDGE;
                    var topEdge = local.Y < EDGE;
                    var rightEdge = belowCursorContext.Width - local.X < EDGE;
                    var botEdge = belowCursorContext.Height - local.Y < EDGE;

                    if (!belowCursorContext.IsSelected) { this.MouseMode = EEditorMouseMode.Pick; }
                    else if (rightEdge && botEdge) { this.MouseMode = EEditorMouseMode.MoveSE; }
                    else if (leftEdge && botEdge) { this.MouseMode = EEditorMouseMode.MoveSW; }
                    else if (topEdge && leftEdge) { this.MouseMode = EEditorMouseMode.MoveNW; }
                    else if (topEdge && rightEdge) { this.MouseMode = EEditorMouseMode.MoveNE; }
                    else if (rightEdge) { this.MouseMode = EEditorMouseMode.MoveE; }
                    else if (leftEdge) { this.MouseMode = EEditorMouseMode.MoveW; }
                    else if (botEdge) { this.MouseMode = EEditorMouseMode.MoveS; }
                    else if (topEdge) { this.MouseMode = EEditorMouseMode.MoveN; }
                    else if (!belowCursorContext.IsSelected) { this.MouseMode = EEditorMouseMode.Pick; }
                    else { this.MouseMode = EEditorMouseMode.Move; }
                }
            }
            if (this.MouseLeftButtonDown)
            {
                var current = e.GetPosition(sender as IInputElement);
                var delta = new Point(current.X - this.MouseDownX, current.Y - this.MouseDownY);
                if (this.MouseMode == EEditorMouseMode.Move || this.MouseMode == EEditorMouseMode.Pick)
                {
                    if (this.MouseDrag ||
                        Math.Abs(delta.X) >= DRAGDIST ||
                        Math.Abs(delta.Y) >= DRAGDIST)
                    {
                        if (this.SnapToGrid)
                        {
                            delta.X -= delta.X % this.GridSize;
                            delta.Y -= delta.Y % this.GridSize;
                        }
                        if (belowCursor != null && belowCursor.DataContext is IEditorItemContent belowCursorContext && !belowCursorContext.IsSelected && !this.MouseDrag)
                        {
                            belowCursorContext.IsSelected = true;
                            this.SelectedItems.Add(belowCursorContext);
                        }
                        this.MouseDrag = true;
                        if (delta.X != 0) { this.MouseDownX = current.X; }
                        if (delta.Y != 0) { this.MouseDownY = current.Y; }
                        foreach (var it in this.SelectedItems)
                        {
                            it.PositionX += (float)delta.X;
                            it.PositionY += (float)delta.Y;
                        }
                    }
                }
                else if (this.MouseMode != EEditorMouseMode.NA)
                {
                    if (belowCursor != null && belowCursor.DataContext is IEditorItemContent belowCursorContext && !belowCursorContext.IsSelected && !this.MouseDrag)
                    {
                        belowCursorContext.IsSelected = true;
                        this.SelectedItems.Add(belowCursorContext);
                    }
                    this.MouseDrag = true;
                    foreach (var it in this.SelectedItems)
                    {
                        var width = it.Width;
                        var height = it.Height;
                        var positionX = it.PositionX;
                        var positionY = it.PositionY;
                        switch (this.MouseMode)
                        {
                            case EEditorMouseMode.MoveE:
                                {
                                    width = it.Width + ((float)delta.X);
                                }
                                break;
                            case EEditorMouseMode.MoveW:
                                {
                                    width = it.Width - ((float)delta.X);
                                    positionX = it.PositionX + ((float)delta.X);
                                }
                                break;
                            case EEditorMouseMode.MoveS:
                                {
                                    height = it.Height + ((float)delta.Y);
                                }
                                break;
                            case EEditorMouseMode.MoveN:
                                {
                                    height = it.Height - ((float)delta.Y);
                                    positionY = it.PositionY + ((float)delta.Y);
                                }
                                break;
                            case EEditorMouseMode.MoveSE:
                                {
                                    height = it.Height + ((float)delta.Y);
                                    width = it.Width + ((float)delta.X);
                                }
                                break;
                            case EEditorMouseMode.MoveSW:
                                {
                                    height = it.Height + ((float)delta.Y);
                                    width = it.Width - ((float)delta.X);
                                    positionX = it.PositionX + ((float)delta.X);
                                }
                                break;
                            case EEditorMouseMode.MoveNE:
                                {
                                    height = it.Height - ((float)delta.Y);
                                    positionY = it.PositionY + ((float)delta.Y);
                                    width = it.Width + ((float)delta.X);
                                }
                                break;
                            case EEditorMouseMode.MoveNW:
                                {
                                    height = it.Height - ((float)delta.Y);
                                    positionY = it.PositionY + ((float)delta.Y);
                                    width = it.Width - ((float)delta.X);
                                    positionX = it.PositionX + ((float)delta.X);
                                }
                                break;
                        }

                        var xUpdated = false;
                        var yUpdated = false;
                        /*if(this.SnapToGrid)
                        {
                            width = Math.Round((width % this.GridSize) / this.GridSize / 2) == 0 ? (width - (width % this.GridSize)) : (width + (this.GridSize - (width % this.GridSize)));
                            height = Math.Round((height % this.GridSize) / this.GridSize / 2) == 0 ? (height - (height % this.GridSize)) : (height + (this.GridSize - (height % this.GridSize)));
                            positionX = Math.Round((positionX % this.GridSize) / this.GridSize / 2) == 0 ? (positionX - (positionX % this.GridSize)) : (positionX + (this.GridSize - (positionX % this.GridSize)));
                            positionY = Math.Round((positionY % this.GridSize) / this.GridSize / 2) == 0 ? (positionY - (positionY % this.GridSize)) : (positionY + (this.GridSize - (positionY % this.GridSize)));
                        }*/
                        if (width != it.Width)
                        {
                            xUpdated = true;
                            it.Width = width;
                        }
                        if (height != it.Height)
                        {
                            yUpdated = true;
                            it.Height = height;
                        }
                        if (positionX != it.PositionX)
                        {
                            xUpdated = true;
                            it.PositionX = positionX;
                        }
                        if (positionY != it.PositionY)
                        {
                            yUpdated = true;
                            it.PositionY = positionY;
                        }

                        if (xUpdated) { this.MouseDownX = current.X; }
                        if (yUpdated) { this.MouseDownY = current.Y; }
                    }
                }
            }
        }
        public void OnMouseLeave(UIElement sender, MouseEventArgs e)
        {
            this.MouseLeftButtonDown = false;
            this.MouseDrag = false;
        }
    }
}
