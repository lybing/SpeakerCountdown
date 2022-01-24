using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace SpeakerCountdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Timer CountDownTimer;
        private Brush tmBackground;
        private Microsoft.Office.Interop.PowerPoint.Application oPPT;

        public MainWindow()
        {
            InitializeComponent();

            winContent.WindowStyle = WindowStyle.None;
            winContent.ResizeMode = ResizeMode.NoResize;
            winContent.Topmost = true;

            tmBackground = this.winContent.Background;

            InitTimer(true);

            oPPT = new Microsoft.Office.Interop.PowerPoint.Application();
            oPPT.SlideShowBegin += OPPT_SlideShowBegin;
            oPPT.SlideShowEnd += OPPT_SlideShowEnd;
        }

        private void OPPT_SlideShowEnd(Microsoft.Office.Interop.PowerPoint.Presentation Pres)
        {
            InitTimer(true);
        }

        private void OPPT_SlideShowBegin(Microsoft.Office.Interop.PowerPoint.SlideShowWindow Wn)
        {
            InitTimer(true);

            CountDownTimer = new Timer(callback: ShowCountDown,
                                       false,
                                       0,
                                       1);
        }

        public void InitTimer(bool isInit)
        {
            ShowCountDown(isInit);

            if (CountDownTimer != null)
            {
                this.txt.Dispatcher.Invoke(new Action(() =>
                {
                    this.winContent.Background = tmBackground;
                }));

                CountDownTimer.Change(-1, 0); 
            }
        }

        internal static int Hour = 0;
        internal static int Minutes = 0;
        internal static int Second = 5;
        internal DateTime EndTime;

        public void ShowCountDown(object param)
        {
            var now = DateTime.Now;
            if (Convert.ToBoolean(param))
            {
                EndTime = now.AddHours(Hour).AddMinutes(Minutes).AddSeconds(Second);
            }
            var diff = EndTime.Subtract(now);

            var txtString = string.Format($"{GetTimeString(Math.Abs(diff.Hours))}:{GetTimeString(Math.Abs(diff.Minutes))}:{GetTimeString(Math.Abs(diff.Seconds))}");

            if (now > EndTime)
            {
                this.txt.Dispatcher.Invoke(new Action(() =>
                {
                    this.winContent.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }));

                txtString = "-" + txtString;
            }

            this.txt.Dispatcher.Invoke(new Action(() =>
            {
                this.txt.Text = txtString;
            }));
        }

        private string GetTimeString(int time)
        {
            if (time < 10)
            {
                return "0" + time.ToString();
            }
            return time.ToString();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            CountDownSetting cs = new CountDownSetting();
            cs.txtTimer.Hour = Hour;
            cs.txtTimer.Minute = Minutes;
            cs.txtTimer.Second = Second;

            cs.Closing += Cs_Closing;
            cs.Show();
            cs.Topmost = true;
        }

        private void Cs_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var cds = sender as SpeakerCountdown.CountDownSetting;
            if (cds != null && cds.IsSave)
            {
                InitTimer(true);
            }
        }

        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void winContent_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
