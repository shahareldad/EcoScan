using System;
using System.Collections.Generic;
using System.Drawing;

namespace TwainDotNet
{
    public class TransferImageEventArgs : EventArgs
    {
        public Bitmap Image { get; private set; }
        public bool ContinueScanning { get; set; }

        public TransferImageEventArgs(Bitmap image, bool continueScanning)
        {
            this.Image = image;
            this.ContinueScanning = continueScanning;
        }
    }

    public class TransferImagesEventArgs : EventArgs
    {
        public List<Bitmap> Images { get; private set; }

        public TransferImagesEventArgs(List<Bitmap> images)
        {
            this.Images = images;
        }
    }
}