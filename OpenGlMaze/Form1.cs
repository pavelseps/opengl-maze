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
    // TODO start end
    // Nicer UI
    public partial class Form1 : Form
    {
        OpenGL.Context context;

        private float _rotateX, _rotateY, _playerX = 0.7f, _playerY = 0.2f, _playerZ = 0.3f, _playerHeight = 0.014f;
        private const float MoveSpeed = 0.002f;
        private List<Keys> _keyDowns = new List<Keys>();

        private bool _freeMove = false;

        private float _resultMoveX = 0;
        private float _resultMoveZ = 0;
        private string _colisionResult = "";

        private App _app;

        Terrain ter;
        Skybox skybox;
        Maze maze;

        internal App App { get => _app; set => _app = value; }

        public Form1()
        {

            InitializeComponent();

            this.Size = new Size(1200, 800);

            InitGL();
        }

        public void Start(string mazeUrl)
        {
            ter = new Terrain(128);
            skybox = new Skybox();
            maze = new Maze(mazeUrl);

            Tuple<float, float> playerPos = maze.GetStartPosition();
            _playerX = playerPos.Item1;
            _playerZ = playerPos.Item2;
        }


        public void End()
        {
            _keyDowns.Clear();
        }

        private void InitGL()
        {
            context = new OpenGL.Context(this, 32, 32, 0);
            gl.MatrixMode(gl.PROJECTION);
            gl.LoadIdentity();
            glu.Perspective(65, this.Width / this.Height, 0.0001f, 100f);
            gl.Enable(gl.DEPTH_TEST);
            gl.ShadeModel(gl.FLAT);
           
            
            gl.Enable(gl.DEPTH_TEST);

            gl.Enable(gl.LIGHT0);
            gl.Lightfv(gl.LIGHT0, gl.DIFFUSE, new float[] { 1, 1, 1, 1 });
            gl.Lightfv(gl.LIGHT0, gl.POSITION, new float[] { 1, 1, 1, 1 });

            gl.LightModelfv(gl.LIGHT_MODEL_AMBIENT, new float[] { 1f, 1f, 1f, 1 });
            gl.Enable(gl.LIGHTING);
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
                _resultMoveX = _playerX + (float)Math.Sin(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
                _resultMoveZ = _playerZ + (float)Math.Cos(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;

                _colisionResult = maze.IsColision(
                    _playerX,
                    _playerZ,
                    _resultMoveX,
                    _resultMoveZ
                    );

                if(_colisionResult == Maze.COLISION_END)
                {
                    _app.Finished();
                }

                if (_freeMove || _colisionResult == Maze.COLISION_FALSE)
                {
                    _playerX = _resultMoveX;
                    if (_freeMove)
                    {
                        _playerY += (float)Math.Sin(_rotateY) * MoveSpeed;
                    }
                    _playerZ = _resultMoveZ;
                }
            }

            if (_keyDowns.Contains(Keys.A))
            {
                _resultMoveX = _playerX + (float)Math.Sin(_rotateX + Math.PI / 2) * MoveSpeed;
                _resultMoveZ = _playerZ + (float)Math.Cos(_rotateX + Math.PI / 2) * MoveSpeed;
                _colisionResult = maze.IsColision(
                    _playerX,
                    _playerZ,
                    _resultMoveX,
                    _resultMoveZ
                    );

                if (_colisionResult == Maze.COLISION_END)
                {
                    _app.Finished();
                }

                if (_freeMove || _colisionResult == Maze.COLISION_FALSE)
                {
                    _playerX = _resultMoveX;
                    _playerZ = _resultMoveZ;
                }
            }

            if (_keyDowns.Contains(Keys.D))
            {
                _resultMoveX = _playerX + (float)Math.Sin(_rotateX - Math.PI / 2) * MoveSpeed;
                _resultMoveZ = _playerZ + (float)Math.Cos(_rotateX - Math.PI / 2) * MoveSpeed;
                _colisionResult = maze.IsColision(
                    _playerX,
                    _playerZ,
                    _resultMoveX,
                    _resultMoveZ
                    );

                if (_colisionResult == Maze.COLISION_END)
                {
                    _app.Finished();
                }

                if (_freeMove || _colisionResult == Maze.COLISION_FALSE)
                {
                    _playerX = _resultMoveX;
                    _playerZ = _resultMoveZ;
                }
            }

            if (_keyDowns.Contains(Keys.S))
            {
                _resultMoveX = _playerX - (float)Math.Sin(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
                _resultMoveZ = _playerZ - (float)Math.Cos(_rotateX) * (float)Math.Cos(_rotateY) * MoveSpeed;
                _colisionResult = maze.IsColision(
                    _playerX,
                    _playerZ,
                    _resultMoveX,
                    _resultMoveZ
                    );

                if (_colisionResult == Maze.COLISION_END)
                {
                    _app.Finished();
                }

                if (_freeMove || _colisionResult == Maze.COLISION_FALSE)
                {
                    _playerX = _resultMoveX;
                    if (_freeMove)
                    {
                        _playerY -= (float)Math.Sin(_rotateY) * MoveSpeed;
                    }
                    _playerZ = _resultMoveZ;
                }
                    
            }

            if (_keyDowns.Contains(Keys.Space))
            {
                if (_freeMove)
                {
                    _playerY += MoveSpeed;
                }
            }
            if (_keyDowns.Contains(Keys.ShiftKey))
            {
                if (_freeMove)
                {
                    _playerY -= MoveSpeed;
                }
            }
            if (!_freeMove)
            {
                _playerY = ter.GetEvelation(_playerX, _playerZ) + _playerHeight;
            }
                    

            glu.LookAt(_playerX, _playerY, _playerZ, _playerX + Math.Sin(_rotateX) * Math.Cos(_rotateY),

                _playerY + Math.Sin(_rotateY),
                _playerZ + Math.Cos(_rotateX) * Math.Cos(_rotateY), 0, 1, 0);


            //Terrain
            ter.Draw();

            //Skybox
            skybox.Draw();
            
            //Maze
            maze.Draw();


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
