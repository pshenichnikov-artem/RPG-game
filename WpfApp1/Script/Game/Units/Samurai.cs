using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Game.Units
{
    internal class Samurai : Unit, IFall
    {
        protected override void Update()
        {
            if (_action["Jump"])
                transform.Position = new Vector2D(transform.Position.X, transform.Position.Y - 17);

            if (_action["Block"] == false && _animator.CurrentAction == "Block" || isLanding && _animator.CurrentAction == "Jump")
                _animator.StopAnimation = false;

         //   GameWindow.DebugRectangle(GetCollider().GetColliderRectangle());

        }

        public void Idle()
        {
            if (_action["Attack"] || !isLanding || _action["Dead"]) return;

            if (isLanding)
                _animator.SetAnimation("Idle");
        }

        public override void Move(int direction)
        {
            if (_action["Attack"] || _action["Dead"]) return;

            base.Move(direction);

            if (!_action["Jump"] && isLanding)
                _animator.SetAnimation("Run");
        }

        public void Jump()
        {
            if (isLanding == false || _action["Attack"] || _action["Jump"] || _action["Dead"])
                return;

            if(_animator.CurrentAction == "Jump")
            {
                _animator.SetAnimation("Idle");
            }

            _action["Jump"] = true;

            _animator.SetAnimation("Jump");
            //TODO
            _animator.DoWorkInFrame(() =>
            {
                _animator.StopAnimation = true;
                _action["Jump"] = false;
            }, 5);
        }


        public void Attack()
        {
            if (_action["Attack"] || _action["Jump"] || _action["Dead"]) return;

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

       //     GameWindow.DebugRectangle(rectangle);

            int damage = new Random().Next(MinDamage, MaxDamage);
            foreach (var unit in enemyUnits)
            {
                unit.TakeDamage(damage);
            }

            _action["Attack"] = false;
        }

        private bool isCanParry = true;

        public void Block()
        {
            if (_action["Attack"] || _action["Jump"] || !isLanding || _action["Dead"]) return;

            _action["Block"] = true;
            _animator.SetAnimation("Block");

            //TODO
            if (_animator.CurrentFrame == 0 /*&& isCanParry*/)
            {
                _action["Parry"] = true;
                isCanParry = false;
                Time.Invoke(() => { _action["Parry"] = false;  }, 200);
                Time.Invoke(() => { isCanParry = true; }, 400);
            }
            else if (_animator.CurrentFrame == _animator.CountFrameInAnimation - 1)
            {
                _animator.StopAnimation = true;
            }
        }


        public override void TakeDamage(int damage)
        {
            if (_action["Parry"] || _action["Dead"])
            {
              //  Debug.WriteLine("succses parry");
                return;
            }

            int resultDamage = _action["Block"] ? (int)Math.Round(damage * 0.75f) : damage;
       //     Debug.WriteLine($"{resultDamage}, {damage}");
            if (Health - resultDamage <= 0)
            {
                Health = 0;
                _action["Dead"] = true;
                Dead();
            }
            else Health -= resultDamage;

            OnChangeHealth();
        }

        protected override void Dead()
        {
            _animator.SetAnimation("Dead");
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

        void IFall.Landing()
        {
            if (!isLanding)
                transform.Position = new Vector2D(transform.Position.X, transform.Position.Y + 10);
        }

        public Samurai(Vector2D position, Size size, int speed, int minDamage, int maxDamage, int maxHealth, int currentHealth = -1)
            : base(position, size, speed, minDamage, maxDamage, maxHealth)
        {
            GetCollider().ChangeOffset(new OffsetParam(35, 55, 30, 1));

            if (currentHealth != -1)
            {
                Health = currentHealth;
                OnChangeHealth();
            }

            _action.Add("Attack", false);
            _action.Add("Move", false);
            _action.Add("Block", false);
            _action.Add("Jump", false);
            _action.Add("Fall", false);
            _action.Add("Dead", false);
            _action.Add("Parry", false);

            _animator.AddAnimation("Idle", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Idle.png", 5);

            _animator.AddAnimation("Hurt", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Hurt.png", 2);

            _animator.AddAnimation("Run", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Run.png", 8);

            _animator.AddAnimation("Attack1", GameWindow.GetDirectory()+
                "\\Resource\\Units\\Samurai_Commander\\Attack_1.png", 4);

            _animator.AddAnimation("Attack2", GameWindow.GetDirectory()+
                "\\Resource\\Units\\Samurai_Commander\\Attack_2.png", 5);

            _animator.AddAnimation("Attack3", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Attack_3.png", 4);

            _animator.AddAnimation("Block", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Protect.png", 2);

            _animator.AddAnimation("Dead", GameWindow.GetDirectory()+
                "\\Resource\\Units\\Samurai_Commander\\Dead.png", 6);

            _animator.AddAnimation("Jump", GameWindow.GetDirectory() +
                "\\Resource\\Units\\Samurai_Commander\\Jump.png", 7);

            _animator.SetAnimation("Idle");

            _animator.EndAnimationEvent += ResetAction;
        }
    }
}