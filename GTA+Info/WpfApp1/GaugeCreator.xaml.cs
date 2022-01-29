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
using System.IO.Compression;
using System.IO;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        GTAMoreInfo gta = new GTAMoreInfo();
        System.Timers.Timer Timer1 = new System.Timers.Timer(10);

        public Window2()
        {
            InitializeComponent();
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;

            textBox.Text = "140";
            textBox1.Text = "365";
            textBox2.Text = "200";
            textBox3.Text = "no";
            textBox4.Text = "tachometer";
            textBox5.Text = "True";
            textBox6.Text = "126,82,126,181";
            textBox7.Text = "105,179,105,66";
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (CustomSpeedo1.Type.Equals("speedometer"))
                {
                    CustomSpeedo1.MinValue = 0;
                    CustomSpeedo1.Speed = 96;
                    CustomSpeedo1.Gear = 5;
                    CustomSpeedo1.Value = 96;
                }
                else if (CustomSpeedo1.Type.Equals("tachometer"))
                {
                    CustomSpeedo1.MaxValue = 1000;
                    CustomSpeedo1.MinValue = 0;
                    CustomSpeedo1.Speed = 96;
                    CustomSpeedo1.Gear = 5;
                    CustomSpeedo1.Value = 0.3 * CustomSpeedo1.MaxValue;
                }
                else MessageBox.Show("Custom Gauge Type not recognized");

                slider.Maximum = CustomSpeedo1.MaxValue;
                CustomSpeedo1.Value = slider.Value;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Load Gauge
            gta.LoadCustomGaugeWindow2();

            textBox.Text = CustomSpeedo1.StartAngle.ToString();
            textBox1.Text = CustomSpeedo1.EndAngle.ToString();
            textBox2.Text = CustomSpeedo1.MaxValue.ToString();
            textBox3.Text = gta.forceUnit;
            textBox4.Text = CustomSpeedo1.Type;
            textBox5.Text = CustomSpeedo1.GearEnabled.ToString();
            textBox6.Text = CustomSpeedo1.GearLoc;
            textBox7.Text = CustomSpeedo1.DigiValLoc;
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //Save Gauge
            System.Windows.Forms.SaveFileDialog SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            SaveFileDialog1.Title = "Save custom gauge as zip";
            SaveFileDialog1.Filter = "zip file|*.zip";
            SaveFileDialog1.FileName = "";
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List list = new List();

                if (Directory.Exists(@"C:\Temp\GTA+Info"))
                {
                    //Directory Exists
                }
                else Directory.CreateDirectory(@"C:\Temp\GTA+Info");

                if (Directory.Exists(@"C:\Temp\GTA+Info\DefaultGauges"))
                {
                    //Directory Exists
                    Directory.Delete(@"C:\Temp\GTA+Info\DefaultGauges");
                }

                if (File.Exists(@"C:\Temp\GTA+Info\background.png") && File.Exists(@"C:\Temp\GTA+Info\needle.png") && File.Exists(@"C:\Temp\GTA+Info\config.xml"))
                {
                    File.Delete(@"C:\Temp\GTA+Info\background.png");
                    File.Delete(@"C:\Temp\GTA+Info\needle.png");
                    File.Delete(@"C:\Temp\GTA+Info\config.xml");
                }

                XDocument xDoc = new XDocument(
                    new XElement("Root",
                                    new XElement("StartAngle", textBox.Text),
                                    new XElement("EndAngle", textBox1.Text),
                                    new XElement("MaxValue", textBox2.Text),
                                    new XElement("ForceUnit", textBox3.Text),
                                    new XElement("GearsEnabled", textBox5.Text),
                                    new XElement("Type", textBox4.Text),
                                    new XElement("DigitalValueLocation", textBox7.Text),
                                    new XElement("GearLocation", textBox6.Text),
                                    new XElement("SpeedColor", CustomSpeedo1.SpeedColor.ToKnownColor()),
                                    new XElement("GearColor", CustomSpeedo1.GearColor.ToKnownColor())
                                )
                );

                xDoc.Save(@"C:\Temp\GTA+Info\config.xml");

                gta.Save(CustomSpeedo1.NeedleImage, @"C:\Temp\GTA+Info\needle.png");
                gta.Save(CustomSpeedo1.GaugeImage, @"C:\Temp\GTA+Info\background.png");

                if (File.Exists(System.IO.Path.GetFullPath(SaveFileDialog1.FileName)))
                {
                    File.Delete(System.IO.Path.GetFullPath(SaveFileDialog1.FileName));
                }

                if (File.Exists(System.IO.Path.GetFullPath(SaveFileDialog1.FileName)))
                {
                    File.Delete(System.IO.Path.GetFullPath(SaveFileDialog1.FileName));
                    ZipFile.CreateFromDirectory(@"C:\Temp\GTA+Info", System.IO.Path.GetFullPath(SaveFileDialog1.FileName));
                }

                else if (File.Exists(System.IO.Path.GetFullPath(SaveFileDialog1.FileName)) == false)
                {
                    ZipFile.CreateFromDirectory(@"C:\Temp\GTA+Info", System.IO.Path.GetFullPath(SaveFileDialog1.FileName));
                }

                File.Delete(@"C:\Temp\GTA+Info\background.png");
                File.Delete(@"C:\Temp\GTA+Info\needle.png");
                File.Delete(@"C:\Temp\GTA+Info\config.xml");
            }
            
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            //Apply Changes
            CustomSpeedo1.StartAngle = Convert.ToDouble(textBox.Text);
            CustomSpeedo1.EndAngle = Convert.ToDouble(textBox1.Text);
            CustomSpeedo1.MaxValue = Convert.ToDouble(textBox2.Text);
            gta.forceUnit = textBox3.Text;
            CustomSpeedo1.Type = textBox4.Text;
            CustomSpeedo1.GearEnabled = Convert.ToBoolean(textBox5.Text);
            CustomSpeedo1.GearLoc = textBox6.Text;
            CustomSpeedo1.DigiValLoc = textBox7.Text;
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            //Load background.png
            System.Windows.Forms.OpenFileDialog OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BitmapImage background = gta.BitmapToImageSource(new System.Drawing.Bitmap(OpenFileDialog1.FileName));

                CustomSpeedo1.GaugeImage = background;
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            //Load needle.png
            System.Windows.Forms.OpenFileDialog OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BitmapImage needle = gta.BitmapToImageSource(new System.Drawing.Bitmap(OpenFileDialog1.FileName));

                CustomSpeedo1.NeedleImage = needle;
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            //Change Speed Color
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CustomSpeedo1.SpeedColor = ColorDialog1.Color;
            }
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            //Change Gear Color
            System.Windows.Forms.ColorDialog ColorDialog1 = new System.Windows.Forms.ColorDialog();
            if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CustomSpeedo1.GearColor = ColorDialog1.Color;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
    }
}
