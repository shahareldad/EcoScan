using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ecoscan.Client.ExportsServices
{
    public interface IExportService
    {
        void Initialize(string filename, ImageFormat imageFormat);

        void SaveColor(List<Bitmap> images);

        void SaveBlackWhite(List<Bitmap> images);
    }
}