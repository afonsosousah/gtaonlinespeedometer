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
using System.Windows.Shapes;
using WpfApp1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        System.Timers.Timer Timer1 = new System.Timers.Timer(5);

        #region Public Properties
        public static DependencyProperty ProgressEnabledProperty =
        DependencyProperty.Register("ProgressEnabled", typeof(bool), typeof(CustomSpeedo), new PropertyMetadata(true));

        public static DependencyProperty ProgressColorProperty =
        DependencyProperty.Register("ProgressColor", typeof(System.Drawing.Color), typeof(CustomSpeedo),
        new PropertyMetadata(System.Drawing.Color.Aqua));

        public static DependencyProperty ProgressBackColorProperty =
        DependencyProperty.Register("ProgressBackColor", typeof(System.Drawing.Color), typeof(CustomSpeedo), new PropertyMetadata(System.Drawing.Color.Gray));

        public bool ProgressEnabled
        {
            get { return (bool)base.GetValue(ProgressEnabledProperty); }
            set { base.SetValue(ProgressEnabledProperty, value); }
        }

        public System.Drawing.Color ProgressColor
        {
            get { return (System.Drawing.Color)base.GetValue(ProgressColorProperty); }
            set { base.SetValue(ProgressColorProperty, value); }
        }

        public System.Drawing.Color ProgressBackColor
        {
            get { return (System.Drawing.Color)base.GetValue(ProgressBackColorProperty); }
            set { base.SetValue(ProgressBackColorProperty, value); }
        }
        #endregion

        public Window1()
        {
            InitializeComponent();
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth - this.Height) * 0.95;
            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height) * 0.95;
            Cursor = Cursors.None;

        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        GTAMoreInfo gta = ((window as MainWindow).gta);
                        float maxSpeed = (float)(gta.GetSpeed() / gta.GetMaxSpeed() * 100);
                        if ((window as MainWindow).comboBox.SelectedIndex.Equals(0))
                        {
                            Speedometer1.Visibility = Visibility.Visible;
                            CustomSpeedo.Visibility = Visibility.Hidden;
                            Speed.Visibility = Visibility.Visible;
                            SpeedStr.Visibility = Visibility.Visible;
                            Speedometer1.MajorDivisionsCount = 10;
                            Speedometer1.CurrentValue = gta.GetSpeed();
                            Speed.Content = ((int)gta.GetSpeed()).ToString();
                            CircularProgressBar((float)(gta.GetRPM() * 100));
                            if (gta.GetGear() == 0)
                            {
                                Gear.Content = "R";
                            }
                            else Gear.Content = gta.GetGear();

                        }
                        else if ((window as MainWindow).comboBox.SelectedIndex.Equals(1))
                        {
                            Speedometer1.Visibility = Visibility.Visible;
                            CustomSpeedo.Visibility = Visibility.Hidden;
                            Speed.Visibility = Visibility.Visible;
                            Speedometer1.MaxValue = 8;
                            Speedometer1.MajorDivisionsCount = Speedometer1.MaxValue;
                            Speedometer1.CurrentValue = gta.GetRPM() * Speedometer1.MaxValue;
                            Speed.Content = ((int)gta.GetSpeed()).ToString();
                            CircularProgressBar(maxSpeed);
                            if (gta.GetGear() == 0)
                            {
                                Gear.Content = "R";
                            }
                            else Gear.Content = gta.GetGear();
                        }
                        else if ((window as MainWindow).comboBox.SelectedIndex.Equals(2))
                        {
                            Speedometer1.Visibility = Visibility.Hidden;
                            CustomSpeedo.Visibility = Visibility.Visible;
                            Speed.Visibility = Visibility.Hidden;
                            SpeedStr.Visibility = Visibility.Hidden;
                            Gear.Visibility = Visibility.Hidden;
                            CustomSpeedo.StartAngle = 140;
                            CustomSpeedo.EndAngle = 365;
                            if (CustomSpeedo.Type.Equals("speedometer"))
                            {
                                CustomSpeedo.MinValue = 0;
                                CustomSpeedo.Speed = gta.GetSpeed();
                                CustomSpeedo.Gear = gta.GetGear();
                                CustomSpeedo.Value = gta.GetSpeed();
                                CircularProgressBar((float)(gta.GetRPM() * 100));
                            }
                            else if (CustomSpeedo.Type.Equals("tachometer"))
                            {
                                CustomSpeedo.MaxValue = 1000;
                                CustomSpeedo.MinValue = 0;
                                CustomSpeedo.Speed = gta.GetSpeed();
                                CustomSpeedo.Gear = gta.GetGear();
                                CustomSpeedo.Value = gta.GetRPM() * CustomSpeedo.MaxValue;
                            CircularProgressBar(maxSpeed);
                            }
                            else MessageBox.Show("Custom Gauge Type not recognized");

                            if (gta.forceUnit != "no")
                            {
                                if (gta.forceUnit == "mph") gta.SpeedUnit = 2.23694;
                                if (gta.forceUnit == "kph") gta.SpeedUnit = 3.6;
                            }

                            if (gta.SpeedUnit == 2.23694) gta.SpeedUnitStr = "mph";
                            if (gta.SpeedUnit == 3.6) gta.SpeedUnitStr = "kph";
                        }
                    }
                }
            });
        }

        private void CircularGaugeControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        System.Drawing.Size SizeConvert(Size size)
        {
            System.Drawing.Size outputSize = new System.Drawing.Size();
            outputSize.Height = (int)size.Height;
            outputSize.Width = (int)size.Width;

            return outputSize;
        }

        public void CircularProgressBar(float percentage)
        {
            if (this.ProgressEnabled)
            {
                using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
                {
                    System.Drawing.Image source = new System.Drawing.Bitmap(310, 310);

                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(source);
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(5, 5, 300, 300);

                    DrawProgress(System.Drawing.Graphics.FromImage(source), rect, percentage, this.ProgressColor, this.ProgressBackColor, rect.Size);

                    source.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    image.Source = bitmapimage;
                };
            }
            else
            {
                BitmapImage bitmapimage = new BitmapImage();
                image.Source = bitmapimage;
            } 
        }

        public void DrawProgress(System.Drawing.Graphics g, System.Drawing.Rectangle rect, float percentage, System.Drawing.Color ProgressColor, System.Drawing.Color ProgressBackColor, System.Drawing.Size ProgressSize)
        {
            try
            {
                //work out the angles for each arc
                float progressAngle = 360 * percentage / 100;
                float remainderAngle = 360 - progressAngle;

                if (percentage <= 100)
                {
                    //create pens to use for the arcs
                    System.Drawing.Pen remainderPen = new System.Drawing.Pen(ProgressBackColor, 5);
                    System.Drawing.Pen progressPen = new System.Drawing.Pen(ProgressColor, 4);
                    //set the smoothing to high quality for better output
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //draw the blue and white arcs
                    g.DrawArc(progressPen, rect, -90, progressAngle);
                    g.DrawArc(remainderPen, rect, progressAngle - 90, remainderAngle);
                    remainderPen.Dispose();
                    progressPen.Dispose();
                }

                if (percentage > 100)
                {
                    //create pens to use for the arcs
                    System.Drawing.Pen remainderPen = new System.Drawing.Pen(System.Drawing.Color.Red, 5);
                    System.Drawing.Pen progressPen = new System.Drawing.Pen(ProgressColor, 4);
                    //set the smoothing to high quality for better output
                    //g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    //draw the blue and white arcs
                    g.DrawArc(progressPen, rect, -90, progressAngle);
                    g.DrawArc(remainderPen, rect, progressAngle - 90, remainderAngle);
                    remainderPen.Dispose();
                    progressPen.Dispose();
                }
                g.Dispose();
            }
            catch (OutOfMemoryException e)
            {
                
            }
        }

        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        System.Drawing.Image ControlsImageToImage(System.Windows.Controls.Image image)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Windows.Media.Imaging.BmpBitmapEncoder bbe = new BmpBitmapEncoder();
                bbe.Frames.Add(BitmapFrame.Create(new Uri(image.Source.ToString(), UriKind.RelativeOrAbsolute)));

                bbe.Save(ms);
                System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms);

                return img2;
            }
        }
    }
}

