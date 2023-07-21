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
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Console.ApplicationSettings;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.Pattern;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.DataRepository;
    using Solarertrag.Factory;
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
            this.InitCommands();

            App.EventAgg.Subscribe<SwitchDialogEventArgs<IViewModel>>(this.HandleSwitchDialogRequest);
            App.EventAgg.Subscribe<CurrentIdEventArgs<IViewModel>>(this.CurrentIdRequest);

            this.NewDatabaseHandler();

            this.StatuslineDescription = $"(0) {Path.GetFileName(App.DatabasePath)}";

            Mouse.OverrideCursor = null;
            this.LoadContent(MenuButtons.MainOverview);
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

        public Guid CurrentId { get; set; }

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.WindowClose, new RelayCommand(p1 => this.WindowCloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.NewDetail, new RelayCommand(p1 => this.NewDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.EditDetail, new RelayCommand(p1 => this.EditDetailHandler(), p2 => true));
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

        private void NewDetailHandler()
        {
            this.LoadContent(MenuButtons.MainDetail, Guid.Empty);
        }

        private void EditDetailHandler()
        {
            if (this.CurrentId != Guid.Empty)
            {
                this.LoadContent(MenuButtons.MainDetail, this.CurrentId);
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

        private void ExcelExportHandler()
        {
            this.LoadContent(MenuButtons.ExcelExport);
        }

        private void SettingsHandler()
        {
            this.LoadContent(MenuButtons.Settings);
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

        private void LoadContent(MenuButtons targetPage, int rowPosition = -1)
        {
            try
            {
                this.CurrentControl = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.CurrentControl = ViewObjectFactory.GetControl(targetPage);
                if (this.CurrentControl != null)
                {
                    if (targetPage == MenuButtons.MainOverview)
                    {
                        MainOverviewVM controlVM = new MainOverviewVM(rowPosition);
                        this.CurrentControl.Focusable = true;
                        this.CurrentControl.Focus();
                        this.CurrentControl.DataContext = controlVM;
                        ((MainOverview)this.CurrentControl).RowPosition = rowPosition;
                    }
                    else if (targetPage == MenuButtons.Settings)
                    {
                        SettingsVM controlVM = new SettingsVM();
                        this.CurrentControl.Focusable = true;
                        this.CurrentControl.Focus();
                        this.CurrentControl.DataContext = controlVM;
                    }
                    else if (targetPage == MenuButtons.ExcelExport)
                    {
                        ExcelExportVM controlVM = new ExcelExportVM();
                        this.CurrentControl.Focusable = true;
                        this.CurrentControl.Focus();
                        this.CurrentControl.DataContext = controlVM;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }

        private void LoadContent(MenuButtons targetPage, Guid entityId, int rowPosition = -1)
        {
            try
            {
                this.CurrentControl = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.CurrentControl = ViewObjectFactory.GetControl(targetPage);
                if (this.CurrentControl != null)
                {
                    if (targetPage == MenuButtons.MainDetail)
                    {
                        MainDetailVM controlVM = new MainDetailVM(entityId, rowPosition);
                        this.CurrentControl.Focusable = true;
                        this.CurrentControl.Focus();
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
                if (obj.EntityId != Guid.Empty)
                {
                    this.LoadContent(obj.TargetPage, obj.EntityId, obj.RowPosition);
                }
                else if (obj.EntityId == Guid.Empty && obj.RowPosition == -2)
                {
                    this.LoadContent(obj.TargetPage, obj.EntityId, -1);
                }
                else
                {
                    this.LoadContent(obj.TargetPage, obj.RowPosition);
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }
    }
}
