using log4net;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ecoscan.Client.ExportsServices
{
    public class SingleImageEachExportService : IExportService
    {
        private string _filename;
        private ImageFormat _imageFormat;
        private readonly ILog _logger = LogManager.GetLogger(typeof(SingleImageEachExportService));

        public void Initialize(string filename, ImageFormat imageFormat)
        {
            _logger.Debug("SingleImageEachExportService.Initialize started.");
            _filename = filename;
            _imageFormat = imageFormat;
        }

        public void SaveColor(List<Bitmap> images)
        {
            _logger.Debug("SingleImageEachExportService.SaveColor started.");
            SaveImages(images);
        }

        public void SaveBlackWhite(List<Bitmap> images)
        {
            _logger.Debug("SingleImageEachExportService.SaveBlackWhite started.");
            SaveImages(images);
        }

        private void SaveImages(List<Bitmap> images)
        {
            _logger.Debug("SingleImageEachExportService.SaveImages started.");
            var extnesion = Path.GetExtension(_filename);
            var length = images.Count;
            _logger.Debug("SingleImageEachExportService.SaveImages image count: " + length);
            for (var index = 0; index < length; index++)
            {
                var newFileName = _filename;
                if (length > 1)
                    newFileName = _filename.Insert(_filename.IndexOf(extnesion), "_" + index);
                _logger.Debug("SingleImageEachExportService.SaveImages newFileName: " + newFileName);
                var image = images[index];
                image.Save(newFileName, _imageFormat);
            }
        }
    }
}