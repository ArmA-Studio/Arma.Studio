using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ArmA.Studio.UI.Attached
{
    public class CueBannerAttached
    {
        public static readonly DependencyProperty CueBannerProperty = DependencyProperty.RegisterAttached("CueBanner", typeof(object), typeof(CueBannerAttached), new FrameworkPropertyMetadata(null, CueBannerPropertyChanged));

        public static object GetCueBanner(Control control)
        {
            return control.GetValue(CueBannerProperty);
        }

        public static void SetCueBanner(Control control, object value)
        {
            control.SetValue(CueBannerProperty, value);
        }
        
        private static void CueBannerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as Control;
            if (c == null)
                return;
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                c.Loaded += Control_Loaded;
                c.IsVisibleChanged += CueBannerAttached_IsVisibleChanged;
                if (c is TextBox)
                {
                    (c as TextBox).TextChanged += CueBannerAttached_TextChanged;
                }
                else if (c is PasswordBox)
                {
                    (c as PasswordBox).PasswordChanged += CueBannerAttached_PasswordChanged;
                }
                else if (c is ComboBox)
                {
                    (c as ComboBox).SelectionChanged += CueBannerAttached_SelectionChanged;
                }
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                c.Loaded -= Control_Loaded;
                c.IsVisibleChanged -= CueBannerAttached_IsVisibleChanged;
                if (c is TextBox)
                {
                    (c as TextBox).TextChanged -= CueBannerAttached_TextChanged;
                }
                else if (c is PasswordBox)
                {
                    (c as PasswordBox).PasswordChanged -= CueBannerAttached_PasswordChanged;
                }
                else if (c is ComboBox)
                {
                    (c as ComboBox).SelectionChanged -= CueBannerAttached_SelectionChanged;
                }
            }
        }

        private static void CueBannerAttached_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control)
            {
                if (e.NewValue is bool && (bool)e.NewValue && CheckDisplayConditions(sender as Control))
                {
                    AddCueBanner(sender as Control);
                }
                else
                {
                    RemoveCueBanner(sender as Control);
                }
            }
        }

        private static void CueBannerAttached_SelectionChanged(object sender, EventArgs e)
        {
            if(sender is Control)
            {
                if(CheckDisplayConditions(sender as Control))
                {
                    AddCueBanner(sender as Control);
                }
                else
                {
                    RemoveCueBanner(sender as Control);
                }
            }
        }

        private static void CueBannerAttached_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Control)
            {
                if (CheckDisplayConditions(sender as Control))
                {
                    AddCueBanner(sender as Control);
                }
                else
                {
                    RemoveCueBanner(sender as Control);
                }
            }
        }

        private static void CueBannerAttached_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Control)
            {
                if (CheckDisplayConditions(sender as Control))
                {
                    AddCueBanner(sender as Control);
                }
                else
                {
                    RemoveCueBanner(sender as Control);
                }
            }
        }
        
        private static bool CheckDisplayConditions(Control c)
        {
            if(c is TextBox)
            {
                return (c as TextBox).Text.Length == 0;
            }
            else if(c is PasswordBox)
            {
                return (c as PasswordBox).SecurePassword.Length == 0;
            }
            else if(c is ComboBox)
            {
                return (c as ComboBox).SelectedIndex == -1;
            }
            else
            {
                return false;
            }
        }
        private static bool HasCueBanner(Control c)
        {
            var layer = AdornerLayer.GetAdornerLayer(c);
            var adorners = layer?.GetAdorners(c);
            return adorners != null && !adorners.All((obj) => !(obj is Adorners.CueBannerAdorner));
        }
        private static void AddCueBanner(Control c)
        {
            if(HasCueBanner(c))
            {
                return;
            }
            var layer = AdornerLayer.GetAdornerLayer(c);
            layer?.Add(new Adorners.CueBannerAdorner(c, GetCueBanner(c)));
        }
        private static void RemoveCueBanner(Control c)
        {
            if (!HasCueBanner(c))
            {
                return;
            }
            var layer = AdornerLayer.GetAdornerLayer(c);
            layer.Remove(layer.GetAdorners(c).First((obj) => obj is Adorners.CueBannerAdorner));
        }

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Control && CheckDisplayConditions(sender as Control))
            {
                AddCueBanner(sender as Control);
            }
        }
    }
}
