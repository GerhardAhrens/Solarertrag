//-----------------------------------------------------------------------
// <copyright file="ViewObjectFactory.cs" company="Lifeprojects.de">
//     Class: ViewObjectFactory
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.07.2023 16:53:53</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;

    using EasyPrototypingNET.WPF;

    using Solarertrag.Core;
    using Solarertrag.View.Controls;

    public static class ViewObjectFactory
    {
        private static readonly ViewObjectStructure ViewObjects = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewObjectFactory"/> class.
        /// </summary>
        static ViewObjectFactory()
        {
            if (ViewObjects == null)
            {
                ViewObjects = new ViewObjectStructure();
            }

            Create();
        }

        public static UserControl GetControl(MenuButtons key, double startOpacity = 1)
        {
            UserControl ctrlView = null;

            using (WaitCursor wc = new WaitCursor())
            {
                if (ViewObjects.ContainsKey(key) == true)
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

        private static void Create()
        {
            try
            {
                if (ViewObjects != null && ViewObjects.Count == 0)
                {
                    ViewObjectContent viewObject = new ViewObjectContent();
                    viewObject.AddUserControl(MenuButtons.MainOverview, MenuButtons.MainOverview.ToDescription(), typeof(MainOverview));
                    ViewObjects.Add(viewObject);

                    viewObject = new ViewObjectContent();
                    viewObject.AddUserControl(MenuButtons.MainDetail, MenuButtons.MainDetail.ToDescription(), typeof(MainDetail));
                    ViewObjects.Add(viewObject);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static UserControl CreateInstance(MenuButtons key)
        {
            ViewObjectContent viewObject = ViewObjects[key];

            if (viewObject.ViewControl != null)
            {
                if (viewObject.ViewControl.GetConstructors().Count() >= 1)
                {
                    if (viewObject.ViewControl.GetConstructors()[0].GetParameters().Count() == 1)
                    {
                        ParameterInfo param = viewObject.ViewControl.GetConstructors()[0].GetParameters()[0];
                        if (param.Name == "pageTyp")
                        {
                            return (UserControl)Activator.CreateInstance(viewObject.ViewControl, key);
                        }
                        else
                        {
                            return (UserControl)Activator.CreateInstance(viewObject.ViewControl, null);
                        }
                    }
                    else
                    {
                        return (UserControl)Activator.CreateInstance(viewObject.ViewControl);
                    }
                }
                else
                {
                    return (UserControl)Activator.CreateInstance(viewObject.ViewControl, viewObject.Opacity, viewObject.Text);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
