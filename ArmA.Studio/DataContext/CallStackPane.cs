using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ArmA.Studio.DataContext.TextEditorUtil;
using ArmA.Studio.UI;
using ArmA.Studio.UI.Commands;
using ICSharpCode.AvalonEdit.Highlighting;
using RealVirtuality.SQF.Parser;

namespace ArmA.Studio.DataContext
{
    public class CallStackPane : PanelBase
    {
        public override string Title { get { return Properties.Localization.PanelDisplayName_CallStack; } }

        public ICommand CmdEntryOnDoubleClick { get { return new RelayCommand((p) => { MessageBox.Show("To be implemented"); }); } }

        public CallStackPane()
        {
        }

        private void DebugContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}