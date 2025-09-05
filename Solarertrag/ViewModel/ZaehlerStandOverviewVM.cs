//-----------------------------------------------------------------------
// <copyright file="ZaehlerStandOverviewVM.cs" company="Lifeprojects.de">
//     Class: ZaehlerStandOverviewVM
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>05.09.2025 10:18:20</date>
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
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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

    public class ZaehlerStandOverviewVM : ViewModelBase<MainOverviewVM>, IViewModel
    {
        private readonly Window mainWindow = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZaehlerStandOverviewVM"/> class.
        /// </summary>
        public ZaehlerStandOverviewVM(ControlContentArgs args)
        {
            if (args.RowPosition != RowItemPosition.GoMove)
            {
                this.RowPosition = -1;
            }
            else
            {
                this.RowPosition = args.RowPosition.GoTo;
            }

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
        public ZaehlerstandMonat CurrentSelectedItem
        {
            get { return this.Get<ZaehlerstandMonat>(); }
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
        public Dictionary<string, string> ZaehlerStandSource
        {
            get { return this.Get<Dictionary<string, string>>(); }
            set { this.Set(value); }
        }

        public int RowPosition { get; set; }
        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.EditDetail, new RelayCommand(p1 => this.EditDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.NewDetail, new RelayCommand(p1 => this.NewDetailHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.DeleteDetail, new RelayCommand(p1 => this.DeleteDetailHandler(), p2 => true));
        }

        private void LoadDataHandler()
        {
            try
            {
                using (ZaehlerstandMonatRepository repository = new ZaehlerstandMonatRepository(App.DatabasePath))
                {
                    IEnumerable<ZaehlerstandMonat> overviewSource = repository.List();
                    if (overviewSource != null)
                    {
                        this.DialogDataView = CollectionViewSource.GetDefaultView(overviewSource);
                        if (this.DialogDataView != null)
                        {
                            this.DialogDataView.Filter = rowItem => this.DataDefaultFilter(rowItem as ZaehlerstandMonat);
                            this.DialogDataView.SortDescriptions.Clear();
                            this.DialogDataView.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Ascending));
                            this.DialogDataView.SortDescriptions.Add(new SortDescription("Month", ListSortDirection.Ascending));
                            this.DialogDataView.GroupDescriptions.Add(new PropertyGroupDescription("Year"));
                            if (this.RowPosition == -1)
                            {
                                this.DialogDataView.MoveCurrentToFirst();
                                var currentItem = this.DialogDataView.Cast<ZaehlerstandMonat>().Where(w => w.Year == DateTime.Now.Year).LastOrDefault();
                                if (currentItem != null)
                                {
                                    this.DialogDataView.MoveCurrentTo(currentItem);
                                    this.RowPosition = this.DialogDataView.CurrentPosition;
                                    this.CurrentSelectedItem = currentItem;
                                }
                            }
                            else
                            {
                                this.DialogDataView.MoveCurrentToPosition(this.RowPosition);
                            }

                            this.ZaehlerStandSource = new Dictionary<string, string>();
                            foreach (var item in this.DialogDataView.Groups)
                            {
                                string key = ((CollectionViewGroup)item).Name.ToString();
                                var itemsYear = ((CollectionViewGroup)item).Items;

                                string lastZaehlerStand = $"Letzter Zählerstand {itemsYear.Cast<ZaehlerstandMonat>().LastOrDefault().Verbrauch.ToString()}";
                                this.ZaehlerStandSource.Add(key, lastZaehlerStand);
                            }
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

        private bool DataDefaultFilter(ZaehlerstandMonat rowItem)
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
                    this.MaxRowCount = this.DialogDataView.Cast<ZaehlerstandMonat>().Count();
                    this.DialogDataView.MoveCurrentToFirst();

                    if (this.MaxRowCount > 0)
                    {
                        this.IsFilterContentFound = false;
                    }
                    else
                    {
                        this.IsFilterContentFound = true;
                    }
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
                        FromPage = CommandButtons.ZaehlerstandOverview,
                        TargetPage = CommandButtons.ZaehlerstandEdit,
                        IsNew = false
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
                        FromPage = CommandButtons.ZaehlerstandOverview,
                        TargetPage = CommandButtons.ZaehlerstandEdit,
                        IsNew = true
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
                using (ZaehlerstandMonatRepository repository = new ZaehlerstandMonatRepository(App.DatabasePath))
                {
                    repository.Delete(this.CurrentSelectedItem.Id);
                }

                this.LoadDataHandler();
            }
        }
    }
}
