using Ecoscan.Client.Models;
using Ecoscan.Client.ParsingServices;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Ninject;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Ecoscan.Client
{
    internal static class Program
    {
        private static StandardKernel _kernel;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintToConsoleHelp();
                return;
            }

            _kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());

            SetupLog4NetConfiguration();
            var model = GetConfigurationModel(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(model, _kernel));
        }

        private static void PrintToConsoleHelp()
        {
            var message = File.ReadAllText("Help.txt");
            Console.WriteLine(message);
        }

        private static ConfigurationModel GetConfigurationModel(string[] args)
        {
            var model = new ConfigurationModel
            {
                ScanColor = false,
                ShowScannerUI = false,
                ScanResolution = 150,
                AutomaticDeskew = false,
                AutomaticBorderDetection = false,
                ShouldTransferAllPages = true,
                MergeMultiImage = true,
                SetDevice = string.Empty,
                GetDevices = false,
                AbortWhenNoPaperDetectable = true,
                SetPageSettings = false,
                SetArea = false,
                TransferCount = -1,
                UseDuplex = false,
                ShowProgressIndicatorUI = true,
                UseDocumentFeeder = true,
                AutomaticRotate = false
            };

            if (args[0].Contains("UseConfigFile"))
                _kernel.Get<IParsingService>("ConfigurationFile").Parse(model, args);
            else
                _kernel.Get<IParsingService>("CommandLine").Parse(model, args);

            if (!model.GetDevices && string.IsNullOrWhiteSpace(model.FileName))
                Environment.Exit(1);

            return model;
        }

        private static void SetupLog4NetConfiguration()
        {
            var appender = GetNewFileApender();
            BasicConfigurator.Configure(appender);
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy == null) return;

            var repo = LogManager.GetRepository();
            hierarchy.Root.Level = repo.LevelMap[ConfigurationManager.AppSettings["DebugLevel"]];
        }

        private static RollingFileAppender GetNewFileApender()
        {
            var appender = new RollingFileAppender
            {
                Name = "Ecoscan.Log",
                File = "Log.txt",
                AppendToFile = true,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "10MB",
                StaticLogFileName = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                DatePattern = "yyyy.MM.dd"
            };

            var layout = new log4net.Layout.PatternLayout("%d %p %m%n");
            appender.Layout = layout;

            layout.ActivateOptions();
            appender.ActivateOptions();
            return appender;
        }
    }
}