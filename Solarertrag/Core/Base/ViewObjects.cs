//-----------------------------------------------------------------------
// <copyright file="ViewObjects.cs" company="www.pta.de">
//     Class: ViewObjects
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>03.08.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class ViewObjects : Dictionary<int, Tuple<string,string,Type>>, IDisposable
    {
        private bool classIsDisposed = false;
        private ConcurrentDictionary<int, Tuple<string, string, Type>> viewObjectControls;

        public ViewObjects()
        {
            if (this.viewObjectControls == null)
            {
                this.viewObjectControls = new ConcurrentDictionary<int, Tuple<string, string, Type>>();
            }
        }

        public ViewObjects(int key, string value, string description, Type type)
        {
            if (this.viewObjectControls == null)
            {
                this.viewObjectControls = new ConcurrentDictionary<int, Tuple<string, string, Type>>();
            }

            Tuple<string, string, Type> valueList = new Tuple<string, string, Type>(value, description, type);

            this.viewObjectControls.AddOrUpdate(key, valueList, (oldkey, oldvalue) => valueList);
        }

        ~ViewObjects()
        {
            this.Dispose(false);
        }

        new public int Count { get { return this.viewObjectControls.Count; } }

        public ConcurrentDictionary<int, Tuple<string, string, Type>> Views { get { return this.viewObjectControls; } }

        public Dictionary<int, Tuple<string, string, Type>> Content { get {return this.viewObjectControls.ToDictionary(entry => entry.Key, entry => entry.Value);}}

        public void Add(int key, string value, string description = null, Type type = default)
        {
            if (this.viewObjectControls == null)
            {
                this.viewObjectControls = new ConcurrentDictionary<int, Tuple<string, string, Type>>();
            }

            Tuple<string, string, Type> valueList = new Tuple<string, string, Type>( value, description, type );

            this.viewObjectControls.AddOrUpdate(key, valueList, (oldkey, oldvalue) => valueList);
        }

        public Tuple<string, string, Type> Get(int key)
        {
            Tuple<string, string, Type> result = new Tuple<string, string, Type>( string.Empty,string.Empty,null);
            if (this.viewObjectControls.ContainsKey(key) == true)
            {
                result = this.viewObjectControls[key];
            }

            return result;
        }

        public string GetValue(int key)
        {
            string result = string.Empty;
            if (this.viewObjectControls.ContainsKey(key) == true)
            {
                result = this.viewObjectControls[key].Item1;
            }

            return result;
        }

        public string GetDescription(int key)
        {
            string result = string.Empty;
            if (this.viewObjectControls.ContainsKey(key) == true)
            {
                result = this.viewObjectControls[key].Item2;
            }

            return result;
        }

        public Type GetType(int key)
        {
            Type result = default;
            if (this.viewObjectControls.ContainsKey(key) == true)
            {
                result = this.viewObjectControls[key].Item3;
            }

            return result;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (classIsDisposed == false)
            {
                if (classDisposing)
                {
                    /*
                    Managed Objekte freigeben. Wenn diese Obbjekte selbst
                    IDisposable implementieren, dann deren Dispose()
                    aufrufen 
                    */
                    this.viewObjectControls = null;
                }

                /*
                 * Hier unmanaged Objekte freigeben (z.B. IntPtr)
                */
            }

            classIsDisposed = true;
        }
        #endregion Dispose
    }
}
