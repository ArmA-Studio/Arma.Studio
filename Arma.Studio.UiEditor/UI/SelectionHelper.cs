using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.UiEditor.UI
{

    public class SelectionHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string callee = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));
        }

        #region Property: Top (System.Double)
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

        public int ZIndex => Int32.MaxValue;
        #region Property: Left (System.Double)
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

        public void Move(Point p)
        {
            if (this.OriginalLeft > p.X)
            {
                this.Left = p.X;
                this.Width = this.OriginalLeft - p.X;
            }
            else
            {
                this.Width = p.X - this.Left;
            }
            if (this.OriginalTop > p.Y)
            {
                this.Top = p.Y;
                this.Height = this.OriginalTop - p.Y;
            }
            else
            {
                this.Height = p.Y - this.Top;
            }
        }

        public readonly double OriginalLeft;
        public readonly double OriginalTop;
        public SelectionHelper(double originalLeft, double originalTop)
        {
            this.Left = this.OriginalLeft = originalLeft;
            this.Top = this.OriginalTop = originalTop;
        }

        public static implicit operator Rect(SelectionHelper selectionHelper) => new Rect(selectionHelper.Left, selectionHelper.Top, selectionHelper.Width, selectionHelper.Height);
    }
}
