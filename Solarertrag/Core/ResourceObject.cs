//-----------------------------------------------------------------------
// <copyright file="ResourceObject.cs" company="Lifeprojects.de">
//     Class: ResourceObject
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.07.2023 18:35:49</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Linq;
    using System.Windows;

    public static class ResourceObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceObject"/> class.
        /// </summary>
        static ResourceObject()
        {
        }

        public static TResult GetAs<TResult>(string objectKey)
        {
            object result = null;

            if (Application.Current.Resources.Contains(objectKey) == true)
            {
                object element = Application.Current.Resources[objectKey];
                result = element == null ? default(TResult) : (TResult)Convert.ChangeType(element, typeof(TResult));

            }

            return (TResult)result;

        }
    }
}
