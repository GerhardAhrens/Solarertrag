//-----------------------------------------------------------------------
// <copyright file="ExcelExportVM.cs" company="Lifeprojects.de">
//     Class: ExcelExportVM
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows;

    using Console.ApplicationSettings;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Exporter;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.IO;
    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.DialogNavigation;
    using Solarertrag.Model;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public class ExcelExportVM : ViewModelBase<ExcelExportVM>, IViewModel
    {
        private readonly Window mainWindow = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExportVM"/> class.
        /// </summary>
        public ExcelExportVM(List<SolarertragMonat> currentData)
        {
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.DialogDescription = ResourceObject.GetAs<string>("DialogDescription");

            this.InitCommands();
            this.LoadDataHandler();
            this.CurrentDataExport = currentData;
            this.Message = MessageFormat(currentData.Count);
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

        private List<SolarertragMonat> CurrentDataExport { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.CloseDetail, new RelayCommand(p1 => this.CloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.ExcelExport, new RelayCommand(p1 => this.ExcelExportHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.OpenFolder, new RelayCommand(p1 => this.OpenFolderHandler(), p2 => true));
        }

        private void LoadDataHandler()
        {
            using (SettingsManager sm = new SettingsManager())
            {
                this.ExcelExportPath = sm.LastExportFile;
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
                        FromPage = CommandButtons.Settings,
                        TargetPage = CommandButtons.MainOverview
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void OpenFolderHandler()
        {
            try
            {
                using (FolderBrowserDialogEx openFile = new FolderBrowserDialogEx())
                {
                    openFile.Title = ResourceObject.GetAs<string>("FolderBrowserDialogEx");
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

        private void ExcelExportHandler()
        {
            using (SettingsManager sm = new SettingsManager())
            {
                sm.LastExportFile = this.ExcelExportPath;
            }

            string exportName = Path.Combine(this.ExcelExportPath, "SolarErtrag.xlsx");

            EasyPrototypingNET.Exporter.Style boldStyle = EasyPrototypingNET.Exporter.Style.BasicStyles.Bold;

            EasyPrototypingNET.Exporter.Style headerStyle = new EasyPrototypingNET.Exporter.Style();
            headerStyle.CurrentFill.SetColor("FFC0C0C0", EasyPrototypingNET.Exporter.Style.Fill.FillType.fillColor);
            headerStyle.CurrentFont.Bold = true;
            headerStyle.CurrentCellXf.HorizontalAlign = EasyPrototypingNET.Exporter.Style.CellXf.HorizontalAlignValue.left; 


            Workbook workbook = new Workbook(exportName, "SolarErtrag");

            List<object> values = new List<object>() { "Jahr", "Monat", "Ertrag in KW/h" , "Bemerkung"};
            workbook.CurrentWorksheet.AddCellRange(values, new Cell.Address(0, 0), new Cell.Address(3, 0));
            workbook.CurrentWorksheet.Cells["A1"].SetStyle(headerStyle);
            workbook.CurrentWorksheet.Cells["B1"].SetStyle(headerStyle);
            workbook.CurrentWorksheet.Cells["C1"].SetStyle(headerStyle);
            workbook.CurrentWorksheet.Cells["D1"].SetStyle(headerStyle);

            workbook.CurrentWorksheet.SetColumnWidth(0, 10f);
            workbook.CurrentWorksheet.SetColumnWidth(1, 10f);
            workbook.CurrentWorksheet.SetColumnWidth(2, 15f);
            workbook.CurrentWorksheet.SetColumnWidth(3, 70f);

            workbook.WorkbookMetadata.Title = "Solarertrag";
            workbook.WorkbookMetadata.Subject = "Solarertrag für";
            workbook.WorkbookMetadata.Creator = UserInfo.TS().CurrentUser;
            workbook.WorkbookMetadata.Keywords = "Solarertrag;KW/h";

            workbook.CurrentWorksheet.SetAutoFilter(0, 2);

            workbook.CurrentWorksheet.GoToNextRow();
            int rowExcel = 2;

            foreach (SolarertragMonat row in this.CurrentDataExport)
            {
                workbook.CurrentWorksheet.AddCell(row.Year, $"A{rowExcel}");
                workbook.CurrentWorksheet.AddCell(row.Month, $"B{rowExcel}");
                workbook.CurrentWorksheet.AddCell(row.Ertrag, $"C{rowExcel}");
                workbook.CurrentWorksheet.AddCell(row.Description, $"D{rowExcel}");
                workbook.CurrentWorksheet.GoToNextRow();
                rowExcel++;
            }

            rowExcel++;
            workbook.CurrentWorksheet.GoToNextRow();
            workbook.CurrentWorksheet.AddCell("Summe", $"B{rowExcel}", boldStyle);
            string summe = $"{this.CurrentDataExport.Sum(s => s.Ertrag)} KW/h";
            workbook.CurrentWorksheet.AddCell(summe, $"C{rowExcel}", boldStyle);

            workbook.Save();

            if (File.Exists(exportName) == true)
            {
                using (RunFileExplorer openFile = new RunFileExplorer())
                {
                    openFile.SelectFile(exportName);
                }
            }
        }
        #endregion Command Handler

        private string MessageFormat(int count)
        {
            if (count > 1)
            {
                return string.Format(ResourceObject.GetAs<string>("MessageSI"), count);
            }
            else
            {
                return ResourceObject.GetAs<string>("MessagePL");
            }
        }
    }
}
