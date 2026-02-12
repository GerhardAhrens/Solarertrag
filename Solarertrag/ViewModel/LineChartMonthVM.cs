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

        private void SelectedYearKey(int obj)
        {
            int currentYear = (int)obj;
        }

        [PropertyBinding]
        public ObservableCollection<ChartLine> ChartLinesSource
        {
            get => base.Get<ObservableCollection<ChartLine>>();
            set => base.Set(value);
        }

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
                    IEnumerable <ZaehlerstandMonat> verbrauchGesamt = repository.ListZaehlerstandAll();
                    this.YearSource = verbrauchGesamt.GroupBy(g => g.Year).ToDictionary(k => k.Key, g => g.Key.ToString());
                    this.YearSelected = DateTime.Now.Year;

                    /*
                    var summeVerbrauchProJahr = verbrauchGesamt.GroupBy(g => g.Year).Select(g => new { Year = g.Key, VerbrauchMin = g.Min(s => s.Verbrauch), VerbrauchMax = g.Max(s => s.Verbrauch) }).ToList().OrderBy(o => o.Year);

                    IEnumerable < SolarertragMonat > ertragGesamt = repository.List().ToList();
                    var summeErtragProJahr = ertragGesamt.GroupBy(g => g.Year).Select(g => new { Year = g.Key, Ertrag = g.Sum(s => s.Ertrag) }).ToList().OrderBy(o => o.Year);

                    this.ChartLinesSource = new ObservableCollection<ChartLine>();
                    ChartLine chartLineV = new ChartLine();
                    chartLineV.Title = "Verbrauch";
                    chartLineV.Stroke = System.Windows.Media.Brushes.Red;

                    foreach (var item in summeVerbrauchProJahr)
                    {
                        if (item.VerbrauchMax == item.VerbrauchMin)
                        {
                            chartLineV.Values.Add(new ChartPoint { Category = item.Year.ToString(), Value = 2_600 });
                        }
                        else
                        {
                            chartLineV.Values.Add(new ChartPoint { Category = item.Year.ToString(), Value = (item.VerbrauchMax - item.VerbrauchMin).ToInt() });
                        }
                    }

                    this.ChartLinesSource.Add(chartLineV);

                    ChartLine chartLineE = new ChartLine();
                    chartLineE.Title = "Solar Ertrag";
                    chartLineE.Stroke = System.Windows.Media.Brushes.Green;
                    foreach (var item in summeErtragProJahr)
                    {
                        chartLineE.Values.Add(new ChartPoint { Category = item.Year.ToString(), Value = item.Ertrag.ToInt() });
                    }

                    this.ChartLinesSource.Add(chartLineE);
                    */
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
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
