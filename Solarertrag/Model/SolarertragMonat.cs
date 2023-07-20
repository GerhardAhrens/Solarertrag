//-----------------------------------------------------------------------
// <copyright file="SolarertragMonat.cs" company="Lifeprojects.de">
//     Class: SolarertragMonat
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.06.2023 17:48:14</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;

    [DebuggerDisplay("Year={this.Year}:Month={this.Month}; Ertrag={this.Ertrag}")]
    public partial class SolarertragMonat : ModelBase<SolarertragMonat>, IModel, INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolarertragMonat"/> class.
        /// </summary>
        public SolarertragMonat()
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

        public double Ertrag { get; set; }

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
