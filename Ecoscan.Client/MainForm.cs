using Ecoscan.Client.ExportsServices;
using Ecoscan.Client.Models;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TwainDotNet;

namespace Ecoscan.Client
{
    public partial class MainForm : Form
    {
        private readonly IEnumerable<string> _multiImageFileTypesCollection = new List<string> { ".tiff", ".tif", ".pdf" };

        private readonly Dictionary<string, ImageFormat> _imageFormatsByExtensionsDictionary = new Dictionary<string, ImageFormat>
        {
            {".tiff", ImageFormat.Tiff}, {".tif", ImageFormat.Tiff}, {".pdf", null},
            {".png", ImageFormat.Png}, {".jpeg", ImageFormat.Jpeg}, {".jpg", ImageFormat.Jpeg},
            {".gif", ImageFormat.Gif}, {".bmp", ImageFormat.Bmp}
        };

        private readonly StandardKernel _kernel;
        private readonly ColourSetting _colourSettings;
        private readonly Twain _twain;
        private readonly ScanSettings _settings;
        private string _filename;

        private readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        private readonly ConfigurationModel _model;

        public MainForm(ConfigurationModel model, StandardKernel kernel)
        {
            InitializeComponent();

            _kernel = kernel;
            _logger.Debug("MainForm started.");

            _model = model;

            _twain = new Twain(new WinFormsWindowMessageHook(this));
            if (model.GetDevices)
            {
                Console.WriteLine(string.Join(Environment.NewLine, _twain.SourceNames));
                Application.Exit();
            }
            if (!string.IsNullOrWhiteSpace(model.SetDevice))
                _twain.SelectSource(model.SetDevice);

            _filename = _model.FileName;
            ValidateFilename();

            _twain.TransferImages += TransferImagesCallback;

            _colourSettings = _model.ScanColor ? ColourSetting.Colour : ColourSetting.BlackAndWhite;

            _settings = new ScanSettings
            {
                AbortWhenNoPaperDetectable = _model.AbortWhenNoPaperDetectable,
                UseDocumentFeeder = _model.UseDocumentFeeder,
                ShowTwainUI = _model.ShowScannerUI,
                ShowProgressIndicatorUI = _model.ShowProgressIndicatorUI,
                UseDuplex = _model.UseDuplex,
                Resolution = new ResolutionSettings
                {
                    ColourSetting = _colourSettings,
                    Dpi = _model.ScanResolution
                },
                ShouldTransferAllPages = _model.ShouldTransferAllPages,
                Rotation = new RotationSettings
                {
                    AutomaticDeskew = _model.AutomaticDeskew,
                    AutomaticBorderDetection = _model.AutomaticBorderDetection,
                    FlipSideRotation = _model.FlipSideRotation,
                    AutomaticRotate = _model.AutomaticRotate
                },
                TransferCount = _model.TransferCount,
                UseAutoFeeder = _model.UseAutoFeeder,
                UseAutoScanCache = _model.UseAutoScanCache
            };
            if (model.SetArea)
            {
                _settings.Area = new AreaSettings(_model.Units, _model.Top, _model.Left, _model.Bottom, _model.Right);
            }
            if (model.SetPageSettings)
            {
                _settings.Page = new PageSettings
                {
                    Orientation = _model.Orientation,
                    Size = _model.PageTypeSize
                };
            }
        }

        private void ValidateFilename()
        {
            _logger.Debug("ValidateFilename started.");
            var extension = Path.GetExtension(_filename);
            if (string.IsNullOrWhiteSpace(extension))
            {
                _logger.Error("Validating file name failed: no file extension found.");
                Environment.Exit(2);
            }
            if (!CheckAccessToFolder(_filename))
            {
                _logger.Error("Validating file name failed: unable to write to output folder.");
                Environment.Exit(3);
            }
        }

        private bool CheckAccessToFolder(string filename)
        {
            _logger.Debug("CheckAccessToFolder started.");
            try
            {
                var folder = Path.GetDirectoryName(filename);
                if (string.IsNullOrWhiteSpace(folder))
                    return false;
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var outputFile = folder + "\\temp.txt";
                File.AppendAllText(outputFile, @"dummy test");
                File.Delete(outputFile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void TransferImagesCallback(object sender, TransferImagesEventArgs args)
        {
            _logger.Debug("TransferImagesCallback started.");
            if (args.Images != null)
            {
                var extension = GetFileExtension(args.Images.Count);
                _logger.Debug("TransferImagesCallback. GetFileExtension returned: " + extension);

                var export = _kernel.Get<IExportService>(extension);
                _logger.Debug("TransferImagesCallback. Using export service: " + export.GetType().Name);
                export.Initialize(_filename, _imageFormatsByExtensionsDictionary[extension]);
                _logger.Debug("TransferImagesCallback. ColourSetting: " + _colourSettings);

                if (_colourSettings == ColourSetting.BlackAndWhite)
                {
                    export.SaveBlackWhite(args.Images);
                }
                if (_colourSettings == ColourSetting.Colour)
                {
                    export.SaveColor(args.Images);
                }
                _logger.Debug("TransferImagesCallback Finished saving images.");
            }
            Close();
        }

        private string GetFileExtension(int imageCount)
        {
            _logger.Debug("GetFileExtension started.");
            var extension = ".tif";
            var userExtension = Path.GetExtension(_filename);
            if (!string.IsNullOrWhiteSpace(userExtension))
            {
                extension = userExtension.ToLower();

                if (_model.MergeMultiImage)
                {
                    if (imageCount > 1 && !_multiImageFileTypesCollection.Contains(extension))
                    {
                        extension = ".tif";
                        _filename = Path.ChangeExtension(_filename, ".tif");
                    }
                }
            }
            return extension;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _logger.Debug("OnClosing started.");
            _twain.TransferImages -= TransferImagesCallback;
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            _logger.Debug("OnLoad started.");
            try
            {
                _twain.StartScanning(_settings);
            }
            catch (FeederEmptyException ex)
            {
                _twain.TransferImages -= TransferImagesCallback;

                _logger.Error("MainForm.OnLoad failed: No paper found on scanner feeder. ", ex);
                Environment.ExitCode = 4;
                Application.Exit();
            }
            catch (TwainException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}