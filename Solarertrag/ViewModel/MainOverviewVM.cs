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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Animation;

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
    public class MainOverviewVM : ViewModelBase<MainOverviewVM>, IViewModel
    {
        private readonly Window mainWindow = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainOverviewVM"/> class.
        /// </summary>
        public MainOverviewVM(int rowPosition = -1)
        {
            this.RowPosition = rowPosition;
            this.mainWindow = Application.Current.Windows.LastActiveWindow();
            this.IsContextMenuEnabled = true;
            this.InitCommands();
            this.LoadDataHandler();
        }

        #region Get/Set Properties
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

        [PropertyBinding]
        public SolarertragMonat CurrentSelectedItem
        {
            get { return this.Get<SolarertragMonat>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string FilterText
        {
            get { return this.Get<string>(); }
            set { this.Set(value, this.RefreshDefaultFilter); }
        }

        [PropertyBinding]
        public bool IsContextMenuEnabled
        {
            get { return this.Get<bool>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string ErtragFull
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string ErtragCurrentYear
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        private int RowPosition { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.EditDetail, new RelayCommand(p1 => this.EditDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.NewDetail, new RelayCommand(p1 => this.NewDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.DeleteDetail, new RelayCommand(p1 => this.DeleteDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand("SelectionChangedCommand", new RelayCommand(p1 => this.SelectionChangedHandler(p1), p2 => true));
        }

        private void LoadDataHandler()
        {
            try
            {
                using (SolarertragMonatRepository repository = new SolarertragMonatRepository(App.DatabasePath))
                {
                    IEnumerable<SolarertragMonat> overviewSource = repository.List();
                    if (overviewSource != null)
                    {
                        this.DialogDataView = CollectionViewSource.GetDefaultView(overviewSource);
                        if (this.DialogDataView != null)
                        {
                            this.DialogDataView.Filter = rowItem => this.DataDefaultFilter(rowItem as SolarertragMonat);
                            this.DialogDataView.SortDescriptions.Clear();
                            this.DialogDataView.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Ascending));
                            this.DialogDataView.SortDescriptions.Add(new SortDescription("Month", ListSortDirection.Ascending));
                            this.DialogDataView.GroupDescriptions.Add(new PropertyGroupDescription("Year"));
                            if (this.RowPosition == -1)
                            {
                                this.DialogDataView.MoveCurrentToFirst();
                            }
                            else
                            {
                                this.DialogDataView.MoveCurrentToPosition(this.RowPosition);
                            }

                            this.MaxRowCount = this.DialogDataView.Count<SolarertragMonat>();
                            double currentYear = this.DialogDataView.Cast<SolarertragMonat>().Where(w => w.Year == DateTime.Now.Year).Sum(x => x.Ertrag);
                            this.ErtragCurrentYear = currentYear.ToString("0.0");

                            App.EventAgg.Publish<SelectedDataEventArgs>(
                                new SelectedDataEventArgs
                                {
                                    Sender = this,
                                    Data = this.DialogDataView.Cast<SolarertragMonat>().ToList(),
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private bool DataDefaultFilter(SolarertragMonat rowItem)
        {
            bool wordFound = false;

            if (rowItem == null)
            {
                this.IsFilterContentFound = false;
                return false;
            }

            string textFilterString = (this.FilterText ?? string.Empty).ToUpper();
            if (string.IsNullOrEmpty(textFilterString) == false)
            {
                string fullRow = rowItem.ToSearchFilter().ToUpper();
                if (string.IsNullOrEmpty(fullRow) == true)
                {
                    return true;
                }

                string[] words = textFilterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words.AsParallel<string>())
                {
                    wordFound = fullRow.Contains(word);

                    if (wordFound == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void RefreshDefaultFilter(string value)
        {
            if (value != null && this.DialogDataView != null)
            {
                try
                {
                    this.DialogDataView.Refresh();
                    this.MaxRowCount = this.DialogDataView.Cast<SolarertragMonat>().Count();
                    this.DialogDataView.MoveCurrentToFirst();

                    if (this.MaxRowCount > 0)
                    {
                        this.IsFilterContentFound = false;

                        double ertragFull = 0;
                        foreach (SolarertragMonat item in this.DialogDataView.Cast<SolarertragMonat>())
                        {
                            ertragFull += item.Ertrag;
                        }

                        this.ErtragFull = ertragFull.ToString("0.0");
                    }
                    else
                    {
                        this.IsFilterContentFound = true;
                    }

                    App.EventAgg.Publish<SelectedDataEventArgs>(
                        new SelectedDataEventArgs
                        {
                            Sender = this,
                            Data = this.DialogDataView.Cast<SolarertragMonat>().ToList(),
                        });
                }
                catch (Exception ex)
                {
                    ExceptionViewer.Show(ex, this.GetType().Name);
                    throw;
                }
            }
        }

        private void EditDetailHandler()
        {
            Guid currentId = this.CurrentSelectedItem.Id;
            int selectedItemPos = this.DialogDataView.CurrentPosition;

            try
            {
                App.EventAgg.Publish<SwitchDialogEventArgs<IViewModel>>(
                    new SwitchDialogEventArgs<IViewModel>
                    {
                        Sender = this,
                        DataType = this as IViewModel,
                        EntityId = currentId,
                        RowPosition = selectedItemPos,
                        FromPage = MenuButtons.MainOverview,
                        TargetPage = MenuButtons.MainDetail
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void NewDetailHandler()
        {
            Guid currentId = Guid.Empty;

            if (this.CurrentSelectedItem != null)
            {
                currentId = this.CurrentSelectedItem.Id;
            }

            int selectedItemPos = this.DialogDataView.CurrentPosition;

            try
            {
                App.EventAgg.Publish<SwitchDialogEventArgs<IViewModel>>(
                    new SwitchDialogEventArgs<IViewModel>
                    {
                        Sender = this,
                        DataType = this as IViewModel,
                        EntityId = Guid.Empty,
                        RowPosition = -2,
                        FromPage = MenuButtons.MainOverview,
                        TargetPage = MenuButtons.MainDetail
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void DeleteDetailHandler()
        {
            if (AppMsgDialog.DeleteDetail(this.CurrentSelectedItem.FullName) == DialogResultsEx.Yes)
            {
                using (SolarertragMonatRepository repository = new SolarertragMonatRepository(App.DatabasePath))
                {
                    repository.Delete(this.CurrentSelectedItem.Id);
                }

                this.LoadDataHandler();
            }
        }

        private void SelectionChangedHandler(object commandParameter)
        {
            if (commandParameter != null)
            {
                IEnumerable<SolarertragMonat> itemsCollection = ((Collection<object>)commandParameter).OfType<SolarertragMonat>();
                if (itemsCollection.Count() == 0)
                {
                    this.IsContextMenuEnabled = false;
                }
                else if (itemsCollection.Count() == 1)
                {
                    this.IsContextMenuEnabled = true;

                    double ertragCurretYear = 0;
                    double ertragTotal = 0;
                    foreach (SolarertragMonat item in this.DialogDataView.Cast<SolarertragMonat>())
                    {
                        if (item.Year == DateTime.Now.Year)
                        {
                            ertragCurretYear += item.Ertrag;
                        }

                        ertragTotal += item.Ertrag;
                    }

                    this.ErtragFull = $"{ertragTotal.ToString("0.0")} KW/h, in {DateTime.Now.Year} {ertragCurretYear.ToString("0.0")} KW/h";

                    App.EventAgg.Publish<CurrentIdEventArgs<IViewModel>>(
                        new CurrentIdEventArgs<IViewModel>
                        {
                            Sender = this,
                            DataType = this as IViewModel,
                            EntityId = this.CurrentSelectedItem.Id,
                        });
                }
                else if (itemsCollection.Count() > 1)
                {
                    this.IsContextMenuEnabled = false;

                    double ertragFull = 0;
                    foreach (SolarertragMonat item in itemsCollection)
                    {
                        ertragFull += item.Ertrag;
                    }

                    this.ErtragFull = $"Total {ertragFull.ToString("0.0")} KW/h ";
                }
            }
        }
    }
}
