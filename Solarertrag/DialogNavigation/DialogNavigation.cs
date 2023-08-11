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
            CommandButtons result = CommandButtons.Home;
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(EmptyPage));

            result = CommandButtons.MainOverview;
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(MainOverview));

            result = CommandButtons.MainDetail;
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(MainDetail));

            result = CommandButtons.Settings;
            viewObjectsSource.Add(result.Key, result.Name, result.Description, typeof(Settings));

            result = CommandButtons.ExcelExport;
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
