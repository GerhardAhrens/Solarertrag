namespace Solarertrag.View.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Solarertrag.ViewModel;

    /// <summary>
    /// Interaktionslogik für MainDetail.xaml
    /// </summary>
    public partial class MainDetail : UserControl
    {
        public MainDetail()
        {
            InitializeComponent();

            //FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, KeyEventArgs>.AddHandler(this, "KeyDown", this.OnKeyDown);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.txtJahr.Focus();
            }), DispatcherPriority.Render);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            string currentControl = (e.OriginalSource as TextBox).Name;
            if (currentControl == "txtJahr" && (e.Key == Key.Tab || e.Key == Key.Enter))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txtMonat.Focus();
                }), DispatcherPriority.Render);
            }
            else if (currentControl == "txtMonat" && (e.Key == Key.Tab || e.Key == Key.Enter))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txtErtrag.Focus();
                }), DispatcherPriority.Render);
            }
            else if (currentControl == "txtErtrag" && (e.Key == Key.Tab || e.Key == Key.Enter))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txtDescription.Focus();
                }), DispatcherPriority.Render);
            }
            else if (currentControl == "txtDescription" && (e.Key == Key.Tab || e.Key == Key.Enter))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txtJahr.Focus();
                }), DispatcherPriority.Render);
            }
        }
    }
}
