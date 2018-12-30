using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using OpenGlMaze;

namespace Grafika
{
    struct BMPImage
    {
        public int Width;
        public int Height;
        public byte[] ImageData;
    }

    class Terrain
    {
        float[,][] verts;

        BMPImage map;
        public static int[] texture = new int[1];

        public Terrain(int n)
        {

            LoadDisplacementMap();


            verts = new float[n, n][];
            float delta = 1f / n;

            float size = 256f * 3;

            for (int x = 0; x < verts.GetLength(0); x++)
            {
                for (int y = 0; y < verts.GetLength(1); y++)
                {
                    verts[x, y] = new float[3]
                    {
                        delta * y,
                        map.ImageData[(x * 3) + (y * 3 * map.Width)] / size,
                        delta * x,
                    };
                }
            }

            TextureLoader loader = new TextureLoader();
            loader.SetTexture("deathvalley_topomap_texture.jpg", texture);

            this.Draw();
        }

        public void LoadDisplacementMap()
        {
            try
            {
                Bitmap bmp = new Bitmap("deathvalley_topomap_update.jpg");
                byte[] data = new byte[bmp.Width * bmp.Height * 3];
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(ptr, data, 0, bmp.Width * bmp.Height * 3);

                map = new BMPImage();
                map.Width = bmp.Width;
                map.Height = bmp.Height;
                map.ImageData = data;


                bmp.UnlockBits(bmpData);

            }
            catch
            {
            }
        }


        public void Draw()
        {
            gl.PushMatrix();

            gl.MatrixMode(gl.MODELVIEW);
            // Matice textur
            gl.MatrixMode(gl.TEXTURE);
            gl.Enable(gl.TEXTURE_2D);
            gl.BindTexture(gl.TEXTURE_2D, texture[0]);

            float res = 1f / verts.GetLength(0);
            gl.Begin(gl.TRIANGLES);

            for (int x = 0; x < verts.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < verts.GetLength(1) - 1; y++)
                {

                    gl.Color3f(1, 1, 1);
                    gl.TexCoord2f(x * res, y * res);
                    gl.Vertex3fv(verts[x, y]);
                    gl.TexCoord2f((x + 1) * res, y * res);
                    gl.Vertex3fv(verts[x + 1, y]);
                    gl.TexCoord2f(x * res, (y + 1) * res);
                    gl.Vertex3fv(verts[x, y + 1]);
                    gl.TexCoord2f((x + 1) * res, y * res);
                    gl.Vertex3fv(verts[x + 1, y]);
                    gl.TexCoord2f((x + 1) * res, (y + 1) * res);
                    gl.Vertex3fv(verts[x + 1, y + 1]);
                    gl.TexCoord2f(x * res, (y + 1) * res);
                    gl.Vertex3fv(verts[x, y + 1]);

                }
            }

            gl.End();

            gl.MatrixMode(gl.MODELVIEW);
            gl.PopMatrix();
        }
    }
}
