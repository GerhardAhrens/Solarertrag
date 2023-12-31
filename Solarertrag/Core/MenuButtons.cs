//-----------------------------------------------------------------------
// <copyright file="MenuButtons.cs" company="Lifeprojects.de">
//     Class: MenuButtons
//     Copyright � www.lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>11.07.2023 14:54:53</date>
//
// <summary>
// Enum Klasse f�r Men�punkte
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.ComponentModel;

    public enum MenuButtons : int
    {
        None = -1,
        [Description("Home - Leere Seite")]
        Home = 0,
        [Description("Hauptdialog")]
        MainOverview = 1,
        [Description("Bearbeitungsdialog")]
        MainDetail = 2,
        [Description("Einstellungen")]
        Settings = 3,
        [Description("ExcelExport")]
        ExcelExport = 4
    }
}
