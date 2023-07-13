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

        private int RowPosition { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.EditDetail, new RelayCommand(p1 => this.EditHandler(), p2 => true));
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
                            if (this.RowPosition == -1)
                            {
                                this.DialogDataView.MoveCurrentToFirst();
                            }
                            else
                            {
                                this.DialogDataView.MoveCurrentToPosition(this.RowPosition);
                            }

                            this.MaxRowCount = this.DialogDataView.Count<SolarertragMonat>();
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

        }

        private void EditHandler()
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
                }
                else if (itemsCollection.Count() > 1)
                {
                    this.IsContextMenuEnabled = false;
                }

                App.EventAgg.Publish<CurrentIdEventArgs<IViewModel>>(
                    new CurrentIdEventArgs<IViewModel>
                    {
                        Sender = this,
                        DataType = this as IViewModel,
                        EntityId = this.CurrentSelectedItem.Id,
                    });
            }
        }
    }
}
