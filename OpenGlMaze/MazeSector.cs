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
        private bool _top = false;
        private bool _right = false;
        private bool _bottom = false;
        private bool _left = false;
        private float _wallSize = 0f;
        private WallPosition _wallPositions = new WallPosition();

        public bool Top { get => _top; set => _top = value; }
        public bool Right { get => _right; set => _right = value; }
        public bool Bottom { get => _bottom; set => _bottom = value; }
        public bool Left { get => _left; set => _left = value; }
        public WallPosition WallPositions { get => _wallPositions; set => _wallPositions = value; }
        public float WallSize { get => _wallSize; set => _wallSize = value; }

        public bool IsColision(float fromX, float fromY, float toX, float toY)
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
            
            //TODO calculate with wall thickness 
            if (Top)
            {
                if (
                    biggerY > _wallPositions.Top.Item2
                    && lowerY - 0.005f <= _wallPositions.Top.Item2
                    && toX >= _wallPositions.Top.Item1
                    && toX <= _wallPositions.Top.Item1 + _wallSize
                    )
                {
                    return true;
                }
            }

            if (Right)
            {
                if (
                    lowerX < _wallPositions.Right.Item1
                    && biggerX + 0.005f >= _wallPositions.Right.Item1
                    && toY >= _wallPositions.Right.Item2
                    && toY <= _wallPositions.Right.Item2 + _wallSize
                    )
                {
                    return true;
                }
            }

            if (Bottom)
            {
                if (
                    lowerY < _wallPositions.Bottom.Item2
                    && biggerY + 0.005f >= _wallPositions.Bottom.Item2
                    && toX >= _wallPositions.Bottom.Item1
                    && toX <= _wallPositions.Bottom.Item1 + _wallSize
                    )
                {
                    return true;
                }
            }

            if (Left)
            {
                if (
                    biggerX > _wallPositions.Left.Item1
                    && lowerX - 0.005f <= _wallPositions.Left.Item1
                    && toY >= _wallPositions.Left.Item2
                    && toY <= _wallPositions.Left.Item2 + _wallSize
                    )
                {
                    return true;
                }
            }


            return false;
        }
    }
}
