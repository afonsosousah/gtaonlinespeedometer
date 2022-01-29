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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CustomSpeedo.xaml
    /// </summary>
    public partial class CustomSpeedo : UserControl
    {

        #region Dependency Properties

        static System.Drawing.Rectangle defaultSize = new System.Drawing.Rectangle()
        {
            Width = 300,
            Height = 300
        };

        public static DependencyProperty AutoScaleProperty =
            DependencyProperty.Register("AutoScale", typeof(bool), typeof(CustomSpeedo), new PropertyMetadata(true));

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CustomSpeedo),
            new PropertyMetadata(double.NegativeInfinity, OnGaugeChanged));

        public static DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(CustomSpeedo), new PropertyMetadata(double.NegativeInfinity));

        public static DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(CustomSpeedo), new PropertyMetadata(0.0d));

        public static DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(CustomSpeedo), new PropertyMetadata(-45.0, OnGaugeChanged));

        public static DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(CustomSpeedo), new PropertyMetadata(225.0, OnGaugeChanged));

        public static DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(System.Drawing.Size), typeof(CustomSpeedo), new PropertyMetadata(defaultSize.Size));

        public static DependencyProperty GearProperty =
            DependencyProperty.Register("Gear", typeof(double), typeof(CustomSpeedo),
            new PropertyMetadata(double.NegativeInfinity, OnGaugeChanged));

        public static DependencyProperty GearEnabledProperty =
            DependencyProperty.Register("GearEnabled", typeof(bool), typeof(CustomSpeedo),
            new PropertyMetadata(true, OnGaugeChanged));

        public static DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(CustomSpeedo),
            new PropertyMetadata(double.NegativeInfinity, OnGaugeChanged));

        public static DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(CustomSpeedo),
            new PropertyMetadata("tachometer", OnGaugeChanged));

        public static DependencyProperty GaugeImageProperty =
            DependencyProperty.Register("GaugeImage", typeof(BitmapImage), typeof(CustomSpeedo), new PropertyMetadata(BitmapToImageSource(WpfApp1.Properties.Resources.help), OnGaugeChanged));

        public static DependencyProperty NeedleImageProperty =
            DependencyProperty.Register("NeedleImage", typeof(BitmapImage), typeof(CustomSpeedo), new PropertyMetadata(BitmapToImageSource(WpfApp1.Properties.Resources.help), OnGaugeChanged));

        public static DependencyProperty GearLocProperty =
            DependencyProperty.Register("GearLoc", typeof(string), typeof(CustomSpeedo),
            new PropertyMetadata("126,82,126,181", OnGaugeChanged));

        public static DependencyProperty DigiValLocProperty =
            DependencyProperty.Register("DigiValLoc", typeof(string), typeof(CustomSpeedo),
            new PropertyMetadata("105,179,105,66", OnGaugeChanged));

        public static DependencyProperty GearColorProperty =
            DependencyProperty.Register("GearColor", typeof(System.Drawing.Color), typeof(CustomSpeedo),
            new PropertyMetadata(System.Drawing.Color.Black, OnGaugeChanged));

        public static DependencyProperty SpeedColorProperty =
            DependencyProperty.Register("SpeedColor", typeof(System.Drawing.Color), typeof(CustomSpeedo),
            new PropertyMetadata(System.Drawing.Color.Black, OnGaugeChanged));

        public static void OnGaugeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gauge = d as CustomSpeedo;
            gauge.UpdateAngle();
        }

        #endregion Dependency Properties

        #region Public Vars

        public bool AutoScale
        {
            get { return (bool)base.GetValue(AutoScaleProperty); }
            set { base.SetValue(AutoScaleProperty, value); }
        }

        public double Value
        {
            get { return (double)base.GetValue(ValueProperty); }
            set { base.SetValue(ValueProperty, value); }
        }

        public double MaxValue
        {
            get { return (double)base.GetValue(MaxValueProperty); }
            set { base.SetValue(MaxValueProperty, value); }
        }

        public double MinValue
        {
            get { return (double)base.GetValue(MinValueProperty); }
            set { base.SetValue(MinValueProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)base.GetValue(StartAngleProperty); }
            set { base.SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get { return (double)base.GetValue(EndAngleProperty); }
            set { base.SetValue(EndAngleProperty, value); }
        }

        public System.Drawing.Size ControlSize
        {
            get { return (System.Drawing.Size)base.GetValue(SizeProperty); }
            set { base.SetValue(SizeProperty, value); }
        }

        public double Gear
        {
            get { return (double)base.GetValue(GearProperty); }
            set { base.SetValue(GearProperty, value); }
        }

        public bool GearEnabled
        {
            get { return (bool)base.GetValue(GearEnabledProperty); }
            set { base.SetValue(GearEnabledProperty, value); }
        }

        public double Speed
        {
            get { return (double)base.GetValue(SpeedProperty); }
            set { base.SetValue(SpeedProperty, value); }
        }

        public string Type
        {
            get { return (string)base.GetValue(TypeProperty); }
            set { base.SetValue(TypeProperty, value); }
        }

        public string GearLoc
        {
            get { return (string)base.GetValue(GearLocProperty); }
            set { base.SetValue(GearLocProperty, value); }
        }

        public string DigiValLoc
        {
            get { return (string)base.GetValue(DigiValLocProperty); }
            set { base.SetValue(DigiValLocProperty, value); }
        }

        public System.Drawing.Color GearColor
        {
            get { return (System.Drawing.Color)base.GetValue(GearColorProperty); }
            set { base.SetValue(GearColorProperty, value); }
        }

        public System.Drawing.Color SpeedColor
        {
            get { return (System.Drawing.Color)base.GetValue(SpeedColorProperty); }
            set { base.SetValue(SpeedColorProperty, value); }
        }

        public BitmapImage GaugeImage
        {
            get { return (BitmapImage)base.GetValue(GaugeImageProperty); }
            set { base.SetValue(GaugeImageProperty, value); }
        }

        public BitmapImage NeedleImage
        {
            get { return (BitmapImage)base.GetValue(NeedleImageProperty); }
            set { base.SetValue(NeedleImageProperty, value); }
        }

        #endregion Public Vars

        public CustomSpeedo()
        {
            InitializeComponent();

            // Add events
            this.Loaded += new RoutedEventHandler(Gauge_Loaded);
        }

        private void Gauge_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAngle();
            AutoSize();
        }

        internal void AutoSize()
        {

            needle.Width = ControlSize.Width;
            needle.Height = ControlSize.Height;
            background.Width = ControlSize.Width;
            background.Height = ControlSize.Height;

            needle.UpdateLayout();
            background.UpdateLayout();

        }

        internal void UpdateAngle()
        {
            if (this.Value == double.NegativeInfinity)
            {
                RotateTransform rotateTransform2 = new RotateTransform(StartAngle, needle.Width / 2, needle.Height / 2);
                needle.RenderTransform = rotateTransform2;
                return;
            }
            needle.Visibility = System.Windows.Visibility.Visible;

            double valueInPercent = this.Value / this.MaxValue;
            if (this.AutoScale)
                valueInPercent = this.Value / this.MaxValue;
            else
                valueInPercent = (this.Value - this.MinValue) / (this.MaxValue - this.MinValue);

            var valueInDegrees = this.Value * (EndAngle - StartAngle) / MaxValue;

            RotateTransform rotateTransform = new RotateTransform(valueInDegrees, needle.Width / 2, needle.Height / 2);
            needle.RenderTransform = rotateTransform;

            if (this.GearEnabled)
            {
                label.Content = Math.Round(this.Speed).ToString();
                if (this.Gear == 0)
                {
                    label1.Content = "R";
                }
                else label1.Content = this.Gear.ToString();
            }

            ThicknessConverter thick = new ThicknessConverter();
            label.Margin = (Thickness)thick.ConvertFrom(this.DigiValLoc);
            label1.Margin = (Thickness)thick.ConvertFrom(this.GearLoc);

            SolidColorBrush speed = new SolidColorBrush(); speed.Color = ConvertColor(this.SpeedColor);
            SolidColorBrush gear = new SolidColorBrush(); gear.Color = ConvertColor(this.GearColor);
            label.Foreground = speed;
            label1.Foreground = gear;

            background.Source = this.GaugeImage;
            needle.Source = this.NeedleImage;

            // Set Binding on Value
            /*Binding value = new Binding("Value");
            value.ElementName = "GaugeControl";
            
            value.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            value.StringFormat = this.ValueFormat;
            ValueText.SetBinding(TextBlock.TextProperty, value);

            var b = ValueText.GetBindingExpression(TextBlock.TextProperty);
            if (b != null)
                b.UpdateTarget();*/
        }
        static BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public Color ConvertColor(System.Drawing.Color inputColor)
        {
            Color outputColor = Color.FromArgb(inputColor.A, inputColor.R, inputColor.G, inputColor.B);
            return outputColor;
        }

        public void CleanGauge()
        {
            this.GaugeImage.Freeze();
            this.NeedleImage.Freeze();
            needle.Source = null;
            background.Source = null;
            UpdateLayout();
            GC.Collect();
        }
    }
}
