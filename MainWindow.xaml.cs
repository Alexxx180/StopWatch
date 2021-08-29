using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;

namespace StopWatch
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static RoutedCommand PlayCmd = new RoutedCommand();
        public static RoutedCommand PauseCmd = new RoutedCommand();
        public static RoutedCommand StopCmd = new RoutedCommand();
        Stopwatch watch = new Stopwatch();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Watch.Tick += TimeTick;
            CommandBinding cb = new CommandBinding(PlayCmd, PlayExecuted, CanExecute);
            CommandBindings.Add(cb);
            CommandBinding cb1 = new CommandBinding(PauseCmd, PauseExecuted, CanExecute);
            CommandBindings.Add(cb1);
            CommandBinding cb2 = new CommandBinding(StopCmd, StopExecuted, CanExecute);
            CommandBindings.Add(cb2);

            PlayBtn.Command = PlayCmd;
            PauseBtn.Command = PauseCmd;
            StopBtn.Command = StopCmd;

            KeyGesture kg = new KeyGesture(Key.F5);
            InputBinding ib = new InputBinding(PlayCmd, kg);
            InputBindings.Add(ib);
            KeyGesture kg1 = new KeyGesture(Key.F6);
            InputBinding ib1 = new InputBinding(PauseCmd, kg1);
            InputBindings.Add(ib1);
            KeyGesture kg2 = new KeyGesture(Key.F8);
            InputBinding ib2 = new InputBinding(StopCmd, kg2);
            InputBindings.Add(ib2);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PlayExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Play(sender, e);
        }
        private void PauseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Pause(sender, e);
        }
        private void StopExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Stop(sender, e);
        }

        public byte[] Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        private byte[] time = new byte[] { 0, 0, 0, 0, 0 };

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion

        public DispatcherTimer Watch = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1)
        };
        private void TimeTick(object sender, EventArgs e)
        {
            TimeSpan span = watch.Elapsed;
            Time[0] = Convert.ToByte(span.Milliseconds / 100);
            Time[1] = Convert.ToByte(span.Seconds);
            Time[2] = Convert.ToByte(span.Minutes);
            Time[3] = Convert.ToByte(span.Hours);
            Time[4] = Convert.ToByte(span.Days);
            if (Time[4] >= 255)
                Stop();
            OnPropertyChanged(nameof(Time));
        }
        private void Play(object sender, RoutedEventArgs e)
        {
            watch.Start();
            Watch.Start();
            Change(PlayBtn, PauseBtn);
        }
        private void Pause(object sender, RoutedEventArgs e)
        {
            if (watch.IsRunning)
                watch.Stop();
            Watch.Stop();
            Change(PauseBtn, PlayBtn);
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            Stop();
            Change(PauseBtn, PlayBtn);
        }
        private void Stop()
        {
            watch.Restart();
            watch.Stop();
            Watch.Stop();
            for (byte i = 0; i < Time.Length; i++)
                Time[i] = 0;
            OnPropertyChanged(nameof(Time));
        }
        private void Change(Button hide, Button show)
        {
            hide.IsEnabled = false;
            hide.Visibility = Visibility.Hidden;
            show.IsEnabled = true;
            show.Visibility = Visibility.Visible;
        }
    }
}
