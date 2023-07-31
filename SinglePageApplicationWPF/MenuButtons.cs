//-----------------------------------------------------------------------
// <copyright file="MenuButtons.cs" company="www.pta.de">
//     Class: MenuButtons
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>31.07.2023 09:30:29</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace SinglePageApplicationWPF
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class MenuButtons : Dictionary<int,string>, IDisposable
    {
        private bool classIsDisposed = false;
        private ConcurrentDictionary<int, string> menuButton;

        public MenuButtons()
        {
            if (menuButton == null)
            {
                this.menuButton = new ConcurrentDictionary<int, string>();
            }
        }

        public MenuButtons(int key, string value)
        {
            if (menuButton == null)
            {
                this.menuButton = new ConcurrentDictionary<int, string>();
            }

            this.menuButton.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }

        new public int Count { get { return menuButton.Count; } }

        public Dictionary<int, string> Content { get {return   menuButton.ToDictionary(entry => entry.Key, entry => entry.Value);}}

        new public void Add(int key, string value)
        {
            if (menuButton == null)
            {
                this.menuButton = new ConcurrentDictionary<int, string>();
            }

            this.menuButton.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }

        public string GetFromKey(int key)
        {
            string result = string.Empty;
            if (this.menuButton.ContainsKey(key) == true)
            {
                result = this.menuButton[key];
            }

            return result;
        }

        ~MenuButtons()
        {
            Dispose(false);
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
                    this.menuButton = null;
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
