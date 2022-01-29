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
using WpfApp1;

public class GTAMoreInfo
{

    #region DLLImports
    [DllImport("kernel32.dll")]
    protected static extern bool ReadProcessMemory(
    IntPtr hProcess,
    Int64 lpBaseAddress,
    byte[] lpBuffer,
    int dwSize,
    out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    protected static extern bool WriteProcessMemory(
    IntPtr hProcess,
    Int64 lpBaseAddress,
    byte[] lpBuffer,
    int dwSize,
    out IntPtr lpNumberOfBytesWritten);

    public int ReadInteger(long BaseAddress)
    {
        IntPtr bytesRead;
        byte[] buffer = new byte[4];
        ReadProcessMemory(process.Handle, BaseAddress, buffer, buffer.Length, out bytesRead);
        return BitConverter.ToInt32(buffer, 0);
    }
    public long ReadInt64(long BaseAddress)
    {
        IntPtr bytesRead;
        byte[] buffer = new byte[8];
        ReadProcessMemory(process.Handle, BaseAddress, buffer, buffer.Length, out bytesRead);
        return BitConverter.ToInt64(buffer, 0);
    }
    public float ReadFloat(long BaseAddress)
    {
        IntPtr bytesRead;
        byte[] buffer = new byte[4];
        ReadProcessMemory(process.Handle, BaseAddress, buffer, buffer.Length, out bytesRead);
        return BitConverter.ToSingle(buffer, 0);
    }
    #endregion

    #region Public Variables
    Process process = Process.GetProcessesByName(processName)[0];
    public static string processName = "GTA5";
    public static int callOnce = 0;

    //CheatEngine code is in green
    public long WorldPtr;                                        //WorldPTR=getAddress('WorldPTR') WorldPTR=WorldPTR+readInteger(WorldPTR+3)+7
    public long UnkPtr;                                          //UnkPTR=UnkPTR+readInteger(UnkPTR+3)+7
                                                                 //Public VehiclePtr As Long = MyAddress + &H1F52068

    //Public ptr_V_v3Velocity = getAddress1(VehiclePtr, &H7D0)
    public long ptr_V_v3Velocity;                                //ptr_V_v3Velocity='[[[WorldPTR]+8]+D28]+7D'
    public long ptr_P_v3Velocity;                                //ptr_P_v3Velocity='[[WorldPTR]+8]+32'
    public long ptr_v3PlayerPos;                                 //ptr_v3PlayerPos='[[WorldPTR]+8]+9'
    public long ptr_InVehicle;                                   //ptr_InVehicle='[[WorldPTR]+8]+146C'
    public long ptr_VehicleMaxSpeed;                             //ptr_VehicleMaxSpeed='[[[WorldPTR]+8]+D28]+8AC'
    public long ptr_VehicleGear;                                 //ptr_VehicleGear='[UnkPTR]+FC4'
    public long ptr_VehicleRPM;                                  //ptr_VehicleRPM='[UnkPTR]+E40'
    public double SpeedUnit;
    public string SpeedUnitStr = "mph";
    public string forceUnit = "no";

    public void getAddresses()
    {
        //CheatEngine code is in green

        WorldPtr = GetWorldPtr();                                        //WorldPTR=getAddress('WorldPTR') WorldPTR=WorldPTR+readInteger(WorldPTR+3)+7
        UnkPtr = GetUnkPtr();                                            //UnkPTR=UnkPTR+readInteger(UnkPTR+3)+7

        ptr_V_v3Velocity = getAddress3(WorldPtr, 0x8, 0xD30, 0x7F0);     //ptr_V_v3Velocity='[[[WorldPTR]+8]+D28]+7D'
        ptr_P_v3Velocity = getAddress2(WorldPtr, 0x8, 0x850);            //ptr_P_v3Velocity='[[WorldPTR]+8]+32'
        ptr_v3PlayerPos = getAddress2(WorldPtr, 0x8, 0x90);              //ptr_v3PlayerPos='[[WorldPTR]+8]+9'
        ptr_InVehicle = getAddress2(WorldPtr, 0x8, 0x148C);              //ptr_InVehicle='[[WorldPTR]+8]+146C'
        ptr_VehicleMaxSpeed = getAddress3(WorldPtr, 0x8, 0xD30, 0x8CC);  //ptr_VehicleMaxSpeed='[[[WorldPTR]+8]+D28]+8AC'
        ptr_VehicleGear = getAddress1(UnkPtr, 0xFD4);                    //ptr_VehicleGear='[UnkPTR]+FC4'
        ptr_VehicleRPM = getAddress1(UnkPtr, 0xE50);                     //ptr_VehicleRPM='[UnkPTR]+E40'

        if (Process.GetProcessesByName("GTA5").Length > 0)
        {
            process = Process.GetProcessesByName(processName)[0];
        }
        else
        {
            MessageBox.Show("GTA not detected or not running");
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    (window as Window1).Close();
                }
            }
            MessageBox.Show("GTA not detected or not running", "GTA+Info");
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Close();
                }
            }
        }

        if (SpeedUnitStr == "mph")
        {
            SpeedUnit = 2.23694;
        }
        else if (SpeedUnitStr == "kph")
        {
            SpeedUnit = 3.6;
        }

    }
    #endregion

    #region Get Addresses
    public long getAddress1(long BaseAddress, long finalOffset)
    {
        long basePtr1 = ReadInt64(BaseAddress);
        long finalAddress = basePtr1 + finalOffset;
        return finalAddress;
    }
    public long getAddress2(long BaseAddress, long offset1, long finalOffset)
    {
        long basePtr1 = ReadInt64(BaseAddress);
        long basePtr2 = ReadInt64(basePtr1 + offset1);
        long finalAddress = basePtr2 + finalOffset;
        return finalAddress;
    }
    public long getAddress3(long BaseAddress, long offset1, long offset2, long finalOffset)
    {
        long basePtr1 = ReadInt64(BaseAddress);
        long basePtr2 = ReadInt64(basePtr1 + offset1);
        long basePtr3 = ReadInt64(basePtr2 + offset2);
        long finalAddress = basePtr3 + finalOffset;
        return finalAddress;
    }
    public long getAddress4(long BaseAddress, long offset1, long offset2, long offset3, long finalOffset)
    {
        long basePtr1 = ReadInt64(BaseAddress);
        long basePtr2 = ReadInt64(basePtr1 + offset1);
        long basePtr3 = ReadInt64(basePtr2 + offset2);
        long basePtr4 = ReadInt64(basePtr3 + offset3);
        long finalAddress = basePtr4 + finalOffset;
        return finalAddress;
    }
    #endregion

    public long GetWorldPtr()
    {
        if (Process.GetProcessesByName(processName).Length > 0)
        {
            Scanner Obj = new Scanner(process, process.Handle, "48 8B 05 ?? ?? ?? ?? 45 ?? ?? ?? ?? 48 8B 48 08 48 85 C9 74 07");

            Obj.setModule(process.MainModule);
            long Address = (long)Obj.FindPattern();

            long WorldPtr = Address + ReadInteger(Address + 0x3) + 0x7;

            return WorldPtr;
        }
        else
            return (long)12345;
    }

    public long GetUnkPtr()
    {
        if (Process.GetProcessesByName(processName).Length > 0)
        {
            Scanner Obj = new Scanner(process, process.Handle, "48 39 3D ?? ?? ?? ?? 75 2D");

            Obj.setModule(process.MainModule);
            long Address = (long)Obj.FindPattern();

            long UnkPtr = Address + ReadInteger(Address + 0x3) + 0x7;

            return UnkPtr;
        }
        else
            return (long)12345;
    }

    public bool IsInVehicle()
    {
        if (ReadInteger(ptr_InVehicle).Equals(2))
        {
            return true;
        }
        else
            return false;
    }

    public double GetSpeed()
    {
        if (IsInVehicle())
        {
            float v3_x = ReadFloat(ptr_V_v3Velocity + 0);
            float v3_y = ReadFloat(ptr_V_v3Velocity + 8);
            float v3_z = ReadFloat(ptr_V_v3Velocity + 4);
            double VehicleSpeed1 = Math.Sqrt((double)(Math.Pow(v3_x, 2) + Math.Pow(v3_y, 2) + Math.Pow(v3_z, 2)));

            float v3_x2 = ReadFloat(ptr_V_v3Velocity + 0);
            float v3_y2 = ReadFloat(ptr_V_v3Velocity + 8);
            float v3_z2 = ReadFloat(ptr_V_v3Velocity + 4);
            double VehicleSpeed2 = Math.Sqrt((double)(Math.Pow(v3_x2, 2) + Math.Pow(v3_y2, 2) + Math.Pow(v3_z2, 2)));

            double VehicleSpeed = VehicleSpeed1 + (VehicleSpeed2 - VehicleSpeed1) * 0.5;
            return VehicleSpeed * SpeedUnit;
        }
        else
        {
            double PlayerSpeed = ReadFloat(ptr_P_v3Velocity);
            return PlayerSpeed * SpeedUnit;
        }
    }

    public double GetRPM()
    {
        double rpm = ReadFloat(ptr_VehicleRPM);
        return rpm;
    }

    public int GetGear()
    {
        int gear = ReadInteger(ptr_VehicleGear);
        return gear;
    }

    public string GetPlayerCoords()
    {
        double v3_x = Math.Round(ReadFloat(ptr_v3PlayerPos + 0), 0);
        double v3_y = Math.Round(ReadFloat(ptr_v3PlayerPos + 4), 0);
        double v3_z = Math.Round(ReadFloat(ptr_v3PlayerPos + 8), 0);

        string str = String.Join(" ", "x=" + v3_x, " y=" + v3_y, " z=" + v3_z);
        return str;
    }

    public double GetMaxSpeed()
    {
        double maxSpeed = ReadFloat(ptr_VehicleMaxSpeed);
        return maxSpeed * SpeedUnit;
    }

    public Color ConvertColor(System.Drawing.Color inputColor)
    {
        Color outputColor = Color.FromArgb(inputColor.A, inputColor.R, inputColor.G, inputColor.B);
        return outputColor;
    }

    public System.Drawing.Color ConvertColor2(Color inputColor)
    {
        System.Drawing.Color outputColor = System.Drawing.Color.FromArgb(inputColor.A, inputColor.R, inputColor.G, inputColor.B);
        return outputColor;
    }

    public BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
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

    public void Save(BitmapImage image, string filePath)
    {
        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        {
            encoder.Save(fileStream);
        }
    }

    public void LoadCustomGaugeWindow1()
    {
        System.Windows.Forms.OpenFileDialog OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        OpenFileDialog1.Title = "Please select a zip file";
        OpenFileDialog1.Filter = "zip file|*.zip";
        OpenFileDialog1.FileName = "";
        if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            callOnce = callOnce + 1;
            if (callOnce < 2)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Window1))
                    {
                        if (File.Exists(@"C:\Temp\GTA+Info\background.png") && File.Exists(@"C:\Temp\GTA+Info\needle.png") && File.Exists(@"C:\Temp\GTA+Info\config.xml"))
                        {
                            (window as Window1).CustomSpeedo.CleanGauge();
                            File.Delete(@"C:\Temp\GTA+Info\background.png");
                            File.Delete(@"C:\Temp\GTA+Info\needle.png");
                            File.Delete(@"C:\Temp\GTA+Info\config.xml");
                        }

                        ZipFile.ExtractToDirectory(OpenFileDialog1.FileName, @"C:\Temp\GTA+Info");

                        System.Drawing.Bitmap needle1 = new System.Drawing.Bitmap(@"C:\Temp\GTA+Info\needle.png");
                        System.Drawing.Bitmap background1 = new System.Drawing.Bitmap(@"C:\Temp\GTA+Info\background.png");

                        BitmapImage needle = BitmapToImageSource(needle1);
                        BitmapImage background = BitmapToImageSource(background1);

                        (window as Window1).CustomSpeedo.NeedleImage = needle;
                        (window as Window1).CustomSpeedo.GaugeImage = background;

                        XDocument xDoc = XDocument.Load(@"C:\Temp\GTA+Info\config.xml");

                        XElement x = xDoc.Root;

                        double StartAngle; if (!x.Element("StartAngle").IsEmpty)
                        { Double.TryParse(x.Element("StartAngle").Value, out StartAngle); (window as Window1).CustomSpeedo.StartAngle = StartAngle; }
                        else MessageBox.Show("config.xml is invalid (couldn't find StartAngle)");

                        double EndAngle; if (!x.Element("EndAngle").IsEmpty)
                        { Double.TryParse(x.Element("EndAngle").Value, out EndAngle); (window as Window1).CustomSpeedo.EndAngle = EndAngle; }
                        else MessageBox.Show("config.xml is invalid (couldn't find EndAngle)");

                        double MaxValue; if (!x.Element("MaxValue").IsEmpty)
                        { Double.TryParse(x.Element("MaxValue").Value, out MaxValue); ; (window as Window1).CustomSpeedo.MaxValue = MaxValue; }
                        else MessageBox.Show("config.xml is invalid (couldn't find MaxValue)");

                        bool GearEnabled; if (!x.Element("GearsEnabled").IsEmpty)
                        { bool.TryParse(x.Element("GearsEnabled").Value, out GearEnabled); (window as Window1).CustomSpeedo.GearEnabled = GearEnabled; }
                        else MessageBox.Show("config.xml is invalid (couldn't find GearsEnabled)");


                        forceUnit = x.Element("ForceUnit").Value;
                        (window as Window1).CustomSpeedo.Type = x.Element("Type").Value;

                        try
                        {
                            (window as Window1).CustomSpeedo.DigiValLoc = x.Element("DigitalValueLocation").Value;
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("config.xml is invalid (couldn't find DigitalValueLocation)");
                        }

                        try
                        {
                            (window as Window1).CustomSpeedo.GearLoc = x.Element("GearLocation").Value;
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("config.xml is invalid (couldn't find GearLocation)");
                        }

                        try
                        {
                            System.Drawing.Color SpeedColor = System.Drawing.Color.FromName(x.Element("SpeedColor").Value);
                            (window as Window1).CustomSpeedo.SpeedColor = SpeedColor;
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("config.xml is invalid (couldn't find SpeedColor)");
                        }

                        try
                        {
                            System.Drawing.Color GearColor = System.Drawing.Color.FromName(x.Element("GearColor").Value);
                            (window as Window1).CustomSpeedo.GearColor = GearColor;
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("config.xml is invalid (couldn't find GearColor)");
                        }

                        /*Form1.CustomSpeedo1.StartAngle = .Element("StartAngle").Value
                        Form1.CustomSpeedo1.EndAngle = .Element("EndAngle").Value
                        Form1.CustomSpeedo1.MaxValue = .Element("MaxValue").Value
                        'forceUnit = .Element("ForceUnit").Value
                        Form1.CustomSpeedo1.Type = .Element("Type").Value
                        Form1.CustomSpeedo1.EnableGears = .Element("GearsEnabled").Value
                        Form1.CustomSpeedo1.DigiValLocationWidth = .Element("DigitalValueLocationWidth").Value
                        Form1.CustomSpeedo1.DigiValLocationHeight = .Element("DigitalValueLocationHeight").Value
                        Form1.CustomSpeedo1.GearLocationWidth = .Element("GearLocationWidth").Value
                        Form1.CustomSpeedo1.GearLocationHeight = .Element("GearLocationHeight").Value

                        Form1.Speedo1.StartAngle = .Element("StartAngle").Value
                        Form1.Speedo1.EndAngle = .Element("EndAngle").Value
                        Form1.Speedo1.MaxValue = .Element("MaxValue").Value*/

                        //needle.EndInit();
                        //background.EndInit();

                        needle.Freeze();
                        background.Freeze();

                        needle1.Dispose();
                        background1.Dispose();

                        GC.Collect();

                        File.Delete(@"C:\Temp\GTA+Info\background.png");
                        File.Delete(@"C:\Temp\GTA+Info\needle.png");
                        File.Delete(@"C:\Temp\GTA+Info\config.xml");

                    }
                    if (window.GetType() == typeof(MainWindow))
                    {
                        if (SpeedUnitStr == "mph" | forceUnit=="mph")
                        {
                            (window as MainWindow).comboBox1.SelectedIndex = 0;
                        }
                        else if (SpeedUnitStr == "kph" | forceUnit == "kph")
                        {
                            (window as MainWindow).comboBox1.SelectedIndex = 1;
                        }
                    }
                    }
            } callOnce = 0;
        }
    }

    public void LoadCustomGaugeWindow2()
    {
        System.Windows.Forms.OpenFileDialog OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        OpenFileDialog1.Title = "Please select a zip file";
        OpenFileDialog1.Filter = "zip file|*.zip";
        OpenFileDialog1.FileName = "";
        if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Window2))
                    {
                        {
                            if (File.Exists(@"C:\Temp\GTA+Info\background.png") && File.Exists(@"C:\Temp\GTA+Info\needle.png") && File.Exists(@"C:\Temp\GTA+Info\config.xml"))
                            {
                                File.Delete(@"C:\Temp\GTA+Info\background.png");
                                File.Delete(@"C:\Temp\GTA+Info\needle.png");
                                File.Delete(@"C:\Temp\GTA+Info\config.xml");
                            }

                            ZipFile.ExtractToDirectory(OpenFileDialog1.FileName, @"C:\Temp\GTA+Info");

                            System.Drawing.Bitmap needle1 = new System.Drawing.Bitmap(@"C:\Temp\GTA+Info\needle.png");
                            System.Drawing.Bitmap background1 = new System.Drawing.Bitmap(@"C:\Temp\GTA+Info\background.png");

                            BitmapImage needle = BitmapToImageSource(needle1);
                            BitmapImage background = BitmapToImageSource(background1);

                            (window as Window2).CustomSpeedo1.NeedleImage = needle;
                            (window as Window2).CustomSpeedo1.GaugeImage = background;

                            XDocument xDoc = XDocument.Load(@"C:\Temp\GTA+Info\config.xml");

                            XElement x = xDoc.Root;

                            double StartAngle; if (!x.Element("StartAngle").IsEmpty)
                            { Double.TryParse(x.Element("StartAngle").Value, out StartAngle); (window as Window2).CustomSpeedo1.StartAngle = StartAngle; }
                            else MessageBox.Show("config.xml is invalid (couldn't find StartAngle)");

                            double EndAngle; if (!x.Element("EndAngle").IsEmpty)
                            { Double.TryParse(x.Element("EndAngle").Value, out EndAngle); (window as Window2).CustomSpeedo1.EndAngle = EndAngle; }
                            else MessageBox.Show("config.xml is invalid (couldn't find EndAngle)");

                            double MaxValue; if (!x.Element("MaxValue").IsEmpty)
                            { Double.TryParse(x.Element("MaxValue").Value, out MaxValue); ; (window as Window2).CustomSpeedo1.MaxValue = MaxValue; }
                            else MessageBox.Show("config.xml is invalid (couldn't find MaxValue)");

                            bool GearEnabled; if (!x.Element("GearsEnabled").IsEmpty)
                            { bool.TryParse(x.Element("GearsEnabled").Value, out GearEnabled); (window as Window2).CustomSpeedo1.GearEnabled = GearEnabled; }
                            else MessageBox.Show("config.xml is invalid (couldn't find GearsEnabled)");


                            forceUnit = x.Element("ForceUnit").Value;
                            (window as Window2).CustomSpeedo1.Type = x.Element("Type").Value;

                            try
                            {
                                (window as Window2).CustomSpeedo1.DigiValLoc = x.Element("DigitalValueLocation").Value;
                            }
                            catch (NullReferenceException e)
                            {
                                MessageBox.Show("config.xml is invalid (couldn't find DigitalValueLocation)");
                            }

                            try
                            {
                                (window as Window2).CustomSpeedo1.GearLoc = x.Element("GearLocation").Value;
                            }
                            catch (NullReferenceException e)
                            {
                                MessageBox.Show("config.xml is invalid (couldn't find GearLocation)");
                            }

                            try
                            {
                                System.Drawing.Color SpeedColor = System.Drawing.Color.FromName(x.Element("SpeedColor").Value);
                                (window as Window2).CustomSpeedo1.SpeedColor = SpeedColor;
                            }
                            catch (NullReferenceException e)
                            {
                                MessageBox.Show("config.xml is invalid (couldn't find SpeedColor)");
                            }

                            try
                            {
                                System.Drawing.Color GearColor = System.Drawing.Color.FromName(x.Element("GearColor").Value);
                                (window as Window2).CustomSpeedo1.GearColor = GearColor;
                            }
                            catch (NullReferenceException e)
                            {
                                MessageBox.Show("config.xml is invalid (couldn't find GearColor)");
                            }

                        /*Form1.CustomSpeedo1.StartAngle = .Element("StartAngle").Value
                        Form1.CustomSpeedo1.EndAngle = .Element("EndAngle").Value
                        Form1.CustomSpeedo1.MaxValue = .Element("MaxValue").Value
                        'forceUnit = .Element("ForceUnit").Value
                        Form1.CustomSpeedo1.Type = .Element("Type").Value
                        Form1.CustomSpeedo1.EnableGears = .Element("GearsEnabled").Value
                        Form1.CustomSpeedo1.DigiValLocationWidth = .Element("DigitalValueLocationWidth").Value
                        Form1.CustomSpeedo1.DigiValLocationHeight = .Element("DigitalValueLocationHeight").Value
                        Form1.CustomSpeedo1.GearLocationWidth = .Element("GearLocationWidth").Value
                        Form1.CustomSpeedo1.GearLocationHeight = .Element("GearLocationHeight").Value

                        Form1.Speedo1.StartAngle = .Element("StartAngle").Value
                        Form1.Speedo1.EndAngle = .Element("EndAngle").Value
                        Form1.Speedo1.MaxValue = .Element("MaxValue").Value*/

                        //needle.EndInit();
                        //background.EndInit();

                        needle.Freeze();
                        background.Freeze();

                        needle1.Dispose();
                        background1.Dispose();
                        
                        GC.Collect();

                        File.Delete(@"C:\Temp\GTA+Info\background.png");
                        File.Delete(@"C:\Temp\GTA+Info\needle.png");
                        File.Delete(@"C:\Temp\GTA+Info\config.xml");
                        }
                    }
                }
            }
    }
}
