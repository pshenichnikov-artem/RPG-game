using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Game
{
    internal class Plane : GameObject, ICollider, IRenderImage
    {
        private Collider? _collider;
        bool isRender = false;

        public Plane(Vector2D position, Size size, string path, OffsetParam param = default) : base(position, size)
        {
            _collider = new Collider(transform, param.offsetLeft, param.offsetRight, param.offsetTop, param.offsetBottom);
            _image.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        public Plane(Vector2D position, Size size, string path, Collider? collider) : base(position, size)
        {
            _collider = collider;
            _image.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        //protected override void Update()
        //{
        //    if(_collider != null)
        //        GameWindow.DebugRectangle(_collider.GetColliderRectangle());
        //}

        void IRenderImage.Render()
        {
            if (isRender) return;

            _image.Width = transform.Size.Width;
            _image.Height = transform.Size.Height;
            _image.Stretch = Stretch.Fill;
            _image.RenderTransform = new ScaleTransform(transform.Direction, 1);
            Canvas.SetLeft(_image, transform.Position.X - transform.Size.Width / 2 * transform.Direction);
            Canvas.SetTop(_image, transform.Position.Y);

            isRender = true;
        }

        public Collider GetCollider() => _collider;
    }
}

public struct OffsetParam
{
    public int offsetLeft;
    public int offsetRight;
    public int offsetTop;
    public int offsetBottom;

    public OffsetParam(int _offsetLeft,
                    int _offsetRight,
                    int _offsetTop,
                    int _offsetBottom)
    {
        ;
        offsetLeft = _offsetLeft;
        offsetRight = _offsetRight;
        offsetTop = _offsetTop;
        offsetBottom = _offsetBottom;
    }
}
