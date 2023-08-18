namespace Solarertrag
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Core.SystemMetrics;

    using Solarertrag.ViewModel;
    using Solarertrag.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DateFormat = "dd.MM.yy HH:mm";
        private DispatcherTimer statusBarDate = null;
        private readonly MainWindowVM rootVM = null;

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

                using (UserPreferences userPrefs = new UserPreferences(this, ApplicationProperties.AssemplyPath))
                {
                    userPrefs.Load();
                }

                if (this.rootVM == null)
                {
                    this.rootVM = new MainWindowVM();
                }

                this.DataContext = this.rootVM;
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
            using (UserPreferences userPrefs = new UserPreferences(this, ApplicationProperties.AssemplyPath))
            {
                userPrefs.Save();
            }
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

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (window.WindowState == System.Windows.WindowState.Normal)
            {
                window.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                window.WindowState = System.Windows.WindowState.Normal;
            }

            window.Focus();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Focus();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = System.Windows.WindowState.Minimized;
        }

        private void OnContextMenuClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button button)
            {
                button.ContextMenu.IsEnabled = true;
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }
    }
}
