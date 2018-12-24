using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArmA.Studio.UiEditor
{
    public class EditorItem : INotifyPropertyChanged,
        Data.UI.AttachedProperties.IOnMouseLeftButtonDown,
        Data.UI.AttachedProperties.IOnMouseLeftButtonUp,
        Data.UI.AttachedProperties.IOnMouseMove,
        Data.UI.AttachedProperties.IOnMouseLeave
    {
        // Most stuff is in https://community.bistudio.com/wiki/Dialog_Control#Controls
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        public IEditorItemContent Wrapped { get => this._Wrapped; set { this._Wrapped = value; this.RaisePropertyChanged(); } }
        private IEditorItemContent _Wrapped;

        public bool IsSelected
        {
            get => this._IsSelected;
            set
            {
                if (this._IsSelected == value)
                {
                    return;
                }
                this._IsSelected = value;
                if (value)
                {
                    this.Owner.SelectedItems.Add(this);
                }
                else
                {
                    this.Owner.SelectedItems.Remove(this);
                    this.MouseMode = EEditorMouseMode.NA;
                }
                this.RaisePropertyChanged();
            }
        }
        private bool _IsSelected;



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
        public Point MouseDownPosition
        {
            get => this._MouseDownPosition;
            set
            {
                if (this._MouseDownPosition == value)
                {
                    return;
                }
                this._MouseDownPosition = value;
                this.RaisePropertyChanged();
            }
        }
        private Point _MouseDownPosition;

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
            this.MouseDownPosition = e.GetPosition(sender.FindParent<Canvas>() as IInputElement);
        }

        public void OnMouseLeftButtonUp(UIElement sender, MouseButtonEventArgs e)
        {
            if (!this.MouseDrag)
            {
                if (this.IsSelected)
                {
                    if (this.Owner.SelectedItems.Count > 1 && !Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        foreach (var it in this.Owner.SelectedItems.ToArray())
                        {
                            if (it == this)
                            {
                                continue;
                            }
                            it.IsSelected = false;
                        }
                    }
                    else
                    {
                        this.IsSelected = false;
                    }
                }
                else
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        foreach (var it in this.Owner.SelectedItems.ToArray())
                        {
                            it.IsSelected = false;
                        }
                    }
                    this.IsSelected = true;
                }
            }

            this.MouseLeftButtonDown = false;
            this.MouseDrag = false;
        }
        public void OnMouseMove(UIElement sender, MouseEventArgs e)
        {
            const int EDGE = 8;
            const int DRAGDIST = 2;
            {
                if (this.IsSelected && !this.MouseDrag)
                {
                    var local = e.GetPosition(sender);
                    var leftEdge = local.X < EDGE;
                    var topEdge = local.Y < EDGE;
                    var rightEdge = this.Wrapped.Width - local.X < EDGE;
                    var botEdge = this.Wrapped.Height - local.Y < EDGE;

                    if (rightEdge && botEdge) { this.MouseMode = EEditorMouseMode.MoveSE; }
                    else if (leftEdge && botEdge) { this.MouseMode = EEditorMouseMode.MoveSW; }
                    else if (topEdge && leftEdge) { this.MouseMode = EEditorMouseMode.MoveNW; }
                    else if (topEdge && rightEdge) { this.MouseMode = EEditorMouseMode.MoveNE; }
                    else if (rightEdge) { this.MouseMode = EEditorMouseMode.MoveE; }
                    else if (leftEdge) { this.MouseMode = EEditorMouseMode.MoveW; }
                    else if (botEdge) { this.MouseMode = EEditorMouseMode.MoveS; }
                    else if (topEdge) { this.MouseMode = EEditorMouseMode.MoveN; }
                    else { this.MouseMode = EEditorMouseMode.Move; }
                }
            }
            {
                if (this.MouseLeftButtonDown && this.IsSelected)
                {
                    var current = e.GetPosition(sender.FindParent<Canvas>() as IInputElement);
                    var delta = new Point(current.X - this.MouseDownPosition.X, current.Y - this.MouseDownPosition.Y);
                    if (this.MouseDrag ||
                        Math.Abs(delta.X) >= DRAGDIST ||
                        Math.Abs(delta.Y) >= DRAGDIST)
                    {
                        this.MouseDrag = true;
                        this.IsSelected = true;
                        this.MouseDownPosition = current;
                        foreach (var it in this.Owner.SelectedItems)
                        {
                            switch (this.MouseMode)
                            {
                                case EEditorMouseMode.Move:
                                    it.Wrapped.PositionX += (float)delta.X;
                                    it.Wrapped.PositionY += (float)delta.Y;
                                    break;
                                case EEditorMouseMode.MoveE:
                                    it.Wrapped.Width += (float)delta.X;
                                    break;
                                case EEditorMouseMode.MoveW:
                                    it.Wrapped.Width -= (float)delta.X;
                                    it.Wrapped.PositionX += (float)delta.X;
                                    break;
                                case EEditorMouseMode.MoveS:
                                    it.Wrapped.Height += (float)delta.Y;
                                    break;
                                case EEditorMouseMode.MoveN:
                                    it.Wrapped.Height -= (float)delta.Y;
                                    it.Wrapped.PositionY += (float)delta.Y;
                                    break;
                                case EEditorMouseMode.MoveSE:
                                    it.Wrapped.Height += (float)delta.Y;
                                    it.Wrapped.Width += (float)delta.X;
                                    break;
                                case EEditorMouseMode.MoveSW:
                                    it.Wrapped.Height += (float)delta.Y;
                                    it.Wrapped.Width -= (float)delta.X;
                                    it.Wrapped.PositionX += (float)delta.X;
                                    break;
                                case EEditorMouseMode.MoveNE:
                                    it.Wrapped.Height -= (float)delta.Y;
                                    it.Wrapped.PositionY += (float)delta.Y;
                                    it.Wrapped.Width += (float)delta.X;
                                    break;
                                case EEditorMouseMode.MoveNW:
                                    it.Wrapped.Height -= (float)delta.Y;
                                    it.Wrapped.PositionY += (float)delta.Y;
                                    it.Wrapped.Width -= (float)delta.X;
                                    it.Wrapped.PositionX += (float)delta.X;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public void OnMouseLeave(UIElement sender, MouseEventArgs e)
        {
            this.MouseLeftButtonDown = false;
            this.MouseDrag = false;
        }

        public EditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var owner) ? owner : null;
        public readonly WeakReference<EditorDataContext> OwnerWeak;
        public EditorItem(EditorDataContext owner, IEditorItemContent wrapped)
        {
            this.OwnerWeak = new WeakReference<EditorDataContext>(owner);
            this._Wrapped = wrapped;
        }
    }
}
