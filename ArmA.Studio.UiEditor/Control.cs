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
    public class Control : INotifyPropertyChanged,
        Data.UI.AttachedProperties.IOnMouseLeftButtonDown,
        Data.UI.AttachedProperties.IOnMouseLeftButtonUp,
        Data.UI.AttachedProperties.IOnMouseMove,
        Data.UI.AttachedProperties.IOnMouseLeave
    {
        // Most stuff is in https://community.bistudio.com/wiki/Dialog_Control#Controls
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));

        /// <summary>
        /// the unique ID number of this control. can be -1 if you don't require access to the control itself from within a script.
        /// </summary>
        public int IDC { get => this._IDC; set { this._IDC = value; this.RaisePropertyChanged(); } }
        private int _IDC;

        /// <summary>
        /// whether the dialog will be moved if this control is dragged (may require "movingEnable" to be 1 in the dialog. In Arma 3 works regardless). Another way of allowing moving of the dialog is to have control of style ST_TITLE_BAR
        /// </summary>
        public bool Moving { get => this._Moving; set { this._Moving = value; this.RaisePropertyChanged(); } }
        private bool _Moving;

        /// <summary>
        /// 
        /// </summary>
        public EControlType Type { get => this._Type; set { this._Type = value; this.RaisePropertyChanged(); } }
        private EControlType _Type;

        /// <summary>
        /// can be combinatorial: style = "0x400+0x02+0x10";
        /// </summary>
        public EControlStyle Style { get => this._Style; set { this._Style = value; this.RaisePropertyChanged(); } }
        private EControlStyle _Style;

        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        public float PositionX { get => this._PositionX; set { this._PositionX = value; this.RaisePropertyChanged(); } }
        private float _PositionX;

        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        public float PositionY { get => this._PositionY; set { this._PositionY = value; this.RaisePropertyChanged(); } }
        private float _PositionY;

        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        public float Width { get => this._Width; set { this._Width = value; this.RaisePropertyChanged(); } }
        private float _Width;

        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        public float Height { get => this._Height; set { this._Height = value; this.RaisePropertyChanged(); } }
        private float _Height;

        /// <summary>
        /// the font size of text (0..1)
        /// </summary>
        public float SizeEx { get => this._SizeEx; set { this._SizeEx = value; this.RaisePropertyChanged(); } }
        private float _SizeEx;

        /// <summary>
        /// the font to use. See the list of available fonts for possible values
        /// </summary>
        public string Font { get => this._Font; set { this._Font = value; this.RaisePropertyChanged(); } }
        private string _Font;

        /// <summary>
        /// text color
        /// </summary>
        public Color ColorText { get => this._ColorText; set { this._ColorText = value; this.RaisePropertyChanged(); } }
        private Color _ColorText;

        /// <summary>
        /// background color
        /// </summary>
        public Color ColorBackground { get => this._ColorBackground; set { this._ColorBackground = value; this.RaisePropertyChanged(); } }
        private Color _ColorBackground;

        /// <summary>
        /// the text or picture to display
        /// </summary>
        public string Text { get => this._Text; set { this._Text = value; this.RaisePropertyChanged(); } }
        private string _Text;

        /// <summary>
        /// can be applied to most controls (0 = no shadow, 1 = drop shadow with soft edges, 2 = stroke).
        /// </summary>
        public EShadow Shadow { get => this._Shadow; set { this._Shadow = value; this.RaisePropertyChanged(); } }
        private EShadow _Shadow;

        /// <summary>
        /// Text to display in a tooltip when control is moused over.
        /// A tooltip can be added to any control type except CT_STATIC and CT_STRUCTURED_TEXT.
        /// Note: As of Arma 3 v1.48 (approx), most controls now support tooltips.
        /// </summary>
        public string Tooltip { get => this._Tooltip; set { this._Tooltip = value; this.RaisePropertyChanged(); } }
        private string _Tooltip;

        /// <summary>
        /// Tooltip background color
        /// </summary>
        public Color TooltipColorShade { get => this._TooltipColorShade; set { this._TooltipColorShade = value; this.RaisePropertyChanged(); } }
        private Color _TooltipColorShade;

        /// <summary>
        /// Tooltip text color
        /// </summary>
        public Color TooltipColorText { get => this._TooltipColorText; set { this._TooltipColorText = value; this.RaisePropertyChanged(); } }
        private Color _TooltipColorText;

        /// <summary>
        /// Tooltip border color
        /// </summary>
        public Color TooltipColorBox { get => this._TooltipColorBox; set { this._TooltipColorBox = value; this.RaisePropertyChanged(); } }
        private Color _TooltipColorBox;

        /// <summary>
        /// Option for entry fields (e.g. RscEdit) to activate autocompletion.
        /// For known script commands and functions use autocomplete = "scripting".
        /// </summary>
        public string Autocomplete { get => this._Autocomplete; set { this._Autocomplete = value; this.RaisePropertyChanged(); } }
        private string _Autocomplete;

        /// <summary>
        /// URL which will be opened when clicking on the control.
        /// Used on e.g. a button control.
        /// Does not utilize the Steam Overlay browser if enabled, opens the link in the default browser set by the OS.
        /// </summary>
        public string URL { get => this._URL; set { this._URL = value; this.RaisePropertyChanged(); } }
        private string _URL;


        public bool IsSelected
        {
            get => this._IsSelected;
            set
            {
                if(this._IsSelected == value)
                {
                    return;
                }
                this._IsSelected = value;
                if(value)
                {
                    this.Owner.SelectedControls.Add(this);
                }
                else
                {
                    this.Owner.SelectedControls.Remove(this);
                }
                this.RaisePropertyChanged();
            }
        }
        private bool _IsSelected;

        public ICommand CmdMouseLeftClick => new RelayCommand<Border>((border) => {
        });



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
                    if (this.Owner.SelectedControls.Count > 1 && !Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        foreach (var it in this.Owner.SelectedControls.ToArray())
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
                        foreach (var it in this.Owner.SelectedControls.ToArray())
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
            if (!this.MouseLeftButtonDown)
            {
                return;
            }
            
            var current = e.GetPosition(sender.FindParent<Canvas>() as IInputElement);
            var delta = new Point(current.X - this.MouseDownPosition.X, current.Y - this.MouseDownPosition.Y);
            if (this.MouseDrag || Math.Abs(delta.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(delta.Y) >= SystemParameters.MinimumVerticalDragDistance)
            {
                this.IsSelected = true;
                this.MouseDrag = true;
                foreach (var it in this.Owner.SelectedControls)
                {
                    it.PositionX += (float)delta.X;
                    it.PositionY += (float)delta.Y;
                }
                this.MouseDownPosition = current;
            }
        }

        public void OnMouseLeave(UIElement sender, MouseEventArgs e)
        {
            this.MouseLeftButtonDown = false;
            this.MouseDrag = false;
        }

        public EditorDataContext Owner => this.OwnerWeak.TryGetTarget(out var owner) ? owner : null;
        public readonly WeakReference<EditorDataContext> OwnerWeak;
        public Control(EditorDataContext owner)
        {
            this.OwnerWeak = new WeakReference<EditorDataContext>(owner);
        }
    }
}
