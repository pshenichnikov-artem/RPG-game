using System.Drawing;

namespace WpfApp1.Script.Engine.Core
{
    internal class Transform
    {
        public Vector2D Position { get; set; }
        public Size Size { get; private set; }
        private int _direction;

        public int Direction
        {
            get { return _direction; }
            set
            {
                if (value < 0) _direction = -1;
                else _direction = 1;
            }
        }

        public Transform(int x, int y, int width, int height)
        {
            Position = new Vector2D(x, y);
            Size = new Size(width, height);
            _direction = 1;
        }

        public Transform(Vector2D position, Size size)
        {
            Position = position;
            Size = size;
            _direction = 1;
        }
    }
}
