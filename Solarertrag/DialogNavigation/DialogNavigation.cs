//-----------------------------------------------------------------------
// <copyright file="DialogNavigation.cs" company="www.pta.de">
//     Class: DialogNavigation
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>31.07.2023 16:37:11</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;

    using EasyPrototypingNET.WPF;

    using Solarertrag.View.Controls;

    public class DialogNavigation
    {
        private const int HOME = 0;
        private const int MAINOVERVIEW = 1;
        private const int MAINDETAIL = 2;
        private const int SETTINGS = 3;
        private const int EXCELEXPORT = 4;

        private static SinglePageApplicationWPF.MenuButtons menuButtonsSource;
        private static SinglePageApplicationWPF.ViewObjects viewObjectsSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogNavigation"/> class.
        /// </summary>
        public DialogNavigation()
        {
            menuButtonsSource = new SinglePageApplicationWPF.MenuButtons();
            viewObjectsSource = new SinglePageApplicationWPF.ViewObjects();
        }

        public void RegisterMenuButtons()
        {
            if (menuButtonsSource != null)
            {
                menuButtonsSource.Add(HOME, "Home", "Home - Leere Seite");
                menuButtonsSource.Add(MAINOVERVIEW, "MainOverview", "Hauptdialog");
                menuButtonsSource.Add(MAINDETAIL, "MainDetail", "Bearbeitungsdialog");
                menuButtonsSource.Add(SETTINGS, "Settings", "Einstellungen");
                menuButtonsSource.Add(EXCELEXPORT, "ExcelExport", "Datenexport nach Excel");
            }
        }

        public void RegisterControls()
        {
            if (viewObjectsSource != null)
            {
                string[] button = menuButtonsSource.Get(HOME);
                viewObjectsSource.Add(HOME, button[0], button[1], typeof(EmptyPage));

                button = menuButtonsSource.Get(MAINOVERVIEW);
                viewObjectsSource.Add(MAINOVERVIEW, button[0], button[1], typeof(MainOverview));

                button = menuButtonsSource.Get(MAINDETAIL);
                viewObjectsSource.Add(MAINDETAIL, button[0], button[1], typeof(MainDetail));

                button = menuButtonsSource.Get(SETTINGS);
                viewObjectsSource.Add(SETTINGS, button[0], button[1], typeof(Settings));

                button = menuButtonsSource.Get(EXCELEXPORT);
                viewObjectsSource.Add(EXCELEXPORT, button[0], button[1], typeof(ExcelExport));
            }
        }

        public static UserControl GetControl(int key)
        {
            UserControl ctrlView = null;

            using (WaitCursor wc = new WaitCursor())
            {
                if (viewObjectsSource.ContainsKey(key) == true)
                {
                    ctrlView = CreateInstance(key);
                }
                else
                {
                    ctrlView = null;
                }
            }

            return ctrlView;
        }

        private static UserControl CreateInstance(int key)
        {
            Tuple<string, string, Type> viewObject = viewObjectsSource.Get(key);

            if (viewObject.Item3 != null)
            {
                if (viewObject.Item3.GetConstructors().Count() >= 1)
                {
                    if (viewObject.Item3.GetConstructors()[0].GetParameters().Count() == 1)
                    {
                        ParameterInfo param = viewObject.Item3.GetConstructors()[0].GetParameters()[0];
                        if (param.Name == "pageTyp")
                        {
                            return (UserControl)Activator.CreateInstance(viewObject.Item3, key);
                        }
                        else
                        {
                            return (UserControl)Activator.CreateInstance(viewObject.Item3, null);
                        }
                    }
                    else
                    {
                        return (UserControl)Activator.CreateInstance(viewObject.Item3);
                    }
                }
                else
                {
                    return (UserControl)Activator.CreateInstance(viewObject.Item3, 1, viewObject.Item2);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
