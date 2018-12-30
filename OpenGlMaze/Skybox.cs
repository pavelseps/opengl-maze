using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{
    class Skybox
    {
        private float size = 2f;
        public static int[] texture = new int[1];
        private float[][] vrt = new float[8][];

        public Skybox()
        {
            TextureLoader loader = new TextureLoader();
            loader.SetTexture("skybox.jpg", texture);

            vrt[0] = new float[3] { 0, 0, 0 };
            vrt[1] = new float[3] { 0, size, 0 };
            vrt[2] = new float[3] { size, size, 0 };
            vrt[3] = new float[3] { size, 0, 0 };
            vrt[4] = new float[3] { 0, 0, size };
            vrt[5] = new float[3] { 0, size, size };
            vrt[6] = new float[3] { size, size, size };
            vrt[7] = new float[3] { size, 0, size };
        }

        public void Draw()
        {

            gl.PushMatrix();
            gl.Translatef(-size / 4f, -size / 4f, -size / 4f);


            gl.MatrixMode(gl.MODELVIEW);
            // Matice textur
            gl.MatrixMode(gl.TEXTURE);
            gl.Enable(gl.TEXTURE_2D);
            gl.BindTexture(gl.TEXTURE_2D, texture[0]);

            gl.Begin(gl.QUADS);
            gl.Color3f(1, 1, 1);
            //Front
            gl.TexCoord2f(0.5f, 0.66f);
            gl.Vertex3fv(vrt[3]);

            gl.TexCoord2f(0.5f, 0.327f);
            gl.Vertex3fv(vrt[2]);

            gl.TexCoord2f(0.25f, 0.327f);
            gl.Vertex3fv(vrt[1]);
           
            gl.TexCoord2f(0.25f, 0.66f);
            gl.Vertex3fv(vrt[0]);

            //Back
            gl.TexCoord2f(1, 0.66f);
            gl.Vertex3fv(vrt[4]);

            gl.TexCoord2f(1, 0.327f);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(0.75f, 0.327f);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(0.75f, 0.66f);
            gl.Vertex3fv(vrt[7]);

            //top
            gl.TexCoord2f(0.5f, 0.327f);
            gl.Vertex3fv(vrt[2]);

            gl.TexCoord2f(0.5f, 0);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(0.25f, 0);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(0.25f, 0.327f);
            gl.Vertex3fv(vrt[1]);

            //Left
            gl.TexCoord2f(0.25f, 0.66f);
            gl.Vertex3fv(vrt[0]);

            gl.TexCoord2f(0.25f, 0.327f);
            gl.Vertex3fv(vrt[1]);

            gl.TexCoord2f(0, 0.327f);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(0, 0.66f);
            gl.Vertex3fv(vrt[4]);

            //Right
            gl.TexCoord2f(0.75f, 0.66f);
            gl.Vertex3fv(vrt[7]);

            gl.TexCoord2f(0.75f, 0.327f);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(0.5f, 0.327f);
            gl.Vertex3fv(vrt[2]);

            gl.TexCoord2f(0.5f, 0.66f);
            gl.Vertex3fv(vrt[3]);

            gl.End();

            gl.MatrixMode(gl.MODELVIEW);
            gl.PopMatrix();
        }
    }
}
