﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Arma.Studio.UiEditor.Data
{
    /// <summary>
    /// Arma Reference: https://community.bistudio.com/wiki/DialogControls-Text#CT_STATIC.3D0
    /// </summary>
    public class ControlStatic : ControlBase
    {
        public override EControlType ControlType => EControlType.CT_STATIC;

        #region Property: Autoplay {autoplay} (System.Boolean)
        /// <summary>
        /// Whether or not to autostart .ogv/.ogg file set as texture
        /// </summary>
        [ArmaName("autoplay")]
        public bool Autoplay
        {
            get => this._Autoplay;
            set
            {
                this._Autoplay = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _Autoplay;
        #endregion
        #region Property: LoopAmount {loops} (System.Int32)
        /// <summary>
        /// How many times to play video
        /// </summary>
        [ArmaName("loops")]
        public int LoopAmount
        {
            get => this._LoopAmount;
            set
            {
                this._LoopAmount = value;
                this.RaisePropertyChanged();
            }
        }
        private int _LoopAmount;
        #endregion
        #region Property: LineSpacing {lineSpacing} (System.Double)
        /// <summary>
        /// Line spacing, required if the <see cref="ControlBase.ControlStyle"/> was set to <see cref="EControlStyle.ST_MULTI"/>.
        /// </summary>
        [ArmaName("lineSpacing")]
        public double LineSpacing
        {
            get => this._LineSpacing;
            set
            {
                this._LineSpacing = value;
                this.RaisePropertyChanged();
            }
        }
        private double _LineSpacing;
        #endregion
        #region Property: TextShadowColor {colorShadow} (EShadow)
        /// <summary>
        /// Sets color of the shadow under text, when <see cref="ControlBase.Shadow"/> = <see cref="EShadow.DropShadow"/>;
        /// </summary>
        [ArmaName("colorShadow")]
        public Color TextShadowColor
        {
            get => this._TextShadowColor;
            set
            {
                this._TextShadowColor = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _TextShadowColor;
        #endregion
        #region Property: BlinkingPeriod {blinkingPeriod} (System.Double)
        /// <summary>
        /// Speed with which control blinks, i.e. smoothly and repeatedly changes opacity from 1 to 0 and back
        /// </summary>
        [ArmaName("blinkingPeriod")]
        public double BlinkingPeriod
        {
            get => this._BlinkingPeriod;
            set
            {
                this._BlinkingPeriod = value;
                this.RaisePropertyChanged();
            }
        }
        private double _BlinkingPeriod;
        #endregion
    }
}