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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;

    using EasyPrototypingNET.WPF;

    using SinglePageApplicationWPF;

    using Solarertrag.View.Controls;

    public static class DialogNavigation
    {
        private const int HOME = 1;
        private const int MAINOVERVIEW = 2;
        private const int MAINDETAIL = 4;
        private const int SETTINGS = 4;
        private const int EXCELEXPORT = 5;

        private static ConcurrentDictionary<int, Tuple<string, string, Type>> Views;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogNavigation"/> class.
        /// </summary>
        static DialogNavigation()
        {
            RegisterControls();
        }

        private static void RegisterControls()
        {
            ViewObjects viewObjectsSource = new ViewObjects();
            CommandButtons result = CommandButtons.FromValue<CommandButtons>(HOME);
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(EmptyPage));

            result = CommandButtons.FromValue<CommandButtons>(MAINOVERVIEW);
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(MainOverview));

            result = CommandButtons.FromValue<CommandButtons>(MAINDETAIL);
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(MainDetail));

            result = CommandButtons.FromValue<CommandButtons>(SETTINGS);
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(Settings));

            result = CommandButtons.FromValue<CommandButtons>(EXCELEXPORT);
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(ExcelExport));

            Views = viewObjectsSource.Views;
        }

        public static UserControl GetControl(CommandButtons commandButton)
        {
            UserControl ctrlView = null;

            using (WaitCursor wc = new WaitCursor())
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (Views.ContainsKey(commandButton.Key) == true)
                {
                    ctrlView = CreateInstance(commandButton.Key);
                    ctrlView.Focusable = true;
                    ctrlView.Focus();
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
            Tuple<string, string, Type> viewObject = Views[key];

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
