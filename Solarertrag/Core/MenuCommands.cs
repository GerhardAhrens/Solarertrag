//-----------------------------------------------------------------------
// <copyright file="MenuCommands.cs" company="Lifeprojects.de">
//     Class: MenuCommands
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.07.2023 12:48:10</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;

    public enum MenuCommands : int
    {
        None = 0,
        WindowClose = 1,
        EditDetail = 2,
        CloseDetail = 3,
        NewDetail = 4,
        SaveDetail = 5,
        DeleteDetail = 6,
        ExcelExport = 7,
        Settings = 8,
        OpenFolder = 9
    }
}
