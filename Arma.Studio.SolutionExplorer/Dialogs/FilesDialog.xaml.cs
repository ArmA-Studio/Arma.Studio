using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
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

namespace Arma.Studio.SolutionExplorer.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class FilesDialog : DialogWindow
    {
        public FilesDialog(FilesDialogDataContext dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }
    }
}
