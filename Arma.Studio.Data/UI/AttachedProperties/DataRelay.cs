using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arma.Studio.Data.UI.AttachedProperties
{
    public class DataRelay : Freezable
    {
        #region DependencyProperty: Source (System.Object)
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(object),
                typeof(DataRelay),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));
        private static void OnSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is DataRelay dataTunnel)
            {
                dataTunnel.Target = e.NewValue;
            }
        }
        /// <summary>
        /// The (preferably) ReadOnly DataSource.
        /// </summary>
        public object Source
        {
            get => this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }
        #endregion
        #region DependencyProperty: Target (System.Object)
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(
                nameof(Target),
                typeof(object),
                typeof(DataRelay),
                new FrameworkPropertyMetadata());
        /// <summary>
        /// The Target Object.
        /// </summary>
        public object Target
        {
            get => this.GetValue(TargetProperty);
            set => this.SetValue(TargetProperty, value);
        }
        #endregion
        protected override Freezable CreateInstanceCore()
        {
            return new DataRelay();
        }
    }
}
