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
    using System.Linq;
    using System.Windows;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.DataRepository;
    using Solarertrag.Model;

    [ViewModel]
    public class LineChartVM : ViewModelBase<LineChartVM>, IViewModel
    {
        private readonly Window mainWindow = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="LineChartVM"/> class.
        /// </summary>
        public LineChartVM(ControlContentArgs args)
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
        public IEnumerable<ZaehlerstandMonat> ZaehlerstandAllSource
        {
            get => base.Get<IEnumerable<ZaehlerstandMonat>>();
            set => base.Set(value);
        }

        [PropertyBinding]
        public IEnumerable<SolarertragMonat> SolarertragAllSource
        {
            get => base.Get<IEnumerable<SolarertragMonat>>();
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
                    this.ZaehlerstandAllSource = repository.ListZaehlerstandAll();
                    this.SolarertragAllSource = repository.List().ToList();
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
                        FromPage = CommandButtons.LineChart,
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
