using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arma.Studio.Data.UI
{
    public class DialogWindow : Window
    {
        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }


        public static DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(DialogWindow));
        public string Header { get { return (string)this.GetValue(HeaderProperty); } set { this.SetValue(HeaderProperty, value); } }

        public static DependencyProperty OKButtonTextProperty = DependencyProperty.Register(nameof(OKButtonText), typeof(string), typeof(DialogWindow), new PropertyMetadata("OK"));
        public string OKButtonText { get { return (string)this.GetValue(OKButtonTextProperty); } set { this.SetValue(OKButtonTextProperty, value); } }

        public static DependencyProperty OKButtonEnabledProperty = DependencyProperty.Register(nameof(OKButtonEnabled), typeof(bool), typeof(DialogWindow), new PropertyMetadata(true));
        public bool OKButtonEnabled { get { return (bool)this.GetValue(OKButtonEnabledProperty); } set { this.SetValue(OKButtonEnabledProperty, value); } }

        public static DependencyProperty OKClickCommandProperty = DependencyProperty.Register(nameof(OKClickCommand), typeof(ICommand), typeof(DialogWindow));
        public ICommand OKClickCommand { get { return this.GetValue(OKClickCommandProperty) as ICommand; } set { this.SetValue(OKClickCommandProperty, value); } }
        public static DependencyProperty OKClickCommandParameterProperty = DependencyProperty.Register(nameof(OKClickCommandParameter), typeof(ICommand), typeof(DialogWindow));
        public ICommand OKClickCommandParameter { get { return this.GetValue(OKClickCommandParameterProperty) as ICommand; } set { this.SetValue(OKClickCommandParameterProperty, value); } }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.GetTemplateChild("PART_OKButton") is Button PART_OKButton)
            {
                PART_OKButton.Click += this.PART_OKButton_Click;
            }
        }

        private void PART_OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKClickCommand?.Execute(this.OKClickCommandParameter);
            try
            {
                this.DialogResult = true;
            }
            catch (InvalidOperationException) { /* We may not always be an actual dialog window, so lets not do this. */ }
        }
    }
}
