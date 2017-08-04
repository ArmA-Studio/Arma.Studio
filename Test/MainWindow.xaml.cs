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
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

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
            this.IncludePaths = new ObservableCollection<string>();
            InitializeComponent();
        }

        public ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessingDirective> Macros { get; private set; }
        public ObservableCollection<RealVirtuality.Lang.Preprocessing.PreProcessedInfo> Infos { get; private set; }
        public ObservableCollection<string> IncludePaths { get; private set; }

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
        public ICommand IncludePathsDoubleClick => new ArmA.Studio.Data.UI.Commands.RelayCommand((p) =>
        {
            var ofd = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false
            };
            var ofd_res = ofd.ShowDialog();
            if (ofd_res == CommonFileDialogResult.Ok)
            {
                IncludePaths.Add(ofd.FileName);
            }
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
                ppstream.OnIncludeLookup += (pps_sender, pps_e) =>
                {
                    foreach (var dir in this.IncludePaths)
                    {
                        if (File.Exists(Path.Combine(dir, pps_e.IncludePath)))
                        {
                            pps_e.SetFile(new Uri(Path.Combine(dir, pps_e.IncludePath), UriKind.Absolute));
                            return;
                        }
                    }
                };
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                using (var writer = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ArmA.Studio.TestSaveFile.file")))
                {
                    foreach(var it in this.IncludePaths)
                    {
                        writer.Write(0);
                        writer.WriteLine(it);
                    }
                    writer.WriteLine(1);
                    writer.Write(this.source.Text);
                }
            }
            catch
            {

            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ArmA.Studio.TestSaveFile.file")))
                {
                    string line;
                    bool flag = true;
                    while (flag && !string.IsNullOrWhiteSpace((line = reader.ReadLine())))
                    {
                        if (line.Length == 0)
                        {
                            continue;
                        }
                        switch (line[0])
                        {
                            case '0':
                                this.IncludePaths.Add(line.Substring(1));
                                break;
                            case '1':
                                flag = false;
                                break;
                        }
                    }
                    this.source.Text = reader.ReadToEnd();
                }
            }
            catch
            {

            }
        }
    }
}
