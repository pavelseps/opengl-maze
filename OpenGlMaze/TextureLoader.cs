using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{
    class TextureLoader
    {

        public void SetTexture(string src, int[] texture)
        {
            gl.Enable(gl.TEXTURE_2D);

            Bitmap image = new Bitmap(src);
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            gl.GenTextures(1, texture);
            gl.BindTexture(gl.TEXTURE_2D, texture[0]);
            gl.TexImage2D(gl.TEXTURE_2D, 0, (int)gl.RGB8, image.Width, image.Height,
                0, gl.BGR, gl.UNSIGNED_BYTE, bitmapdata.Scan0);
            gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR); // Linear Filtering
            gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR); // Linear Filtering
            
        }
    }
}
