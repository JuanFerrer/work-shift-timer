using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

//Circle colours: https://colorlib.com/etc/metro-colors/

namespace WorkShiftTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Regex regex = new Regex(@"\D");
        static string endTime = "";
        static string breakTime = "";
        static DateTime end = new DateTime();
        public MainWindow()
        {
            InitializeComponent();

            endTime = regex.Replace(endTimeTextBox.Text, "");
            breakTime = regex.Replace(breakTimeTextBox.Text, "");

            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 09, 00, 0);
            end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(endTime.Substring(0, 2)), int.Parse(endTime.Substring(2, 2)), 0);
            int totalSeconds = (int)(end - start).TotalSeconds;
            int hour = (int)(end - start).Minutes;
            int minute = (int)(end - start).Seconds;
            int second = (int)(end - start).Milliseconds;

            // Calculate values
            var left = end - DateTime.Now;

            // Set label
            timerLabel.Content = left.ToString(@"hh\:mm");

            // Set circle values
            totalCircularProgress.Value = 100 - (left.TotalSeconds / totalSeconds * 100);
            hourCircularProgress.Value = DateTime.Now.Minute / 60.0 * 100;
            minuteCircularProgress.Value = DateTime.Now.Second / 60.0 * 100;

            var timer = new DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                // Calculate values
                left = end - DateTime.Now;

                // Set label
                timerLabel.Content = left.ToString(@"hh\:mm");

                if (DateTime.Now.ToString("HHmm") == breakTime)
                {
                    timerLabel.Opacity = timerLabel.Opacity == 0 ? 1 : 0;
                }

                // Set circle values
                totalCircularProgress.Value = 100 - (left.TotalSeconds / totalSeconds * 100);
                hourCircularProgress.Value = DateTime.Now.Minute / 60.0 * 100;
                minuteCircularProgress.Value = DateTime.Now.Second / 60.0 * 100;

                if (DateTime.Now.Hour == 13 && DateTime.Now.Minute == 30)
                {
                    timerLabel.Content = left.ToString(@"hh\:mm");
                }
            };
            timer.Start();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textbox = sender as System.Windows.Controls.TextBox;
            var oldVal = "";
            if (textbox.Name == "endTimeTextBox")
            {
                oldVal = endTime;
                endTime = regex.Replace(endTimeTextBox.Text, "");
                if (endTime.Length < 4)
                {
                    endTime = oldVal;
                }
                else
                {
                    end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(endTime.Substring(0, 2)), int.Parse(endTime.Substring(2, 2)), 0);
                }
            }
            else if (textbox.Name == "breakTimeTextBox")
            {
                oldVal = breakTime;
                breakTime = regex.Replace(breakTimeTextBox.Text, "");
                if (breakTime.Length < 4)
                {
                    breakTime = oldVal;
                }
            }
        }
    }
}
