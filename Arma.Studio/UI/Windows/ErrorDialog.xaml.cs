using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Arma.Studio.UI.Windows
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : DialogWindow
    {
        internal static ErrorDialogDataContext DataContextInstance
        {
            get
            {
                if (_DataContextInstance == null)
                {
                    App.Current.Dispatcher.Invoke(() => new ErrorDialog());
                }
                return _DataContextInstance;
            }
            set { _DataContextInstance = value; }
        }
        private static ErrorDialogDataContext _DataContextInstance;
        internal static bool DataContextInstanceExists => _DataContextInstance != null;
        public ErrorDialog()
        {
            this.DataContext = DataContextInstance = new ErrorDialogDataContext(this);
            try
            {
                this.Owner = App.Current.MainWindow;
            }
            catch
            { }
            this.InitializeComponent();
            this.Show();
            this.Focus();
        }
        private bool soundplayed = false;
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible && !this.soundplayed)
            {
                System.Media.SystemSounds.Exclamation.Play();
                this.soundplayed = true;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            DataContextInstance = null;
        }
    }
}
