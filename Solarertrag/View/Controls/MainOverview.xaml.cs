namespace Solarertrag.View.Controls
{
    using System.Windows.Controls;

    using Solarertrag.ViewModel;

    /// <summary>
    /// Interaktionslogik für MainOverview.xaml
    /// </summary>
    public partial class MainOverview : UserControl
    {
        private readonly MainOverviewVM controlVM = null;

        public MainOverview()
        {
            this.InitializeComponent();

            if (this.controlVM == null)
            {
                this.controlVM = new MainOverviewVM();
            }

            this.DataContext = this.controlVM;
        }
    }
}
