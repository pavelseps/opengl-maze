using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlMaze
{
    class WallPosition
    {
        public Tuple<float, float> Top;
        public Tuple<float, float> Right;
        public Tuple<float, float> Bottom;
        public Tuple<float, float> Left;
    }

    class MazeSector
    {
        public const string WALL = "wall";
        public const string NONE = "none";
        public const string START = "start";
        public const string END = "end";

        private string _top = MazeSector.NONE;
        private string _right = MazeSector.NONE;
        private string _bottom = MazeSector.NONE;
        private string _left = MazeSector.NONE;
        private float _wallSize = 0f;
        private WallPosition _wallPositions = new WallPosition();

        public string Top { get => _top; set => _top = value; }
        public string Right { get => _right; set => _right = value; }
        public string Bottom { get => _bottom; set => _bottom = value; }
        public string Left { get => _left; set => _left = value; }
        public WallPosition WallPositions { get => _wallPositions; set => _wallPositions = value; }
        public float WallSize { get => _wallSize; set => _wallSize = value; }

        public string IsColision(float fromX, float fromY, float toX, float toY)
        {
            float biggerX = 0;
            float lowerX = 0;
            float biggerY = 0;
            float lowerY = 0;

            if (fromX > toX)
            {
                biggerX = fromX;
                lowerX = toX;
            }
            else
            {
                biggerX = toX;
                lowerX = fromX;
            }

            if (fromY > toY)
            {
                biggerY = fromY;
                lowerY = toY;
            }
            else
            {
                biggerY = toY;
                lowerY = fromY;
            }
            
            if (Top == MazeSector.WALL || Top == MazeSector.END)
            {
                if (
                    biggerY > _wallPositions.Top.Item2
                    && lowerY - 0.005f <= _wallPositions.Top.Item2
                    && toX >= _wallPositions.Top.Item1
                    && toX <= _wallPositions.Top.Item1 + _wallSize
                    )
                {
                    return _top == MazeSector.END ? Maze.COLISION_END : Maze.COLISION_TRUE;
                }
            }

            if (Right == MazeSector.WALL || Right == MazeSector.END)
            {
                if (
                    lowerX < _wallPositions.Right.Item1
                    && biggerX + 0.005f >= _wallPositions.Right.Item1
                    && toY >= _wallPositions.Right.Item2
                    && toY <= _wallPositions.Right.Item2 + _wallSize
                    )
                {
                    return _right == MazeSector.END ? Maze.COLISION_END : Maze.COLISION_TRUE;
                }
            }

            if (Bottom == MazeSector.WALL || Bottom == MazeSector.END)
            {
                if (
                    lowerY < _wallPositions.Bottom.Item2
                    && biggerY + 0.005f >= _wallPositions.Bottom.Item2
                    && toX >= _wallPositions.Bottom.Item1
                    && toX <= _wallPositions.Bottom.Item1 + _wallSize
                    )
                {
                    return _bottom == MazeSector.END ? Maze.COLISION_END : Maze.COLISION_TRUE;
                }
            }

            if (Left == MazeSector.WALL || Left == MazeSector.END)
            {
                if (
                    biggerX > _wallPositions.Left.Item1
                    && lowerX - 0.005f <= _wallPositions.Left.Item1
                    && toY >= _wallPositions.Left.Item2
                    && toY <= _wallPositions.Left.Item2 + _wallSize
                    )
                {
                    return _left == MazeSector.END ? Maze.COLISION_END : Maze.COLISION_TRUE;
                }
            }


            return Maze.COLISION_FALSE;
        }
    }
}
