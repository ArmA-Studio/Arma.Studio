using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Macros = new ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessingDirective>();
            this.Infos = new ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessedInfo>();
            InitializeComponent();
        }

        public ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessingDirective> Macros { get; private set; }
        public ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessedInfo> Infos { get; private set; }

        public ICommand InfosDoubleClick => new ArmA.Studio.Data.UI.Commands.RelayCommand((p) =>
        {
            var ppi = (RealVirtuality.Lang.Preprocessing.PreProcessedInfo)p;
            var builder = new StringBuilder();
            builder.AppendLine($"Line: {ppi.Line}");
            builder.AppendLine($"Column: {ppi.Column}");
            builder.AppendLine($"Parameters: {string.Join(", ", ppi.InfoParams)}");
            builder.AppendLine($"Targeted Macro: {ppi.Directive.Name}");

            MessageBox.Show(builder.ToString());
        });
        public ICommand MacrosDoubleClick => new ArmA.Studio.Data.UI.Commands.RelayCommand((p) =>
        {
            var ppd = (RealVirtuality.Lang.Preprocessing.PreProcessingDirective)p;
            var builder = new StringBuilder();
            builder.AppendLine($"Name: {ppd.Name}");
            builder.AppendLine($"Parameters: {string.Join(", ", ppd.Parameters)}");
            builder.AppendLine($"Content: {ppd.UnparsedText}");
            MessageBox.Show(builder.ToString());
        });

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(source.Text);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var ppstream = new RealVirtuality.Lang.Preprocessing.PreProcessingStream(stream);
                target.Text = new StreamReader(ppstream).ReadToEnd();
                this.Macros.Clear();
                foreach (var it in ppstream.PreProcessingDirectives)
                {
                    this.Macros.Add(it);
                }
                this.Infos.Clear();
                foreach (var it in ppstream.PreProcessedInfos)
                {
                    this.Infos.Add(it);
                }
            }
        }
    }
}
