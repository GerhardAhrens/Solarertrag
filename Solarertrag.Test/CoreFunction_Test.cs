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

    using SinglePageApplicationWPF.Base;

    [TestClass]
    public class CoreFunction_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreFunction_Test"/> class.
        /// </summary>
        public CoreFunction_Test()
        {
        }

        [TestMethod]
        public void CreateNoneButton()
        {
            CommandButton noneButton = CommandButton.None;

            Assert.AreEqual(noneButton, CommandButton.None);
            Assert.AreEqual(noneButton.Key, 0);
        }

        [TestMethod]
        public void GetAll()
        {
            CommandButton noneButton = CommandButton.None;

            var list = CommandButton.GetAll<CommandButton>();
            Assert.AreEqual(list.Count(), 2);
        }

        [TestMethod]
        public void ParseCommandButton()
        {
            CommandButton outVar;
            var result = CommandButton.TryGetFromValueOrName("None", out outVar);
            Assert.AreEqual(typeof(CommandButton), outVar.GetType().BaseType);
        }

        [TestMethod]
        public void GetDifference_IsEquals()
        {
            CommandButton noneButton1 = CommandButton.None;
            CommandButton noneButton2 = CommandButton.None;
            Assert.AreEqual(noneButton1, noneButton2);
            Assert.AreEqual(CommandButton.GetDifference(noneButton1,noneButton2),0);
        }

        [TestMethod]
        public void GetDifference_IsNotEquals()
        {
            CommandButton noneButton = CommandButton.None;
            CommandButton overviewButton = CommandButton.Overview;
            Assert.AreNotEqual(noneButton, overviewButton);
            Assert.AreEqual(CommandButton.GetDifference(noneButton, overviewButton), 1);
        }

        [TestMethod]
        public void GetFromValue()
        {
            CommandButton result = CommandButton.FromValue<CommandButton>(0);
            Assert.AreEqual(result, CommandButton.None);
        }

        [TestMethod]
        public void FromNameCaseSensitiv()
        {
            CommandButton result = CommandButton.FromName<CommandButton>("None");
            Assert.AreEqual(result, CommandButton.None);
        }

        [TestMethod]
        public void FromNameUpper()
        {
            CommandButton result = CommandButton.FromName<CommandButton>("NONE");
            Assert.AreEqual(result, CommandButton.None);
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

    public abstract class CommandButton : EnumBase
    {
        public static readonly CommandButton None = new NoneButton();
        public static readonly CommandButton Overview = new OverviewButton();

        private CommandButton(int value, string name = null, string description = null) : base(value, name, description)
        {
        }

        public abstract string Code { get; }

        private class NoneButton : CommandButton
        {
            public NoneButton() : base(0, "None")
            {
            }

            public override string Code => "NO";
        }

        private class OverviewButton : CommandButton
        {
            public OverviewButton() : base(1, "Overview","Übersicht")
            {
            }

            public override string Code => "OV";
        }
    }
}
