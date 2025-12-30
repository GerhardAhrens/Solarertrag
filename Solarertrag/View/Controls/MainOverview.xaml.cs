namespace Solarertrag.View.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;
    using Solarertrag.ViewModel;

    /// <summary>
    /// Interaktionslogik für MainOverview.xaml
    /// </summary>
    public partial class MainOverview : UserControl
    {
        public MainOverview()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private int rowPosition;

        public int RowPosition
        {
            get { return this.rowPosition; }

            set 
            { 
                this.rowPosition = value; 
                if (this.rowPosition> 0)
                {
                    this.SetFocus(this.rowPosition);
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => {
                this.lvwMain.Focusable = true;
                this.lvwMain.Focus();
                this.lvwMain.SelectedIndex = 0;
                this.lvwMain.Focus();
            }));
        }

        private void SetFocus(int rowPosition)
        {
            Action action = () =>
            {
                if (this.lvwMain.HasItems == true)
                {
                    this.lvwMain.Focus();
                    this.lvwMain.SelectedIndex = rowPosition;
                    if (this.lvwMain.SelectedIndex > 0)
                    {
                        this.lvwMain.ScrollIntoView(this.lvwMain.Items[this.lvwMain.SelectedIndex]);
                        this.lvwMain.SelectedItem = this.lvwMain.Items[this.lvwMain.SelectedIndex];
                        this.lvwMain.Focus();
                    }
                }
            };

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, action);
        }

        private void ScrollBar_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
