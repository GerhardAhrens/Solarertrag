//-----------------------------------------------------------------------
// <copyright file="MainWindowVM.cs" company="Lifeprojects.de">
//     Class: MainWindowVM
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>28.06.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.Pattern;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.DataRepository;
    using Solarertrag.Model;
    using Solarertrag.View.Controls;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public partial class MainWindowVM : ViewModelBase<MainWindowVM>, IViewModel
    {
        private readonly Window mainWindow = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowVM"/> class.
        /// </summary>
        public MainWindowVM()
        {
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.ApplicationVersion = ApplicationProperties.VersionWithName;

            //TraceLogger.LogInformation($"AppVersion: {this.ApplicationVersion}");

            this.InitCommands();

            App.EventAgg.Subscribe<SwitchDialogEventArgs<IViewModel>>(this.HandleSwitchDialogRequest);
            App.EventAgg.Subscribe<CurrentIdEventArgs<IViewModel>>(this.CurrentIdRequest);
            App.EventAgg.Subscribe<SelectedDataEventArgs>(this.SelectedDataRequest);

            this.NewDatabaseHandler();

            this.StatuslineDescription = $"Datenbank: {Path.GetFileName(App.DatabasePath)}";

            Mouse.OverrideCursor = null;

            this.LoadContent(LoadContentArgs.MainOverview());
        }

        #region Get/Set Properties
        [PropertyBinding]
        public string ApplicationVersion
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string StatuslineDescription
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public UserControl CurrentControl
        {
            get { return this.Get<UserControl>(); }
            private set { this.Set(value); }
        }

        private Guid CurrentId { get; set; }


        private List<SolarertragMonat> CurrentData { get; set; }

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.Home, new RelayCommand(p1 => this.LoadContent(LoadContentArgs.MainHome()), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.MainOverview, new RelayCommand(p1 => this.LoadContent(LoadContentArgs.MainOverview()), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.WindowClose, new RelayCommand(p1 => this.WindowCloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.NewDetail, new RelayCommand(p1 => this.NewDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.ExcelExport, new RelayCommand(p1 => this.ExcelExportHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.Settings, new RelayCommand(p1 => this.SettingsHandler(), p2 => true));
        }

        private void WindowCloseHandler()
        {
            if (App.ExitQuestion == true)
            {
                DialogResultsEx dialogResult = AppMsgDialog.ApplicationExit();
                if (dialogResult == DialogResultsEx.Yes)
                {
                    Window currentWindow = Application.Current.Windows.LastActiveWindow();
                    if (currentWindow != null)
                    {
                        currentWindow.Close();
                    }
                }
            }
            else
            {
                Window currentWindow = Application.Current.Windows.LastActiveWindow();
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }
        }

        private void NewDatabaseHandler()
        {
            Result<bool> createResult = null;
            if (File.Exists(App.DatabasePath) == false)
            {
                using (DatabaseManager dm = new DatabaseManager(App.DatabasePath))
                {
                    createResult = dm.CreateNewDatabase();
                }
            }
        }

        private void LoadDatabaseHandler()
        {
            Result<bool> openResult = null;
            if (File.Exists(App.DatabasePath) == true)
            {
                using (DatabaseManager dm = new DatabaseManager(App.DatabasePath))
                {
                    openResult = dm.OpenDatabase();
                }
            }
        }

        private void NewDetailHandler()
        {
            this.LoadContent(LoadContentArgs.MainDetail(Guid.Empty, 0));
        }

        private void ExcelExportHandler()
        {
            this.LoadContent(LoadContentArgs.MainExcel());
        }

        private void SettingsHandler()
        {
            this.LoadContent(LoadContentArgs.MainSettings());
        }

        private void LoadContent(ControlContentArgs args)
        {
            if (args == null)
            {
                return;
            }

            try
            {
                this.CurrentControl = DialogNavigation.GetControl(args.TargetPage);
                if (this.CurrentControl != null)
                {
                    if (args.TargetPage == CommandButtons.MainOverview)
                    {
                        MainOverviewVM controlVM = new MainOverviewVM(args);
                        this.CurrentControl.DataContext = controlVM;
                        if (args.RowPosition == RowItemPosition.GoMove)
                        {
                            ((MainOverview)this.CurrentControl).RowPosition = args.RowPosition.GoTo;
                        }
                        else
                        {
                            ((MainOverview)this.CurrentControl).RowPosition = controlVM.RowPosition;
                        }
                    }
                    else if (args.TargetPage == CommandButtons.MainDetail)
                    {
                        MainDetailVM controlVM = new MainDetailVM(args.EntityId, args.RowPosition.GoTo);
                        this.CurrentControl.DataContext = controlVM;
                    }
                    else if (args.TargetPage == CommandButtons.Settings)
                    {
                        SettingsVM controlVM = new SettingsVM();
                        this.CurrentControl.DataContext = controlVM;
                    }
                    else if (args.TargetPage == CommandButtons.ExcelExport)
                    {
                        if (this.CurrentData == null || this.CurrentData.Count == 0)
                        {
                            AppMsgDialog.NoDataFound();
                            ControlContentArgs overviewArgs = new ControlContentArgs();
                            overviewArgs.TargetPage = CommandButtons.MainOverview;
                            overviewArgs.RowPosition = RowItemPosition.GoFirst;
                            overviewArgs.EntityId = Guid.Empty;
                            this.LoadContent(overviewArgs);
                            return;
                        }

                        ExcelExportVM controlVM = new ExcelExportVM(this.CurrentData);
                        this.CurrentControl.DataContext = controlVM;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }

        }

        private void CurrentIdRequest(CurrentIdEventArgs<IViewModel> obj)
        {
            this.CurrentId = obj.EntityId;
        }

        private void HandleSwitchDialogRequest(SwitchDialogEventArgs<IViewModel> obj)
        {
            try
            {
                if (obj.TargetPage == CommandButtons.MainOverview)
                {
                    this.LoadContent(LoadContentArgs.MainOverview(obj.RowPosition));
                }
                else if (obj.TargetPage == CommandButtons.MainDetail)
                {
                    this.LoadContent(LoadContentArgs.MainDetail(this.CurrentId));
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }

        private void SelectedDataRequest(SelectedDataEventArgs args)
        {
            this.CurrentData = args.Data;
        }
    }
}
