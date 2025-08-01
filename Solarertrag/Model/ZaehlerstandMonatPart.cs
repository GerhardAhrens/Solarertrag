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
    using System.ComponentModel;
    using System.Diagnostics;

    using EasyPrototypingNET.Core.Application;
    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;

    [DebuggerDisplay("Year={this.Year}:Month={this.Month}; Verbrauch={this.Verbrauch}")]
    public partial class ZaehlerstandMonat : ModelBase<ZaehlerstandMonat>, IModel, INotifyPropertyChanged
    {
        public string FullName
        {
            get
            {
                return $"{this.Year}.{this.Month}-{this.Verbrauch}";
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
