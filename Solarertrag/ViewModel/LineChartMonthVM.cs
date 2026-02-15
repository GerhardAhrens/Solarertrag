//-----------------------------------------------------------------------
// <copyright file="LineChartVM.cs" company="Lifeprojects.de">
//     Class: LineChartVM
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.01.2026</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Chart;
    using Solarertrag.Core;
    using Solarertrag.DataRepository;
    using Solarertrag.Model;

    [ViewModel]
    public class LineChartMonthVM : ViewModelBase<LineChartMonthVM>, IViewModel
    {
        private readonly Window mainWindow = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="LineChartMonthVM"/> class.
        /// </summary>
        public LineChartMonthVM(ControlContentArgs args)
        {
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.DialogDescription = ResourceObject.GetAs<string>("DialogDescriptionLineChart");

            this.InitCommands();
            this.LoadDataHandler();
        }

        #region Get/Set Properties
        [PropertyBinding]
        public string DialogDescription
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string ExcelExportPath
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string Message
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public Dictionary<int, string> YearSource
        {
            get => base.Get<Dictionary<int, string>>();
            set => base.Set(value);
        }

        [PropertyBinding]
        public int YearSelected
        {
            get => base.Get<int>();
            set => base.Set(value, this.SelectedYearKey);
        }

        [PropertyBinding]
        public ObservableCollection<ChartLine> ChartLinesSource
        {
            get => base.Get<ObservableCollection<ChartLine>>();
            set => base.Set(value);
        }

        [PropertyBinding]
        public string YAchseTitel
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }


        public IEnumerable<ZaehlerstandMonat> VerbrauchGesamt { get; set; }
        public IEnumerable<SolarertragMonat> ErtragGesamt { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.CloseDetail, new RelayCommand(p1 => this.CloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.ExcelExport, new RelayCommand(p1 => this.SaveChartHandler(), p2 => true));
        }

        private void LoadDataHandler()
        {
            try
            {
                using (SolarertragMonatRepository repository = new SolarertragMonatRepository(App.DatabasePath))
                {
                    this.VerbrauchGesamt = repository.ListZaehlerstandAll();
                    this.ErtragGesamt = repository.List().ToList();

                    this.YearSource = this.VerbrauchGesamt.GroupBy(g => g.Year).ToDictionary(k => k.Key, g => g.Key.ToString());
                    this.YearSelected = DateTime.Now.Year;
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void SelectedYearKey(int obj)
        {
            int currentYear = (int)obj;
            this.YAchseTitel = $"Monat {currentYear}†";

            var summeVerbrauchImJahr = this.VerbrauchGesamt.Where(w => w.Year == currentYear).GroupBy(g => g.Month).Select(g => new { Monat = g.Key, VerbrauchMin = g.Min(s => s.Verbrauch), VerbrauchMax = g.Max(s => s.Verbrauch) }).ToList().OrderBy(o => o.Monat);
            var summeErtragImJahr = this.ErtragGesamt.Where(w => w.Year == currentYear).GroupBy(g => g.Month).Select(g => new { Monat = g.Key, Ertrag = g.Sum(s => s.Ertrag) }).ToList().OrderBy(o => o.Monat);

            this.ChartLinesSource = null;
            this.ChartLinesSource = new ObservableCollection<ChartLine>();
            ChartLine chartLineV = new ChartLine();
            chartLineV.Title = "Verbrauch (in KW/h)";
            chartLineV.Stroke = System.Windows.Media.Brushes.Red;

            foreach (var item in summeVerbrauchImJahr)
            {
                chartLineV.Values.Add(new ChartPoint { Category = item.Monat.ToString(), Value = (item.VerbrauchMax - item.VerbrauchMin).ToInt() });
            }

            if (chartLineV.Values.Any(a => a.Category == "1") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "1", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "2") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "2", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "3") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "3", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "4") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "4", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "5") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "5", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "6") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "6", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "7") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "7", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "8") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "8", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "9") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "9", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "10") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "10", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "11") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "11", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "12") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "12", Value = 0 });
            }

            chartLineV.Values.OrderBy(m =>m.Category);
            this.ChartLinesSource.Add(chartLineV);

            ChartLine chartLineE = new ChartLine();
            chartLineE.Title = "Solar Ertrag (KW/h)";
            chartLineE.Stroke = System.Windows.Media.Brushes.Green;
            foreach (var item in summeErtragImJahr)
            {
                chartLineE.Values.Add(new ChartPoint { Category = item.Monat.ToString(), Value = item.Ertrag.ToInt() });
            }

            if (chartLineE.Values.Any(a => a.Category == "1") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "1", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "2") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "2", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "3") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "3", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "4") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "4", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "5") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "5", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "6") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "6", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "7") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "7", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "8") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "8", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "9") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "9", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "10") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "10", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "11") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "11", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "12") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "12", Value = 0 });
            }

            chartLineE.Values.OrderBy(m => m.Category);
            this.ChartLinesSource.Add(chartLineE);
        }


        #region Command Handler
        private void CloseHandler()
        {
            try
            {
                App.EventAgg.Publish<SwitchDialogEventArgs<IViewModel>>(
                    new SwitchDialogEventArgs<IViewModel>
                    {
                        Sender = this,
                        EntityId = Guid.Empty,
                        RowPosition = -1,
                        DataType = this as IViewModel,
                        FromPage = CommandButtons.LineChartYear,
                        TargetPage = CommandButtons.MainOverview
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void SaveChartHandler()
        {
        }
        #endregion Command Handler
    }
}
