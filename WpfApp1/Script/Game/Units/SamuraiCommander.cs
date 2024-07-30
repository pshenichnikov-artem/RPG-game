using WpfApp1.Script.Engine.Core;
using System.Drawing;
using WpfApp1.Script.Engine.Interfaces;
using System.Diagnostics;
using System.Transactions;
using System;

namespace WpfApp1.Script.Game.Units
{
    internal class SamuraiCommander : Unit, IFall
    {

        protected override void Update()
        {
            if (_action["Dead"]) return;

          //  GameWindow.DebugRectangle(GetCollider().GetColliderRectangle());
        }

        public override void Move(int direction)
        {
            if (_action["Attack"] || _action["Dead"]) return;

            base.Move(direction);

            if (!_action["Jump"] && isLanding)
            {
                if(Speed <=4)
                    _animator.SetAnimation("Walk");
                else
                    _animator.SetAnimation("Run");
            }
        }

        public void Idle()
        {
            if (_action["Attack"] || !isLanding || _action["Dead"]) return;

            if (isLanding)
                _animator.SetAnimation("Idle");
        }

        public override void TakeDamage(int damage)
        {
            if (_action["Block"] || _action["Dead"]) return;

            Health -= damage;

            if (Health <= 0)
            {
                _action["Dead"] = true;
                Dead();
            }
        }

        protected override void Dead()
        {
            _animator.SetAnimation("Dead");
            _action["Dead"] = true;

            _animator.DoWorkInFrame(
                () =>
                {
                    _animator.DoWorkInFrame(() =>
                    {
                        _animator.StopAnimation = true;
                        base.Dead();
                    }, 0);
                },
                _animator.CountFrameInAnimation - 1
                );
        }

        public void Attack()
        {
            if (_action["Attack"] || _action["Jump"]) return;

            _animator.SetAnimation("Attack" + new Random().Next(1, 4).ToString());
            _action["Attack"] = true;

            _animator.DoWorkInFrame(() =>
            {
                HitEnemy();

                if (_animator.CurrentAction == "Attack2")
                {
                    _action["Attack"] = true;
                    _animator.DoWorkInFrame(() =>
                    {
                        _animator.StopAnimation = true;
                        _animator.SkipCountFrame = 10;
                        _animator.DoWorkInFrame(() => _action["Attack"] = false, 0);
                    }, 0);
                }
            }, _animator.CountFrameInAnimation - 1);
        }

        private void HitEnemy()
        {

            Rectangle rectangle = Rectangle.FromLTRB(
                        transform.Position.X - transform.Size.Width / 2 * (transform.Direction == -1 ? 1 : 0),
                        transform.Position.Y + transform.Size.Height / 3,

                        transform.Position.X + transform.Size.Width / 2 * (transform.Direction == 1 ? 1 : 0),
                        transform.Position.Y + transform.Size.Height - 20
                    );
            var enemyUnits = CollisionManager.CheckCollisionWithColliders<Unit>(
                rectangle, this
                );

        //    GameWindow.DebugRectangle(rectangle);

            int damage = new Random().Next(MinDamage, MaxDamage);
            foreach (var unit in enemyUnits)
            {
                unit.TakeDamage(damage);
            }

            _action["Attack"] = false;
        }

        public void Block()
        {

            if (_action["Attack"] || !isLanding || _action["Dead"]) return;

            _action["Block"] = true;
            _animator.SetAnimation("Block");

            if (_animator.CurrentFrame == _animator.CountFrameInAnimation - 1)
            {
                _animator.StopAnimation = true;
            }
        }


        void IFall.Landing()
        {
            if (!isLanding)
                transform.Position = new Vector2D(transform.Position.X, transform.Position.Y + 10);
        }

        public SamuraiCommander(Vector2D position, Size size, int speed, int minDamage, int maxDamage, int maxHealth)
            : base(position, size, speed, minDamage, maxDamage, maxHealth)
        {
            GetCollider().ChangeOffset(new OffsetParam(35, 55, 30, 1));

            _action.Add("Attack", false);
            _action.Add("Move", false);
            _action.Add("Block", false);
            _action.Add("Jump", false);
            _action.Add("Fall", false);
            _action.Add("Dead", false);

            _animator.AddAnimation("Idle", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Idle.png", 6);

            _animator.AddAnimation("Walk", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Walk.png", 9);

            _animator.AddAnimation("Run", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Run.png", 8);

            _animator.AddAnimation("Attack1", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Attack_1.png", 4);

            _animator.AddAnimation("Attack2", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Attack_2.png", 5);

            _animator.AddAnimation("Attack3", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Attack_3.png", 4);

            _animator.AddAnimation("Block", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Protection.png", 2);

            _animator.AddAnimation("Dead", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Dead.png", 6);

            _animator.AddAnimation("Jump", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai\\Jump.png", 9);

            _animator.SetAnimation("Idle");

            _animator.EndAnimationEvent += ResetAction;
        }
    }
}
