using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArmA.Studio.UiEditor
{
    public class Control : INotifyPropertyChanged
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
        public string Autocomplete { get => this._Tooltip; set { this._Tooltip = value; this.RaisePropertyChanged(); } }
        private string _Autocomplete;

        /// <summary>
        /// URL which will be opened when clicking on the control.
        /// Used on e.g. a button control.
        /// Does not utilize the Steam Overlay browser if enabled, opens the link in the default browser set by the OS.
        /// </summary>
        public string URL { get => this._URL; set { this._URL = value; this.RaisePropertyChanged(); } }
        private string _URL;
    }
}
