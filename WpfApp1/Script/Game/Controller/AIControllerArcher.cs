using System.Collections.Generic;
using System;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Game.Controller
{
    internal class AIControllerArcher : IUpdate
    {
        private SamuraiArcher _archer;

        private int _skipTick = 0;
        private Action? _continueAction;

        void IUpdate.Update()
        {
            if (_archer.IsDead)
            {
                GameWindow.Destroy(this);
                return;
            }

            if (_skipTick > 0)
            {
                _skipTick--;
                if (_continueAction != null) _continueAction();
                return;
            }

            Unit? targetUnit = FindTarget();

            if (targetUnit == null)
            {
                Patrol();
            }
            else
            {
                int offset = Math.Abs(targetUnit.transform.Position.X - _archer.transform.Position.X);
                _archer.transform.Direction = targetUnit.transform.Position.X - _archer.transform.Position.X;
                if (offset < 80)
                {
                    _archer.Attack();
                    SkipTick(100, () => _archer.Idle());
                }
                else
                {
                    _archer.Shot();
                    SkipTick(300, () => _archer.Idle());
                }
            }
        }

        private Vector2D _startPosition;
        private bool _moveToRight = false;

        private void Patrol()
        {
            if (Math.Abs(_archer.transform.Position.X - _startPosition.X) <= 500 && CheckPlain())
            {
                if (_moveToRight) _archer.Move(1);
                else _archer.Move(-1);
            }
            else
            {
                SkipTick(300, () =>
                {
                    if (FindTarget() is Samurai) SkipTick(0);
                    if (_skipTick == 1) _archer.transform.Direction *= -1;
                    _archer.Idle();
                });
                _startPosition = _archer.transform.Position;
                _moveToRight = !_moveToRight;
            }
        }

        private bool CheckPlain()
        {
            Rectangle rectangle = Rectangle.FromLTRB(
                        _archer.transform.Position.X + 20 * _archer.transform.Direction - 1 * (_archer.transform.Direction == -1 ? 1 : 0),
                        _archer.transform.Position.Y + _archer.transform.Size.Height,

                        _archer.transform.Position.X + 20 * _archer.transform.Direction - +(1 * (_archer.transform.Direction == 1 ? 1 : 0)),
                        _archer.transform.Position.Y + _archer.transform.Size.Height + 20
                    );
           // GameWindow.DebugRectangle(rectangle);
            var land = CollisionManager.CheckCollisionWithColliders<Plane>(
                rectangle, this
                );

            rectangle = Rectangle.FromLTRB(
                        _archer.transform.Position.X + 40 * _archer.transform.Direction -
                        1 * (_archer.transform.Direction == -1 ? 1 : 0),

                        _archer.transform.Position.Y + _archer.transform.Size.Height / 3,

                        _archer.transform.Position.X + 40 * _archer.transform.Direction +
                        1 * (_archer.transform.Direction == 1 ? 1 : 0),

                        _archer.transform.Position.Y + _archer.transform.Size.Height - 20
                    );
          //  GameWindow.DebugRectangle(rectangle);

            var wall = CollisionManager.CheckCollisionWithColliders<Plane>(
                rectangle, this
                );

            return land.Count != 0 && wall.Count == 0;
        }

        private void SkipTick(int countTick, Action? action = null)
        {
            _skipTick = countTick;
            _continueAction = action;
        }

        private Unit? FindTarget()
        {
            Unit? targetUnit = null;
            Rectangle rectangle = Rectangle.FromLTRB(
                        _archer.transform.Position.X - 500,
                        _archer.transform.Position.Y + _archer.transform.Size.Height / 3,

                        _archer.transform.Position.X + 500,
                        _archer.transform.Position.Y + _archer.transform.Size.Height - 20
                    );

     //       GameWindow.DebugRectangle(rectangle);

            List<GameObject> gameObjects = CollisionManager.CheckCollisionWithColliders<GameObject>(rectangle, _archer);
            GameObject? gameObject = gameObjects.FirstOrDefault(obj => obj is Samurai);

            if (gameObject != null && gameObject is Samurai)
            {
                targetUnit = (Unit)gameObject;
            }
            return targetUnit;
        }

        public AIControllerArcher(SamuraiArcher archer)
        {
            _archer = archer;
            _startPosition = _archer.transform.Position;
        }
    }

}
