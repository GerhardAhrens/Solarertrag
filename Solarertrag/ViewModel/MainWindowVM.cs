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
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Input;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.Pattern;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.DataRepository;

    [SupportedOSPlatform("windows")]
    //[ViewModel]
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

            this.StatuslineDescription = $"(0) {Path.GetFileName(App.DatabasePath)}";

            Mouse.OverrideCursor = null;
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
        public bool IsDatabaseOpen
        {
            get { return this.Get<bool>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public bool IsFilterContentFound
        {
            get { return this.Get<bool>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public ICollectionView DialogDataView
        {
            get { return this.Get<ICollectionView>(); }
            private set { this.Set(value); }
        }

        private string ExportProjectName { get; set; }

        private string CurrentDatabaseFile { get; set; }

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("WindowCloseCommand", new RelayCommand(p1 => this.WindowCloseHandler(), p2 => true));
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
            using (DatabaseManager dm = new DatabaseManager(App.DatabasePath))
            {
                createResult = dm.CreateNewDatabase();
            }
        }

        private void LoadDatabaseHandler()
        {
            Result<bool> openResult = null;
            using (DatabaseManager dm = new DatabaseManager(App.DatabasePath))
            {
                openResult = dm.OpenDatabase();
            }
        }
    }
}
