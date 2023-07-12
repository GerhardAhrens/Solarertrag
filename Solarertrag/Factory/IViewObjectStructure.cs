//-----------------------------------------------------------------------
// <copyright file="IViewObjectStructure.cs" company="Lifeprojects.de">
//     Class: IViewObjectStructure
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>13.01.2021</date>
//
// <summary>
// Interface zur Klasse ViewObjectStructure
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using EasyPrototyping.Core;

    using Solarertrag.Core;

    public interface IViewObjectStructure
    {
        ViewObjectContent this[string key] { get; }

        ViewObjectContent this[MenuButtons menueObject] { get; }
    }
}
