using System.Drawing;

namespace WpfApp1.Script.Engine.Core
{
    internal struct Vector2D
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static implicit operator Point(Vector2D vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static implicit operator PointF(Vector2D vector)
        {
            return new PointF(vector.X, vector.Y);
        }
    }
}
