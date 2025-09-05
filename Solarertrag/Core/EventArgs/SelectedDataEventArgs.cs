//-----------------------------------------------------------------------
// <copyright file="SwitchDialogEventArgs.cs" company="Lifeprojects.de">
//     Class: SwitchDialogEventArgs
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>29.05.2021</date>
//
// <summary>
// Argument Class zum Darstellen des Wechseln zwischen zwei Dialogen
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Collections.Generic;

    using EasyPrototypingNET.Pattern;

    using Solarertrag.Model;

    /// <summary>
    /// Argument beim Wechslem zwischen den UserControl-Dialogen
    /// </summary>
    /// <typeparam name="IViewModel"></typeparam>
    public class SelectedDataEventArgs : EventArgs, IPayload
    {
        /// <summary>
        /// UserControl-Dialog Content
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// UserControl-Dialog Typ
        /// </summary>
        public List<SolarertragMonat> SolarData { get; set; }
        public List<ZaehlerstandMonat> ZaehlerStandData { get; set; }

    }
}
