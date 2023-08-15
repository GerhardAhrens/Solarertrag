/*
 * <copyright file="LoadContentArgs.cs" company="Lifeprojects.de">
 *     Class: LoadContentArgs
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 // <date>15.08.2023 13:34:00</date>
 * <Project>Solarertrag</Project>
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

    public class LoadContentArgs
    {
        public static ControlContentArgs MainOverview(int rowPos = 0)
        {
            ControlContentArgs overviewArgs = new ControlContentArgs();
            overviewArgs.TargetPage = CommandButtons.MainOverview;
            if (rowPos > 0)
            {
                overviewArgs.RowPosition = RowItemPosition.GoMove;
                overviewArgs.RowPosition.GoTo = rowPos;
            }
            else
            {
                overviewArgs.RowPosition = RowItemPosition.GoFirst;
            }

            overviewArgs.EntityId = Guid.Empty;
            return overviewArgs;
        }

        public static ControlContentArgs MainDetail(Guid id, int rowPos = 0)
        {
            ControlContentArgs overviewArgs = new ControlContentArgs();
            overviewArgs.TargetPage = CommandButtons.MainDetail;
            if (rowPos > 0)
            {
                overviewArgs.RowPosition = RowItemPosition.GoMove;
                overviewArgs.RowPosition.GoTo = rowPos;
            }
            else
            {
                overviewArgs.RowPosition = RowItemPosition.GoFirst;
            }

            overviewArgs.EntityId = id;
            return overviewArgs;
        }

        public static ControlContentArgs MainHome()
        {
            ControlContentArgs overviewArgs = new ControlContentArgs();
            overviewArgs.TargetPage = CommandButtons.Home;
            overviewArgs.RowPosition = RowItemPosition.None;
            overviewArgs.EntityId = Guid.Empty;
            return overviewArgs;
        }

        public static ControlContentArgs MainSettings()
        {
            ControlContentArgs overviewArgs = new ControlContentArgs();
            overviewArgs.TargetPage = CommandButtons.Settings;
            overviewArgs.RowPosition = RowItemPosition.None;
            overviewArgs.EntityId = Guid.Empty;
            return overviewArgs;
        }

        public static ControlContentArgs MainExcel()
        {
            ControlContentArgs overviewArgs = new ControlContentArgs();
            overviewArgs.TargetPage = CommandButtons.ExcelExport;
            overviewArgs.RowPosition = RowItemPosition.None;
            overviewArgs.EntityId = Guid.Empty;
            return overviewArgs;
        }
    }
}
