using Arma.Studio.Data.UI.AttachedProperties;
using Arma.Studio.UiEditor.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arma.Studio.UiEditor.UI
{

    public class CanvasManager : INotifyPropertyChanged,
        IOnMouseDown,
        IOnMouseUp,
        IOnMouseLeave,
        IOnMouseMove,
        IOnInitialized
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));
        #region Collection: SelectedNodes (System.Collections.ObjectModel.ObservableCollection<Data.IControlElement>)
        public ObservableCollection<Data.IControlElement> SelectedNodes
        {
            get => this._SelectedNodes;
            private set
            {
                if (this._SelectedNodes != null)
                {
                    foreach (var it in this._SelectedNodes)
                    {
                        it.IsSelected = false;
                    }
                    this._SelectedNodes.CollectionChanged -= this.SelectedNodes_CollectionChanged;
                }
                this._SelectedNodes = value;
                if (this._SelectedNodes != null)
                {
                    foreach (var it in this._SelectedNodes)
                    {
                        it.IsSelected = true;
                    }
                    this._SelectedNodes.CollectionChanged += this.SelectedNodes_CollectionChanged;
                }
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<Data.IControlElement> _SelectedNodes;
        #endregion
        public IControlElement SelectedNode
        {
            get => this._SelectedNode;
            set
            {
                this._SelectedNode = value;
                this.RaisePropertyChanged();
            }
        }
        private IControlElement _SelectedNode;

        #region Property: SelectionHelper (SelectionHelper)
        public SelectionHelper SelectionHelper
        {
            get => this._SelectionHelper;
            set
            {
                this._SelectionHelper = value;
                this.RaisePropertyChanged();
            }
        }
        private SelectionHelper _SelectionHelper;
        #endregion

        public CanvasManager(UiEditorDataContext owner)
        {
            this.SelectedNodes = new ObservableCollection<IControlElement>();
            this.GridSize = 20;
            this.Owner = owner;
            this.ShowGrid = true;
            this.PreMovePositions = new List<Tuple<IControlElement, Point>>();
        }
        private readonly UiEditorDataContext Owner;

        private void SelectedNodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IControlElement node in e.NewItems ?? Array.Empty<object>())
            {
                node.IsSelected = true;
            }
            foreach (IControlElement node in e.OldItems ?? Array.Empty<object>())
            {
                node.IsSelected = false;
            }
        }

        public void SelectAll()
        {
            foreach (var it in this.Owner.ControlElements)
            {
                this.SelectedNodes.Add(it);
            }
            this.SelectedNode = this.SelectedNodes.Count == 1 ? this.SelectedNodes.First() : null;
        }
        public void Select(IControlElement node)
        {
            this.SelectedNodes = new ObservableCollection<IControlElement>();
            this.SelectedNodes.Add(node);
            this.SelectedNode = node;
        }
        public bool IsSelected(IControlElement node)
        {
            return this.SelectedNodes.Contains(node);
        }
        public void MultiSelect(IControlElement node)
        {
            if (this.SelectedNodes.Contains(node))
            {
                this.SelectedNodes.Remove(node);
            }
            else
            {
                this.SelectedNodes.Add(node);
            }
            this.SelectedNode = this.SelectedNodes.Count == 1 ? this.SelectedNodes.First() : null;
        }

        public void Clear()
        {
            this.SelectedNodes = new ObservableCollection<IControlElement>();
            this.SelectedNode = null;
        }


        #region Property: GridSize (System.Int32)
        public int GridSize
        {
            get => this._GridSize;
            set
            {
                this._GridSize = value;
                this.RaisePropertyChanged();
            }
        }
        private int _GridSize;
        #endregion
        #region Property: Width (System.Double)
        public double Width
        {
            get => this._Width;
            set
            {
                this._Width = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Width;
        #endregion
        #region Property: Height (System.Double)
        public double Height
        {
            get => this._Height;
            set
            {
                this._Height = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Height;
        #endregion

        #region Property: ShowGrid (System.Boolean)
        public bool ShowGrid
        {
            get => this._ShowGrid;
            set
            {
                this._ShowGrid = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _ShowGrid;
        #endregion

        public void SetPosition(IControlElement node, Point p) => this.SetPosition(node, p.X, p.Y);
        public void SetPosition(IControlElement node, double left, double top)
        {
            var p = this.GetPosition(node, left, top);
            node.Left = p.X;
            node.Top = p.Y;
        }
        public Point GetPosition(IControlElement node, double left, double top)
        {
            var gridX = this.GridSize;
            var gridY = this.GridSize;
            var halfNodeWidth = node.Width / 2;
            var halfNodeHeight = node.Height / 2;
            if (gridX > 0)
            {
                // Check if mid is closer to centered-left or centered-right
                var deltaX = (left + halfNodeWidth) % gridX;
                if (deltaX > gridX / 2)
                { // right
                    // Move left to grid-based left
                    left = ((int)(left + halfNodeWidth) / gridX + 1) * gridX;
                    // Center left
                    left -= halfNodeWidth;
                }
                else
                { // left
                    // Move left to grid-based left
                    left = ((int)(left + halfNodeWidth) / gridX) * gridX;
                    // Center left
                    left -= halfNodeWidth;
                }
            }
            if (gridY > 0)
            {
                // Check if mid is closer to centered-top or centered-bot
                var deltaY = (top + halfNodeHeight) % gridY;
                if (deltaY > gridY / 2)
                { // bot
                    // Move top to grid-based top
                    top = ((int)(top + halfNodeHeight) / gridY + 1) * gridY;
                    // Center top
                    top -= halfNodeHeight;
                }
                else
                { // top
                    top = ((int)(top + halfNodeHeight) / gridY) * gridY - halfNodeHeight;
                }
            }
            if (left < 0) { left = 0; }
            else if (left > this.Width - node.Width) { left = this.Width - node.Width; }
            if (top < 0) { top = 0; }
            else if (top > this.Height - node.Height) { top = this.Height - node.Height; }
            return new Point(left, top);
        }

        #region Property: IsMouseDown (System.Boolean)
        public bool IsLeftMouseButtonDown
        {
            get => this._IsLeftMouseButtonDown;
            set
            {
                this._IsLeftMouseButtonDown = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsLeftMouseButtonDown;
        #endregion
        #region Property: IsMouseMoveActive (System.Boolean)
        public bool IsMouseMoveActive
        {
            get => this._IsMouseMoveActive;
            set
            {
                this._IsMouseMoveActive = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsMouseMoveActive;
        #endregion
        #region Property: IsMouseSelectionSquareActive (System.Boolean)
        public bool IsMouseSelectionSquareActive
        {
            get => this._IsMouseSelectionSquareActive;
            set
            {
                this._IsMouseSelectionSquareActive = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsMouseSelectionSquareActive;
        #endregion
        #region Property: MouseMovePosition (System.Windows.Point)
        public Point MouseMovePosition
        {
            get => this._MouseMovePosition;
            set
            {
                this._MouseMovePosition = value;
                this.RaisePropertyChanged();
            }
        }
        private Point _MouseMovePosition;
        #endregion
        #region Property: LastMouseRightClickPosition (System.Windows.Point)
        public Point LastMouseRightClickPosition
        {
            get => this._LastMouseRightClickPosition;
            set
            {
                this._LastMouseRightClickPosition = value;
                this.RaisePropertyChanged();
            }
        }
        private Point _LastMouseRightClickPosition;
        #endregion
        private bool MouseDownHadChildBelow;
        #region IOnMouseDown
        public void OnMouseDown(UIElement sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var mousePosition = e.GetPosition(sender);
                this.IsMouseMoveActive = false;
                this.MouseMovePosition = mousePosition;
                this.IsLeftMouseButtonDown = true;
                if (sender is Canvas canvas)
                {
                    var children = canvas.GetChildrenBelowCursor();
                    var ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
                    if (children.Where((it) => it.DataContext is IControlElement node).Any())
                    {
                        var node = children
                            .Where((it) => it.DataContext is IControlElement)
                            .Select((it) => it.DataContext as IControlElement)
                            .First();
                        if (!ctrl && !this.IsSelected(node))
                        {
                            this.Select(node);
                            e.Handled = true;
                        }
                        this.MouseDownHadChildBelow = true;
                    }
                    else if (!ctrl)
                    {
                        this.MouseDownHadChildBelow = false;
                        e.Handled = true;
                    }
                }
            }
        }
        #endregion
        #region IOnMouseUp
        public void OnMouseUp(UIElement sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    if (this.IsMouseSelectionSquareActive)
                    {
                        this.IsMouseSelectionSquareActive = false;
                        this.SelectionHelper = null;
                    }
                    else if (!this.IsMouseMoveActive)
                    {
                        var children = canvas.GetChildrenBelowCursor();

                        var ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
                        if (children.Where((it) => it.DataContext is IControlElement node).Any())
                        {
                            var node = children
                                .Where((it) => it.DataContext is IControlElement)
                                .Select((it) => it.DataContext as IControlElement)
                                .First();
                            if (ctrl)
                            {
                                this.MultiSelect(node);
                                e.Handled = true;
                            }
                            else if (this.IsSelected(node))
                            {
                                this.Select(node);
                                e.Handled = true;
                            }
                        }
                        else if (!ctrl)
                        { // If nothing is below mouse && CTRL is not pressed, clear selection
                            this.Clear();
                        }
                    }
                    else
                    {
                        var newPositions = this.PreMovePositions
                            .Select((it) => new Tuple<IControlElement, Point>(it.Item1, this.GetPosition(it.Item1, it.Item1.Left, it.Item1.Top)))
                            .ToDictionary((it) => it.Item1, (it) => it.Item2);
                        var selectedNodes = this.SelectedNodes.ToArray();
                        foreach (var node in selectedNodes)
                        {
                            var pos = newPositions[node];
                            node.Left = pos.X;
                            node.Top = pos.Y;
                        }
                        e.Handled = true;
                    }
                    this.IsLeftMouseButtonDown = false;
                    this.IsMouseMoveActive = false;
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    this.LastMouseRightClickPosition = e.GetPosition(sender);
                }
            }
        }
        #endregion
        #region IOnMouseLeave
        public void OnMouseLeave(UIElement sender, MouseEventArgs e)
        {
            if (sender is Canvas)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.IsLeftMouseButtonDown = false;
                this.IsMouseMoveActive = false;
            }
            if (this.IsMouseSelectionSquareActive)
            {
                this.IsMouseSelectionSquareActive = false;
                this.SelectionHelper = null;
            }
        }
        #endregion
        #region IOnMouseMove
        private List<Tuple<IControlElement, Point>> PreMovePositions;
        public void OnMouseMove(UIElement sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.IsLeftMouseButtonDown)
                {
                    var newPos = e.GetPosition(this.Canvas);
                    var deltaPosX = newPos.X - this.MouseMovePosition.X;
                    var deltaPosY = newPos.Y - this.MouseMovePosition.Y;
                    var isMouseMoveDeltaReached = SystemParameters.MinimumHorizontalDragDistance <= Math.Abs(deltaPosX) ||
                        SystemParameters.MinimumVerticalDragDistance <= Math.Abs(deltaPosY);
                    if (this.IsMouseMoveActive || (isMouseMoveDeltaReached && this.MouseDownHadChildBelow))
                    {
                        if (!this.IsMouseMoveActive)
                        {
                            this.PreMovePositions = this.SelectedNodes.Select((it) => new Tuple<IControlElement, Point>(it, new Point(it.Left, it.Top))).ToList();
                        }
                        this.MouseMovePosition = newPos;
                        this.IsMouseMoveActive = true;
                        foreach (var node in this.SelectedNodes)
                        {
                            var newLeft = node.Left + deltaPosX;
                            if (newLeft < 0) { newLeft = 0; }
                            else if (newLeft > this.Width - node.Width) { newLeft = this.Width - node.Width; }
                            node.Left = newLeft;

                            var newTop = node.Top + deltaPosY;
                            if (newTop < 0) { newTop = 0; }
                            else if (newTop > this.Height - node.Height) { newTop = this.Height - node.Height; }
                            node.Top = newTop;
                        }
                    }
                    else if (this.IsMouseSelectionSquareActive || (isMouseMoveDeltaReached && !this.MouseDownHadChildBelow))
                    {
                        if (!this.IsMouseSelectionSquareActive)
                        {
                            this.IsMouseSelectionSquareActive = true;
                            this.SelectionHelper = new SelectionHelper(this.MouseMovePosition.X, this.MouseMovePosition.Y);
                            if (!this.MouseDownHadChildBelow && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                            {
                                this.Clear();
                            }
                        }
                        var currentlyInSquare = this.Canvas.GetChildrenInsideTouching(this.SelectionHelper).Where((it) => it.DataContext is IControlElement).ToArray();
                        this.SelectionHelper.Move(newPos);
                        var newlyInSquare = this.Canvas.GetChildrenInsideTouching(this.SelectionHelper).Where((it) => it.DataContext is IControlElement).ToArray();
                        var deltaInSquare = currentlyInSquare.Except(newlyInSquare).Union(newlyInSquare.Except(currentlyInSquare));
                        foreach (var frameworkElement in deltaInSquare)
                        {
                            this.MultiSelect(frameworkElement.DataContext as IControlElement);
                        }
                    }
                }
            }
        }
        #endregion
        public Canvas Canvas { get; private set; }
        public ScrollViewer ScrollViewer { get; private set; }
        public void OnInitialized(FrameworkElement sender, EventArgs e)
        {
            if (sender is Canvas canvas)
            {
                this.Canvas = canvas;
            }
            else if (sender is ScrollViewer scrollViewer)
            {
                this.ScrollViewer = scrollViewer;
            }
        }

    }
}
