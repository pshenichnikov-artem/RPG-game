using System;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Game;
using WpfApp1;
using System.Windows.Media;
using WpfApp1.Script.Game.Units;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal class Arrow : GameObject, IOnCollision
{
    private int _minDamage;
    private int _maxDamage;
    private int _rotateAngle = 0;
    private Vector2D _startPosition;
    private bool _isActive = true;
    private Collider _collider;

    protected override void Update()
    {
        int _speed = 4;
        int deltaX = transform.Direction * _speed;
     //   GameWindow.DebugRectangle(GetCollider().GetColliderRectangle());
        transform.Position += new Vector2D(deltaX, 0);

        if (Math.Abs(transform.Position.X - _startPosition.X) >= 450)
        {
            transform.Position += new Vector2D(0, 2);

            if (_rotateAngle < 30)
            {
                int _rotationSpeed = 2;
                _rotateAngle += _rotationSpeed;
                GameWindow.DoInMainThread(() =>
                {
                    _image.LayoutTransform = new RotateTransform(_rotateAngle);
                    _collider.ChangeOffset(new OffsetParam(20, 10 - _rotateAngle, _rotateAngle + 45, 45 - _rotateAngle));
                });
            }
        }
    }

    public Arrow(Vector2D position, int direction, int minDamage, int maxDamage) : base(position, new Size(100, 100))
    {
        _minDamage = minDamage;
        _maxDamage = maxDamage;
        _startPosition = transform.Position;
        transform.Direction = direction;
        _collider = new Collider(transform, new OffsetParam(20, 10, 45, 45));
        GameWindow.DoInMainThread(() => _image.Source = (new BitmapImage(new Uri(GameWindow.GetDirectory() + "\\Resource\\Units\\Samurai_Archer\\Arrow.png", UriKind.RelativeOrAbsolute))));
    }

    public Collider GetCollider() => _collider;
    void IOnCollision.OnCollision(CollisionEventArgs args)
    {
        GameObject? gameObject = (GameObject)args.collidedWith.FirstOrDefault(x => x is Samurai || x is Plane);
         
        if (gameObject == null) return;

        if (gameObject is Samurai samurai && _isActive)
        {
            samurai.TakeDamage(new Random().Next(_minDamage,_maxDamage));
            _isActive = false;
        }
        else if (gameObject is Plane plane)
        {
            GameWindow.Destroy(this);
            Time.Invoke(() => { GameWindow.DestoyImage(_image); }, 5000);
        }
    }
}