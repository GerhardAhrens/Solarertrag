namespace Solarertrag
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    using Console.ApplicationSettings;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Logger;
    using EasyPrototypingNET.Pattern;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";
        private static readonly string MessageBoxTitle = "Solarertrag Application";
        private static readonly string UnexpectedError = "An unexpected error occured.";

        public App()
        {
            string exePath = string.Empty;
            string exeName = string.Empty;
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            WeakEventManager<Application, DispatcherUnhandledExceptionEventArgs>.AddHandler(this, "DispatcherUnhandledException", this.OnDispatcherUnhandledException);

            /*
            SinglePageApplicationWPF.MenuButtons mb = new SinglePageApplicationWPF.MenuButtons(1, "MB1");
            mb.Add(2, "MB2");
            var aa = mb.Count;

            foreach (KeyValuePair<int,string> item in mb.Content)
            {

            }
            */
            try
            {
                string assemblyName = ApplicationProperties.AssemblyName;
                exePath = ApplicationProperties.ProgramDataPath;
                exeName = $"{assemblyName}.exe";

                /*
                string fileName = $"c:\\Temp\\Solarertrag.log";
                TraceLoggerConfiguration traceConfig = new TraceLoggerConfiguration().ForFile(fileName);
                traceConfig.MaxLogFiles = 2;
                TraceLogger logger = new TraceLogger(traceConfig);
                logger.Level(TraceLevel.Off);

                TraceLogger.LogInformation($"AssemblyName: {assemblyName}");
                TraceLogger.LogInformation($"Programmpfad: {exePath}\\{exeName}");
                */

                EventAgg = new EventAggregator();
                InitializeCultures(DEFAULTLANGUAGE);
                RunningOn = DevelopmentTarget.GetPlatform(Assembly.GetEntryAssembly());

                /*
                TraceLogger.LogInformation($"Default Language: {DEFAULTLANGUAGE}");
                TraceLogger.LogInformation($"GetPlatform: {RunningOn}");
                */

                using (SettingsManager sm = new SettingsManager())
                {
                    if (sm.IsExist ==  false)
                    {
                        string databaseFullPath = Path.Combine(exePath, $"{assemblyName}.db");
                        sm.Database = databaseFullPath;
                        DatabasePath = databaseFullPath;
                    }
                    else
                    {
                        DatabasePath = Path.Combine(exePath, sm.Database);
                        ExitQuestion = sm.ExitQuestion;
                    }
                }

                /*
                TraceLogger.LogInformation($"Datenbank: {DatabasePath}");
                TraceLogger.LogInformation($"ExitQuestion: {ExitQuestion}");
                */
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserDomainName", Environment.UserDomainName);
                ex.Data.Add("UserName", Environment.UserName);
                ex.Data.Add("exePath", exePath);
                ErrorMessage(ex, "General Error: ");
                Application.Current.Shutdown(0);
            }
        }

        public static DevelopmentTarget RunningOn { get; private set; }

        public static string DatePattern { get; private set; }

        public static EventAggregator EventAgg { get; private set; }

        public static string DatabasePath { get; private set; }

        public static bool ExitQuestion { get; set; }

        public static void ErrorMessage(Exception ex, string message = "")
        {
            string expMsg = ex.Message;
            var aex = ex as AggregateException;

            if (aex != null && aex.InnerExceptions.Count == 1)
            {
                expMsg = aex.InnerExceptions[0].Message;
            }

            if (string.IsNullOrEmpty(message) == true)
            {
                message = UnexpectedError;
            }

            StringBuilder errorText = new StringBuilder();
            if (ex.Data != null && ex.Data.Count > 0)
            {
                foreach (DictionaryEntry item in ex.Data)
                {
                    errorText.AppendLine($"{item.Key} : {item.Value}");
                }
            }

            MessageBox.Show(
                message + $"{expMsg}\n{ex.Message}\n{errorText.ToString()}",
                MessageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private static void InitializeCultures(string language)
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            DatePattern = $"{DateTimeFormatInfo.CurrentInfo.ShortDatePattern}";

            FrameworkPropertyMetadata frameworkMetadata = new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(new CultureInfo(language).IetfLanguageTag));
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkMetadata);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string app = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            Debug.WriteLine($"{app}-{(e.Exception as Exception).Message}");
            /*TraceLogger.LogError((e.Exception as Exception).Message);*/
        }

        private void ApplicationExit()
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }
}
