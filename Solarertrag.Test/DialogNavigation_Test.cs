//-----------------------------------------------------------------------
// <copyright file="CoreFunction_Test.cs" company="www.pta.de">
//     Class: CoreFunction_Test
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>01.08.2023 09:29:34</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Test
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SinglePageApplicationWPF;

    using Solarertrag.Core;

    [TestClass]
    public class DialogNavigation_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogNavigation_Test"/> class.
        /// </summary>
        public DialogNavigation_Test()
        {
        }

        [TestMethod]
        public void RegisterButton_Test()
        {
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
