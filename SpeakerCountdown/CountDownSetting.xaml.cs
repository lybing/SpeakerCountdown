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

namespace SpeakerCountdown
{
    /// <summary>
    /// CountDownSetting.xaml 的交互逻辑
    /// </summary>
    public partial class CountDownSetting : Window
    {
        public CountDownSetting()
        {
            InitializeComponent();
        }

        internal bool IsSave { get; set; }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsSave = true;
            MainWindow.Hour = txtTimer.Hour;
            MainWindow.Minutes = txtTimer.Minute;
            MainWindow.Second = txtTimer.Second;

            this.Close();
        }
    }
}
