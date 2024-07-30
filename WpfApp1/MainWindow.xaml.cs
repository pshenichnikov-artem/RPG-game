using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Script.Engine;
using WpfApp1.Script.Engine.Core;
using WpfApp1.Script.Engine.Interfaces;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Input;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public bool isPause = true;

        public static GameWindow? Instance;

        private static Time _time;
        private static Input _input;
        private static CollisionManager _collisionManager;
        internal static RenderManager _renderManager;
        internal static LifecycleManager _lifecycleManager;
        internal static Scene _scene;

        private static List<object> instantiate = new List<object>();
        private static List<object> destroy = new List<object>();

        private int millisecondsPerFrame60FPS = 10;

        public Thread GameLoopTask = null;

        public GameWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            //WindowStyle = WindowStyle.None;
            //ResizeMode = ResizeMode.NoResize;

            Loaded += MainWindow_Loaded;
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                isPause = false;
            }
            else if (WindowState == WindowState.Normal)
            {
                isPause = true;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Instance = this;
            _time = new Time();
            _input = new Input(this);

            _collisionManager = new CollisionManager();
            _renderManager = new RenderManager();
            _lifecycleManager = new LifecycleManager();

            _scene = new Scene();

            CreateMenu();

            Closing += MainWindow_Closing;
        }

        private void OnStart(object? sender, EventArgs e)
        {
            ClearWindow();

            if (GameLoopTask != null)
            {
                isActiveGameLoop = false;
                GameLoopTask.Join();
            }

            instantiate.Clear();
            destroy.Clear();
            _lifecycleManager.Clear();

            isActiveGameLoop = true;
            GameLoopTask = new Thread(GameLoop)
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };

            GameLoopTask.Start();


            _scene.CreateMap1();
            Cursor = Cursors.None;
        }

        public static void ClearWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                var elementsToRemove = Instance.canvas.Children.Cast<UIElement>()
                .Where(element => element != Instance.BackGround)
                .ToList();

                foreach (var element in Instance.BackGround.Children.Cast<UIElement>().ToList())
                {
                    if (Instance.BackGround.Children.Contains(element))
                        Instance.BackGround.Children.Remove(element);
                }

                foreach (var element in elementsToRemove)
                {
                    if (Instance.canvas.Children.Contains(element))
                        Instance.canvas.Children.Remove(element);
                }
            });
        }

        public void CreateMenu()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Cursor = Cursors.Arrow;

                Button playButton = new Button();
                playButton.Click += OnStart;
                playButton.Width = 200;
                playButton.Height = 200;

                ImageBrush brush = new ImageBrush(new BitmapImage(
                    new Uri(GetDirectory() + "\\Resource\\Buttons\\Play.png")));
                Canvas.SetLeft(playButton, Width / 2 - 100);
                Canvas.SetTop(playButton, Height / 2 - 100);
                playButton.Background = brush;

                Style style = new Style(typeof(Button));
                style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
                playButton.Style = style;

                Image image = new Image();
                image.Width = 1920;
                image.Height = 1080;
                image.Source = new BitmapImage(new Uri(GetDirectory() + "\\Resource\\Background\\Background.png", UriKind.RelativeOrAbsolute));

                canvas.Children.Add(image);
                canvas.Children.Add(playButton);
            });

        }

        public bool isActiveGameLoop = true;

        public static string GetDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo parentDirectory = Directory.GetParent(currentDirectory).Parent.Parent;
            return parentDirectory.ToString().Replace("\\", "\\\\");
            //Releas
            //return currentDirectory;
        }

        public async void GameLoop()
        {
            instantiate.ForEach(x => InstantinateObject(x));
            instantiate.Clear();
            destroy.ForEach(x => DestroyObject(x));
            destroy.Clear();

            _lifecycleManager.InvokeAsync<IAwake>(x => x.Awake());
            _lifecycleManager.InvokeAsync<IStart>(x => x.Start());

            while (!cancellationTokenSource.Token.IsCancellationRequested && isActiveGameLoop)
            {
                try
                {
                    instantiate.ForEach(x => InstantinateObject(x));
                    instantiate.Clear();
                    destroy.ForEach(x => DestroyObject(x));
                    destroy.Clear();
                }
                catch (Exception ex)
                {
                    instantiate.Clear();
                    destroy.Clear();
                }


                while (!isPause)
                {
                    await Task.Delay(100);
                }

                _time.Start();
                /*await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var rect in rectangles)//Убрать
                    {
                        Instance.BackGround.Children.Remove(rect);//Убрать
                    }
                });
                rectangles.Clear();//Убрать*/

                //TODO поставить физику после коллайдера, а также продумать как
                //нормально выставить padding в коллизии у юнита
                _lifecycleManager.InvokeAsync<IFall>(x => x.Landing());
                _collisionManager.CheckCollison();

                _time.UpdateDeltaTime();
                _lifecycleManager.InvokeAsync<IUpdate>(x => x.Update());

                _lifecycleManager.InvokeAsync<IFixedUpdate>(x => x.FixedUpdate());

                try
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _lifecycleManager.Invoke<IRenderImage>(x => x.Render());

                        /*foreach (var rect in rectangles)//Убрать
                        {

                            Instance.BackGround.Children.Add(rect);

                        }*/
                    });
                }
                catch (TaskCanceledException) { }
                catch (Exception) { }


                int timeUntilNextFrame = millisecondsPerFrame60FPS - _time.Stop();
                if (timeUntilNextFrame > 0)
                    Thread.Sleep(timeUntilNextFrame);
            }
            GameLoopTask = null;
        }

        public static Image CreateImage()
        {
            TaskCompletionSource<Image> tcs = new TaskCompletionSource<Image>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Image img = new Image();
                Instance.BackGround.Children.Add(img);
                tcs.SetResult(img);
            });

            return tcs.Task.Result;
        }

        public static Image CreateUIImage()
        {
            TaskCompletionSource<Image> tcs = new TaskCompletionSource<Image>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Image img = new Image();
                Instance.canvas.Children.Add(img);
                tcs.SetResult(img);
            });

            return tcs.Task.Result;
        }

        public static void DestoyImage(Image image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (image != null)
                {
                    Panel? parent = image.Parent as Panel;

                    if (parent != null && parent.Children.Contains(image))
                    {
                        parent.Children.Remove(image);
                    }
                }
            });
        }

        public static void DoInMainThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                action();
            });
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        //Добавление объектов
        public static void Instantiate(object item)
        {
            instantiate.Add(item);
        }

        public static void Destroy(object item)
        {
            destroy.Add(item);
        }

        private void InstantinateObject(object item)
        {
            if (item is IAwake awake)
                _lifecycleManager.Add<IAwake>(awake);

            if (item is IStart start)
                _lifecycleManager.Add<IStart>(start);

            if (item is IUpdate update)
                _lifecycleManager.Add<IUpdate>(update);

            if (item is IFixedUpdate fixedUpdate)
                _lifecycleManager.Add<IFixedUpdate>(fixedUpdate);

            if (item is IRenderImage image)
                _lifecycleManager.Add<IRenderImage>(image);

            if (item is IFall fall)
                _lifecycleManager.Add<IFall>(fall);

            if (item is ICollider collider)
                _collisionManager.Add(collider);
        }

        private void DestroyObject(object item)
        {
            if (item is IAwake awake)
                _lifecycleManager.Remove<IAwake>(awake);

            if (item is IStart start)
                _lifecycleManager.Remove<IStart>(start);

            if (item is IUpdate update)
                _lifecycleManager.Remove<IUpdate>(update);

            if (item is IFixedUpdate fixedUpdate)
                _lifecycleManager.Remove<IFixedUpdate>(fixedUpdate);

            if (item is IRenderImage image)
                _lifecycleManager.Remove<IRenderImage>(image);

            if (item is IFall fall)
                _lifecycleManager.Remove<IFall>(fall);

            if (item is ICollider collider)
                _collisionManager.Remove(collider);
        }

        private static List<System.Windows.Shapes.Rectangle> rectangles = new List<System.Windows.Shapes.Rectangle>();
        public static void Rectangle(System.Drawing.Rectangle rectangle_)
        {
            /*
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
                    // Устанавливаем свойства прямоугольника
                    rectangle.Width = Math.Max(rectangle_.Right, rectangle_.Left) - Math.Min(rectangle_.Right, rectangle_.Left);
                    rectangle.Height = Math.Max(rectangle_.Top, rectangle_.Bottom) - Math.Min(rectangle_.Top, rectangle_.Bottom);

                    Canvas.SetLeft(rectangle, rectangle_.Left);
                    Canvas.SetTop(rectangle, rectangle_.Top);

                    // Задаем цвет и другие свойства прямоугольника
                    rectangle.Stroke = Brushes.Red;
                    rectangle.Fill = Brushes.Transparent;
                    rectangles.Add(rectangle);
                }
                catch (TaskCanceledException) { }
                catch (NullReferenceException) { }
                catch (Exception) { }
            });
            */
        }

        //Фон при смерти
        private static Rectangle transparentOverlay;
        private static double opacityStep = 0.05; // Шаг изменения прозрачности
        private static DispatcherTimer timer;
        private static TextBlock textBlock;
        private static TextBlock textBlock2;

        public static void CreateAndAnimateTransparentOverlay()
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                // Создаем прямоугольник для прозрачного оверлея
                transparentOverlay = new Rectangle();
                transparentOverlay.Width = Instance.canvas.ActualWidth;
                transparentOverlay.Height = Instance.canvas.ActualHeight;
                transparentOverlay.Fill = Brushes.Black;
                transparentOverlay.Opacity = 0; // Начальная прозрачность

                textBlock = new TextBlock();
                textBlock.Text = "YOU DIED";
                textBlock.Foreground = Brushes.Red;
                textBlock.FontSize = 128;
                textBlock.Opacity = 0; // Начальная прозрачность

                textBlock2 = new TextBlock();
                textBlock2.Text = "нажмите любую кнопку";
                textBlock2.Foreground = Brushes.White;
                textBlock2.FontSize = 36;
                textBlock2.Opacity = 0; // Начальная прозрачность

                Instance.canvas.Children.Add(transparentOverlay);
                Instance.canvas.Children.Add(textBlock);
                Instance.canvas.Children.Add(textBlock2);

                Canvas.SetLeft(textBlock, (1920) / 2 - 470);
                Canvas.SetTop(textBlock, (1080) / 2 - 400);
                Canvas.SetLeft(textBlock2, (1920) / 2 - 400);
                Canvas.SetTop(textBlock2, (1080) / 2 - 200);

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += Timer_Tick;
                timer.Start();
            });
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            // Увеличиваем прозрачность прямоугольника
            transparentOverlay.Opacity += opacityStep;
            textBlock.Opacity += opacityStep;
            textBlock2.Opacity += opacityStep;

            if (transparentOverlay.Opacity >= 1)
            {
                Instance.KeyDown += Instance.OnMenu;
            }
            // Если прозрачность достигла 1, останавливаем таймер
            if (transparentOverlay.Opacity >= 1)
            {
                timer.Stop();
            }
        }

        private void OnMenu(object sender, EventArgs e)
        {
            GameWindow.Instance.isPause = false;
            GameWindow.Instance.isActiveGameLoop = false;
            Time.Invoke(() =>
            {
                GameWindow.Instance.GameLoopTask?.Join();
                GameWindow.ClearWindow();
                GameWindow.Instance.StopGameCycle(null, null);
                GameWindow.Instance.GameLoopTask = null;
                GameWindow.Instance.CreateMenu();
            }, 150);
            Instance.KeyDown -= Instance.OnMenu;
        }

        public async void StopGameCycle(object sender, KeyEventArgs e)
        {
            await Task.Delay(200);
            _lifecycleManager.Clear();
            instantiate.Clear();
            destroy.Clear();
        }
    }
}
