using Ecoscan.Client.Models;
using System;
using System.Configuration;
using System.Linq;
using TwainDotNet.TwainNative;

namespace Ecoscan.Client.ParsingServices
{
    public class ConfigurationParsingService : IParsingService
    {
        public void Parse(ConfigurationModel model, string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.AllKeys.Contains("AreaSize"))
            {
                model.SetArea = true;
                var areaDims = appSettings["AreaSize"];
                var dims = areaDims.Split(',');
                model.Left = float.Parse(dims[0]);
                model.Top = float.Parse(dims[1]);
                model.Right = float.Parse(dims[2]);
                model.Bottom = float.Parse(dims[3]);
            }
            if (appSettings.AllKeys.Contains("Units"))
            {
                Units units;
                if (Enum.TryParse(appSettings["Units"], out units))
                {
                    model.SetArea = true;
                    model.Units = units;
                }
            }
            if (appSettings.AllKeys.Contains("Orientation"))
            {
                Orientation orientation;
                if (Enum.TryParse(appSettings["Orientation"], out orientation))
                {
                    model.SetPageSettings = true;
                    model.Orientation = orientation;
                }
            }
            if (appSettings.AllKeys.Contains("PageTypeSize"))
            {
                PageType pageTypeSize;
                if (Enum.TryParse(appSettings["PageTypeSize"], out pageTypeSize))
                {
                    model.SetPageSettings = true;
                    model.PageTypeSize = pageTypeSize;
                }
            }
            if (appSettings.AllKeys.Contains("UseAutoFeeder"))
            {
                bool useAutoFeeder;
                if (bool.TryParse(appSettings["UseAutoFeeder"], out useAutoFeeder))
                    model.UseAutoFeeder = useAutoFeeder;
            }
            if (appSettings.AllKeys.Contains("UseAutoScanCache"))
            {
                bool useAutoScanCache;
                if (bool.TryParse(appSettings["UseAutoScanCache"], out useAutoScanCache))
                    model.UseAutoScanCache = useAutoScanCache;
            }
            if (appSettings.AllKeys.Contains("FlipSideRotation"))
            {
                FlipRotation flipSideRotation;
                if (Enum.TryParse(appSettings["FlipSideRotation"], out flipSideRotation))
                    model.FlipSideRotation = flipSideRotation;
            }
            if (appSettings.AllKeys.Contains("TransferCount"))
            {
                short transferCount;
                if (short.TryParse(appSettings["TransferCount"], out transferCount))
                    model.TransferCount = transferCount;
            }
            if (appSettings.AllKeys.Contains("AutomaticRotate"))
            {
                bool automaticRotate;
                if (bool.TryParse(appSettings["AutomaticRotate"], out automaticRotate))
                    model.AutomaticRotate = automaticRotate;
            }
            if (appSettings.AllKeys.Contains("AbortWhenNoPaperDetectable"))
            {
                bool abortWhenNoPaperDetectable;
                if (bool.TryParse(appSettings["AbortWhenNoPaperDetectable"], out abortWhenNoPaperDetectable))
                    model.AbortWhenNoPaperDetectable = abortWhenNoPaperDetectable;
            }
            if (appSettings.AllKeys.Contains("UseDocumentFeeder"))
            {
                bool useDocumentFeeder;
                if (bool.TryParse(appSettings["UseDocumentFeeder"], out useDocumentFeeder))
                    model.UseDocumentFeeder = useDocumentFeeder;
            }
            if (appSettings.AllKeys.Contains("ShowProgressIndicatorUI"))
            {
                bool showProgressIndicatorUi;
                if (bool.TryParse(appSettings["ShowProgressIndicatorUI"], out showProgressIndicatorUi))
                    model.ShowProgressIndicatorUI = showProgressIndicatorUi;
            }
            if (appSettings.AllKeys.Contains("UseDuplex"))
            {
                bool useDuplex;
                if (bool.TryParse(appSettings["UseDuplex"], out useDuplex))
                    model.UseDuplex = useDuplex;
            }
            if (appSettings.AllKeys.Contains("FileName"))
                model.FileName = appSettings["FileName"];

            if (appSettings.AllKeys.Contains("SetDevice"))
            {
                model.SetDevice = appSettings["SetDevice"];
            }
            if (appSettings.AllKeys.Contains("GetDevices"))
            {
                bool getDevices;
                if (bool.TryParse(appSettings["GetDevices"], out getDevices))
                    model.GetDevices = getDevices;
            }
            if (appSettings.AllKeys.Contains("MergeMultiImage"))
            {
                bool mergeMultiImage;
                if (bool.TryParse(appSettings["MergeMultiImage"], out mergeMultiImage))
                    model.MergeMultiImage = mergeMultiImage;
            }
            if (appSettings.AllKeys.Contains("ShouldTransferAllPages"))
            {
                bool shouldTransferAllPages;
                if (bool.TryParse(appSettings["ShouldTransferAllPages"], out shouldTransferAllPages))
                    model.ShouldTransferAllPages = shouldTransferAllPages;
            }
            if (appSettings.AllKeys.Contains("AutomaticBorderDetection"))
            {
                bool automaticBorderDetection;
                if (bool.TryParse(appSettings["AutomaticBorderDetection"], out automaticBorderDetection))
                    model.AutomaticBorderDetection = automaticBorderDetection;
            }
            if (appSettings.AllKeys.Contains("AutomaticDeskew"))
            {
                bool automaticDeskew;
                if (bool.TryParse(appSettings["AutomaticDeskew"], out automaticDeskew))
                    model.AutomaticDeskew = automaticDeskew;
            }
            if (appSettings.AllKeys.Contains("ScanResolution"))
            {
                int resoltuionValue;
                if (int.TryParse(appSettings["ScanResolution"], out resoltuionValue))
                    model.ScanResolution = resoltuionValue;
            }
            if (appSettings.AllKeys.Contains("ScanColor"))
            {
                bool scanColorValue;
                if (bool.TryParse(appSettings["ScanColor"], out scanColorValue))
                    model.ScanColor = scanColorValue;
            }
            if (appSettings.AllKeys.Contains("ShowScannerUI"))
            {
                bool showScannerUiValue;
                if (bool.TryParse(appSettings["ShowScannerUI"], out showScannerUiValue))
                    model.ShowScannerUI = showScannerUiValue;
            }
        }
    }
}