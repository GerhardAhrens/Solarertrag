//-----------------------------------------------------------------------
// <copyright file="CommandButtons.cs" company="www.pta.de">
//     Class: CommandButtons
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>09.08.2023 15:20:38</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SinglePageApplicationWPF.Base;

    public abstract class CommandButtons : EnumBase
    {
        public static readonly CommandButtons None = new NoneButton();
        public static readonly CommandButtons Home = new HomeButton();
        public static readonly CommandButtons MainOverview = new MainOverviewButton();
        public static readonly CommandButtons MainDetail = new MainDetailButton();
        public static readonly CommandButtons Settings = new SettingsButton();
        public static readonly CommandButtons ExcelExport = new ExcelExportButton();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandButtons"/> class.
        /// </summary>
        private CommandButtons(int value, string name = null, string description = null) : base(value, name, description)
        {
        }

        public abstract string Code { get; }

        private class NoneButton : CommandButtons
        {
            public NoneButton() : base(0, "None")
            {
            }

            public override string Code => "NO";
        }

        private class HomeButton : CommandButtons
        {
            public HomeButton() : base(1, "Home", "Leere Seite")
            {
            }

            public override string Code => "OV";
        }

        private class MainOverviewButton : CommandButtons
        {
            public MainOverviewButton() : base(1, "MainOverview", "Übersicht")
            {
            }

            public override string Code => "OV";
        }

        private class MainDetailButton : CommandButtons
        {
            public MainDetailButton() : base(2, "MainDetail", "Bearbeitungsdialog")
            {
            }

            public override string Code => "DE";
        }

        private class SettingsButton : CommandButtons
        {
            public SettingsButton() : base(3, "Settings", "Einstellungen")
            {
            }

            public override string Code => "SE";
        }

        private class ExcelExportButton : CommandButtons
        {
            public ExcelExportButton() : base(4, "ExcelExport", "Datenexport nach Excel")
            {
            }

            public override string Code => "EX";
        }
    }
}
