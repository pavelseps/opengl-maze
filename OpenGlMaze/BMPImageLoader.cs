using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{
    struct BMPImage
    {
        public int Width;
        public int Height;
        public byte[] ImageData;
    }

    class BMPImageLoader
    {
        public BMPImage LoadDisplacementMap(string url)
        {
            Bitmap bmp = new Bitmap(url);
            byte[] data = new byte[bmp.Width * bmp.Height * 3];
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bmpData.Scan0;
            Marshal.Copy(ptr, data, 0, bmp.Width * bmp.Height * 3);

            BMPImage map = new BMPImage();
            map.Width = bmp.Width;
            map.Height = bmp.Height;
            map.ImageData = data;


            bmp.UnlockBits(bmpData);

            return map;
        }
    }
}
