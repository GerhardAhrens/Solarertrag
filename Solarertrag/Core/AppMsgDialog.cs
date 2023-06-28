﻿//-----------------------------------------------------------------------
// <copyright file="AppMsgDialog.cs" company="Lifeprojects.de">
//     Class: AppMsgDialog
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.06.2023</date>
//
// <summary>
// Class with MessageBox Dialogs Texts
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Input;

    using EasyPrototypingNET.WPF;

    [SupportedOSPlatform("windows")]
    public class AppMsgDialog
    {
        private const string APPLICATIONNAME = "Solarertrag Tool";
        public static DialogResultsEx ApplicationExit()
        {
            DialogResultsEx dialogResult = DialogResultsEx.None;

            dialogResult = MessageBoxEx.Show(APPLICATIONNAME, "Wollen Sie das Programm beenden?", string.Empty, MessageBoxButton.YesNo, InstructionIcon.Question, DialogResultsEx.No);

            return dialogResult;
        }

        public static DialogResultsEx FuncNotImplementation(string addText = "")
        {
            DialogResultsEx dialogResult = DialogResultsEx.None;

            dialogResult = MessageBoxEx.Show("Fehlende Funktion",
                $"Die Funktion '{addText}' ist nicht vorhanden!!",
                "Die aufgerufene Funktion wurde noch nicht implementiert. Versuchen Sie es zu einem späteren Zeitpunkt.",
                MessageBoxButton.OK, InstructionIcon.Information);

            return dialogResult;
        }

        private static void CleanUp()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
