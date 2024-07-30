using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Game
{
    internal class Portal : GameObject, IOnCollision
    {
        private Collider _collider;
        private Animator _animator;
        private Action _action;

        //protected override void Update()
        //{
        //   // GameWindow.DebugRectangle(_collider.GetColliderRectangle());
        //}

        public Portal(Vector2D position, Size size, Action action) : base(position, size)
        {
            _collider = new Collider(transform, new OffsetParam(24, 20, 30, 30));
            _animator = new Animator(_image, new OffsetParam(250, 250, 0, 500));
            _animator.AddAnimation("Idle", GameWindow.GetDirectory() + "\\Resource\\portal.png", 4);
            _animator.SetAnimation("Idle");
            _action = action;
        }

        Collider ICollider.GetCollider() => _collider;

        void IOnCollision.OnCollision(CollisionEventArgs args)
        {
            Samurai samurai = (Samurai)args.collidedWith.FirstOrDefault(s => s is Samurai);

            if (samurai != null)
            {
                _action(); 
            }
        }
    }
}
