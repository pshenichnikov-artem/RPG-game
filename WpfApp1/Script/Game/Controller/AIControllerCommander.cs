using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;
using System.Diagnostics;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Game.Controller
{
    internal class AIControllerCommander : IUpdate
    {
        private SamuraiCommander _commander;

        private int _skipTick = 0;
        private Action? _continueAction;

        void IUpdate.Update()
        {
            if (_commander.IsDead)
            {
                GameWindow.Destroy(this);
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
                if (Approach(targetUnit) == false)
                {
                    if (targetUnit.IsAttack && new Random().Next(0, 100) < 30)
                    {
                        _commander.Block();
                        SkipTick(100, () => _commander.Block());
                    }
                    else
                    {
                        _commander.Attack();
                        SkipTick(100, () => _commander.Idle());
                    }
                }
            }
        }

        private Vector2D _startPosition;
        private bool _moveToRight = false;

        private void Patrol()
        {
            if (Math.Abs(_commander.transform.Position.X - _startPosition.X) <= 500 && CheckPlain())
            {
                if (_moveToRight) _commander.Move(1);
                else _commander.Move(-1);
            }
            else
            {
                SkipTick(300, () =>
                {
                    if (FindTarget() is Samurai)
                    {
                        SkipTick(0);
                    }
                    else
                    {
                        if (_skipTick == 1) _commander.transform.Direction *= -1;
                        _commander.Idle();
                    }
                });
                _startPosition = _commander.transform.Position;
                _moveToRight = !_moveToRight;
            }
        }

        private bool CheckPlain()
        {
            Rectangle rectangle = Rectangle.FromLTRB(
                        _commander.transform.Position.X + 20 * _commander.transform.Direction - 1 * (_commander.transform.Direction == -1 ? 1 : 0),
                        _commander.transform.Position.Y + _commander.transform.Size.Height,

                        _commander.transform.Position.X + 20 * _commander.transform.Direction - +(1 * (_commander.transform.Direction == 1 ? 1 : 0)),
                        _commander.transform.Position.Y + _commander.transform.Size.Height + 20
                    );
          //  GameWindow.DebugRectangle(rectangle);
            var land = CollisionManager.CheckCollisionWithColliders<Plane>(
                rectangle, this
                );

            rectangle = Rectangle.FromLTRB(
                        _commander.transform.Position.X + 20 * _commander.transform.Direction -
                        1 * (_commander.transform.Direction == -1 ? 1 : 0),

                        _commander.transform.Position.Y + _commander.transform.Size.Height / 3,

                        _commander.transform.Position.X + 20 * _commander.transform.Direction +
                        1 * (_commander.transform.Direction == 1 ? 1 : 0),

                        _commander.transform.Position.Y + _commander.transform.Size.Height - 20
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
                        _commander.transform.Position.X - 500,
                        _commander.transform.Position.Y + _commander.transform.Size.Height / 3,

                        _commander.transform.Position.X + 500,
                        _commander.transform.Position.Y + _commander.transform.Size.Height - 20
                    );

           // GameWindow.DebugRectangle(rectangle);

            List<GameObject> gameObjects = CollisionManager.CheckCollisionWithColliders<GameObject>(rectangle, _commander);
            GameObject? gameObject = gameObjects.FirstOrDefault(obj => obj is Samurai);

            if (gameObject != null)
            {
                targetUnit = (Unit)gameObject;
            }

            return targetUnit;
        }

        private bool Approach(Unit targetUnit)
        {
            bool isMove = false;
            if (targetUnit.transform.Position.X - 40 > _commander.transform.Position.X)
            {
                _commander.Move(1);
                isMove = true;
            }
            else if (targetUnit.transform.Position.X + 40 < _commander.transform.Position.X)
            {
                _commander.Move(-1);
                isMove = true;
            }
            return isMove;
        }

        public AIControllerCommander(SamuraiCommander commander)
        {
            _commander = commander;
            _startPosition = _commander.transform.Position;
        }
    }
}
