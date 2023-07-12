//-----------------------------------------------------------------------
// <copyright file="MainDetailVM.cs" company="Lifeprojects.de">
//     Class: MainDetailVM
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.07.2023 17:33:40</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.ViewModel
{
    using System;
    using System.Runtime.Versioning;
    using System.Windows;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.WPF;

    using PertNET.DataRepository;

    using Solarertrag.Core;
    using Solarertrag.Model;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public class MainDetailVM : ViewModelBase<MainDetailVM>, IViewModel
    {
        private readonly Window mainWindow = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDetailVM"/> class.
        /// </summary>
        public MainDetailVM(Guid entityId)
        {
            this.CurrentId = entityId;
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.InitCommands();

            if (this.CurrentId == Guid.Empty)
            {
                this.DialogDescription = "Neuer Eintrag erfassen";
            }
            else
            {
                this.DialogDescription = "Gewählter Eintrag bearbeiten";
            }
        }

        #region Get/Set Properties
        [PropertyBinding]
        public string DialogDescription
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public SolarertragMonat CurrentSelectedItem
        {
            get { return this.Get<SolarertragMonat>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public int Year
        {
            get { return this.Get<int>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public int Month
        {
            get { return this.Get<int>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public double Ertrag
        {
            get { return this.Get<double>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string Description
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        private Guid CurrentId { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.CloseDetail, new RelayCommand(p1 => this.CloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.SaveDetail, new RelayCommand(p1 => this.SaveDetailHandler(), p2 => true));
        }

        private void LoadDataHandler()
        {
            try
            {
                if (this.CurrentId != Guid.Empty)
                {
                    using (SolarertragMonatRepository repository = new SolarertragMonatRepository(App.DatabasePath))
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }

        private void ShowData()
        {

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
                        DataType = this as IViewModel,
                        FromPage = MenuButtons.MainDetail,
                        TargetPage = MenuButtons.MainOverview
                    }); ;
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }

        private void SaveDetailHandler()
        {

        }
        #endregion Command Handler
    }
}
