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
        public void RegisterXYZ_Test()
        {
        }

        [TestMethod]
        public void SetGoFirstPosition()
        {
            RowItemPosition first = RowItemPosition.GoFirst;
            Assert.AreEqual(first, RowItemPosition.GoFirst);
            Assert.AreEqual(first.Key, 1);

            int pos = first.GoTo;
            Assert.AreEqual(pos, 0);
        }

        [TestMethod]
        public void SetGoAnyPosition()
        {
            RowItemPosition moveToPos = RowItemPosition.GoMove;
            Assert.AreEqual(moveToPos, RowItemPosition.GoMove);
            Assert.AreEqual(moveToPos.Key, 3);

            int pos = moveToPos.GoTo;
            Assert.AreEqual(pos, 0);

            moveToPos.GoTo = 99;
            int posNew = moveToPos.GoTo;
            Assert.AreEqual(posNew,99);

            RowItemPosition first = RowItemPosition.GoFirst;
            Assert.AreEqual(first.GoTo, 0);
        }

        [TestMethod]
        public void NewPosition()
        {
            RowItemPosition newPos = RowItemPosition.GoNew;
            newPos.EntityId = new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}");

            Assert.AreEqual(newPos, RowItemPosition.GoNew);
            Assert.AreEqual(newPos.EntityId, new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}"));
            Assert.AreEqual(newPos.RowStatus, RowStatus.New);
        }

        [TestMethod]
        public void NewPositionForArgs()
        {
            ControlContentArgs args = new ControlContentArgs();
            args.TargetPage = CommandButtons.MainDetail;
            args.RowPosition = RowItemPosition.GoNew;
            args.RowPosition.EntityId = new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}");

            ControlContentArgs testArgs(ControlContentArgs args)
            {
                return args;
            }

            ControlContentArgs result = testArgs(args);

            Assert.AreEqual(result.TargetPage, CommandButtons.MainDetail);
            Assert.AreEqual(result.RowPosition, RowItemPosition.GoNew);
            Assert.AreEqual(result.RowPosition.EntityId, new Guid("{583274EC-B6D6-497D-991B-708654EEEA94}"));
            Assert.AreEqual(result.RowPosition.RowStatus, RowStatus.New);
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
