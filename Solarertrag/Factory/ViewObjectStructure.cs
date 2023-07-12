//-----------------------------------------------------------------------
// <copyright file="ViewObjectStructure.cs" company="Lifeprojects.de">
//     Class: ViewObjectStructure
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>13.01.2021</date>
//
// <summary>
// Verwaltung der Menüdefinitionen in einem ObservableDictionary
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Collections.Generic;

    using EasyPrototypingNET.Core.Collection;

    using Solarertrag.Core;

    public sealed class ViewObjectStructure : IViewObjectStructure
    {
        private static ObservableDictionary<MenuButtons, ViewObjectContent> viewObjects = null;

        public ViewObjectStructure()
        {
            if (viewObjects == null)
            {
                viewObjects = new ObservableDictionary<MenuButtons, ViewObjectContent>();
            }
        }

        public ViewObjectStructure(ViewObjectContent menueContent) : this()
        {
            if (viewObjects == null)
            {
                viewObjects = new ObservableDictionary<MenuButtons, ViewObjectContent>();
            }

            viewObjects.Add(menueContent.MenueObject, menueContent);
        }

        public int Count
        {
            get
            {
                return viewObjects.Count;
            }
        }

        public Dictionary<MenuButtons, ViewObjectContent>.KeyCollection Keys
        {
            get
            {
                return viewObjects.Keys;
            }
        }

        public ViewObjectContent this[string key]
        {
            get
            {
                if (key.EndsWith("Command", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    key = key.Replace("Command", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                }

                MenuButtons mobject = key.ToEnum<MenuButtons>();
                if (viewObjects.ContainsKey(mobject) == true)
                {
                    return viewObjects[mobject];
                }
                else
                {
                    return null;
                }
            }
        }

        public ViewObjectContent this[MenuButtons menueObject]
        {
            get
            {
                if (viewObjects.ContainsKey(menueObject) == true)
                {
                    return viewObjects[menueObject];
                }
                else
                {
                    return null;
                }
            }
        }

        public static string Command(MenuButtons menueObject)
        {
            string result = string.Empty;

            result = viewObjects[menueObject].Command;

            return result;
        }

        public static string Text(MenuButtons menueObject)
        {
            string result = string.Empty;

            result = viewObjects[menueObject].Text;

            return result;
        }

        public bool ContainsKey(string key)
        {
            bool result = false;

            if (key.EndsWith("Command", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                key = key.Replace("Command", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            }

            MenuButtons mobject = key.ToEnum<MenuButtons>();
            if (viewObjects.ContainsKey(mobject) == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool ContainsKey(MenuButtons menueObject)
        {
            bool result = false;

            if (viewObjects.ContainsKey(menueObject) == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public void Add(ViewObjectContent menueContent)
        {
            if (viewObjects.ContainsKey(menueContent.MenueObject) == false)
            {
                viewObjects.Add(menueContent.MenueObject, menueContent);
            }
            else
            {
                throw new Exception($"Der Key '{menueContent.MenueObject}' ist bereits in der Auflistung der ViewObjectContent vorhanden!");
            }
        }
    }
}
