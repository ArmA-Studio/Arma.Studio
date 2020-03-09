using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arma.Studio.UI.Windows
{
    internal class ErrorDialogDataContext
    {
        internal interface IErrorContainer
        {
            string ErrorMessage { get; }
            string FullStackTrace { get; }
        }

        internal class ExceptionContainer : IErrorContainer
        {
            private readonly Exception ex;
            public string ErrorMessage => this.ex.Message;
            public string FullStackTrace { get { var builder = new StringBuilder(); GetExceptionStackTrace(this.ex, builder); return builder.ToString(); } }
            public ExceptionContainer(Exception ex)
            {
                this.ex = ex;
            }
        }
        internal class StringContainer : IErrorContainer
        {
            public string ErrorMessage { get; }
            public string FullStackTrace { get; }
            public StringContainer(string msg, string additional)
            {
                this.ErrorMessage = msg;
                this.FullStackTrace = additional;
            }
        }
        public ICommand CmdOk => new RelayCommand((p) => this.Owner.Close());

        private readonly Window Owner;

        public ObservableCollection<IErrorContainer> Errors { get; }
        public ErrorDialogDataContext(Window errorDialog)
        {
            this.Errors = new ObservableCollection<IErrorContainer>();
            this.Owner = errorDialog;
        }

        public void Add(Exception ex)
        {
            this.Errors.Add(new ExceptionContainer(ex));
        }
        public void Add(string msg, string additional = "")
        {
            this.Errors.Add(new StringContainer(msg, additional));
        }


        private static void GetExceptionStackTrace(Exception ex, StringBuilder builder, int level = 0)
        {
            builder.Append(new string('\t', level));
            var extype = ex.GetType();
            builder.Append(extype.Namespace);
            builder.AppendLine(extype.Name);
            builder.Append(new string('\t', level));
            builder.AppendLine(ex.Message);
            if (ex.Data.Count > 0)
            {
                builder.Append(new string('\t', level));
                builder.AppendLine("With Datapairs:");
                foreach (System.Collections.DictionaryEntry it in ex.Data)
                {
                    string tabstop = new string('\t', level + 1);
                    string spaces = new string(' ', it.Key.ToString().Length + 3);
                    builder.Append(tabstop);
                    builder.Append(it.Key.ToString());
                    builder.Append(" : ");
                    string str = Convert.ToString(it.Value);
                    builder.AppendLine(str.Replace("\n", "\n" + tabstop + spaces));
                }
            }
            builder.Append(new string('\t', level));
            if (ex.StackTrace != null)
            {
                builder.AppendLine(ex.StackTrace.Replace("\n", String.Concat('\n', new string('\t', level))));
            }
            if (ex.InnerException != null)
            {
                builder.Append(new string('\t', level));
                builder.AppendLine("--------------------------------------------------");
                GetExceptionStackTrace(ex.InnerException, builder, level + 1);
            }
        }
    }
}
