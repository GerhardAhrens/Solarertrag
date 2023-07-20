/*
 * <copyright file="SettingsManager.cs" company="Lifeprojects.de">
 *     Class: SettingsManager
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.01.2023 17:37:29</date>
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

namespace Console.ApplicationSettings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using EasyPrototypingNET.Settings;

    public class SettingsManager : LocalSettingsManagerBase
    {
        private const string DATABASE = "Database";
        private const string LASTEDIT = "LastEdit";
        private const string EXITQUESTION = "ExitQuestion";
        private const string LASTEXPORTFILE = "LastExportFile";

        private LocalSettings localSettings = null;
        private string database = string.Empty;
        private DateTime lastEdit = DateTime.Now.DefaultDate();
        private bool exitQuestion = true;
        private string lastExportFile = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager()
        {
            this.localSettings = new LocalSettings(SettingsLocation.ProgramData);
            if (this.localSettings != null)
            {
                this.InitSettings();
            }
        }

        public bool IsExist { get; private set; }

        public string Database
        {
            get { return this.database; }
            set
            {
                this.database = value;
                this.UpdateSettings<string>(DATABASE, this.database);
            }
        }

        public DateTime LastEdit
        {
            get { return this.lastEdit; }
            set
            {
                this.lastEdit = value;
                this.UpdateSettings<DateTime>(LASTEDIT, this.lastEdit);
            }
        }

        public bool ExitQuestion
        {
            get { return this.exitQuestion; }
            set
            {
                this.exitQuestion = value;
                this.UpdateSettings<bool>(EXITQUESTION, this.exitQuestion);
            }
        }

        public string LastExportFile
        {
            get { return this.lastExportFile; }
            set
            {
                this.lastExportFile = value;
                this.UpdateSettings<string>(LASTEXPORTFILE, this.lastExportFile);
            }
        }

        public override void InitSettings()
        {
            if (this.localSettings.IsExitSettings() == false)
            {
                this.IsExist = false;
                this.localSettings.AddOrSet(DATABASE, typeof(string), string.Empty);
                this.localSettings.AddOrSet(LASTEDIT, typeof(DateTime), DateTime.Now.DefaultDate());
                this.localSettings.AddOrSet(EXITQUESTION, typeof(bool), true);
                this.localSettings.AddOrSet(LASTEXPORTFILE, typeof(string), string.Empty);
                this.localSettings.Save();
            }
            else
            {
                this.IsExist = true;
                this.localSettings.Load();
            }

            if (localSettings.Exists(DATABASE) == true)
            {
                if (localSettings[DATABASE] != null)
                {
                    this.Database = localSettings[DATABASE].ToString();
                }
                else
                {
                    this.Database = string.Empty;
                }
            }

            if (localSettings.Exists(LASTEDIT) == true)
            {
                if (localSettings[LASTEDIT] != null)
                {
                    this.LastEdit = localSettings[LASTEDIT].ToDateTime();
                }
                else
                {
                    this.LastEdit = DateTime.Now;
                }
            }

            if (localSettings.Exists(EXITQUESTION) == true)
            {
                if (localSettings[EXITQUESTION] != null)
                {
                    this.ExitQuestion = localSettings[EXITQUESTION].ToBool();
                }
                else
                {
                    this.ExitQuestion = true;
                }
            }

            if (localSettings.Exists(LASTEXPORTFILE) == true)
            {
                if (localSettings[LASTEXPORTFILE] != null)
                {
                    this.LastExportFile = localSettings[LASTEXPORTFILE].ToString();
                }
                else
                {
                    this.LastExportFile = string.Empty;
                }
            }
        }

        public override void UpdateSettings<T>(string key, T value)
        {
            this.localSettings.AddOrSet(key, typeof(T), value);
            this.localSettings.Save();
        }

        protected override void DisposeManagedResources()
        {
            this.localSettings = null;
        }

        protected override void DisposeUnmanagedResources()
        {
        }
    }
}
