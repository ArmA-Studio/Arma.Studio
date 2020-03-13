using Arma.Studio.Data;
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
        IOnMouseEnter,
        IOnMouseMove,
        IOnInitialized,
        IOnPreviewMouseWheel,
        IOnDragEnter,
        IOnDragLeave,
        IOnDragOver,
        IOnDrop,
        Studio.Data.UI.IKeyInteractible
    {

        public enum EBackgroundMode
        {
            NA,
            Grid,
            Arma
        }

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
                (Application.Current as IApp).MainWindow.PropertyHost = value is null && this.SelectedNodes.Count == 0 ? (Studio.Data.UI.IPropertyHost)this.Owner : value;
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
        #region Property: Zoom (System.Double)
        public double Zoom
        {
            get => this._Zoom;
            set
            {
                this._Zoom = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Zoom;
        #endregion
        #region Property: HorizontalScrollOffset (System.Double)
        public double HorizontalScrollOffset
        {
            get => this._HorizontalScrollOffset;
            set
            {
                this._HorizontalScrollOffset = value;
                this.RaisePropertyChanged();
            }
        }
        private double _HorizontalScrollOffset;
        #endregion
        #region Property: VerticalScrollOffset (System.Double)
        public double VerticalScrollOffset
        {
            get => this._VerticalScrollOffset;
            set
            {
                this._VerticalScrollOffset = value;
                this.RaisePropertyChanged();
            }
        }
        private double _VerticalScrollOffset;
        #endregion
        #region Property: Cursor (System.Windows.Input.Cursor)
        public Cursor Cursor
        {
            get => this._Cursor;
            set
            {
                this._Cursor = value;
                this.RaisePropertyChanged();
            }
        }
        private Cursor _Cursor;
        #endregion
        #region Property: HighlightAll (System.Boolean)
        public bool HighlightAll
        {
            get => this._HighlightAll;
            set
            {
                this._HighlightAll = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _HighlightAll;
        #endregion

        public CanvasManager(UiEditorDataContext owner)
        {
            this.SelectedNodes = new ObservableCollection<IControlElement>();
            this.GridSize = 20;
            this.Owner = owner;
            this.ShowGrid = true;
            this.PreMovePositions = new List<Tuple<IControlElement, Point>>();
            this.Width = 1920;
            this.Height = 1080;
            this.Zoom = 0.5;
            this.Cursor = Cursors.Arrow;
            this.BackgroundMode = EBackgroundMode.Grid;
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
            foreach (var it in this.Owner.BackgroundControls)
            {
                this.SelectedNodes.Add(it);
            }
            foreach (var it in this.Owner.ForegroundControls)
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
        #region Property: BackgroundMode (EBackgroundMode)
        public EBackgroundMode BackgroundMode
        {
            get => this._BackgroundMode;
            set
            {
                this._BackgroundMode = value;
                this.RaisePropertyChanged();
            }
        }
        private EBackgroundMode _BackgroundMode;
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

        #region Property: EditorMouseMode (EEditorMouseMode)
        public EEditorMouseMode EditorMouseMode
        {
            get => this._EditorMouseMode;
            set
            {
                this._EditorMouseMode = value;
                this.RaisePropertyChanged();
            }
        }
        private EEditorMouseMode _EditorMouseMode;
        #endregion
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
            (Application.Current as IApp).MainWindow.KeyInteractible = null;
        }
        #endregion
        #region IOnMouseMove
        private List<Tuple<IControlElement, Point>> PreMovePositions;
        public void OnMouseMove(UIElement sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this.Canvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.IsLeftMouseButtonDown)
                {
                    var deltaPosX = mousePosition.X - this.MouseMovePosition.X;
                    var deltaPosY = mousePosition.Y - this.MouseMovePosition.Y;
                    var isMouseMoveDeltaReached = SystemParameters.MinimumHorizontalDragDistance <= Math.Abs(deltaPosX) ||
                        SystemParameters.MinimumVerticalDragDistance <= Math.Abs(deltaPosY);
                    if (this.IsMouseMoveActive || (isMouseMoveDeltaReached && this.MouseDownHadChildBelow))
                    {
                        if (!this.IsMouseMoveActive)
                        {
                            this.PreMovePositions = this.SelectedNodes.Select((it) => new Tuple<IControlElement, Point>(it, new Point(it.Left, it.Top))).ToList();
                        }
                        this.MouseMovePosition = mousePosition;
                        this.IsMouseMoveActive = true;
                        foreach (var node in this.SelectedNodes)
                        {
                            switch (this.EditorMouseMode)
                            {
                                case EEditorMouseMode.MoveN:
                                    {
                                        var newTop = node.Top + deltaPosY;
                                        if (newTop < 0) { newTop = 0; }
                                        else if (newTop > this.Height - node.Height) { newTop = this.Height - node.Height; }
                                        var topDelta = node.Top - newTop;
                                        var newHeight = node.Height + topDelta;
                                        node.Height = newHeight < 0 ? 0 : newHeight;
                                        if (node.Height != 0)
                                        {
                                            node.Top = newTop;
                                        }
                                    }
                                    break;
                                case EEditorMouseMode.MoveE:
                                    {
                                        var newWidth = node.Width + deltaPosX;
                                        if (newWidth < 0) { newWidth = 0; }
                                        else if (newWidth > this.Width - node.Left) { newWidth = this.Width - node.Left; }
                                        node.Width = newWidth;
                                    }
                                    break;
                                case EEditorMouseMode.MoveS:
                                    {
                                        var newHeight = node.Height + deltaPosY;
                                        if (newHeight < 0) { newHeight = 0; }
                                        else if (newHeight > this.Height - node.Top) { newHeight = this.Height - node.Top; }
                                        node.Height = newHeight;
                                    }
                                    break;
                                case EEditorMouseMode.MoveW:
                                    {
                                        var newLeft = node.Left + deltaPosX;
                                        if (newLeft < 0) { newLeft = 0; }
                                        else if (newLeft > this.Width - node.Width) { newLeft = this.Width - node.Width; }
                                        var leftDelta = node.Left - newLeft;
                                        var newWidth = node.Width + leftDelta;
                                        node.Width = newWidth < 0 ? 0 : newWidth;
                                        if (node.Width != 0)
                                        {
                                            node.Left = newLeft;
                                        }
                                    }
                                    break;
                                case EEditorMouseMode.MoveNE:
                                    {
                                        var newTop = node.Top + deltaPosY;
                                        if (newTop < 0) { newTop = 0; }
                                        else if (newTop > this.Height - node.Height) { newTop = this.Height - node.Height; }
                                        var topDelta = node.Top - newTop;
                                        var newHeight = node.Height + topDelta;
                                        node.Height = newHeight < 0 ? 0 : newHeight;
                                        if (node.Height != 0)
                                        {
                                            node.Top = newTop;
                                        }
                                    }
                                    {
                                        var newWidth = node.Width + deltaPosX;
                                        if (newWidth < 0) { newWidth = 0; }
                                        else if (newWidth > this.Width - node.Left) { newWidth = this.Width - node.Left; }
                                        node.Width = newWidth;
                                    }
                                    break;
                                case EEditorMouseMode.MoveNW:
                                    {
                                        var newTop = node.Top + deltaPosY;
                                        if (newTop < 0) { newTop = 0; }
                                        else if (newTop > this.Height - node.Height) { newTop = this.Height - node.Height; }
                                        var topDelta = node.Top - newTop;
                                        var newHeight = node.Height + topDelta;
                                        node.Height = newHeight < 0 ? 0 : newHeight;
                                        if (node.Height != 0)
                                        {
                                            node.Top = newTop;
                                        }
                                    }
                                    {
                                        var newLeft = node.Left + deltaPosX;
                                        if (newLeft < 0) { newLeft = 0; }
                                        else if (newLeft > this.Width - node.Width) { newLeft = this.Width - node.Width; }
                                        var leftDelta = node.Left - newLeft;
                                        var newWidth = node.Width + leftDelta;
                                        node.Width = newWidth < 0 ? 0 : newWidth;
                                        if (node.Width != 0)
                                        {
                                            node.Left = newLeft;
                                        }
                                    }
                                    break;
                                case EEditorMouseMode.MoveSE:
                                    {
                                        var newHeight = node.Height + deltaPosY;
                                        if (newHeight < 0) { newHeight = 0; }
                                        else if (newHeight > this.Height - node.Top) { newHeight = this.Height - node.Top; }
                                        node.Height = newHeight;
                                    }
                                    {
                                        var newWidth = node.Width + deltaPosX;
                                        if (newWidth < 0) { newWidth = 0; }
                                        else if (newWidth > this.Width - node.Left) { newWidth = this.Width - node.Left; }
                                        node.Width = newWidth;
                                    }
                                    break;
                                case EEditorMouseMode.MoveSW:
                                    {
                                        var newHeight = node.Height + deltaPosY;
                                        if (newHeight < 0) { newHeight = 0; }
                                        else if (newHeight > this.Height - node.Top) { newHeight = this.Height - node.Top; }
                                        node.Height = newHeight;
                                    }
                                    {
                                        var newLeft = node.Left + deltaPosX;
                                        if (newLeft < 0) { newLeft = 0; }
                                        else if (newLeft > this.Width - node.Width) { newLeft = this.Width - node.Width; }
                                        var leftDelta = node.Left - newLeft;
                                        var newWidth = node.Width + leftDelta;
                                        node.Width = newWidth < 0 ? 0 : newWidth;
                                        if (node.Width != 0)
                                        {
                                            node.Left = newLeft;
                                        }
                                    }
                                    break;
                                case EEditorMouseMode.Move:
                                case EEditorMouseMode.NA:
                                default:
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
                                    break;
                            }
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
                        this.SelectionHelper.Move(mousePosition);
                        var newlyInSquare = this.Canvas.GetChildrenInsideTouching(this.SelectionHelper).Where((it) => it.DataContext is IControlElement).ToArray();
                        var deltaInSquare = currentlyInSquare.Except(newlyInSquare).Union(newlyInSquare.Except(currentlyInSquare));
                        foreach (var frameworkElement in deltaInSquare)
                        {
                            this.MultiSelect(frameworkElement.DataContext as IControlElement);
                        }
                    }
                }
            }
            else if (this.IsMouseSelectionSquareActive)
            {
                this.IsMouseSelectionSquareActive = false;
                this.SelectionHelper = null;
            }
            else
            {
                var itemsBelow = this.Canvas.GetChildrenBelowCursor().Select((it) => it.DataContext).Cast<IControlElement>().ToArray();
                int borderCoef = (int)(6 / this.Zoom);
                foreach (var it in itemsBelow.Take(1))
                {
                    var rect = new Rect(it.Left, it.Top, it.Width, it.Height);
                    bool isTop() => mousePosition.Y >= rect.Top && mousePosition.Y <= rect.Top + borderCoef;
                    bool isBot() => mousePosition.Y >= rect.Bottom - borderCoef && mousePosition.Y <= rect.Bottom;
                    bool isLeft() => mousePosition.X >= rect.Left && mousePosition.X <= rect.Left + borderCoef;
                    bool isRight() => mousePosition.X >= rect.Right - borderCoef && mousePosition.X <= rect.Right;


                    if (isLeft() && isTop())
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        this.EditorMouseMode = EEditorMouseMode.MoveNW;
                    }
                    else if (isTop() && isRight())
                    {
                        this.Cursor = Cursors.SizeNESW;
                        this.EditorMouseMode = EEditorMouseMode.MoveNE;
                    }
                    else if (isBot() && isLeft())
                    {
                        this.Cursor = Cursors.SizeNESW;
                        this.EditorMouseMode = EEditorMouseMode.MoveSW;
                    }
                    else if (isBot() && isRight())
                    {
                        this.Cursor = Cursors.SizeNWSE;
                        this.EditorMouseMode = EEditorMouseMode.MoveSE;
                    }
                    else if (isLeft())
                    {
                        this.Cursor = Cursors.SizeWE;
                        this.EditorMouseMode = EEditorMouseMode.MoveW;
                    }
                    else if (isRight())
                    {
                        this.Cursor = Cursors.SizeWE;
                        this.EditorMouseMode = EEditorMouseMode.MoveE;
                    }
                    else if (isTop())
                    {
                        this.Cursor = Cursors.SizeNS;
                        this.EditorMouseMode = EEditorMouseMode.MoveN;
                    }
                    else if (isBot())
                    {
                        this.Cursor = Cursors.SizeNS;
                        this.EditorMouseMode = EEditorMouseMode.MoveS;
                    }
                    else
                    {
                        this.Cursor = Cursors.Hand;
                        this.EditorMouseMode = EEditorMouseMode.Move;
                    }
                }
                if (!itemsBelow.Any())
                {
                    this.Cursor = Cursors.Arrow;
                    this.EditorMouseMode = EEditorMouseMode.NA;
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

        public void OnPreviewMouseWheel(UIElement sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && (Application.Current as IApp).MainWindow.ActiveDockable == this.Owner)
            {
                e.Handled = true;

                // Get current relative position of mouse
                var currentMousePosition = e.GetPosition(this.ScrollViewer);
                var currentDeltaMouseLeft = currentMousePosition.X / this.ScrollViewer.ActualWidth;
                var currentDeltaMouseTop = currentMousePosition.Y / this.ScrollViewer.ActualHeight;

                // Calculate & apply new zoom level
                var zoomDelta = e.Delta / 1000.0;
                var oldZoom = this.Zoom;
                var newZoom = this.Zoom * (1 + zoomDelta);
                newZoom = newZoom > 0.1 ? (newZoom < 5 ? newZoom : 5) : 0.1;
                this.Zoom = newZoom;

                // Force Update on layouts of canvas & ScrollViewer
                this.Canvas.UpdateLayout();
                this.ScrollViewer.UpdateLayout();

                // Adjust position of Horizontal- & VerticalOffset according to previous MouseDelta
                this.ScrollViewer.ScrollToHorizontalOffset(
                    this.ScrollViewer.HorizontalOffset +
                    (currentDeltaMouseLeft * this.ScrollViewer.ActualWidth * (newZoom - oldZoom)));
                this.ScrollViewer.ScrollToVerticalOffset(
                    this.ScrollViewer.VerticalOffset +
                    (currentDeltaMouseTop * this.ScrollViewer.ActualHeight * (newZoom - oldZoom)));
            }
        }

        public void OnDragEnter(UIElement sender, DragEventArgs e)
        {
            this.Owner.OnDragEnter(sender, e);
        }

        public void OnDragLeave(UIElement sender, DragEventArgs e)
        {
            this.Owner.OnDragLeave(sender, e);
        }

        public void OnDragOver(UIElement sender, DragEventArgs e)
        {
            this.Owner.OnDragOver(sender, e);
        }

        public void OnDrop(UIElement sender, DragEventArgs e)
        {
            this.Owner.OnDrop(sender, e);
        }

        public void OnMouseEnter(UIElement sender, MouseEventArgs e)
        {
            (Application.Current as IApp).MainWindow.KeyInteractible = this;
        }

        public bool KeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Delete)
            {
                foreach (var it in this.SelectedNodes)
                {
                    if (this.Owner.ForegroundControls.Contains(it))
                    {
                        this.Owner.ForegroundControls.Remove(it);
                    }
                    else if (this.Owner.BackgroundControls.Contains(it))
                    {
                        this.Owner.BackgroundControls.Remove(it);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
