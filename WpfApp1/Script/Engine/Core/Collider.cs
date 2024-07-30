using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Engine.Core
{
    internal class Collider
    {
        private readonly Transform _transform;

        private OffsetParam _offsetParam;
        public OffsetParam OffsetParam => _offsetParam;

        public Rectangle GetColliderRectangle() =>
            Rectangle.FromLTRB(
                    _transform.Position.X - _transform.Size.Width / 2 + (_transform.Direction == -1 ? _offsetParam.offsetRight : _offsetParam.offsetLeft),
                _transform.Position.Y + _offsetParam.offsetTop,
                _transform.Position.X + _transform.Size.Width / 2 - (_transform.Direction == 1 ? _offsetParam.offsetRight : _offsetParam.offsetLeft),
                _transform.Position.Y + _transform.Size.Height - _offsetParam.offsetBottom);

        public void ChangeOffset(OffsetParam offsetParam)
        {
            _offsetParam = offsetParam;
        }

        public bool CheckColliderIntersection(Rectangle rectangle) => GetColliderRectangle().IntersectsWith(rectangle);

        public Collider(Transform transform, int offsetLeft, int offsetRight, int offsetTop, int offsetBottom)
        {
            _transform = transform;
            ChangeOffset(new OffsetParam(offsetLeft, offsetRight, offsetTop, offsetBottom));
        }

        public Collider(Transform transform, OffsetParam offsetParam)
        {
            _transform = transform;
            ChangeOffset(offsetParam);
        }
    }
}
