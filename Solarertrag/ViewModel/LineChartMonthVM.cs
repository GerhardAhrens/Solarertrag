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

                    this.ChartLinesSource = new ObservableCollection<ChartLine>();
                    this.YearSource = this.VerbrauchGesamt.GroupBy(g => g.Year).OrderByDescending(o => o.Key).ToDictionary(k => k.Key, g => g.Key.ToString());
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

            this.ChartLinesSource.Clear();
            ChartLine chartLineV = new ChartLine();
            chartLineV.Title = "Verbrauch (in KW/h)";
            chartLineV.Stroke = System.Windows.Media.Brushes.Red;

            foreach (var item in summeVerbrauchImJahr)
            {
                chartLineV.Values.Add(new ChartPoint { Category = item.Monat.ToString("00"), Value = (item.VerbrauchMax - item.VerbrauchMin).ToInt() });
            }

            if (chartLineV.Values.Any(a => a.Category == "01") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "01", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "02") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "02", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "03") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "03", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "04") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "04", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "05") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "05", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "06") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "06", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "07") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "07", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "08") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "08", Value = 0 });
            }
            if (chartLineV.Values.Any(a => a.Category == "09") == false)
            {
                chartLineV.Values.Add(new ChartPoint { Category = "09", Value = 0 });
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

            ChartLine aa = (ChartLine)chartLineV.Values.OrderBy(m =>m.Category);
            this.ChartLinesSource.Add(chartLineV);

            ChartLine chartLineE = new ChartLine();
            chartLineE.Title = "Solar Ertrag (KW/h)";
            chartLineE.Stroke = System.Windows.Media.Brushes.Green;
            foreach (var item in summeErtragImJahr)
            {
                chartLineE.Values.Add(new ChartPoint { Category = item.Monat.ToString("00"), Value = item.Ertrag.ToInt() });
            }

            if (chartLineE.Values.Any(a => a.Category == "01") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "01", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "02") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "02", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "03") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "03", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "04") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "04", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "05") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "05", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "06") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "06", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "07") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "07", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "08") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "08", Value = 0 });
            }
            if (chartLineE.Values.Any(a => a.Category == "09") == false)
            {
                chartLineE.Values.Add(new ChartPoint { Category = "09", Value = 0 });
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
