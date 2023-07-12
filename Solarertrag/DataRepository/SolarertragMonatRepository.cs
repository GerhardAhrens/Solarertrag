//-----------------------------------------------------------------------
// <copyright file="EffortProjectRepository.cs" company="Lifeprojects.de">
//     Class: EffortProjectRepository
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>06.07.2022 14:33:35</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace PertNET.DataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiteDB;

    using Solarertrag.BaseClass;
    using Solarertrag.Model;

    public class SolarertragMonatRepository : RepositoryBase<SolarertragMonat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffortProjectRepository"/> class.
        /// </summary>
        public SolarertragMonatRepository(string databaseFile) : base(databaseFile)
        {
            if (string.IsNullOrEmpty(databaseFile) == false)
            {
                this.Database = base.DatabaseIntern;
                this.Collection = base.CollectionIntern;
            }
        }

        public LiteDatabase Database { get; private set; }

        public ILiteCollection<SolarertragMonat> Collection { get; private set; }

        public override int Count()
        {
            return base.Count();
        }

        public override SolarertragMonat ListById(Guid id)
        {
            SolarertragMonat result = null;

            if (id != Guid.Empty)
            {
                result = base.ListById(id);
            }

            return result;
        }

        public override void Add(SolarertragMonat entity)
        {
            try
            {
                ILiteCollection<SolarertragMonat> entityCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));

                if (entityCollection != null)
                {
                    BsonValue resultEntity = entityCollection.Insert(entity);
                }
            }
            catch (Exception ex)
            {
                string errotrText = ex.Message;
                throw;
            }
        }

        public override bool Update(SolarertragMonat entity)
        {
            try
            {
                ILiteCollection<SolarertragMonat> entityCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));

                if (entityCollection != null)
                {
                    BsonValue resultEntity = entityCollection.Update(entity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string errotrText = ex.Message;
                throw;
            }

            return false;
        }

        public override bool Delete(Guid id)
        {
            try
            {
                ILiteCollection<SolarertragMonat> entityCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));
                if (id != Guid.Empty)
                {
                    bool result = entityCollection.Delete(id);

                    return result;
                }

            }
            catch (Exception ex)
            {
                string errotrText = ex.Message;
                throw;
            }

            return false;
        }

        public bool ExistEntry(int chapterA, int chapterB, int chapterC)
        {
            bool result = false;

            try
            {
                ILiteCollection<SolarertragMonat> entityCollection = this.DatabaseIntern.GetCollection<SolarertragMonat>(nameof(SolarertragMonat));
                //result = entityCollection.Find(f => f.ChapterA == chapterA && f.ChapterB == chapterB && f.ChapterC == chapterC).Any();
            }
            catch (Exception ex)
            {
                string errotrText = ex.Message;
                throw;
            }

            return result;
        }
    }
}
