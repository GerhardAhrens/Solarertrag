//-----------------------------------------------------------------------
// <copyright file="CommandButtons.cs" company="www.pta.de">
//     Class: CommandButtons
//     Copyright � www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>09.08.2023 15:20:38</date>
//
// <summary>
// Klasse f�r 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    public class CommandButtons : EnumBase
    {
        public static readonly CommandButtons None = new NoneButton();
        public static readonly CommandButtons Home = new HomeButton();
        public static readonly CommandButtons MainOverview = new MainOverviewButton();
        public static readonly CommandButtons MainDetail = new MainDetailButton();
        public static readonly CommandButtons Settings = new SettingsButton();
        public static readonly CommandButtons ExcelExport = new ExcelExportButton();
        public static readonly CommandButtons ZaehlerstandEdit = new ZaehlerstandEditButton();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandButtons"/> class.
        /// </summary>
        private CommandButtons(int value, string name = null, string description = null) : base(value, name, description)
        {
        }

        public virtual string Tooltip { get; }

        private class NoneButton : CommandButtons
        {
            public NoneButton() : base(0, "None")
            {
            }

            public override string Tooltip => "Ohne Funktion";
        }

        private class HomeButton : CommandButtons
        {
            public HomeButton() : base(1, "Home", "Leere Seite")
            {
            }

            public override string Tooltip => "Startseite ohne Datenansicht";
        }

        private class MainOverviewButton : CommandButtons
        {
            public MainOverviewButton() : base(2, "MainOverview", "�bersicht")
            {
            }

            public override string Tooltip => "�vbersicht anzeigen";
        }

        private class MainDetailButton : CommandButtons
        {
            public MainDetailButton() : base(3, "MainDetail", "Bearbeitungsdialog")
            {
            }

            public override string Tooltip => "Bearbeitungsdialog anzeigen";
        }

        private class SettingsButton : CommandButtons
        {
            public SettingsButton() : base(4, "Settings", "Einstellungen")
            {
            }

            public override string Tooltip => "Programmeinstellungen bearbeiten";
        }

        private class ExcelExportButton : CommandButtons
        {
            public ExcelExportButton() : base(5, "ExcelExport", "Datenexport nach Excel")
            {
            }

            public override string Tooltip => "Ausgew�hlte Datens�tze nach Excel exportieren";
        }

        private class ZaehlerstandEditButton : CommandButtons
        {
            public ZaehlerstandEditButton() : base(6, "Z�hlerstand", "Z�hlerstand bearbeiten")
            {
            }

            public override string Tooltip => "Ausgew�hlte Datens�tze nach Excel exportieren";
        }
    }
}
