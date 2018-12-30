using Grafika;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;

namespace OpenGlMaze
{
    public partial class Form1 : Form
    {
        OpenGL.Context context;
        float cubeSize = 1;

        private float _rotateX, _rotateY, _playerX = 0, _playerY = 0.1f, _playerZ = 0;
        private const float MoveSpeed = 0.01f;
        private List<Keys> _keyDowns = new List<Keys>();
       

        float[] world_translation = new float[3];
        float[] world_rotation = new float[3];

        Terrain ter;
        Skybox skybox;
        Wall wall;


        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(800, 800);

            InitGL();
        }

        private void InitGL()
        {
            context = new OpenGL.Context(this, 32, 32, 0);
            gl.MatrixMode(gl.PROJECTION);
            gl.LoadIdentity();
            glu.Perspective(40, this.Width / this.Height, 0.1f, 100f);
            gl.Enable(gl.DEPTH_TEST);
            gl.ShadeModel(gl.FLAT);

            world_translation[0] = world_translation[1] = world_translation[2] = 0;
            
            gl.Enable(gl.DEPTH_TEST);

            gl.Enable(gl.LIGHT0);
            gl.Lightfv(gl.LIGHT0, gl.DIFFUSE, new float[] { 1, 1, 1, 1 });
            gl.Lightfv(gl.LIGHT0, gl.POSITION, new float[] { 1, 1, 1, 1 });

            gl.LightModelfv(gl.LIGHT_MODEL_AMBIENT, new float[] { 1f, 1f, 1f, 1 });
            gl.Enable(gl.LIGHTING);
            

            ter = new Terrain(128);
            skybox = new Skybox();
            wall = new Wall();
        }

        public void Draw()
        {

            gl.ClearColor(0, 0.6f, 0, 1);
            gl.Clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

            gl.PolygonMode(gl.FRONT, gl.FILL);
            gl.PolygonMode(gl.BACK, gl.LINE);

            gl.MatrixMode(gl.MODELVIEW);
            gl.LoadIdentity();


            Point mid = new Point(this.Left + this.Width / 2, this.Top + this.Height / 2);
            if (this.Focused)
            {
                _rotateX -= (Cursor.Position.X - mid.X) / 500f;
                _rotateY -= (Cursor.Position.Y - mid.Y) / 500f;
                if (_rotateY > (Math.PI - 0.0001f) / 2) _rotateY = (float)(Math.PI - 0.0001f) / 2;
                if (_rotateY < (-Math.PI + 0.0001f) / 2) _rotateY = (float)(-Math.PI + 0.0001f) / 2;
                Cursor.Position = mid;
            }

            if (_keyDowns.Contains(Keys.W))
            {
                _playerX += (float)Math.Sin(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
                _playerY += (float)Math.Sin(_rotateY) * MoveSpeed;
                _playerZ += (float)Math.Cos(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
            }

            if (_keyDowns.Contains(Keys.A))
            {
                _playerX += (float)Math.Sin(_rotateX + Math.PI / 2) * MoveSpeed;
                _playerZ += (float)Math.Cos(_rotateX + Math.PI / 2) * MoveSpeed;
            }

            if (_keyDowns.Contains(Keys.D))
            {
                _playerX += (float)Math.Sin(_rotateX - Math.PI / 2) * MoveSpeed;
                _playerZ += (float)Math.Cos(_rotateX - Math.PI / 2) * MoveSpeed;
            }

            if (_keyDowns.Contains(Keys.S))
            {
                _playerX -= (float)Math.Sin(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
                _playerY -= (float)Math.Sin(_rotateY) * MoveSpeed;
                _playerZ -= (float)Math.Cos(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
            }

            if (_keyDowns.Contains(Keys.Space))
            {
                _playerY += MoveSpeed;
            }

            if (_keyDowns.Contains(Keys.ShiftKey))
            {
                _playerY -= MoveSpeed;
            }

            glu.LookAt(_playerX, _playerY, _playerZ, _playerX + Math.Sin(_rotateX) * Math.Cos(_rotateY),
                _playerY + Math.Sin(_rotateY),
                _playerZ + Math.Cos(_rotateX) * Math.Cos(_rotateY), 0, 1, 0);


            //Terrain
            ter.Draw();

            //Skybox
            skybox.Draw();

            
            //Test wall
            wall.Draw();

            // Light
            gl.Lightfv(gl.LIGHT0, gl.POSITION, new float[] { 0.5f, 0.5f, 1f });

            context.SwapBuffers();
        }
        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_keyDowns.Contains(e.KeyCode))
            {
                _keyDowns.Add(e.KeyCode);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            _keyDowns.Remove(e.KeyCode);
        }
    }
}
