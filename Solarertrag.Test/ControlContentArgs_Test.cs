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

    using SinglePageApplicationWPF.Core;

    using Solarertrag.Core;

    [TestClass]
    public class ControlContentArgs_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlContentArgs_Test"/> class.
        /// </summary>
        public ControlContentArgs_Test()
        {
        }

        [TestMethod]
        public void Create_ControlContentArgsToGoFirst()
        {
            ControlContentArgs args = new ControlContentArgs();
            args.TargetPage = CommandButtons.MainOverview;
            args.RowPosition = RowItemPosition.GoFirst;
            args.EntityId = new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}");
        }

        [TestMethod]
        public void Create_ControlContentArgsToAnyPosition()
        {
            ControlContentArgs args = new ControlContentArgs();
            args.TargetPage = CommandButtons.MainOverview;
            args.RowPosition = RowItemPosition.GoMove;
            args.RowPosition.GoTo = 99;
            args.EntityId = new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}");

            ControlContentArgs testArgs(ControlContentArgs args)
            {
                return args;
            }

            ControlContentArgs result = testArgs(args);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TargetPage, CommandButtons.MainOverview);
            Assert.AreEqual(result.RowPosition.GoTo, 99);
            Assert.AreEqual(result.EntityId, new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}"));
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
