using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfApp1.Script.Engine.Core
{
    internal class Input
    {
        public static bool W { get; private set; }
        public static bool S { get; private set; }
        public static bool A { get; private set; }
        public static bool D { get; private set; }
        public static bool Z { get; private set; }
        public static bool X { get; private set; }
        public static bool C { get; private set; }
        public static bool V { get; private set; }
        public static bool Up { get; private set; }
        public static bool Down { get; private set; }
        public static bool Left { get; private set; }
        public static bool Right { get; private set; }

        private static Input _instance;

        public Input(GameWindow control)
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("The object Input has already been created");
            }

            control.KeyUp += OnKeyUp;
            control.KeyDown += OnKeyDown;
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    Up = true;
                    break;
                case Key.Down:
                    Down = true;
                    break;
                case Key.Left:
                    Left = true;
                    break;
                case Key.Right:
                    Right = true;
                    break;
                case Key.W:
                    W = true;
                    break;
                case Key.S:
                    S = true;
                    break;
                case Key.A:
                    A = true;
                    break;
                case Key.D:
                    D = true;
                    break;
                case Key.Z:
                    Z = true;
                    break;
                case Key.X:
                    X = true;
                    break;
                case Key.C:
                    C = true;
                    break;
                case Key.V:
                    V = true;
                    break;
            }
        }

        private static void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    Up = false;
                    break;
                case Key.Down:
                    Down = false;
                    break;
                case Key.Left:
                    Left = false;
                    break;
                case Key.Right:
                    Right = false;
                    break;
                case Key.W:
                    W = false;
                    break;
                case Key.S:
                    S = false;
                    break;
                case Key.A:
                    A = false;
                    break;
                case Key.D:
                    D = false;
                    break;
                case Key.Z:
                    Z = false;
                    break;
                case Key.X:
                    X = false;
                    break;
                case Key.C:
                    C = false;
                    break;
                case Key.V:
                    V = false;
                    break;
            }
        }
    }
}
