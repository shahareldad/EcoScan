using Ecoscan.Client.Models;
using System;
using TwainDotNet.TwainNative;

namespace Ecoscan.Client.ParsingServices
{
    public class CommandLineParsingService : IParsingService
    {
        public void Parse(ConfigurationModel model, string[] args)
        {
            foreach (var argument in args)
            {
                if (argument.Contains("AreaSize"))
                {
                    model.SetArea = true;
                    var areaDims = argument.Split(':')[1];
                    var dims = areaDims.Split(',');
                    model.Left = float.Parse(dims[0]);
                    model.Top = float.Parse(dims[1]);
                    model.Right = float.Parse(dims[2]);
                    model.Bottom = float.Parse(dims[3]);
                    continue;
                }
                if (argument.Contains("Units"))
                {
                    model.SetArea = true;
                    model.Units = (Units)Enum.Parse(typeof(Units), argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("PageTypeSize"))
                {
                    model.SetPageSettings = true;
                    model.PageTypeSize = (PageType)Enum.Parse(typeof(PageType), argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("Orientation"))
                {
                    model.SetPageSettings = true;
                    model.Orientation = (Orientation)Enum.Parse(typeof(Orientation), argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("UseAutoFeeder"))
                {
                    model.UseAutoFeeder = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("UseAutoScanCache"))
                {
                    model.UseAutoScanCache = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("FlipSideRotation"))
                {
                    model.FlipSideRotation = (FlipRotation)Enum.Parse(typeof(FlipRotation), argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("TransferCount"))
                {
                    model.TransferCount = short.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("AutomaticRotate"))
                {
                    model.AutomaticRotate = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("AbortWhenNoPaperDetectable"))
                {
                    model.AbortWhenNoPaperDetectable = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("UseDocumentFeeder"))
                {
                    model.UseDocumentFeeder = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("ShowProgressIndicatorUI"))
                {
                    model.ShowProgressIndicatorUI = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("UseDuplex"))
                {
                    model.UseDuplex = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("FileName"))
                {
                    model.FileName = argument.Substring(argument.IndexOf(':') + 1);
                    continue;
                }
                if (argument.Contains("SetDevice"))
                {
                    model.SetDevice = argument.Substring(argument.IndexOf(':') + 1);
                    continue;
                }
                if (argument.Contains("GetDevices"))
                {
                    model.GetDevices = true;
                    continue;
                }
                if (argument.Contains("MergeMultiImage"))
                {
                    model.MergeMultiImage = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("ShouldTransferAllPages"))
                {
                    model.ShouldTransferAllPages = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("AutomaticBorderDetection"))
                {
                    model.AutomaticBorderDetection = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("ScanColor"))
                {
                    model.ScanColor = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("AutomaticDeskew"))
                {
                    model.AutomaticDeskew = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("ShowScannerUI"))
                {
                    model.ShowScannerUI = bool.Parse(argument.Split(':')[1]);
                    continue;
                }
                if (argument.Contains("ScanResolution"))
                {
                    model.ScanResolution = int.Parse(argument.Split(':')[1]);
                    continue;
                }
            }
        }
    }
}