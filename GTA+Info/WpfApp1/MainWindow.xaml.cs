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
using System.Reflection;
using memory;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.IO.Compression;
using System.IO;
using System.Xml.Linq;
using Forms = System.Windows.Forms;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Timers.Timer Timer1 = new System.Timers.Timer(5);
        System.Timers.Timer Timer2 = new System.Timers.Timer(500);
        public GTAMoreInfo gta = new GTAMoreInfo();
        Window1 window1 = new Window1();
        Window2 window2 = new Window2();
        Window3 window3 = new Window3();
        bool moveSpeedo = false;

        public string SpeedUnitStr = new GTAMoreInfo().SpeedUnitStr;

        public MainWindow()
        {

            InitializeComponent();
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
            Timer2.Enabled = true;
            Timer2.Elapsed += Timer2_Elapsed;
            Speedometer1.MaxValue = 300;
            this.Closed += MainWindow_Closed;

            //MessageBox.Show(gta.GetUnkPtr().ToString());

            window3.Show();

            if (Process.GetProcessesByName("GTA5").Length > 0)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 0, 255, 0);
                GTAStatus.Content = "Running";
                GTAStatus.Foreground = brush;
                gta.getAddresses();
            }
            else
            {
                MessageBox.Show("GTA not detected or not running");
                this.Close();
            }

            if (Directory.Exists(@"C:\Temp\GTA+Info"))
            {
                //Directory Exists
            }
            else Directory.CreateDirectory(@"C:\Temp\GTA+Info");

            if (Directory.Exists(@"C:\Temp\GTA+Info\DefaultGauges"))
            {
                //Directory Exists
                System.IO.DirectoryInfo di = new DirectoryInfo(@"C:\Temp\GTA+Info\DefaultGauges");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(@"C:\Temp\GTA+Info\DefaultGauges");
            }
        }

        private void Timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (gta.IsInVehicle())
            {
                gta.getAddresses();
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            File.Delete(@"C:\Temp\GTA+Info\background.png");
            File.Delete(@"C:\Temp\GTA+Info\needle.png");
            File.Delete(@"C:\Temp\GTA+Info\config.xml");

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    (window as Window1).Close();
                }
            }
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Speedometer1.MaxValue = window1.Speedometer1.MaxValue;
                Speedometer1.MajorDivisionsCount = window1.Speedometer1.MajorDivisionsCount;
                if (window1.Visibility.Equals(Visibility.Hidden) && gta.IsInVehicle() == true)
                {
                    window1.Show();
                }
                else if (gta.IsInVehicle() == false) window1.Hide();


                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Window1))
                    {
                        (window as Window1).SpeedStr.Content = gta.SpeedUnitStr;
                    }
                };

                if (gta.IsInVehicle() == false)
                {
                    window3.label.Content = "PlayerSpeed: " + Math.Round(gta.GetSpeed(), 1);
                    window3.label.Visibility = Visibility.Visible;
                } else window3.label.Visibility = Visibility.Hidden;

                window3.label1.Content = gta.GetPlayerCoords();

                if (gta.SpeedUnitStr == "mph")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else if (gta.SpeedUnitStr == "kph")
                {
                    comboBox1.SelectedIndex = 1;
                }
            });
        }

        private void Speedometer_Click(object sender, RoutedEventArgs e)
        {
            SpeedometerPage.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
            this.Width = 450;
            this.Height = 500;
        }
        private void Overlay_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 180;
            OverlayPage.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 240;
            SettingsPage.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 200;
            AboutPage.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SpeedometerPage.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
            this.Width = 450;
            this.Height = 300;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();

            if (window2.Visibility != Visibility.Visible) window2.Visibility = Visibility.Visible; 

            /*foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window2))
                {
                        (window as Window2).Show();
                }
            }*/
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Speedometer1.GaugeBackgroundColor = gta.ConvertColor(ColorDialog1.Color);
                window1.Speedometer1.GaugeBackgroundColor = gta.ConvertColor(ColorDialog1.Color);
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                window1.ProgressColor = ColorDialog1.Color;
                //window1.Speedometer1.GaugeBackgroundColor = ColorDialog1.Color;
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                window1.ProgressBackColor = ColorDialog1.Color;
                //window1.Speedometer1.GaugeBackgroundColor = ColorDialog1.Color;
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            if (window1.ProgressEnabled)
            {
                window1.ProgressEnabled = false;
            }
            else window1.ProgressEnabled = true;
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex.Equals(0))
            {
                gta.SpeedUnit = 2.23694;
                gta.SpeedUnitStr = "mph";
            }
            else if (comboBox1.SelectedIndex.Equals(1))
            {
                gta.SpeedUnit = 3.6;
                gta.SpeedUnitStr = "kph";
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            if (window1.Gear.Visibility.Equals(Visibility.Visible))
            {
                window1.Gear.Visibility = Visibility.Hidden;
            }
            else window1.Gear.Visibility = Visibility.Visible;
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            string Prompt = "Enter new MaxValue";
            string Title = "GTA+Info";
            string Result = Microsoft.VisualBasic.Interaction.InputBox(Prompt, Title, "200", 150, 150);
            int intResult;

            if (Result.Equals(""))
            {
                MessageBox.Show("You entered nothing :|");
            }
            else if (Int32.TryParse(Result, out intResult))
            {
                Speedometer1.MaxValue = intResult;
                window1.Speedometer1.MaxValue = intResult;
            }
            else
                MessageBox.Show("Input type was incorrect. Try again");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex.Equals(2))
            {
                gta.LoadCustomGaugeWindow1();
            }
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            if (window3.Top == (System.Windows.SystemParameters.PrimaryScreenHeight - window3.Height) * 0.83)
            {
                window3.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - window3.Height) * 0.58;
            }
            else window3.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - window3.Height) * 0.83;

        }

        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = gta.ConvertColor(ColorDialog1.Color);
                window3.label.Foreground = brush;
                window3.label1.Foreground = brush;
            }
        }

        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
            Menu.Visibility = Visibility.Visible;
            OverlayPage.Visibility = Visibility.Hidden;
        }

        private void Button13_Click(object sender, RoutedEventArgs e)
        {
            //Load Settings
            Speedometer1.GaugeBackgroundColor = gta.ConvertColor(WpfApp1.Properties.Settings.Default.DialColor);
            window1.Speedometer1.GaugeBackgroundColor = gta.ConvertColor(WpfApp1.Properties.Settings.Default.DialColor);
            window1.ProgressColor = WpfApp1.Properties.Settings.Default.ProgressColor;
            window1.ProgressBackColor = WpfApp1.Properties.Settings.Default.ProgressBackColor;
            window1.Speedometer1.MaxValue = WpfApp1.Properties.Settings.Default.MaxValue;
        }

        private void Button14_Click(object sender, RoutedEventArgs e)
        {
            //Save Settings
            WpfApp1.Properties.Settings.Default.DialColor = gta.ConvertColor2(Speedometer1.GaugeBackgroundColor);
            WpfApp1.Properties.Settings.Default.ProgressColor = window1.ProgressColor;
            WpfApp1.Properties.Settings.Default.ProgressBackColor = window1.ProgressBackColor;
            WpfApp1.Properties.Settings.Default.MaxValue = window1.Speedometer1.MaxValue;
            Properties.Settings.Default.Save();
        }

        private void Button15_Click(object sender, RoutedEventArgs e)
        {
            //Reset Settings
            WpfApp1.Properties.Settings.Default.DialColor = System.Drawing.Color.White;
            WpfApp1.Properties.Settings.Default.ProgressColor = System.Drawing.Color.Aqua;
            WpfApp1.Properties.Settings.Default.ProgressBackColor = System.Drawing.Color.Gray;
            WpfApp1.Properties.Settings.Default.MaxValue = 200;

            Speedometer1.GaugeBackgroundColor = gta.ConvertColor(WpfApp1.Properties.Settings.Default.DialColor);
            window1.ProgressColor = WpfApp1.Properties.Settings.Default.ProgressColor;
            window1.ProgressBackColor = WpfApp1.Properties.Settings.Default.ProgressBackColor;
            window1.Speedometer1.MaxValue = WpfApp1.Properties.Settings.Default.MaxValue;
        }

        private void Button16_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
            SettingsPage.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
        }

        private void Button17_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
            AboutPage.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_RequestNavigate1(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            window3.Show();
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            window3.Hide();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            moveSpeedo = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            moveSpeedo = false;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (moveSpeedo)
            {
            if ((e.Key == Key.Up && Keyboard.IsKeyDown(Key.LeftCtrl)) || e.Key == Key.NumPad8)
            {
                Point point = new Point(0, -5);
                window1.Left = window1.Left + point.X;
                window1.Top = window1.Top + point.Y;
            }
            if ((e.Key == Key.Down && Keyboard.IsKeyDown(Key.LeftCtrl)) || e.Key == Key.NumPad2)
            {
                Point point = new Point(0, 5);
                window1.Left = window1.Left + point.X;
                window1.Top = window1.Top + point.Y;
            }
            if ((e.Key == Key.Left && Keyboard.IsKeyDown(Key.LeftCtrl)) || e.Key == Key.NumPad4)
            {
                Point point = new Point(-5, 0);
                window1.Left = window1.Left + point.X;
                window1.Top = window1.Top + point.Y;
            }
            if ((e.Key == Key.Right && Keyboard.IsKeyDown(Key.LeftCtrl)) || e.Key == Key.NumPad6)
            {
                Point point = new Point(5, 0);
                window1.Left = window1.Left + point.X;
                window1.Top = window1.Top + point.Y;
            }
            }

        }
    }
}
