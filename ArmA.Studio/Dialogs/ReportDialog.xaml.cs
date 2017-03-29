using System.Windows;

namespace ArmA.Studio.Dialogs
{
    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class ReportDialog : Window
    {
        public ReportDialog(ReportDialogDataContext dc)
        {
            this.DataContext = dc;
            InitializeComponent();
        }
    }
}
