//-----------------------------------------------------------------------
// <copyright file="Note.cs" company="www.lifeprojects.de">
//     Class: Note
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>30.06.2022 13:44:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Model
{
    using EasyPrototypingNET.Core.Application;
    using EasyPrototypingNET.Interface;

    using Solarertrag.Core;

    public sealed partial class Note : ModelBase<Note>, IModel
    {
        public string FullName
        {
            get
            {
                return $"{this.ObjectId}-{this.ObjectName} [{this.Content}]";
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
