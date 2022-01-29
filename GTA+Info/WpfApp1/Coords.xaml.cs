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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        System.Timers.Timer Timer1 = new System.Timers.Timer(5);
        GTAMoreInfo gta = new GTAMoreInfo();

        public Window3()
        {
            InitializeComponent();
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width) * 0.011;
            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height) * 0.83;
            Timer1.Enabled = true;
            Timer1.Elapsed += Timer1_Elapsed;
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                
            });
        }
    }
}
