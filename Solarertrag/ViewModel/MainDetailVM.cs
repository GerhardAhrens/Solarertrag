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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.ExceptionHandling;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.Pattern;
    using EasyPrototypingNET.WPF;

    using PertNET.DataRepository;

    using Solarertrag.Core;
    using Solarertrag.Model;

    [SupportedOSPlatform("windows")]
    [ViewModel]
    public class MainDetailVM : ViewModelBase<MainDetailVM>, IViewModel
    {
        private readonly Window mainWindow = null;
        private readonly Dictionary<string, Func<Result<string>>> validationDelegates = new Dictionary<string, Func<Result<string>>>();
        private readonly HashSet<string> propertyNames = new HashSet<string>();

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

            this.propertyNames = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(s => s.Name).ToHashSet();
            this.ValidationErrors = new ObservableCollectionEx<string>();
            this.RegisterValidations();
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
        public string Year
        {
            get { return this.Get<string>(); }
            set { this.Set(value, this.CheckContent); }
        }

        [PropertyBinding]
        public string Month
        {
            get { return this.Get<string>(); }
            set { this.Set(value, this.CheckContent); }
        }

        [PropertyBinding]
        public string Ertrag
        {
            get { return this.Get<string>(); }
            set { this.Set(value, this.CheckContent); }
        }

        [PropertyBinding]
        public string Description
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public ObservableCollectionEx<string> ValidationErrors
        {
            get { return this.Get<ObservableCollectionEx<string>>(); }
            set { this.Set(value); }
        }

        private Guid CurrentId { get; set; }

        private int RowPosition { get; set; }

        private bool IsDirty { get; set; }

        #endregion Get/Set Properties

        protected sealed override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(MenuCommands.CloseDetail, new RelayCommand(p1 => this.CloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand(MenuCommands.SaveDetail, new RelayCommand(p1 => this.SaveDetailHandler(), p2 => CanSaveDetailHandler()));
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
                        this.ShowData();
                    }
                }
                else
                {
                    this.CurrentSelectedItem = new SolarertragMonat();
                    this.ShowData();
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
            if (this.CurrentId != Guid.Empty)
            {
                this.Year = this.CurrentSelectedItem.Year.ToString();
                this.Month = this.CurrentSelectedItem.Month.ToString();
                this.Ertrag = this.CurrentSelectedItem.Ertrag.ToString();
                this.Description = this.CurrentSelectedItem.Description;
            }
            else
            {
                this.Year = DateTime.Now.Year.ToString();
                this.Month = DateTime.Now.Month.ToString();
                this.Ertrag = "0";
                this.Description = string.Empty;
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
                        RowPosition = this.RowPosition,
                        DataType = this as IViewModel,
                        FromPage = MenuButtons.MainDetail,
                        TargetPage = MenuButtons.MainOverview
                    });
            }
            catch (Exception ex)
            {
                ExceptionViewer.Show(ex, this.GetType().Name);
                throw;
            }
        }

        private bool CanSaveDetailHandler()
        {
            return this.ValidationErrors.Count.ToBool() == true ? false : true;
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
                            original.Year = this.Year.ToInt();
                            original.Month = this.Month.ToInt();
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
                        original.Year = this.Year.ToInt();
                        original.Month = this.Month.ToInt();
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

        #region Validierung
        private void RegisterValidations()
        {
            this.validationDelegates.Add(nameof(this.Year), () =>
            {
                return SolarErtragValidation<MainDetailVM>.This(this).InRangeYear(x => x.Year, 2020, 2100);
            });

            this.validationDelegates.Add(nameof(this.Month), () =>
            {
                return SolarErtragValidation<MainDetailVM>.This(this).InRangeMonth(x => x.Month, 1, 12);
            });

            this.validationDelegates.Add(nameof(this.Ertrag), () =>
            {
                return SolarErtragValidation<MainDetailVM>.This(this).GreaterThanZero(x => x.Ertrag);
            });
        }
        #endregion Validierung

        private void CheckContent<T>(T value, string propertyName)
        {
            if (this.CurrentSelectedItem == null)
            {
                return;
            }

            PropertyInfo propInfo = this.CurrentSelectedItem.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyName);
            if (propInfo == null)
            {
                this.IsDirty = false;
                return;
            }

            var propValue = propInfo.GetValue(this.CurrentSelectedItem);

            if (propValue == null)
            {
                this.IsDirty = true;
                return;
            }

            if (propValue.Equals(value) == false)
            {
                this.IsDirty = true;
            }

            this.ValidationErrors.Clear();
            foreach (string property in this.propertyNames)
            {
                Func<Result<string>> function = null;
                if (validationDelegates.TryGetValue(property, out function) == true)
                {
                    Result<string> ruleText = this.DoValidation(function, property);
                    if (string.IsNullOrEmpty(ruleText.Value) == false)
                    {
                        this.ValidationErrors.Add(ruleText.Value);
                    }
                }
            }
        }
    }
}
