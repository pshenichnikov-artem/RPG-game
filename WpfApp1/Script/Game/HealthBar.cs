using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Game
{
    internal class HealthBar
    {
        private Unit _unit;
        private ConcurrentStack<Image> _heartImage;

        private void ChangeHealth()
        {
            if (_unit.Health < _heartImage.Count)
            {
                for (int i = _heartImage.Count; i > 0 && i > _unit.Health; i--)
                {
                    if (_heartImage.TryPop(out Image image))
                    {
                        GameWindow.DestoyImage(image);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _unit.Health; i++)
                {
                    _heartImage.Push(CreateHeart());
                }
            }
        }

        private Image CreateHeart()
        {
            Image image = GameWindow.CreateUIImage();
            GameWindow.DoInMainThread(() =>
            {
                image.Source = (new BitmapImage(new Uri(GameWindow.GetDirectory() + "\\Resource\\heart.png", UriKind.RelativeOrAbsolute)));
                image.Width = 32;
                image.Stretch = Stretch.Fill;
                Canvas.SetLeft(image, 30 + 40 * _heartImage.Count - 1);
                Canvas.SetTop(image, 10);
            });
            return image;
        }

        public HealthBar(Unit unit)
        {
            _unit = unit;
            _heartImage = new ConcurrentStack<Image>();
            unit.ChangeHealth += ChangeHealth;

            for(int i=0;i<unit.Health;i++)
            {
                _heartImage.Push(CreateHeart());
            }
        }
    }
}
