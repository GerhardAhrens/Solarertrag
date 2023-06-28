
//-----------------------------------------------------------------------
// <copyright file="DatabaseManager.cs" company="Lifeprojects.de">
//     Class: DatabaseManager
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>01.07.2022</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.DataRepository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Pattern;

    using LiteDB;

    using Solarertrag.Model;

    [SupportedOSPlatform("windows")]
    public sealed class DatabaseManager : DisposableBase
    {
        public DatabaseManager(string databaseFile)
        {
            this.DatabaseFile = databaseFile;
        }

        private string DatabaseFile { get; set; }

        private ConnectionString ConnectionDB { get; set; }

        private LiteDatabase DatabaseIntern { get; set; }

        private ILiteCollection<Note> NoteCollection { get; set; }

        private ILiteCollection<SolarertragMonat> SolarertragMonatCollection { get; set; }

        private ILiteCollection<WorkUserInfo> WorkUserInfoCollection { get; set; }

        private ILiteCollection<DatabaseInfo> DatabaseInfoCollection { get; set; }

        public Result<bool> ExistDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    FileInfo fileInfoe = new FileInfo(this.DatabaseFile);
                    if (fileInfoe.Exists == true)
                    {
                        result = true;
                    }

                    ticks = objectRuntime.ResultMilliseconds();
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> CreateNewDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    this.ConnectionDB = this.Connection(this.DatabaseFile);

                    this.DatabaseIntern = new LiteDatabase(this.ConnectionDB);
                    this.DatabaseIntern.UserVersion = 3;
                    if (this.DatabaseIntern != null)
                    {
                        this.SolarertragMonatCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));
                        this.NoteCollection = this.DatabaseIntern.GetCollection<Note>(nameof(Note));
                        this.WorkUserInfoCollection = this.DatabaseIntern.GetCollection<WorkUserInfo>(nameof(WorkUserInfo));
                        this.DatabaseInfoCollection = this.DatabaseIntern.GetCollection<DatabaseInfo>(nameof(DatabaseInfo));

                        WorkUserInfo workUserInfo = new WorkUserInfo();
                        workUserInfo.Id = Guid.NewGuid();
                        workUserInfo.UserId = UserInfo.TS().CurrentDomainUser;
                        this.WorkUserInfoCollection.Insert(workUserInfo);

                        DatabaseInfo databaseInfo = new DatabaseInfo();
                        databaseInfo.Id = Guid.NewGuid();
                        databaseInfo.Version = 1;
                        this.DatabaseInfoCollection.Insert(databaseInfo);
                    }

                    ticks = objectRuntime.ResultMilliseconds();
                }

                result = true;
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> OpenDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    this.ConnectionDB = this.Connection(this.DatabaseFile);

                    this.DatabaseIntern = new LiteDatabase(this.ConnectionDB);
                    if (this.DatabaseIntern != null)
                    {
                        this.SolarertragMonatCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));
                        this.NoteCollection = this.DatabaseIntern.GetCollection<Note>(nameof(Note));
                        this.WorkUserInfoCollection = this.DatabaseIntern.GetCollection<WorkUserInfo>(nameof(WorkUserInfo));
                        this.DatabaseInfoCollection = this.DatabaseIntern.GetCollection<DatabaseInfo>(nameof(DatabaseInfo));

                        IEnumerable<SolarertragMonat> effortProjectInfo = this.SolarertragMonatCollection.FindAll().ToList();
                        IEnumerable<WorkUserInfo> workUserInfo = this.WorkUserInfoCollection.FindAll().ToList();
                    }

                    ticks = objectRuntime.ResultMilliseconds();
                }

                result = true;
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> Close()
        {
            bool result = false;

            try
            {
                Result<bool> existResult = this.ExistDatabase();
                if (existResult != null && existResult.Success == true)
                {
                    this.SolarertragMonatCollection = null;
                    this.NoteCollection = null;
                    this.WorkUserInfoCollection = null;
                    this.DatabaseInfoCollection = null;
                    this.DatabaseIntern = null;
                }

                result = true;

                Result<bool>.SuccessResult(result, true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, false);
        }

        public Result<bool> Delete(string databaseFile)
        {
            bool result = false;
            try
            {
                result = true;

                Result<bool>.SuccessResult(result, true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, false);
        }

        protected override void DisposeManagedResources()
        {
        }

        protected override void DisposeUnmanagedResources()
        {
        }

        private ConnectionString Connection(string databaseFile)
        {
            ConnectionString conn = new ConnectionString(databaseFile);
            conn.Connection = ConnectionType.Shared;

            return conn;
        }
    }
}
