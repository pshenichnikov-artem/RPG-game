using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Xml;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Game.Units
{
    internal abstract class Unit : GameObject, IOnCollision, IFixedUpdate
    {
        protected event Action _changeHealth;
        public event Action ChangeHealth{
          add { _changeHealth += value; } remove { _changeHealth -= value; } }

        private Collider _collider;
        public Collider GetCollider() => _collider;

        protected Animator _animator;

        protected Dictionary<string, bool> _action;

        protected int _maxHealth;

        protected bool isLanding = false;
        public bool IsDead => _action["Dead"];
        public bool IsAttack => _action["Attack"];
        public int Speed { get; protected set; }
        public int MinDamage { get; protected set; }
        public int MaxDamage { get; protected set; }
        public int Health { get; protected set; }

        protected void ResetAction()
        {
            foreach (string key in _action.Keys)
            {
                _action[key] = false;
            }
        }

        protected virtual void OnChangeHealth()
        {
            _changeHealth?.Invoke();
        }

        protected virtual void Dead()
        {
            GameWindow.Destroy(this);
            GameWindow.Destroy(_animator);
            GameWindow.Destroy(_collider);
        }

        public virtual void Move(int direction)
        {
            transform.Direction = direction;

            transform.Position = new Vector2D(transform.Position.X + (int)(Speed * (Time.DeltaTime + 0.5f)) * transform.Direction,
                transform.Position.Y);
        }

        public abstract void TakeDamage(int damage);

        public Unit(Vector2D position, Size size, int speed, int minDamage, int maxDamage, int maxHealth) :
            base(position, size)
        {
            Speed = speed;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            _maxHealth = maxHealth;
            Health = maxHealth;
            _action = new Dictionary<string, bool>();
            _animator = new Animator(_image, new OffsetParam(128,128,37,128 - 37));
            //128 37 128 128-37
            _collider = new Collider(transform, 0, 0, 0, 0);
        }


        public virtual void OnCollision(CollisionEventArgs args)
        {
            List<ICollider> gameObjects = args.collidedWith;

            foreach (ICollider gameObject in gameObjects)
            {
                if (gameObject is Plane plane)
                {
                    Rectangle unitColliderRect = _collider.GetColliderRectangle();


                    (int offsetX, int offsetY) = CollisionManager.GetOffsetColliderRectangle
                    (
                            unitColliderRect,
                            gameObject.GetCollider().GetColliderRectangle(),
                            unitColliderRect.Width / 2.5f,
                            10
                        );

                    offsetX *= -1;
                    offsetY *= -1;

                  //  Debug.WriteLine(offsetY);
                    if (offsetY <= 0) isLanding = true;

                    offsetX = offsetX < 0 ? offsetX -= 1 : offsetX > 0 ? offsetX += 1 : offsetX;
                    offsetY = offsetY < 0 ? offsetY -= 1 : offsetY > 0 ? offsetY += 1 : offsetY;

                    if (offsetY != 0 && Math.Abs(offsetX) == 8) offsetX = 0;

                    transform.Position = new Vector2D(transform.Position.X + offsetX,
                         transform.Position.Y + offsetY);
                }
            }
        }

        public void FixedUpdate()
        {
            isLanding = false;
        }
    }
}
