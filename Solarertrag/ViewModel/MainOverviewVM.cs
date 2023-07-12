//-----------------------------------------------------------------------
// <copyright file="MainOverviewVM.cs" company="Lifeprojects.de">
//     Class: MainOverviewVM
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

    using Solarertrag.Core;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public class MainOverviewVM : ViewModelBase<MainOverviewVM>, IViewModel
    {
        private readonly Window mainWindow = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainOverviewVM"/> class.
        /// </summary>
        public MainOverviewVM()
        {
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.InitCommands();
        }

        #region Get/Set Properties
        [PropertyBinding]
        public string FilterText
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.Edit, new RelayCommand(p1 => this.EditHandler(), p2 => true));
        }

        private void EditHandler()
        {
            try
            {
                App.EventAgg.Publish<SwitchDialogEventArgs<IViewModel>>(
                    new SwitchDialogEventArgs<IViewModel>
                    {
                        Sender = this,
                        DataType = this as IViewModel,
                        FromPage = MenuButtons.MainOverview,
                        TargetPage = MenuButtons.MainDetail
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }
    }
}
