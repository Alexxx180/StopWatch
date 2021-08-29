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
using System.Windows.Threading;

namespace StopWatch
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Watch.Tick += Time;
        }

        public byte[] time = { 0, 0, 0, 0, 0 };
        private byte[] intervals = { 100, 60, 60, 24, 255 };

        public DispatcherTimer Watch = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, 10),
        };
        private void Time(object sender, EventArgs e)
        {
            for (byte i = 0; i < time.Length; i++)
            {
                if (time[i] + 1 < intervals[i])
                {
                    time[i]++;
                    break;
                }
                time[i] = 0;
            }
            if (time[4] >= 255)
                Stop();
        }
        private void Play(object sender, RoutedEventArgs e)
        {
            Watch.Start();
            Button play = sender as Button;
            Hide(play);
            Show(play.Tag as Button);
        }
        private void Pause(object sender, RoutedEventArgs e)
        {
            Watch.Stop();
            Button pause = sender as Button;
            Hide(pause);
            Show(pause.Tag as Button);
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        private void Stop()
        {
            Watch.Stop();
            for (byte i = 0; i < time.Length; i++)
                time[i] = 0;
        }
        private void Hide(Button btn)
        {
            btn.IsEnabled = false;
            btn.Visibility = Visibility.Hidden;
        }
        private void Show(Button btn)
        {
            btn.IsEnabled = true;
            btn.Visibility = Visibility.Visible;
        }
    }
}
