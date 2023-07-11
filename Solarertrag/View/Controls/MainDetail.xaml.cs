namespace Solarertrag.View.Controls
{
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
                this.controlVM = new MainDetailVM();
            }

            this.DataContext = this.controlVM;
        }
    }
}
