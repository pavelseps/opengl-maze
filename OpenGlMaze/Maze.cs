using OpenGL;
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
            _mazeStructure = LoadFromImage("maze.jpg");

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

        public MazeSector[,] LoadFromImage(string url)
        {
            BMPImageLoader loader = new BMPImageLoader();
            

            BMPImage mazeImage = loader.LoadDisplacementMap(url);
            _mazeLength = mazeImage.Width;
            MazeSector[,] _mazeStructure = new MazeSector[_mazeLength, _mazeLength];
            bool isTop = false;
            bool isRight = false;
            bool isBottom = false;
            bool isLeft = false;
            bool isVertical = false;
            bool isHorizontal = false;
            bool[] imagePoint = new bool[9];
       
            for (int row = 0; row < _mazeLength; row++)
            {
                for (int column = 0; column < _mazeLength; column++)
                {

                    imagePoint = GetAurounImagePoint(mazeImage, (column * 3) + (row * 3 * mazeImage.Width), 2);

                    if(row != 0 && column != 0)
                    {
                        isTop = _mazeStructure[column, Math.Max(row - 1, 0)].Bottom;
                        isLeft = _mazeStructure[Math.Max(column - 1, 0), row].Right;
                    }
                    else
                    {
                        isTop = false;
                        isLeft = false;
                    }
                    isVertical = imagePoint[4] && (imagePoint[1] || imagePoint[7]);
                    isHorizontal = imagePoint[4] && (imagePoint[3] || imagePoint[5]);
                    isRight = isHorizontal? imagePoint[4] && imagePoint[1] : isVertical;
                    isBottom = isVertical ? imagePoint[4] && imagePoint[3] : isHorizontal;

                    _mazeStructure[column, row] = new MazeSector()
                    {
                        Top = isTop,
                        Right = isRight,
                        Bottom = isBottom,
                        Left = isLeft
                    };
                    

                    /*mazeImage.ImageData[(column * 3) + (row * 3 * mazeImage.Width) + 2]; // blue (walls)
                    mazeImage.ImageData[(column * 3) + (row * 3 * mazeImage.Width) + 1]; // green (start)
                    mazeImage.ImageData[(column * 3) + (row * 3 * mazeImage.Width)]; // red (end)*/
                }
            }


            return _mazeStructure;
        }

        private bool[] GetAurounImagePoint(BMPImage image, int index, int offset)
        {

            int imageWidth = image.Width * 3;

            bool isOnTop = index < imageWidth;
            bool isOnRight = index % imageWidth == imageWidth - 3;
            bool isOnBottom = index >= imageWidth * (image.Height - 1);
            bool isOnLeft = index % image.Width == 0;

            bool[] ret = new bool[9];
            ret[0] = isOnTop || isOnLeft ? false : GetImagePointResult(image.ImageData[index - imageWidth - 3 + offset]);
            ret[1] = isOnTop ? false : GetImagePointResult(image.ImageData[index - imageWidth + offset]);
            ret[2] = isOnTop || isOnRight ? false : GetImagePointResult(image.ImageData[index - imageWidth + 3 + offset]);

            ret[3] = isOnLeft ? false : GetImagePointResult(image.ImageData[index - 3 + offset]);
            ret[4] = GetImagePointResult(image.ImageData[index + offset]); // middle
            ret[5] = isOnRight ? false : GetImagePointResult(image.ImageData[index + 3 + offset]);

            ret[6] = isOnBottom || isOnLeft ? false : GetImagePointResult(image.ImageData[index + imageWidth - 3 + offset]);
            ret[7] = isOnBottom ? false : GetImagePointResult(image.ImageData[index + imageWidth + offset]);
            ret[8] = isOnBottom || isOnRight ? false : GetImagePointResult(image.ImageData[index + imageWidth + 3 + offset]);

            if (!ret[4])
            {
                int aa = 0;
            }

            return ret;
        }

        private bool GetImagePointResult(int value)
        {
            return value < 127;
        }
    }
}
