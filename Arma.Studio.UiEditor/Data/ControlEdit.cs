using Arma.Studio.Data.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Arma.Studio.UiEditor.Data
{
    public class ControlEdit : ControlBase
    {
        public const string PropGroup_CT_Edit = "CT_Edit";
        public override EControlType ControlType => EControlType.CT_EDIT;

        protected override void OnPropertyChanged(string callee)
        {
            base.OnPropertyChanged(callee);
            switch (callee)
            {
                case nameof(ControlBase.ControlStyle):
                    {
                        this.RaisePropertyChanged(nameof(this.IsMultiline));
                        this.RaisePropertyChanged(nameof(this.NoBorder));
                    }
                    break;
            }
        }

        #region Property: NoBorder (System.Boolean)
        [Property("No Border", Group = PropGroup_Style)]
        public bool NoBorder
        {
            get => (this.ControlStyle & EControlStyle.ST_NO_RECT) == EControlStyle.ST_NO_RECT;
            set
            {
                if (value)
                {
                    this.ControlStyle |= EControlStyle.ST_NO_RECT;
                }
                else
                {
                    this.ControlStyle &= ~EControlStyle.ST_NO_RECT;
                }
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region Property: IsMultiline (System.Boolean)
        [Property("Is Multiline", Group = PropGroup_CT_Edit)]
        public bool IsMultiline
        {
            get => (this.ControlStyle & EControlStyle.ST_MULTI) == EControlStyle.ST_MULTI;
            set
            {
                if (value)
                {
                    this.ControlStyle |= EControlStyle.ST_MULTI;
                }
                else
                {
                    this.ControlStyle &= ~EControlStyle.ST_MULTI;
                }
                this.RaisePropertyChanged();
            }
        }
        #endregion







        #region Property: AutoComplete {autocomplete} (System.String)
        /// <summary>
        /// Option for entry fields (e.g. RscEdit) to activate autocompletion.
        /// For known script commands and functions use autocomplete = "scripting".
        /// </summary>
        [ArmaName("autocomplete", Group = PropGroup_CT_Edit)]
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
        #region Property: CanModify {canModify} (System.Boolean)
        /// <summary>
        /// When false, only LEFT/RIGHT/HOME/END, CTRL + C, SHIFT + LEFT/RIGHT/HOME/END keys are allowed
        /// </summary>
        [ArmaName("canModify", Group = PropGroup_CT_Edit)]
        public bool CanModify
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
        #region Property: MaxChars {maxChars} (System.Int32)
        /// <summary>
        /// Default: 2147483647. The limit for how many characters could be displayed or entered, counting new line characters too
        /// </summary>
        [ArmaName("maxChars", Group = PropGroup_CT_Edit)]
        public int MaxChars
        {
            get => this._MaxChars;
            set
            {
                this._MaxChars = value;
                this.RaisePropertyChanged();
            }
        }
        private int _MaxChars;
        #endregion
        #region Property: ForceDrawCaret {forceDrawCaret} (System.Boolean)
        /// <summary>
        /// Default: false. When true, the caret will be drawn even when control has no focus or is disabled
        /// </summary>
        [ArmaName("forceDrawCaret", Group = PropGroup_CT_Edit)]
        public bool ForceDrawCaret
        {
            get => this._ForceDrawCaret;
            set
            {
                this._ForceDrawCaret = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _ForceDrawCaret;
        #endregion
        #region Property: ColorSelection {colorSelection} (System.Windows.Media.Color)
        /// <summary>
        /// The text selection highlight color
        /// </summary>
        [ArmaName("colorSelection", Group = PropGroup_CT_Edit)]
        public Color ColorSelection
        {
            get => this._ColorSelection;
            set
            {
                this._ColorSelection = value;
                this.RaisePropertyChanged();
            }
        }
        private Color _ColorSelection;
        #endregion


        public ControlEdit()
        {
            this.CanModify = true;
            this.MaxChars = 2147483647;
            this.ForceDrawCaret = false;
            this.ForegroundColorDisabled = Colors.Gray;
        }
    }
}