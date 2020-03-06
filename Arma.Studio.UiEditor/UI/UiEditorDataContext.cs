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
using System.Windows.Input;

namespace Arma.Studio.UiEditor.UI
{
    public class UiEditorDataContext : DockableBase, IOnPreviewMouseWheel
    {
        #region Collection: CanvasManager (CanvasManager)
        public CanvasManager CanvasManager
        {
            get => this._CanvasManager;
            set
            {
                if (this._CanvasManager == value)
                {
                    return;
                }
                this._CanvasManager = value;
                this.RaisePropertyChanged();
            }
        }
        private CanvasManager _CanvasManager;
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
        #region Interface: IOnPreviewMouseWheel
        public void OnPreviewMouseWheel(UIElement sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && (Application.Current as IApp).MainWindow.ActiveDockable == this)
            {
                e.Handled = true;

                // Get current relative position of mouse
                var currentMousePosition = e.GetPosition(this.CanvasManager.ScrollViewer);
                var currentDeltaMouseLeft = currentMousePosition.X / this.CanvasManager.ScrollViewer.ActualWidth;
                var currentDeltaMouseTop = currentMousePosition.Y / this.CanvasManager.ScrollViewer.ActualHeight;

                // Calculate & apply new zoom level
                var zoomDelta = e.Delta / 1000.0;
                var oldZoom = this.Zoom;
                var newZoom = this.Zoom * (1 + zoomDelta);
                newZoom = newZoom > 0.1 ? (newZoom < 5 ? newZoom : 5) : 0.1;
                this.Zoom = newZoom;

                // Force Update on layouts of canvas & ScrollViewer
                this.CanvasManager.Canvas.UpdateLayout();
                this.CanvasManager.ScrollViewer.UpdateLayout();

                // Adjust position of Horizontal- & VerticalOffset according to previous MouseDelta
                this.CanvasManager.ScrollViewer.ScrollToHorizontalOffset(
                    this.CanvasManager.ScrollViewer.HorizontalOffset +
                    (currentDeltaMouseLeft * this.CanvasManager.ScrollViewer.ActualWidth * (newZoom - oldZoom)));
                this.CanvasManager.ScrollViewer.ScrollToVerticalOffset(
                    this.CanvasManager.ScrollViewer.VerticalOffset +
                    (currentDeltaMouseTop * this.CanvasManager.ScrollViewer.ActualHeight * (newZoom - oldZoom)));
            }
        }
        #endregion


        #region Property: IDD (System.Int32)
        /// <summary>
        /// The unique ID number of this dialog. can be -1 if you don't require access to the dialog itself from within a script.
        /// </summary>
        public int IDD
        {
            get => this._IDD;
            set
            {
                this._IDD = value;
                this.RaisePropertyChanged();
            }
        }
        private int _IDD;
        #endregion
        #region Property: EnableSimulation (System.Boolean)
        /// <summary>
        /// Specifies whether the game continues while the dialog is shown or not.
        /// </summary>
        public bool EnableSimulation
        {
            get => this._EnableSimulation;
            set
            {
                this._EnableSimulation = value;
                this.RaisePropertyChanged();
            }
        }
        private bool _EnableSimulation;
        #endregion
        #region Collection: BackgroundControls (ObservableCollection<IControlElement>)
        public ObservableCollection<IControlElement> BackgroundControls
        {
            get => this._BackgroundControls;
            set
            {
                if (this._BackgroundControls == value)
                {
                    return;
                }
                this._BackgroundControls = value;
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<IControlElement> _BackgroundControls;
        #endregion
        #region Collection: ForegroundControls (ObservableCollection<IControlElement>)
        public ObservableCollection<IControlElement> ForegroundControls
        {
            get => this._ForegroundControls;
            set
            {
                if (this._ForegroundControls == value)
                {
                    return;
                }
                this._ForegroundControls = value;
                this.RaisePropertyChanged();
            }
        }
        private ObservableCollection<IControlElement> _ForegroundControls;
        #endregion

        public UiEditorDataContext()
        {
            this._IDD = -1;
            this._EnableSimulation = true;
            this._BackgroundControls = new ObservableCollection<IControlElement>();
            this._ForegroundControls = new ObservableCollection<IControlElement>();
        }
    }
}
