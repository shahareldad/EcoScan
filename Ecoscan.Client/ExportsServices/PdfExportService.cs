using log4net;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ecoscan.Client.ExportsServices
{
    public class PdfExportService : IExportService
    {
        private string _filename;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PdfExportService));

        public void Initialize(string filename, ImageFormat imageFormat)
        {
            _logger.Debug("PdfExportService.Initialize started.");
            _filename = filename;
        }

        public void SaveColor(List<Bitmap> images)
        {
            _logger.Debug("PdfExportService.SaveColor started.");

            SaveToPdf(images, (doc, bitmap, index) =>
            {
                _logger.Debug("PdfExportService.SaveColor anonymous started.");

                var page = new PdfPage();
                using (var ms = new MemoryStream())
                {
                    images[index].Save(ms, ImageFormat.Jpeg);
                    using (var tempBitmap = new Bitmap(ms))
                    {
                        var img = XImage.FromGdiPlusImage(tempBitmap);
                        page.Width = img.PointWidth;
                        page.Height = img.PointHeight;
                        doc.Pages.Add(page);
                        var xgr = XGraphics.FromPdfPage(doc.Pages[index]);
                        xgr.DrawImage(img, 0, 0);
                    }
                }
            });
        }

        public void SaveBlackWhite(List<Bitmap> images)
        {
            _logger.Debug("PdfExportService.SaveBlackWhite started.");

            SaveToPdf(images, (doc, bitmap, index) =>
            {
                _logger.Debug("PdfExportService.SaveBlackWhite anonymous started.");

                var page = new PdfPage();
                var img = XImage.FromGdiPlusImage(bitmap);
                page.Width = img.PointWidth;
                page.Height = img.PointHeight;
                doc.Pages.Add(page);
                var xgr = XGraphics.FromPdfPage(doc.Pages[index]);
                xgr.DrawImage(img, 0, 0);
            });
        }

        private void SaveToPdf(List<Bitmap> images, Action<PdfDocument, Bitmap, int> action)
        {
            _logger.Debug("PdfExportService.SaveToPdf started.");

            var length = images.Count;
            var doc = new PdfDocument();
            for (var index = 0; index < length; index++)
            {
                action(doc, images[index], index);
            }
            doc.Save(_filename);
            doc.Close();
        }
    }
}