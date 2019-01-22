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

        public const string COLISION_FALSE = "false";
        public const string COLISION_TRUE = "true";
        public const string COLISION_START = "start";
        public const string COLISION_END = "end";


        public Maze(string mazeUrl)
        {
            _mazeStructure = LoadFromImage(mazeUrl);

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
                        if(_mazeStructure[column, row].Top == MazeSector.WALL)
                        {
                            _wallHorizontal.Draw();
                        }
                    }

                    if (column == 0)
                    {
                        if (_mazeStructure[column, row].Left == MazeSector.WALL)
                        {
                            _wallVertical.Draw();
                        }
                    }

                    if (_mazeStructure[column, row].Bottom == MazeSector.WALL)
                    {
                        gl.PushMatrix();
                        gl.Translatef(0, 0, Delta);
                        _wallHorizontal.Draw();
                        gl.PopMatrix();
                    }
                    gl.Translatef(Delta, 0, 0);

                    if (_mazeStructure[column, row].Right == MazeSector.WALL)
                    {
                        _wallVertical.Draw();
                    }
                }
                gl.PopMatrix();
                gl.Translatef(0, 0, Delta);
            }

            gl.PopMatrix();
        }
        

        public string IsColision(float fromX, float fromY, float toX, float toY)
        {
            float relativeColumn = fromX - _mazeOffset;
            float relativeRow = fromY - _mazeOffset;
            int wallIndexColumn = (int)Math.Floor(relativeColumn / Delta);
            int wallIndexRow = (int)Math.Floor(relativeRow / Delta);
            int maxSize = _mazeLength - 1;
            string item = "";

            wallIndexColumn = Math.Max(wallIndexColumn, 0);
            wallIndexColumn = Math.Min(wallIndexColumn, maxSize);
            wallIndexRow = Math.Max(wallIndexRow, 0);
            wallIndexRow = Math.Min(wallIndexRow, maxSize);

            //Center
            item = _mazeStructure[wallIndexColumn, wallIndexRow].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset);
            if (item != Maze.COLISION_FALSE)
            {
                return item;
            }

            //Left bottom
            if(wallIndexColumn > 0 && wallIndexRow > 0)
            {
                item = _mazeStructure[wallIndexColumn - 1, wallIndexRow - 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset);
                if (item != Maze.COLISION_FALSE)
                {
                    return item;
                }
            }

            //Left top
            if (wallIndexColumn > 0 && wallIndexRow < maxSize)
            {
                item = _mazeStructure[wallIndexColumn - 1, wallIndexRow + 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset);
                if (item != Maze.COLISION_FALSE)
                {
                    return item;
                }
            }

            
            //Right bottom
            if (wallIndexColumn < maxSize && wallIndexRow > 0)
            {
                item = _mazeStructure[wallIndexColumn + 1, wallIndexRow - 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset);
                if (item != Maze.COLISION_FALSE)
                {
                    return item;
                }
            }

            //Right top
            if (wallIndexColumn < maxSize && wallIndexRow < maxSize)
            {
                item = _mazeStructure[wallIndexColumn + 1, wallIndexRow + 1].IsColision(relativeColumn, relativeRow, toX - _mazeOffset, toY - _mazeOffset);
                if (item != Maze.COLISION_FALSE)
                {
                    return item;
                }
            }


            return Maze.COLISION_FALSE;
        }

        public MazeSector[,] LoadFromImage(string url)
        {
            BMPImageLoader loader = new BMPImageLoader();
            

            BMPImage mazeImage = loader.LoadDisplacementMap(url);
            _mazeLength = mazeImage.Width;
            MazeSector[,] _mazeStructure = new MazeSector[_mazeLength, _mazeLength];
            string isTop = MazeSector.NONE;
            string isRight = MazeSector.NONE;
            string isBottom = MazeSector.NONE;
            string isLeft = MazeSector.NONE;
            bool isVertical = false;
            bool isHorizontal = false;
            bool isEnd = false;
            bool isStart = false;
            bool[] imagePoint = new bool[9];
            int index = 0;
       
            for (int row = 0; row < _mazeLength; row++)
            {
                for (int column = 0; column < _mazeLength; column++)
                {
                    index = (column * 3) + (row * 3 * mazeImage.Width);
                    imagePoint = GetAurounImagePoint(mazeImage, index);
                    isEnd = !GetImagePointResult(mazeImage.ImageData[index + 2]) && imagePoint[4];
                    isStart = !GetImagePointResult(mazeImage.ImageData[index + 1]) && imagePoint[4];

                    if (row != 0 && column != 0)
                    {
                        isTop = _mazeStructure[column, Math.Max(row - 1, 0)].Bottom;
                        isLeft = _mazeStructure[Math.Max(column - 1, 0), row].Right;
                    }
                    else
                    {
                        isTop = MazeSector.NONE;
                        isLeft = MazeSector.NONE;
                    }
                    isVertical = imagePoint[4] && (imagePoint[1] || imagePoint[7]);
                    isHorizontal = imagePoint[4] && (imagePoint[3] || imagePoint[5]);
                    isRight = isHorizontal? BoolToMazeSectorType(imagePoint[4] && imagePoint[1]) : BoolToMazeSectorType(isVertical);
                    isBottom = isVertical ? BoolToMazeSectorType(imagePoint[4] && imagePoint[3]) : BoolToMazeSectorType(isHorizontal);

                    if (isStart)
                    {
                        if (isRight == MazeSector.WALL && (column == _mazeLength - 1 || column == 0))
                        {
                            isRight = MazeSector.START;
                        }
                        if (isBottom == MazeSector.WALL && (row == _mazeLength - 1 || row == 0))
                        {
                            isBottom = MazeSector.START;
                        }
                    }

                    if (isEnd)
                    {
                        if (isRight == MazeSector.WALL && (column == _mazeLength - 1 || column == 0))
                        {
                            isRight = MazeSector.END;
                        }
                        if (isBottom == MazeSector.WALL && (row == _mazeLength - 1 || row == 0))
                        {
                            isBottom = MazeSector.END;
                        }
                    }

                    _mazeStructure[column, row] = new MazeSector()
                    {
                        Top = isTop,
                        Right = isRight,
                        Bottom = isBottom,
                        Left = isLeft
                    };
                }
            }


            return _mazeStructure;
        }

        private bool[] GetAurounImagePoint(BMPImage image, int index)
        {

            int imageWidth = image.Width * 3;

            bool isOnTop = index < imageWidth;
            bool isOnRight = index % imageWidth == imageWidth - 3;
            bool isOnBottom = index >= imageWidth * (image.Height - 1);
            bool isOnLeft = index % image.Width == 0;

            bool[] ret = new bool[9];
            ret[0] = isOnTop || isOnLeft ? false : GetImagePointResult(image.ImageData[index - imageWidth - 3]);
            ret[1] = isOnTop ? false : GetImagePointResult(image.ImageData[index - imageWidth]);
            ret[2] = isOnTop || isOnRight ? false : GetImagePointResult(image.ImageData[index - imageWidth + 3]);

            ret[3] = isOnLeft ? false : GetImagePointResult(image.ImageData[index - 3]);
            ret[4] = GetImagePointResult(image.ImageData[index]); // middle
            ret[5] = isOnRight ? false : GetImagePointResult(image.ImageData[index + 3]);

            ret[6] = isOnBottom || isOnLeft ? false : GetImagePointResult(image.ImageData[index + imageWidth - 3]);
            ret[7] = isOnBottom ? false : GetImagePointResult(image.ImageData[index + imageWidth]);
            ret[8] = isOnBottom || isOnRight ? false : GetImagePointResult(image.ImageData[index + imageWidth + 3]);

            return ret;
        }

        private string BoolToMazeSectorType(bool val)
        {
            return val ? MazeSector.WALL : MazeSector.NONE;
        }

        private bool GetImagePointResult(int value)
        {
            return value < 127;
        }

        public Tuple<float, float> GetStartPosition()
        {
            for (int row = 0; row < _mazeLength; row++)
            {
                for (int column = 0; column < _mazeLength; column++)
                {
                    if(_mazeStructure[column, row].Top == Maze.COLISION_START)
                    {
                        return new Tuple<float, float>(_mazeOffset + column * _delta + (_delta / 2), _mazeOffset + row * _delta - _delta * 1.5f);
                    }

                    if (_mazeStructure[column, row].Right == Maze.COLISION_START)
                    {
                        return new Tuple<float, float>(_mazeOffset + column * _delta + _delta * 1.5f, _mazeOffset + row * _delta + (_delta / 2));
                    }

                    if (_mazeStructure[column, row].Bottom == Maze.COLISION_START)
                    {
                        return new Tuple<float, float>(_mazeOffset + column * _delta + (_delta / 2), _mazeOffset + row * _delta + _delta * 0.5f);
                    }

                    if (_mazeStructure[column, row].Left == Maze.COLISION_START)
                    {
                        return new Tuple<float, float>(_mazeOffset + column * _delta - _delta * 1.5f, _mazeOffset + row * _delta + (_delta / 2));
                    }
                }
            }

            return new Tuple<float, float>(0.7f, 0.2f);
        }
    }
}
