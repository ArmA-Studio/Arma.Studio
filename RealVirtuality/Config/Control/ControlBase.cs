using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RealVirtuality.Config.Control.Attributes;

namespace RealVirtuality.Config.Control
{
    public abstract class ControlBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        ///<summary>
        /// The unique ID number of this control. can be -1 if you don't require access to the control itself from within a script
        ///</summary>
        [ConfigPathDescriptor("/idc")]
        public int IDC { get { return this._IDC; } set { this._IDC = value; this.RaisePropertyChanged(); } }
        private int _IDC;
        ///<summary>
        /// Whether the dialog will be moved if this control is dragged (requires "movingEnable" to be 1 in the dialog)
        ///</summary>
        [ConfigPathDescriptor("/moving")]
        public bool moving { get { return this._moving; } set { this._moving = value; this.RaisePropertyChanged(); } }
        private bool _moving;
        ///<summary>
        /// Type of this control. Check <see cref="EType"/> for more info about available values.
        ///</summary>
        [ConfigPathDescriptor("/type")]
        public abstract EType ControlType { get; }
        ///<summary>
        ///can be combinatorial: style = "0x400+0x02+0x10"
        ///</summary>
        [ConfigPathDescriptor("/style")]
        public int style { get { return this._style; } set { this._style = value; this.RaisePropertyChanged(); } }
        private int _style;
        /// <summary>
        /// the position X of the control in fractions of screen size.
        /// </summary>
        [ConfigPathDescriptor("/x")]
        public double PositionX { get { return this._PositionX; } set { this._PositionX = value; RaisePropertyChanged(); } }
        private double _PositionX;
        /// <summary>
        /// the position Y of the control in fractions of screen size.
        /// </summary>
        [ConfigPathDescriptor("/y")]
        public double PositionY { get { return this._PositionY; } set { this._PositionY = value; RaisePropertyChanged(); } }
        private double _PositionY;
        /// <summary>
        /// the Width of the control in fractions of screen size.
        /// </summary>
        [ConfigPathDescriptor("/w")]
        public double Width { get { return this._Width; } set { this._Width = value; RaisePropertyChanged(); } }
        private double _Width;
        /// <summary>
        /// the Height of the control in fractions of screen size.
        /// </summary>
        [ConfigPathDescriptor("/h")]
        public double Height { get { return this._Height; } set { this._Height = value; RaisePropertyChanged(); } }
        private double _Height;
        ///<summary>
        ///the font size of text (0..1)
        ///</summary>
        [ConfigPathDescriptor("/sizeEx")]
        public float SizeEx { get { return this._SizeEx; } set { this._SizeEx = value; this.RaisePropertyChanged(); } }
        private float _SizeEx;
        ///<summary>
        ///the font to use. See the list of available fonts for possible values
        ///</summary>
        [ConfigPathDescriptor("/font")]
        public string Font { get { return this._Font; } set { this._Font = value; this.RaisePropertyChanged(); } }
        private string _Font;
        ///<summary>
        ///text color
        ///</summary>
        [ConfigPathDescriptor("/foreground", Converter = typeof(ColorConverter))]
        public Color Foreground { get { return this._foreground; } set { this._foreground = value; this.RaisePropertyChanged(); } }
        private Color _foreground;
        ///<summary>
        ///background color
        ///</summary>
        [ConfigPathDescriptor("/background", Converter = typeof(ColorConverter))]
        public Color Background { get { return this._Background; } set { this._Background = value; this.RaisePropertyChanged(); } }
        private Color _Background;
        ///<summary>
        ///the text or picture to display
        ///</summary>
        [ConfigPathDescriptor("/text")]
        public string Content { get { return this._Content; } set { this._Content = value; this.RaisePropertyChanged(); } }
        private string _Content;
        ///<summary>
        ///can be applied to most controls (0 = no shadow, 1 = drop shadow with soft edges, 2 = stroke).
        ///</summary>
        [ConfigPathDescriptor("/shadow")]
        public EShadow Shadow { get { return this._shadow; } set { this._shadow = value; this.RaisePropertyChanged(); } }
        private EShadow _shadow;
        ///<summary>
        ///Text to display in a tooltip when control is moused over. A tooltip can be added to any control type except CT_STATIC and CT_STRUCTURED_TEXT. Note: As of Arma 3 v1.48 (approx), most controls now support tooltips.
        ///</summary>
        [ConfigPathDescriptor("/tooltip")]
        public string Tooltip { get { return this._tooltip; } set { this._tooltip = value; this.RaisePropertyChanged(); } }
        private string _tooltip;
        ///<summary>
        ///Tooltip background color
        ///</summary>
        [ConfigPathDescriptor("/tooltipColorShade", Converter = typeof(ColorConverter))]
        public Color TooltipColorShade { get { return this._tooltipColorShade; } set { this._tooltipColorShade = value; this.RaisePropertyChanged(); } }
        private Color _tooltipColorShade;
        ///<summary>
        ///Tooltip text color
        ///</summary>
        [ConfigPathDescriptor("/tooltipColorText", Converter = typeof(ColorConverter))]
        public Color TooltipColorText { get { return this._tooltipColorText; } set { this._tooltipColorText = value; this.RaisePropertyChanged(); } }
        private Color _tooltipColorText;
        ///<summary>
        ///Tooltip border color	
        ///</summary>
        [ConfigPathDescriptor("/TooltipColorBox", Converter = typeof(ColorConverter))]
        public Color tooltipColorBox { get { return this._tooltipColorBox; } set { this._tooltipColorBox = value; this.RaisePropertyChanged(); } }
        private Color _tooltipColorBox;
        ///<summary>
        ///Option for entry fields (e.g. RscEdit) to activate autocompletion. For known script commands and functions use autocomplete = "scripting".
        ///</summary>
        [ConfigPathDescriptor("/Autocompete", Converter = typeof(ColorConverter))]
        public string autocompete { get { return this._autocompete; } set { this._autocompete = value; this.RaisePropertyChanged(); } }
        private string _autocompete;
    }
}