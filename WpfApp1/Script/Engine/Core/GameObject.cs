using System;
using System.Drawing;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Engine.Core
{
    internal abstract class GameObject : IRenderImage, IUpdate, IStart, IAwake
    {
        protected Image _image { get; private set; }
        public Transform transform { get; private set; }

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void Update() { }

        protected GameObject(Vector2D position, Size size)
        {
            transform = new Transform(position, size);
            _image = GameWindow.CreateImage();
        }

        void IAwake.Awake() => Awake();
        void IStart.Start() => Start();
        void IUpdate.Update() => Update();

        void IRenderImage.Render()
        {
            _image.Width = transform.Size.Width;
            _image.Height = transform.Size.Height;
            _image.Stretch = Stretch.Fill;
            _image.RenderTransform = new ScaleTransform(transform.Direction, 1);
            Canvas.SetLeft(_image, transform.Position.X - transform.Size.Width/2 * transform.Direction);
            Canvas.SetTop(_image, transform.Position.Y);
        }
    }
}
