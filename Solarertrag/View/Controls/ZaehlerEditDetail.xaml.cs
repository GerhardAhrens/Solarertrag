namespace Solarertrag.View.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Solarertrag.ViewModel;

    /// <summary>
    /// Interaktionslogik für ZaehlerEditDetail.xaml
    /// </summary>
    public partial class ZaehlerEditDetail : UserControl
    {
        public ZaehlerEditDetail()
        {
            InitializeComponent();
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
                    this.txtVerbrauch.Focus();
                }), DispatcherPriority.Render);
            }
            else if (currentControl == "txtVerbrauch" && (e.Key == Key.Tab || e.Key == Key.Enter))
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
