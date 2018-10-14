using BitMiracle.LibTiff.Classic;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ecoscan.Client.ExportsServices
{
    public class TiffExportService : IExportService
    {
        private string _filename;
        private readonly ILog _logger = LogManager.GetLogger(typeof(TiffExportService));

        public void Initialize(string filename, ImageFormat imageFormat)
        {
            _logger.Debug("TiffExportService.Initialize started.");
            _filename = filename;
        }

        public void SaveColor(List<Bitmap> images)
        {
            _logger.Debug("TiffExportService.SaveColor started.");

            var extraSettings = new Action<Tiff>(output =>
            {
                _logger.Debug("TiffExportService.SaveColor anonymous started.");

                // lists not complete
                // not working => JP2000,JBIG,THUNDERSCAN,
                // working but not when transfering file when hosted in .Net 2.0 => JPEG
                // working => LZW
                output.SetField(TiffTag.COMPRESSION, Compression.LZW);
                output.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                output.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                output.SetField(TiffTag.BITSPERSAMPLE, 8);
            });

            SaveTiff(images, PixelFormat.Format24bppRgb, true, extraSettings);
        }

        public void SaveBlackWhite(List<Bitmap> images)
        {
            _logger.Debug("TiffExportService.SaveBlackWhite started.");

            var extraSettings = new Action<Tiff>(output =>
            {
                _logger.Debug("TiffExportService.SaveBlackWhite anonymous started.");

                // lists not complete
                // not working => CCITTRLEW,ADOBE_DEFLATE,DEFLATE,DCS,IT8BL,IT8CTPAD,IT8LW,JBIG,
                // SGILOG24,THUNDERSCAN,SGILOG,PIXARLOG,PIXARFILM,OJPEG,...
                // working with color inversion when transfering file when hosted in .Net 2.0 => CCITTFAX4,CCITTFAX3
                // working => PACKBITS
                output.SetField(TiffTag.COMPRESSION, Compression.PACKBITS);
                output.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                output.SetField(TiffTag.SUBFILETYPE, 0);
                output.SetField(TiffTag.BITSPERSAMPLE, 1);
                output.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                output.SetField(TiffTag.T6OPTIONS, 0);
            });

            SaveTiff(images, PixelFormat.Format1bppIndexed, false, extraSettings);
        }

        private void SaveTiff(List<Bitmap> images, PixelFormat pixelFormat, bool useConvertSamples, Action<Tiff> extraSettings)
        {
            _logger.Debug("PdfExportService.SaveTiff started.");

            var numberOfPages = images.Count;
            using (var output = Tiff.Open(_filename, "w"))
            {
                for (var page = 0; page < numberOfPages; ++page)
                {
                    var byteArray = GetImageRasterBytes(images[page], pixelFormat);

                    output.SetField(TiffTag.IMAGEWIDTH, images[page].Width);
                    output.SetField(TiffTag.IMAGELENGTH, images[page].Height);
                    output.SetField(TiffTag.ROWSPERSTRIP, images[page].Height);
                    output.SetField(TiffTag.XRESOLUTION, images[page].HorizontalResolution);
                    output.SetField(TiffTag.YRESOLUTION, images[page].VerticalResolution);
                    output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.INCH);

                    extraSettings(output);

                    // specify that it's a page within the multipage file
                    output.SetField(TiffTag.SUBFILETYPE, FileType.PAGE);
                    // specify the page number
                    output.SetField(TiffTag.PAGENUMBER, page, numberOfPages);

                    var stride = byteArray.Length / images[page].Height;
                    if (useConvertSamples)
                        ConvertSamples(byteArray, images[page].Width, images[page].Height);

                    for (int i = 0, offset = 0; i < images[page].Height; i++)
                    {
                        output.WriteScanline(byteArray, offset, i, 0);
                        offset += stride;
                    }

                    _logger.Debug("PdfExportService.SaveTiff about to write page to file.");
                    output.WriteDirectory();
                }
            }
            _logger.Debug("PdfExportService.SaveTiff finished writing file.");
        }

        private static byte[] GetImageRasterBytes(Bitmap bmp, PixelFormat format)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            byte[] bits;

            try
            {
                // Lock the managed memory
                var bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);

                // Declare an array to hold the bytes of the bitmap.
                bits = new byte[bmpdata.Stride * bmpdata.Height];

                // Copy the values into the array.
                System.Runtime.InteropServices.Marshal.Copy(bmpdata.Scan0, bits, 0, bits.Length);

                // Release managed memory
                bmp.UnlockBits(bmpdata);
            }
            catch
            {
                return null;
            }

            return bits;
        }

        /// <summary>
        /// Converts BGR samples into RGB samples
        /// </summary>
        private static void ConvertSamples(byte[] data, int width, int height)
        {
            var stride = data.Length / height;
            const int samplesPerPixel = 3;

            for (var y = 0; y < height; y++)
            {
                var offset = stride * y;
                var strideEnd = offset + width * samplesPerPixel;

                for (var i = offset; i < strideEnd; i += samplesPerPixel)
                {
                    var temp = data[i + 2];
                    data[i + 2] = data[i];
                    data[i] = temp;
                }
            }
        }
    }
}