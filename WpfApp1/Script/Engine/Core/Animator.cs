using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Engine.Core
{
    internal class Animator : IRenderImage, IFixedUpdate
    {
        private readonly Image _image;

        private event Action _endAnimationEvent;
        public event Action EndAnimationEvent 
        { 
            add { _endAnimationEvent += value; } 
            remove { _endAnimationEvent -= value; }
        }

        private int _checkCurrentFrame = -1;
        private Action _action;

        private Dictionary<string, (BitmapImage bitmapImage, int frameCount)> animations;

        private float _currentFrame;
        private string _currentAction = "";

        private int _continueAnimationFromCurrentTick = -1;
        public int SkipCountFrame { 
            set { _continueAnimationFromCurrentTick = value > 0 ? value : _continueAnimationFromCurrentTick = -1; } }

        public string CurrentAction => _currentAction;
        public int CountFrameInAnimation => animations[_currentAction].frameCount;
        public int CurrentFrame => (int)Math.Floor(_currentFrame);
        public bool StopAnimation { get; set; }
        public float AnimationSpeed { get; set; }
        
        public void SetAnimation(string animationName)
        {
            if (_currentAction == animationName) return;

            StopAnimation = false;
            _currentAction = animationName;
            _currentFrame = 0;
        }
        public void AddAnimation(string name, string path, int frameCount) =>
            animations.Add(name, (new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)), frameCount));

        void IFixedUpdate.FixedUpdate()
        {
            //Скипаем кадры
            if(_continueAnimationFromCurrentTick > 0)
            {
                _continueAnimationFromCurrentTick--;
                if(_continueAnimationFromCurrentTick == 0)
                {

                    _continueAnimationFromCurrentTick = -1;
                    StopAnimation = false;
                }
            }

            if (StopAnimation) return;

            //Обновляем кадры
            _currentFrame += AnimationSpeed * Time.DeltaTime;

            if (_currentFrame > CountFrameInAnimation)
            {
                _currentFrame = 0;
                _endAnimationEvent?.Invoke();
            }
            
            if (_checkCurrentFrame!= -1 && _checkCurrentFrame == CurrentFrame)
            {
                _checkCurrentFrame = -1;
                _action();
            }
        }

        public void DoWorkInFrame(Action action, int checkCurrentFrame)
        {
            _checkCurrentFrame = checkCurrentFrame;
            _action = action;
        }

        public void Render()
        {
            if (StopAnimation) return;

            int x = _offsetCutImage.offsetLeft;
            int y = _offsetCutImage.offsetTop;
            int width = _offsetCutImage.offsetRight;
            int height = _offsetCutImage.offsetBottom;

            CroppedBitmap croppedBitmap = new CroppedBitmap(
                    animations[_currentAction].bitmapImage,
                    //TODO продумать обрезание картинки
                    new Int32Rect(x * CurrentFrame, y, width, height)
                    );

            _image.Source = croppedBitmap;
        }
        private OffsetParam _offsetCutImage;
        public Animator(Image image, OffsetParam offsetParam)
        {
            //TODO
            GameWindow.Instantiate(this);

            _offsetCutImage = offsetParam;

            AnimationSpeed = 8f;
            StopAnimation = false;

            _currentFrame = 0;
            _image = image;
            animations = new Dictionary<string, (BitmapImage bitmapImage, int frameCount)>();
        }
    }
}
