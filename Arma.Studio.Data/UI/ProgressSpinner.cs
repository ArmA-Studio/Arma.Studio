using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Arma.Studio.Data.UI
{
    public class ProgressSpinner : FrameworkElement
    {
        static ProgressSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressSpinner), new FrameworkPropertyMetadata(typeof(ProgressSpinner)));
        }
        #region DependencyProperty: FragmentActive [AffectsRender] (System.Windows.Media.Color)
        public static readonly DependencyProperty FragmentActiveProperty = DependencyProperty.Register(
                "FragmentActive",
                typeof(Color),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public Color FragmentActive
        {
            get => (Color)this.GetValue(FragmentActiveProperty);
            set => this.SetValue(FragmentActiveProperty, value);
        }

        #endregion
        #region DependencyProperty: FragmentInactive [AffectsRender] (System.Windows.Media.Color)
        public static readonly DependencyProperty FragmentInactiveProperty = DependencyProperty.Register(
                "FragmentInactive",
                typeof(Color),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(Colors.LightGray, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public Color FragmentInactive
        {
            get => (Color)this.GetValue(FragmentInactiveProperty);
            set => this.SetValue(FragmentInactiveProperty, value);
        }

        #endregion
        #region DependencyProperty: FragmentBorderFill [AffectsRender] (System.Windows.Media.Brush)
        public static readonly DependencyProperty FragmentBorderFillProperty = DependencyProperty.Register(
                nameof(FragmentBorderFill),
                typeof(Brush),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public Brush FragmentBorderFill
        {
            get => this.GetValue(FragmentBorderFillProperty) as Brush;
            set => this.SetValue(FragmentBorderFillProperty, value);
        }

        #endregion
        #region DependencyProperty: FragmentBorderThickness [AffectsRender] (System.Int32)
        public static readonly DependencyProperty FragmentBorderThicknessProperty = DependencyProperty.Register(
                "FragmentBorderThickness",
                typeof(int),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender)
                );
        public int FragmentBorderThickness
        {
            get => (int)this.GetValue(FragmentBorderThicknessProperty);
            set => this.SetValue(FragmentBorderThicknessProperty, value);
        }

        #endregion
        #region DependencyProperty: InnerDistance [AffectsRender] (System.Double)
        public static readonly DependencyProperty InnerDistanceProperty = DependencyProperty.Register(
                "InnerDistance",
                typeof(double),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsRender)
                );
        public double InnerDistance
        {
            get => (double)this.GetValue(InnerDistanceProperty);
            set => this.SetValue(InnerDistanceProperty, value);
        }

        #endregion
        #region DependencyProperty: OuterDistance [AffectsRender] (System.Double)
        public static readonly DependencyProperty OuterDistanceProperty = DependencyProperty.Register(
                "OuterDistance",
                typeof(double),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(0.25, FrameworkPropertyMetadataOptions.AffectsRender)
                );
        public double OuterDistance
        {
            get => (double)this.GetValue(OuterDistanceProperty);
            set => this.SetValue(OuterDistanceProperty, value);
        }

        #endregion
        #region DependencyProperty: FragmentCount [PropertyChangedCallback, AffectsRender] (System.Int32)
        public static readonly DependencyProperty FragmentCountProperty = DependencyProperty.Register(
                "ElementCount",
                typeof(int),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(8, FrameworkPropertyMetadataOptions.AffectsRender, FragmentCount_PropertyChangedCallback)
            );
        public int FragmentCount
        {
            get => (int)this.GetValue(FragmentCountProperty);
            set => this.SetValue(FragmentCountProperty, value);
        }
        private static void FragmentCount_PropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is ProgressSpinner spinner))
            {
                return;
            }

            spinner.Animation = new Int32Animation(spinner.FragmentCount, new Duration(spinner.Delay))
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
        }
        #endregion
        #region DependencyProperty: DistanceMultiplier [AffectsRender] (System.Double)
        public static readonly DependencyProperty DistanceMultiplierProperty = DependencyProperty.Register(
                "DistanceMultiplier",
                typeof(double),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public double DistanceMultiplier
        {
            get => (double)this.GetValue(DistanceMultiplierProperty);
            set => this.SetValue(DistanceMultiplierProperty, value);
        }
        #endregion
        #region DependencyProperty: CurrentFragment [AffectsRender] (System.Int32)
        public static readonly DependencyProperty CurrentFragmentProperty = DependencyProperty.Register(
                "CurrentFragment",
                typeof(int),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public int CurrentFragment
        {
            get => (int)this.GetValue(CurrentFragmentProperty);
            set => this.SetValue(CurrentFragmentProperty, value);
        }
        #endregion
        #region DependencyProperty: IsAnimating [PropertyChangedCallback, AffectsRender] (System.Boolean)
        public static readonly DependencyProperty IsAnimatingProperty = DependencyProperty.Register(
                "IsAnimating",
                typeof(bool),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, IsAnimating_PropertyChangedCallback)
            );
        public bool IsAnimating
        {
            get => (bool)this.GetValue(IsAnimatingProperty);
            set => this.SetValue(IsAnimatingProperty, value);
        }

        private static void IsAnimating_PropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is ProgressSpinner spinner))
            {
                return;
            }

            if (spinner.Animation == null)
            {
                spinner.Animation = new Int32Animation(spinner.FragmentCount, new Duration(spinner.Delay))
                {
                    RepeatBehavior = RepeatBehavior.Forever
                };
            }
            spinner.BeginAnimation(CurrentFragmentProperty, (e.NewValue == null ? false : (bool)e.NewValue) ? spinner.Animation : null);
        }
        #endregion
        #region DependencyProperty: Delay [PropertyChangedCallback, AffectsRender] (System.TimeSpan)
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
                nameof(Delay),
                typeof(TimeSpan),
                typeof(ProgressSpinner),
                new FrameworkPropertyMetadata(new TimeSpan(0, 0, 0, 0, 750), FrameworkPropertyMetadataOptions.AffectsRender, Delay_PropertyChangedCallback)
            );
        public TimeSpan Delay
        {
            get => (TimeSpan)this.GetValue(DelayProperty);
            set => this.SetValue(IsAnimatingProperty, value);
        }
        private static void Delay_PropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is ProgressSpinner spinner))
            {
                return;
            }
            spinner.Animation = new Int32Animation(spinner.FragmentCount, new Duration(spinner.Delay))
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

        }
        #endregion


        public Int32Animation Animation { get; private set; }


        public ProgressSpinner()
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double heightHalved = this.RenderSize.Height / 2;
            double widthHalved = this.RenderSize.Width / 2;
            var fragmentActive = this.FragmentActive;
            var fragmentInactive = this.FragmentInactive;
            int fragmentBorderThickness = this.FragmentBorderThickness;
            var fragmentBorderFill = this.FragmentBorderFill;
            int fragmentCount = this.FragmentCount;
            int currentFragment = this.CurrentFragment;
            bool isAnimating = this.IsAnimating;

            double innerDistance = this.InnerDistance;
            double outerDistance = this.OuterDistance;
            var pen = new Pen(fragmentBorderFill, fragmentBorderThickness);
            pen.Freeze();

            double degreePerFragment = 360 / this.DistanceMultiplier;
            double radPerFragment = Math.PI * degreePerFragment / 180.0;

            var vectorBotRight = new Vector(Math.Cos(radPerFragment / 2), Math.Sin(radPerFragment / 2));
            var vectorBotLeft = new Vector(Math.Cos(-(radPerFragment / 2)), Math.Sin(-(radPerFragment / 2)));

            var vectorTopRight = vectorBotRight * (1 - outerDistance);
            var vectorTopLeft = vectorBotLeft * (1 - outerDistance);

            var fragmentInactiveBrush = new SolidColorBrush(fragmentInactive);
            fragmentInactiveBrush.Freeze();

            vectorBotRight *= innerDistance;
            vectorBotLeft *= innerDistance;

            vectorBotRight *= widthHalved;
            vectorBotLeft *= widthHalved;

            vectorTopRight *= widthHalved;
            vectorTopLeft *= widthHalved;
            for (int i = 0; i < fragmentCount; i++)
            {
                var geo = new StreamGeometry();
                using (var geoContext = geo.Open())
                {
                    geoContext.BeginFigure(new Point(vectorBotLeft.X, vectorBotLeft.Y), true, false);
                    geoContext.LineTo(new Point(vectorBotRight.X, vectorBotRight.Y), false, false);
                    geoContext.LineTo(new Point(vectorTopRight.X, vectorTopRight.Y), false, false);
                    geoContext.LineTo(new Point(vectorTopLeft.X, vectorTopLeft.Y), false, false);
                }
                var transformgroup = new TransformGroup();
                var rotTransform = new RotateTransform(360 / fragmentCount * i);
                rotTransform.Freeze();
                transformgroup.Children.Add(rotTransform);
                var locTransform = new TranslateTransform(widthHalved, heightHalved);
                locTransform.Freeze();
                transformgroup.Children.Add(locTransform);
                transformgroup.Freeze();
                geo.Transform = transformgroup;
                geo.Freeze();
                if (isAnimating)
                {
                    double leftStrength, rightStrength;
                    if (i >= currentFragment)
                    {
                        leftStrength = (double)(fragmentCount - Math.Abs(i - currentFragment)) / fragmentCount;
                        rightStrength = 1 - (double)(fragmentCount - Math.Abs(i - currentFragment)) / fragmentCount;
                    }
                    else
                    {
                        leftStrength = 1 - (double)(fragmentCount - Math.Abs(i - currentFragment)) / fragmentCount;
                        rightStrength = (double)(fragmentCount - Math.Abs(i - currentFragment)) / fragmentCount;
                    }
                    var brush = new SolidColorBrush(Color.FromArgb(
                        (byte)(leftStrength * fragmentInactive.A + rightStrength * fragmentActive.A),
                        (byte)(leftStrength * fragmentInactive.R + rightStrength * fragmentActive.R),
                        (byte)(leftStrength * fragmentInactive.G + rightStrength * fragmentActive.G),
                        (byte)(leftStrength * fragmentInactive.B + rightStrength * fragmentActive.B)
                    ));
                    brush.Freeze();
                    drawingContext.DrawGeometry(brush, pen, geo);
                }
                else
                {
                    drawingContext.DrawGeometry(fragmentInactiveBrush, pen, geo);
                }
            }
            base.OnRender(drawingContext);
        }
    }
}
