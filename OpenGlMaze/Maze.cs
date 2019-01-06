using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{

    class Maze
    {
        private MazeSector[,] _mazeStructure;
        public MazeSector[,] MazeStructure { get => _mazeStructure; set => _mazeStructure = value; }
        public float Delta { get => _delta; set => _delta = value; }
        public float MazeOffset { get => _mazeOffset; set => _mazeOffset = value; }

        private int _mazeLength;
        private Wall _wallHorizontal;
        private Wall _wallVertical;
        private float _mazeSize = 0.25f;
        private float _mazeOffset = 0.375f;
        float _delta = 0.1f;


        public Maze()
        {
            _mazeLength = 6;
            _mazeStructure = new MazeSector[_mazeLength, _mazeLength];

            _mazeStructure[0, 0] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = true};
            _mazeStructure[1, 0] = new MazeSector() { Top = true, Right = false, Bottom = true, Left = false };
            _mazeStructure[2, 0] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = false };
            _mazeStructure[3, 0] = new MazeSector() { Top = true, Right = true, Bottom = false, Left = false };
            _mazeStructure[4, 0] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = true };
            _mazeStructure[5, 0] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = false };

            _mazeStructure[0, 1] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = false };
            _mazeStructure[1, 1] = new MazeSector() { Top = true, Right = true, Bottom = true, Left = false };
            _mazeStructure[2, 1] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[3, 1] = new MazeSector() { Top = false, Right = true, Bottom = true, Left = true };
            _mazeStructure[4, 1] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[5, 1] = new MazeSector() { Top = false, Right = true, Bottom = true, Left = true };

            _mazeStructure[0, 2] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = true };
            _mazeStructure[1, 2] = new MazeSector() { Top = true, Right = false, Bottom = false, Left = false };
            _mazeStructure[2, 2] = new MazeSector() { Top = false, Right = false, Bottom = false, Left = false };
            _mazeStructure[3, 2] = new MazeSector() { Top = true, Right = false, Bottom = true, Left = false };
            _mazeStructure[4, 2] = new MazeSector() { Top = false, Right = false, Bottom = false, Left = false };
            _mazeStructure[5, 2] = new MazeSector() { Top = true, Right = true, Bottom = false, Left = false };

            _mazeStructure[0, 3] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[1, 3] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[2, 3] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = true };
            _mazeStructure[3, 3] = new MazeSector() { Top = true, Right = true, Bottom = false, Left = false };
            _mazeStructure[4, 3] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[5, 3] = new MazeSector() { Top = false, Right = true, Bottom = true, Left = true };

            _mazeStructure[0, 4] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[1, 4] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = true };
            _mazeStructure[2, 4] = new MazeSector() { Top = true, Right = true, Bottom = false, Left = false };
            _mazeStructure[3, 4] = new MazeSector() { Top = false, Right = true, Bottom = true, Left = true };
            _mazeStructure[4, 4] = new MazeSector() { Top = false, Right = true, Bottom = false, Left = true };
            _mazeStructure[5, 4] = new MazeSector() { Top = true, Right = true, Bottom = false, Left = true };

            _mazeStructure[0, 5] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = true };
            _mazeStructure[1, 5] = new MazeSector() { Top = true, Right = true, Bottom = true, Left = false };
            _mazeStructure[2, 5] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = true };
            _mazeStructure[3, 5] = new MazeSector() { Top = true, Right = true, Bottom = true, Left = false };
            _mazeStructure[4, 5] = new MazeSector() { Top = false, Right = false, Bottom = true, Left = true };
            _mazeStructure[5, 5] = new MazeSector() { Top = false, Right = true, Bottom = true, Left = false };
            
            Delta = _mazeSize / _mazeLength;
            _wallHorizontal = new Wall(true, Delta);
            _wallVertical = new Wall(false, Delta);

            float positionX = 0;
            float positionY = 0;
            for (int row = 0; row < _mazeStructure.GetLength(0); row++)
            {
                positionX = 0;
                for (int column = 0; column < _mazeStructure.GetLength(1); column++)
                {
                    _mazeStructure[column, row].WallSize = Delta;
                    _mazeStructure[column, row].WallPositions.Top = new Tuple<float, float>(positionX, positionY);
                    _mazeStructure[column, row].WallPositions.Right = new Tuple<float, float>(positionX + Delta, positionY);
                    _mazeStructure[column, row].WallPositions.Bottom = new Tuple<float, float>(positionX, positionY + Delta);
                    _mazeStructure[column, row].WallPositions.Left = new Tuple<float, float>(positionX, positionY);
                    positionX += Delta;
                }
                positionY += Delta;
            }

            Draw();
        }

        public void Draw()
        {
            gl.PushMatrix();
            gl.Translatef(_mazeOffset, 0, _mazeOffset);

            for (int row = 0; row < _mazeStructure.GetLength(0); row++)
            {
                gl.PushMatrix();
                for (int column = 0; column < _mazeStructure.GetLength(1); column++)
                {
                    if(row == 0)
                    {
                        if(_mazeStructure[column, row].Top)
                        {
                            _wallHorizontal.Draw();
                        }
                    }

                    if (column == 0)
                    {
                        if (_mazeStructure[column, row].Left)
                        {
                            _wallVertical.Draw();
                        }
                    }

                    if (_mazeStructure[column, row].Bottom)
                    {
                        gl.PushMatrix();
                        gl.Translatef(0, 0, Delta);
                        _wallHorizontal.Draw();
                        gl.PopMatrix();
                    }
                    gl.Translatef(Delta, 0, 0);

                    if (_mazeStructure[column, row].Right)
                    {
                        _wallVertical.Draw();
                    }
                }
                gl.PopMatrix();
                gl.Translatef(0, 0, Delta);
            }

            gl.PopMatrix();
        }
        

        public bool IsColision(float fromX, float fromY, float toX, float toY)
        {
            float relativeColumn = fromX - _mazeOffset;
            float relativeRow = fromY - _mazeOffset;
            int wallIndexColumn = (int)Math.Floor(relativeColumn / Delta);
            int wallIndexRow = (int)Math.Floor(relativeRow / Delta);
            int maxSize = _mazeLength - 1;

            wallIndexColumn = Math.Max(wallIndexColumn, 0);
            wallIndexColumn = Math.Min(wallIndexColumn, maxSize);
            wallIndexRow = Math.Max(wallIndexRow, 0);
            wallIndexRow = Math.Min(wallIndexRow, maxSize);
            
            //Center
            if (_mazeStructure[wallIndexColumn, wallIndexRow].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset))
            {
                return true;
            }

            //Left bottom
            if(wallIndexColumn > 0 && wallIndexRow > 0)
            {
                if (_mazeStructure[wallIndexColumn - 1, wallIndexRow - 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset))
                {
                    return true;
                }
            }

            //Left top
            if (wallIndexColumn > 0 && wallIndexRow < maxSize)
            {
                if (_mazeStructure[wallIndexColumn - 1, wallIndexRow + 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset))
                {
                    return true;
                }
            }

            
            //Right bottom
            if (wallIndexColumn < maxSize && wallIndexRow > 0)
            {
                if (_mazeStructure[wallIndexColumn + 1, wallIndexRow - 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset))
                {
                    return true;
                }
            }

            //Right top
            if (wallIndexColumn < maxSize && wallIndexRow < maxSize)
            {
                if (_mazeStructure[wallIndexColumn + 1, wallIndexRow + 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset))
                {
                    return true;
                }
            }


            return false;
        }
    }
}
