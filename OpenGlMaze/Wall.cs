using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{

    class Wall
    {
        private float thickness = 0.0002f;
        private float height = 0.025f;
        private float _width;
        public static int[] texture = new int[1];
        float[][] vrt = new float[8][];
        bool _isVertical = false;


        public Wall(bool isVertical = false, float width = 0.04f)
        {
            _width = width;

            _isVertical = isVertical;
            TextureLoader loader = new TextureLoader();
            loader.SetTexture("wall.jpg", texture);

            vrt[0] = new float[3] { 0, 0, 0 };
            vrt[1] = new float[3] { 0, height, 0 };
            vrt[2] = new float[3] { _isVertical ? _width : thickness, height, 0 };
            vrt[3] = new float[3] { _isVertical ? _width : thickness, 0, 0 };
            vrt[4] = new float[3] { 0, 0, _isVertical ? thickness : _width };
            vrt[5] = new float[3] { 0, height, _isVertical ? thickness : _width };
            vrt[6] = new float[3] { _isVertical ? _width : thickness, height, _isVertical ? thickness : _width };
            vrt[7] = new float[3] { _isVertical ? _width : thickness, 0, _isVertical ? thickness : _width };
        }

        public void Draw()
        {

            gl.PushMatrix();
            gl.MatrixMode(gl.MODELVIEW);

            // Matice textur
            gl.MatrixMode(gl.TEXTURE);
            gl.Enable(gl.TEXTURE_2D);
            gl.BindTexture(gl.TEXTURE_2D, texture[0]);

            gl.Begin(gl.QUADS);
            gl.Color3f(1, 1, 1);
            //Front
            gl.TexCoord2f(0, 0);
            gl.Vertex3fv(vrt[0]);

            gl.TexCoord2f(0, 1);
            gl.Vertex3fv(vrt[1]);

            gl.TexCoord2f(1, 1);
            gl.Vertex3fv(vrt[2]);

            gl.TexCoord2f(1, 0);
            gl.Vertex3fv(vrt[3]);

            //Back
            gl.TexCoord2f(0, 0);
            gl.Vertex3fv(vrt[7]);

            gl.TexCoord2f(0, 1);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(1, 1);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(1, 0);
            gl.Vertex3fv(vrt[4]);

            //top
            gl.TexCoord2f(0, 0);
            gl.Vertex3fv(vrt[1]);

            gl.TexCoord2f(0, 1);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(1, 1);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(1, 0);
            gl.Vertex3fv(vrt[2]);

            //Left
            gl.TexCoord2f(0, 0);
            gl.Vertex3fv(vrt[4]);

            gl.TexCoord2f(0, 1);
            gl.Vertex3fv(vrt[5]);

            gl.TexCoord2f(1, 1);
            gl.Vertex3fv(vrt[1]);

            gl.TexCoord2f(1, 0);
            gl.Vertex3fv(vrt[0]);

            //Right
            gl.TexCoord2f(0, 0);
            gl.Vertex3fv(vrt[3]);

            gl.TexCoord2f(0, 1);
            gl.Vertex3fv(vrt[2]);

            gl.TexCoord2f(1, 1);
            gl.Vertex3fv(vrt[6]);

            gl.TexCoord2f(1, 0);
            gl.Vertex3fv(vrt[7]);

            gl.End();

            gl.MatrixMode(gl.MODELVIEW);
            gl.PopMatrix();
        }
    }
}
