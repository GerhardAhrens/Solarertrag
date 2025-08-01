//-----------------------------------------------------------------------
// <copyright file="ZaehlerstandMonatRepository.cs" company="Lifeprojects.de">
//     Class: ZaehlerstandMonatRepository
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

    public class ZaehlerstandMonatRepository : RepositoryBase<ZaehlerstandMonat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZaehlerstandMonatRepository"/> class.
        /// </summary>
        public ZaehlerstandMonatRepository(string databaseFile) : base(databaseFile)
        {
            if (string.IsNullOrEmpty(databaseFile) == false)
            {
                this.Database = base.DatabaseIntern;
                this.Collection = base.CollectionIntern;
            }
        }

        public LiteDatabase Database { get; private set; }

        public ILiteCollection<ZaehlerstandMonat> Collection { get; private set; }

        public override int Count()
        {
            return base.Count();
        }

        public override ZaehlerstandMonat ListById(Guid id)
        {
            ZaehlerstandMonat result = null;

            if (id != Guid.Empty)
            {
                result = base.ListById(id);
            }

            return result;
        }

        public override void Add(ZaehlerstandMonat entity)
        {
            try
            {
                ILiteCollection<ZaehlerstandMonat> entityCollection = this.DatabaseIntern.GetCollection<ZaehlerstandMonat>(nameof(ZaehlerstandMonat));

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

        public override bool Update(ZaehlerstandMonat entity)
        {
            try
            {
                ILiteCollection<ZaehlerstandMonat> entityCollection = this.DatabaseIntern.GetCollection<ZaehlerstandMonat>(nameof(ZaehlerstandMonat));

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
                ILiteCollection<ZaehlerstandMonat> entityCollection = this.DatabaseIntern.GetCollection<ZaehlerstandMonat>(nameof(ZaehlerstandMonat));
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
    }
}
