using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Game.Controller
{
    internal class PlayerController : IUpdate
    {
        private CameraController _camera;
        private Samurai _samurai;
        private HealthBar _healthBar;
        private bool isDoAction;

        public void Update()
        {
            isDoAction = false;

            if (Input.Z)
            {
                _samurai.Attack();
                isDoAction = true;
            }
            if (Input.Up)
            {
                _samurai.Jump();
                isDoAction = true;
            }

            if (Input.X)
            {
                _samurai.Block();
                isDoAction = true;
            }
            if (Input.Right && !isDoAction)
            {
                _samurai.Move(1);
                isDoAction = true;
            }
            if (Input.Left && !isDoAction)
            {
                _samurai.Move(-1);
                isDoAction = true;
            }

            if (!isDoAction)
            {
                _samurai.Idle();
            }
        }

        private void Destroy()
        {
            if (_samurai.Health == 0)
            {
                GameWindow.Destroy(this);
                Time.Invoke(()=> GameWindow.CreateAndAnimateTransparentOverlay(), 5000);
            }
        }

        public PlayerController(Samurai samurai)
        {
            _samurai = samurai;
            _camera = new CameraController(samurai.transform);
            _healthBar = new HealthBar(samurai);
            samurai.ChangeHealth += Destroy;
        }
    }
}
