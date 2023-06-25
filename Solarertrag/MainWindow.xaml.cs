namespace Solarertrag
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Threading;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Core.SystemMetrics;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DateFormat = "dd.MM.yy HH:mm";
        private DispatcherTimer statusBarDate = null;

        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "SizeChanged", this.OnSizeChanged);

            try
            {
                this.InitTimer();
                this.InfoDeviceType = SystemMetricsInfo.DetectingDeviceType();
                this.statusbarUserDomainName.Content = UserInfo.TS().CurrentDomainUser;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private InfoDeviceType InfoDeviceType { get; set; }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            int countMonitors = SystemMetricsInfo.CountMonitors;

            string windowSize = $"{this.ActualWidth.ToInt()}x{this.ActualHeight.ToInt()}x{countMonitors}:{this.InfoDeviceType}";
            this.tbMonitorSize.Content = windowSize;
        }

        private void InitTimer()
        {
            this.statusBarDate = new DispatcherTimer();
            this.statusBarDate.Interval = new TimeSpan(0, 0, 1);
            this.statusBarDate.Start();
            this.statusBarDate.Tick += new EventHandler(
                delegate (object s, EventArgs a) {
                    this.dtStatusBarDate.Content = DateTime.Now.ToString(DateFormat);
                });
        }
    }
}
