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
    using EasyPrototypingNET.Core.Application;
    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;

    [DebuggerDisplay("Year={this.Year}:Month={this.Month}; Ertrag={this.Ertrag}")]
    public partial class SolarertragMonat : ModelBase<SolarertragMonat>, IModel, INotifyPropertyChanged
    {
        public string FullName
        {
            get
            {
                return $"{this.Year}.{this.Month}-{this.Ertrag}";
            }
        }

        public string Timestamp
        {
            get
            {
                string result = string.Empty;

                using (TimeStamp ts = new TimeStamp())
                {
                    result = ts.MaxEntry(this.CreatedOn, this.CreatedBy, this.ModifiedOn, this.ModifiedBy);
                }

                return result;
            }
        }
    }
}
