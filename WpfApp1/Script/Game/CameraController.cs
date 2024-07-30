using System.Windows.Controls;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;
using System.Diagnostics;

namespace WpfApp1.Script.Game
{
    internal class CameraController : IRenderImage
    {
        double screenWidth;
        double screenHeight;

        double backgroundWidth;
        double backgroundHeight; 

        private readonly Transform _targetTransform;
        
        void IRenderImage.Render()
        {
            double targetX = _targetTransform.Position.X;
            double targetY = _targetTransform.Position.Y;

            if (!(targetX < screenWidth / 2) && !(backgroundWidth - targetX < screenWidth / 2 + 64))
            {
                Canvas.SetLeft(GameWindow.Instance.BackGround, screenWidth / 2 - targetX);
            }
           
            if(!(backgroundHeight - targetY > 800) && !(backgroundHeight - targetY < /*580*/350))
            {
                Canvas.SetTop(GameWindow.Instance.BackGround, 1080 / 2 - targetY);
            }
        }

        public CameraController(Transform targetTransform)
        {
            GameWindow.Instantiate(this);

            _targetTransform = targetTransform;

            backgroundWidth = GameWindow.Instance.BackGround.Width;
            backgroundHeight = GameWindow.Instance.BackGround.Height;

            screenWidth = GameWindow.Instance.canvas.ActualWidth;
            screenHeight = GameWindow.Instance.canvas.ActualWidth;

            Canvas.SetLeft(GameWindow.Instance.BackGround, 10);
            Canvas.SetTop(GameWindow.Instance.BackGround, -backgroundHeight + screenHeight - 180);
        }
    }
}
