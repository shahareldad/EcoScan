using TwainDotNet.TwainNative;

namespace Ecoscan.Client.Models
{
    public class ConfigurationModel
    {
        public bool ShowProgressIndicatorUI;
        public string FileName { get; set; }
        public bool ScanColor { get; set; }
        public bool ShowScannerUI { get; set; }
        public int ScanResolution { get; set; }
        public bool AutomaticDeskew { get; set; }
        public bool AutomaticBorderDetection { get; set; }
        public bool ShouldTransferAllPages { get; set; }
        public bool MergeMultiImage { get; set; }
        public bool GetDevices { get; set; }
        public string SetDevice { get; set; }
        public bool AbortWhenNoPaperDetectable { get; set; }
        public bool UseDocumentFeeder { get; set; }
        public bool UseDuplex { get; set; }
        public FlipRotation FlipSideRotation { get; set; }
        public bool AutomaticRotate { get; set; }
        public short TransferCount { get; set; }
        public bool? UseAutoFeeder { get; set; }
        public bool? UseAutoScanCache { get; set; }
        public Orientation Orientation { get; set; }
        public PageType PageTypeSize { get; set; }
        public Units Units { get; set; }
        public float Top { get; set; }
        public float Left { get; set; }
        public float Bottom { get; set; }
        public float Right { get; set; }
        public bool SetPageSettings { get; set; }
        public bool SetArea { get; set; }
    }
}