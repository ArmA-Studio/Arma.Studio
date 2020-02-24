using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.Data.TextEditor
{
    public class WordCompletionInfo : ICodeCompletionInfo
    {

        public ImageSource ImageSource => null;
        public string Text { get; private set; }
        public object Content { get; private set; }
        public object Description { get; private set; }
        public double Priority => 0;

        public WordCompletionInfo(string word)
        {
            this.Text = word;
            this.Content = new System.Windows.Controls.TextBlock { Text = word };
        }
        public WordCompletionInfo(string word, string description)
        {
            this.Text = word;
            this.Description = description;
            this.Content = new System.Windows.Controls.TextBlock { Text = word };
        }

        public WordCompletionInfo(string ltype, string @operator, string rtype, string description)
        {
            this.Text = @operator;
            this.Description = description;
            var panel = new System.Windows.Controls.StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };
            if (!String.IsNullOrWhiteSpace(ltype) )
            {
                var textblock_ltype = new System.Windows.Controls.TextBlock { Text = ltype, FontStyle = System.Windows.FontStyles.Italic, Margin = new System.Windows.Thickness(0, 0, 6, 0) };
                panel.Children.Add(textblock_ltype);
            }
            var textblock_operator = new System.Windows.Controls.TextBlock { Text = @operator, FontWeight = System.Windows.FontWeights.Bold };
            panel.Children.Add(textblock_operator);
            if (!String.IsNullOrWhiteSpace(rtype))
            {
                var textblock_rtype = new System.Windows.Controls.TextBlock { Text = rtype, FontStyle = System.Windows.FontStyles.Italic, Margin = new System.Windows.Thickness(6, 0, 0, 0) };
                panel.Children.Add(textblock_rtype);
            }
            this.Content = panel;
        }
        public string Complete(string input)
        {
            return this.Text;
        }
    }
}
