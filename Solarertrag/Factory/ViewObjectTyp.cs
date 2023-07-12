//-----------------------------------------------------------------------
// <copyright file="ViewObjectTyp.cs" company="Lifeprojects.de">
//     Class: ViewObjectTyp
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>13.01.2021</date>
//
// <summary>
// Klasse mit Enums zur Differenzierung von ViewObject Typen (UserControl, Window, None)
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System.ComponentModel;

    using EasyPrototyping.Core;

    public enum ViewObjectTyp
    {
        [Description("Keine Darstellung")]
        None = 0,
        [Description("UserControl als Content")]
        UserControl = 1,
        [Description("Window, Dialog")]
        WindowDialog = 2
    }
}
