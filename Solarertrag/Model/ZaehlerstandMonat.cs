//-----------------------------------------------------------------------
// <copyright file="ZaehlerstandMonat.cs" company="Lifeprojects.de">
//     Class: ZaehlerstandMonat
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.08.2025</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Model
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;

    [DebuggerDisplay("Year={this.Year}:Month={this.Month}; Verbrauch={this.Verbrauch}")]
    public partial class ZaehlerstandMonat : ModelBase<ZaehlerstandMonat>, IModel, INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZaehlerstandMonat"/> class.
        /// </summary>
        public ZaehlerstandMonat()
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = UserInfo.TS().CurrentDomainUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        public Guid Id { get; set; }

        [SearchFilter]
        public int Year { get; set; }

        [SearchFilter]
        public int Month { get; set; }

        public double Verbrauch { get; set; }

        [SearchFilter]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}
