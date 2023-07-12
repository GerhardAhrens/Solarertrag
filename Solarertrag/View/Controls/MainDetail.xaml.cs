namespace Solarertrag.View.Controls
{
    using System;
    using System.Windows.Controls;

    using Solarertrag.ViewModel;

    /// <summary>
    /// Interaktionslogik für MainDetail.xaml
    /// </summary>
    public partial class MainDetail : UserControl
    {
        private readonly MainDetailVM controlVM = null;

        public MainDetail()
        {
            InitializeComponent();
            if (this.controlVM == null)
            {
                this.controlVM = new MainDetailVM(Guid.Empty);
            }

            this.DataContext = this.controlVM;
        }
    }
}
