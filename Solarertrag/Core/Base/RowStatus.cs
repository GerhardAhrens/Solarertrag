//-----------------------------------------------------------------------
// <copyright file="RowStatus.cs" company="Lifeprojects.de">
//     Class: RowStatus
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.08.2023 14:05:51</date>
//
// <summary>
// Enum Klasse für den Status einzelner Datensätze
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;

    public enum RowStatus : int
    {
        None = 0,
        New = 1,
        Delete = 2,
        Modified = 3,
    }
}
