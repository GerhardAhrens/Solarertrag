//-----------------------------------------------------------------------
// <copyright file="SettingsVM.cs" company="Lifeprojects.de">
//     Class: SettingsVM
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Console.ApplicationSettings;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.IO;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public class SettingsVM : ViewModelBase<SettingsVM>, IViewModel
    {
        private readonly Window mainWindow = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsVM"/> class.
        /// </summary>
        public SettingsVM()
        {
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.DialogDescription = "Programmeinstellungen bearbeiten";
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
        public bool IsExitQuestion
        {
            get { return this.Get<bool>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string AssemblyPath
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

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.CloseDetail, new RelayCommand(p1 => this.CloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.SaveDetail, new RelayCommand(p1 => this.SaveHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.OpenFolder, new RelayCommand(p1 => this.OpenFolderHandler(), p2 => true));
        }

        private void LoadDataHandler()
        {
            using (SettingsManager sm = new SettingsManager())
            {
                this.IsExitQuestion = sm.ExitQuestion;
                this.ExcelExportPath = sm.LastExportFile;
            }

            this.AssemblyPath = ApplicationProperties.AssemplyPath;
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
                        FromPage = MenuButtons.Settings,
                        TargetPage = MenuButtons.MainOverview
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void SaveHandler()
        {
            using (SettingsManager sm = new SettingsManager())
            {
                sm.ExitQuestion = this.IsExitQuestion;
                sm.LastExportFile = this.ExcelExportPath;

                App.ExitQuestion = sm.ExitQuestion;
            }
        }

        private void OpenFolderHandler()
        {
            try
            {
                using (FolderBrowserDialogEx openFile = new FolderBrowserDialogEx())
                {
                    openFile.Title = "Verzeichnis für den Export auswählen";
                    openFile.ShowNewFolderButton = true;
                    openFile.RootFolder = Environment.SpecialFolder.MyComputer;
                    openFile.OpenDialog();
                    if (string.IsNullOrEmpty(openFile.SelectedPath) == false)
                    {
                        this.ExcelExportPath = openFile.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
            }
        }
        #endregion Command Handler
    }
}
