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
        public MainDetailVM(Guid entityId, int rowPosition)
        {
            this.CurrentId = entityId;
            this.RowPosition = rowPosition;
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
        public string Ertrag
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string Description
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        private Guid CurrentId { get; set; }

        private int RowPosition { get; set; }
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
                        this.CurrentSelectedItem = repository.ListById(this.CurrentId);
                        this.Year = this.CurrentSelectedItem.Year;
                        this.Month = this.CurrentSelectedItem.Month;
                        this.Ertrag = this.CurrentSelectedItem.Ertrag.ToString();
                        this.Description = this.CurrentSelectedItem.Description;
                    }
                }
                else
                {
                    this.Year = DateTime.Now.Year;
                    this.Month = DateTime.Now.Month;
                    this.Ertrag = "0";
                    this.Description = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
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
                        RowPosition = this.RowPosition,
                        DataType = this as IViewModel,
                        FromPage = MenuButtons.MainDetail,
                        TargetPage = MenuButtons.MainOverview
                    }); ;
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private void SaveDetailHandler()
        {
            try
            {
                using (SolarertragMonatRepository repository = new SolarertragMonatRepository(App.DatabasePath))
                {
                    if (this.CurrentId != Guid.Empty)
                    {
                        SolarertragMonat original = SolarertragMonat.ToClone(this.CurrentSelectedItem);
                        if (original != null)
                        {
                            original.Year = this.Year;
                            original.Month = this.Month;
                            original.Ertrag = this.Ertrag.ToDouble();
                            original.Description = this.Description;
                            original.ModifiedBy = UserInfo.TS().CurrentUser;
                            original.ModifiedOn = UserInfo.TS().CurrentTime;
                            repository.Update(original);
                        }
                    }
                    else
                    {
                        SolarertragMonat original = new SolarertragMonat();
                        original.Year = this.Year;
                        original.Month = this.Month;
                        original.Ertrag = this.Ertrag.ToDouble();
                        original.Description = this.Description;
                        original.CreatedBy = UserInfo.TS().CurrentUser;
                        original.CreatedOn = UserInfo.TS().CurrentTime;
                        repository.Add(original);
                    }
                }

                this.CloseHandler();
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }
        #endregion Command Handler
    }
}
