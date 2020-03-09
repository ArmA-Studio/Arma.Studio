using Arma.Studio.Data.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Arma.Studio.UiEditor.Data
{
    public abstract class ControlBase : IControlElement, IPropertyHost
    {
        public const string PropGroup_Visual = "Visual";
        public const string PropGroup_Behavior = "Behavior";
        public const string PropGroup_Text = "Text";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string callee = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));
        }
        #region Property: IsSelected (System.Boolean)
        public bool IsSelected
        {
            get => this._IsSelected;
            set
            {
                this._IsSelected = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsSelected;
        #endregion

        #region Property: ZIndex (System.Int32)
        [Property("Z-Index", Group = PropGroup_Visual)]
        public int ZIndex
        {
            get => this._ZIndex;
            set
            {
                this._ZIndex = value;
                this.RaisePropertyChanged();
            }
        }
        private int _ZIndex;
        #endregion

        #region Property: IDC {idc} (System.Int32)
        /// <summary>
        /// the unique ID number of this control. can be -1 if you don't require access to the control itself from within a script.
        /// </summary>
        [ArmaName("idc")]
        public int IDC
        {
            get => this._IDC;
            set
            {
                this._IDC = value;
                this.RaisePropertyChanged();
            }
        }
        private int _IDC;
        #endregion
        #region Property: CanMoveDialog {moving} (System.Boolean)
        /// <summary>
        /// whether the dialog will be moved if this control is dragged
        /// (may require "movingEnable" to be 1 in the dialog. In Arma 3 works regardless).
        /// Another way of allowing moving of the dialog is to have control of style ST_TITLE_BAR
        /// </summary>
        [ArmaName("moving", Group = PropGroup_Behavior)]
        public bool CanMoveDialog
        {
            get => this._CanMoveDialog;
            set
            {
                this._CanMoveDialog = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _CanMoveDialog;
        #endregion
        #region Property: ControlType {type} (EControlType)
        [ArmaName("type", Display = false)]
        public abstract EControlType ControlType { get; }
        #endregion
        #region Property: ControlStyle {style} (EControlStyle)
        /// <summary>
        /// can be combinatorial: style = "0x400+0x02+0x10";
        /// </summary>
        [ArmaName("style", Display = false)]
        public EControlStyle ControlStyle
        {
            get => this._ControlStyle;
            set
            {
                this._ControlStyle = value;
                this.RaisePropertyChanged();
            }
        }
        private EControlStyle _ControlStyle;
        #endregion
        #region Property: Left {x} (System.Double)
        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        [ArmaName("x", Group = PropGroup_Visual)]
        public double Left
        {
            get => this._Left;
            set
            {
                this._Left = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Left;
        #endregion
        #region Property: Top {y} (System.Double)
        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        [ArmaName("y", Group = PropGroup_Visual)]
        public double Top
        {
            get => this._Top;
            set
            {
                this._Top = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Top;
        #endregion
        #region Property: Width {w} (System.Double)
        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        [ArmaName("w", Group = PropGroup_Visual)]
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
        #region Property: Height {h} (System.Double)
        /// <summary>
        /// the position and size of the control in fractions of screen size.
        /// </summary>
        [ArmaName("h", Group = PropGroup_Visual)]
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
        #region Property: FontSize {sizeEx} (System.Double)
        /// <summary>
        /// the font size of text (0..1)
        /// 
        /// Useful to note is that the following should yield the correct sizeEx value: (0.0264 * safeZoneH) * FontSize
        /// </summary>
        [ArmaName("sizeEx", Group = PropGroup_Text)]
        public double FontSize
        {
            get => this._FontSize;
            set
            {
                this._FontSize = value;
                this.RaisePropertyChanged();
            }
        }
        private double _FontSize;
        #endregion
        #region Property: Font {font} (System.String)
        /// <summary>
        /// the font to use. See the list of available fonts for possible values
        /// </summary>
        [ArmaName("font", Group = PropGroup_Text)]
        public string Font
        {
            get => this._Font;
            set
            {
                this._Font = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Font;
        #endregion
        #region Property: ForegroundColor {colorText} (System.Windows.Media.Color)
        /// <summary>
        /// text color
        /// </summary>
        [ArmaName("colorText", Group = PropGroup_Text)]
        public Color ForegroundColor
        {
            get => this._ForegroundColor;
            set
            {
                this._ForegroundColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _ForegroundColor;
        #endregion
        #region Property: BackgroundColor {colorBackground} (System.Windows.Media.Color)
        /// <summary>
        /// background color
        /// </summary>
        [ArmaName("colorBackground", Group = PropGroup_Visual)]
        public Color BackgroundColor
        {
            get => this._BackgroundColor;
            set
            {
                this._BackgroundColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _BackgroundColor;
        #endregion
        #region Property: Text {text} (System.String)
        /// <summary>
        /// the text or picture to display
        /// </summary>
        [ArmaName("text", Group = PropGroup_Text)]
        public string Text
        {
            get => this._Text;
            set
            {
                this._Text = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Text;
        #endregion
        #region Property: Shadow {shadow} (EShadow)
        /// <summary>
        /// can be applied to most controls (0 = no shadow, 1 = drop shadow with soft edges, 2 = stroke).
        /// </summary>
        [ArmaName("shadow", Group = PropGroup_Text)]
        public EShadow Shadow
        {
            get => this._Shadow;
            set
            {
                this._Shadow = value;
                this.RaisePropertyChanged();
            }
        }
        private EShadow _Shadow;
        #endregion
        #region Property: IsShown {show} (System.Boolean)
        /// <summary>
        /// Initial visibility of the control
        /// </summary>
        [ArmaName("show", Group = PropGroup_Visual)]
        public bool IsShown
        {
            get => this._IsShown;
            set
            {
                this._IsShown = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsShown;
        #endregion
        #region Property: ToolTip {tooltip} (System.String)
        /// <summary>
        /// Text to display in a tooltip when control is moused over.
        /// A tooltip can be added to any control type except CT_STATIC and CT_STRUCTURED_TEXT.
        /// Note: As of Arma 3 v1.48 (approx), most controls now support tooltips.
        /// </summary>
        [ArmaName("tooltip", Group = PropGroup_Text)]
        public string ToolTip
        {
            get => this._ToolTip;
            set
            {
                this._ToolTip = value;
                this.RaisePropertyChanged();
            }
        }
        private string _ToolTip;
        #endregion
        #region Property: ToolTipForegroundColor {tooltipColorText} (System.Windows.Media.Color)
        /// <summary>
        /// Tooltip text color
        /// </summary>
        [ArmaName("tooltipColorText", Group = PropGroup_Text)]
        public Color ToolTipForegroundColor
        {
            get => this._ToolTipForegroundColor;
            set
            {
                this._ToolTipForegroundColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _ToolTipForegroundColor;
        #endregion
        #region Property: ToolTipBackgroundColor {tooltipColorShade} (System.Windows.Media.Color)
        /// <summary>
        /// Tooltip background color
        /// </summary>
        [ArmaName("tooltipColorShade", Group = PropGroup_Text)]
        public Color ToolTipBackgroundColor
        {
            get => this._ToolTipBackgroundColor;
            set
            {
                this._ToolTipBackgroundColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _ToolTipBackgroundColor;
        #endregion
        #region Property: ToolTipBorderColor {tooltipColorBox} (System.Windows.Media.Color)
        /// <summary>
        /// Tooltip border color
        /// </summary>
        [ArmaName("tooltipColorBox", Group = PropGroup_Text)]
        public Color ToolTipBorderColor
        {
            get => this._ToolTipBorderColor;
            set
            {
                this._ToolTipBorderColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _ToolTipBorderColor;
        #endregion
        #region Property: AutoComplete {autocomplete} (System.String)
        /// <summary>
        /// Option for entry fields (e.g. RscEdit) to activate autocompletion.
        /// For known script commands and functions use autocomplete = "scripting".
        /// </summary>
        [ArmaName("autocomplete", Group = PropGroup_Behavior)]
        public string AutoComplete
        {
            get => this._AutoComplete;
            set
            {
                this._AutoComplete = value;
                this.RaisePropertyChanged();
            }
        }
        private string _AutoComplete;
        #endregion
        #region Property: IsDeletable {deletable} (System.Boolean)
        /// <summary>
        /// Whether or not control can be deleted by scripts with ctrlDelete command
        /// </summary>
        [ArmaName("deletable", Group = PropGroup_Behavior)]
        public bool IsDeletable
        {
            get => this._IsDeletable;
            set
            {
                this._IsDeletable = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _IsDeletable;
        #endregion
        #region Property: Opacity {fade} (System.Double)
        /// <summary>
        /// Initial fade of the control
        /// </summary>
        [ArmaName("fade", Group = PropGroup_Visual)]
        public double Opacity
        {
            get => this._Opacity;
            set
            {
                this._Opacity = value;
                this.RaisePropertyChanged();
            }
        }
        private double _Opacity;
        #endregion
        #region Property: Url {url} (System.String)
        /// <summary>
        /// URL which will be opened when clicking on the control.
        /// Used on e.g. a button control.
        /// Does not utilize the Steam Overlay browser if enabled, opens the link in the default browser set by the OS.
        /// </summary>
        [ArmaName("url", Group = PropGroup_Behavior)]
        public string Url
        {
            get => this._Url;
            set
            {
                this._Url = value;
                this.RaisePropertyChanged();
            }
        }
        private string _Url;
        #endregion

        #region Property: ClassName (System.String)
        [Property("Classname")]
        public string ClassName
        {
            get => this._ClassName;
            set
            {
                this._ClassName = value;
                this.RaisePropertyChanged();
            }
        }
        private string _ClassName;
        #endregion

        public ControlBase()
        {
            this.ForegroundColor = Colors.Black;
            this.BackgroundColor = Colors.Transparent;
            this.Shadow = EShadow.DropShadow;
            this.FontSize = 0.04;
            this.Opacity = 1;
            this.IsShown = true;
            this.Font = "TahomaB";
            this.ClassName = "MY_" + Enum.GetName(typeof(EControlType), this.ControlType).Substring(3) + "_" + new Random().Next();
        }
    }
}
