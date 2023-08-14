/*
 * <copyright file="ControlContentArgs.cs" company="Lifeprojects.de">
 *     Class: ControlContentArgs
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>09.08.2023 19:28:32</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace Solarertrag.Core
{
    using System;

    using SinglePageApplicationWPF.Core;

    public class ControlContentArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlContentArgs"/> class.
        /// </summary>
        public ControlContentArgs()
        {
        }

        public CommandButtons TargetPage { get; set; }

        public RowItemPosition RowPosition { get; set; }

        public Guid EntityId { get; set; }
    }
}
