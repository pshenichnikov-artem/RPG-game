using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Script.Game;
using WpfApp1.Script.Game.Controller;
using WpfApp1.Script.Game.Units;

namespace WpfApp1.Script.Engine.Core
{
    internal class Scene
    {
        private Dictionary<int, string> _planeDictionary;
        private static Scene _instance = null;
        private List<Image> _background;

        public Scene()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("Error create scene");
            }
            _background = new List<Image>();
            _planeDictionary = new Dictionary<int, string>
            {
                //Пустой
                { 1,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\NotGrass.png" },
                //Одиночные
                { 2,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Top.png" },
                { 3,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Left.png" },
                { 4,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Right.png" },
                { 5,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Bottom.png" },
                //Двойные
                { 6,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftTop.png" },
                { 7,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftBottom.png" },

                { 8,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\RightTop.png" },
                { 9,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\RightBottom.png" },

                { 10,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\TopBottom.png" },
                { 11,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftRight.png" },
                //Тройные
                { 12,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftTopBottom.png" },
                { 13,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\RightTopBottom.png" },

                { 14,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftTopRight.png" },
                { 15,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftBottomRight.png" },
                //Четвертной
                { 16,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LeftTopRightBottom.png" },

                //Уголки
                { 17,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpperLeftCorner.png" },
                { 18,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpperRightCorner.png" },
                { 19,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LowerLeftCorner.png" },
                { 20,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LowerRightCorner.png" },
                //Двойные уголки
                { 21,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpL_LowerL_Corner.png" },
                { 22,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpR_LowerR_Corner.png" },

                { 23,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpL_LowerR_Corner.png" },
                { 24,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpR_LowerL_Corner.png" },

                { 25,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\UpL_UpR_Corner.png" },
                { 26,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\LowerL_LowerR_Corner.png" },
                
                //Одна сторона и один уголок
                { 27,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Left_LowerL.png" },

                { 28,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Tile_37.png" },
                { 29,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Tile_45.png" },
                { 30,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Tile_42.png" },
                { 31,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map1\\Tile_41.png" },

                { 32,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_02.png" },
                { 33,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_37.png" },
                { 34,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_38.png" },
                { 35,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_10.png" },
                 { 36,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_13.png" },
                 { 37,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_20.png" },
                 { 38,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_28.png" },
                 { 39,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_29.png" },
                 { 41,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_24.png" },
                 { 42,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_26.png" },
                 { 43,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_23.png" },
                 { 44,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_14.png" },
                 { 45,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_11.png" },
                 { 46,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_36.png" },
                 { 47,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_18.png" },
                 { 48,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_22.png" },
                 { 49,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_17.png" },
                 { 50,  GameWindow.GetDirectory() + "\\Resource\\Tiles\\map2\\Tile_19.png" },
            };

        }
        Samurai samuraii;
        public void CreateMap1()
        {
            byte _ = 0;
            byte[,] map = {
                            { 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,19},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _,12,10,13, _, 6,10,10,10,10,10,10,10,10,10,13, _,12,10,10,10,10,10,10,10,10,10,10,10,21},
                            { 4, _, _, _,12,13, _,12,10,13, _,12,10,13, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                    /*5*/   {22,10,13, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, 6,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,30,10,10,13, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, 3},
                            {18, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 8, _, _,11, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, 3},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, _, _,11, _, 7,10,10,13, _, 6, 2, 8, _, _, _, _, _, _, _, _, 7,10,10,10,10,10,21},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,22,13, _,11, _, _, _, _, _, _, 3, 1, 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                    /*11*/  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, _, _,11, _, _, _, _, _, _, 3, 1, 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, _, _,11, _, _, _,12,31, 2,17, 1, 4, _, _, _, _, _, _, _, _, _, _, _,12,10,10,21},
                            {20, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 9, _,12,29, _, _, _, _, 3, 1, 1, 1, 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, 3, 1, 1, 1, 4, _, _, _, _, _, _, _, _, _,12,13, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _,16, _, 3, 1, 1, 1, 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            {22,10,10,10,10,10,10, 8, _, _, 6,10,10,10,10,10,10,10,10, 9, _, _, _, _, 3, 1, 1, 1, 4, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _,11, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, 7, 5, 5, 5,27, _, _, _, _, _, _,12,10,13, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _,15, _,12,29, _, _, _, _, _, _, _, _, _, _, _, _,16, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                            { 4, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,11, _, _, _, _, _, _, _, _, _, _, _, _, _, _, 3},
                  /*21*/    {18, 2, 2, 2, 2, 2, 2, 2, 2, 2,25, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,25, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,17}
            };                                           //10                                                  //28                                          //43

            GameWindow.Instance.BackGround.Width = map.GetLength(1) * 64;
            GameWindow.Instance.BackGround.Height = map.GetLength(0) * 64;

            //Небо
            Image _imageSky = GameWindow.CreateImage();
            _imageSky.Width = GameWindow.Instance.BackGround.Width;
            _imageSky.Height = GameWindow.Instance.BackGround.Height;
            _imageSky.Stretch = Stretch.Fill;
            _imageSky.Source = new BitmapImage(new Uri(GameWindow.GetDirectory() + "\\Resource\\Background\\Layers\\1.png", UriKind.RelativeOrAbsolute));
            _background.Add(_imageSky);



            CreateMap(map);


            GameWindow.Instantiate(
                new Portal(new Vector2D(42 * 64, 10 * 64), new Size(64, 128), async () =>
            {
                h = samuraii.Health;
                GameWindow.Instance.isPause = false;
                GameWindow.Instance.isActiveGameLoop = false;

                Time.Invoke(() =>
                {
                    GameWindow.DoInMainThread(() =>
                    {
                        GameWindow._lifecycleManager.Clear();
                        GameWindow.ClearWindow();
                        GameWindow.Instance.StopGameCycle(null,null);
                        GameWindow.Instance.GameLoopTask = null;
                        if (GameWindow.Instance.GameLoopTask == null)
                        {
                            GameWindow.Instance.GameLoopTask = new Thread(GameWindow.Instance.GameLoop)
                            {
                                IsBackground = true,
                                Priority = ThreadPriority.Highest
                            };

                            GameWindow.Instance.isActiveGameLoop = true;
                            GameWindow.Instance.isPause = true;

                            GameWindow.Instance.GameLoopTask.Start();
                            CreateMap2();
                        }
                    });
                }, 150);
            }));
            CreateUnitsMap1();
        }
        int h = 100;///
        private void CreateUnitsMap1()
        {
            //Самурай
            SamuraiCommander commander = new SamuraiCommander(new Vector2D(64 * 2, 18 * 64), new System.Drawing.Size(128, 128), 4, 1, 1, 6);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            SamuraiArcher archer = new SamuraiArcher(new Vector2D(3 * 64, 6 * 64), new System.Drawing.Size(128, 128), 4, 1, 1, 4);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            commander = new SamuraiCommander(new Vector2D(64 * 14, 6 * 64), new System.Drawing.Size(128, 128), 4, 1, 1, 4);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            archer = new SamuraiArcher(new Vector2D(24 * 64, 1 * 64), new System.Drawing.Size(128, 128), 4, 1, 3, 8);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);


            commander = new SamuraiCommander(new Vector2D(64 * 24, 4 * 64), new System.Drawing.Size(128, 128), 4, 2, 3, 10);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);


            commander = new SamuraiCommander(new Vector2D(64 * 35, 4 * 64), new System.Drawing.Size(128, 128), 4, 2, 3, 10);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 21, 19 * 64), new System.Drawing.Size(128, 128), 4, 2, 5, 14);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 26, 19 * 64), new System.Drawing.Size(128, 128), 4, 2, 5, 14);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);


            archer = new SamuraiArcher(new Vector2D(23 * 64, 7 * 64), new System.Drawing.Size(128, 128), 4, 3, 5, 5);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            archer = new SamuraiArcher(new Vector2D(37 * 64, 1 * 64), new System.Drawing.Size(128, 128), 4, 3, 5, 8);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            commander = new SamuraiCommander(new Vector2D(64 * 38, 19 * 64), new System.Drawing.Size(128, 128), 4, 4, 8, 20);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);


            samuraii = new Samurai(new Vector2D(64, (int)GameWindow.Instance.BackGround.Height - 600), new System.Drawing.Size(128, 128), 8, 1, 1, 20);
            GameWindow.Instantiate(new PlayerController(samuraii));
            GameWindow.Instantiate(samuraii);
        }

        public void CreateMap2()
        {
            byte _ = 0;
            byte[,] map = {
                            {38,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,37,39},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47,42, _, _, _, _, _, _, _, _, _,44,41,41,41,41,41,41,41,42, _,41,41,41,41,41,42, _,43,41,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _,44,48, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                    /*5*/   {36, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36,41,41,41,41,49, _, _,43,41,41,41,41,41,41,41,41,41,41,41,41,41,41,48, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,46, _, _,35},
                            {36, _, _, _, _,47, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _,47, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _,43,41,32,32,32,32,32,49, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _,50,41,41,41,41,41,41,49, _, _,47, _, _, _, _, _, _, _, _, _, _, _,35,45,45,45,45,36, _, _, _, _, _, _, _, _, _,46,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _,47, _,44,48, _, _, _, _, _, _, _,46, _, _, _,35,45,45,45,45,36, _, _, _, _, _, _, _, _, _, _,35},
                    /*11*/  {36, _, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _,35,45,45,45,37,48, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _,35,45,45,36, _, _, _,46, _, _, _, _, _, _,46, _,35},
                            {36, _, _, _, _, _,44,41,41,41,41,41,41,41,48, _, _, _, _, _, _, _,43,42, _, _, _,35,45,45,36, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _,47, _, _, _, _, _, _, _, _, _, _, _, _,43,42, _, _, _, _, _, _,35,45,45,36, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _,43,41,41,41,48, _, _, _, _, _, _, _, _, _,43,42, _, _, _, _, _, _, _, _, _,35,45,45,36, _, _,46, _, _, _, _, _, _,46, _, _,35},
                   /*16*/   {36, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35,45,45,36, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {36, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,35,45,45,36, _, _, _, _, _, _, _, _, _, _, _, _,35},
                            {33,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,34} 
            };

            GameWindow.Instance.BackGround.Width = map.GetLength(1) * 64;
            GameWindow.Instance.BackGround.Height = map.GetLength(0) * 64;

            Image _imageSky = GameWindow.CreateImage();
            _imageSky.Width = GameWindow.Instance.BackGround.Width;
            _imageSky.Height = GameWindow.Instance.BackGround.Height;
            _imageSky.Stretch = Stretch.Fill;
            _imageSky.Source = new BitmapImage(new Uri(GameWindow.GetDirectory() + "\\Resource\\Background\\Layers\\1.png", UriKind.RelativeOrAbsolute));
            _background.Add(_imageSky);

            CreateMap(map);

            GameWindow.Instantiate(new Portal(new Vector2D(14 * 64, 1 * 64), new Size(64, 128), () =>
            {
                GameWindow.Instance.isPause = false;
                GameWindow.Instance.isActiveGameLoop = false;
                Time.Invoke(() =>
                {
                    GameWindow.DoInMainThread(() =>
                    {
                        GameWindow.Instance.GameLoopTask?.Join();
                        GameWindow.ClearWindow();
                        GameWindow.Instance.StopGameCycle(null, null);
                        GameWindow.Instance.GameLoopTask = null;
                        GameWindow.Instance.CreateMenu();
                    });
                }, 150);
            }));

            CreateUnitsMap2(h);
        }

        private void CreateUnitsMap2(int health)
        {
            //Самурай
            SamuraiCommander commander = new SamuraiCommander(new Vector2D(64 * 2, 16 * 64), new System.Drawing.Size(128, 128), 4, 1, 1, 6);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 3, 13 * 64), new System.Drawing.Size(128, 128), 4, 1, 1, 6);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 3, 3 * 64), new System.Drawing.Size(128, 128), 4, 1, 3, 8);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 7, 5 * 64), new System.Drawing.Size(128, 128), 4, 3, 7, 2);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 20, 11 * 64), new System.Drawing.Size(128, 128), 4, 8, 10, 1);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 27, 5 * 64), new System.Drawing.Size(128, 128), 4, 2, 4, 6);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            commander = new SamuraiCommander(new Vector2D(64 * 20, 3 * 64), new System.Drawing.Size(128, 128), 4, 2, 4, 20);
            GameWindow.Instantiate(new AIControllerCommander(commander));
            GameWindow.Instantiate(commander);

            SamuraiArcher archer = new SamuraiArcher(new Vector2D(8 * 64, 16 * 64), new System.Drawing.Size(128, 128), 4, 2, 4, 6);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            archer = new SamuraiArcher(new Vector2D(19 * 64, 16 * 64), new System.Drawing.Size(128, 128), 4, 7, 10, 2);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            archer = new SamuraiArcher(new Vector2D(36 * 64, 1 * 64), new System.Drawing.Size(128, 128), 4, 2, 3, 4);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);

            archer = new SamuraiArcher(new Vector2D(24 * 64, 1 * 64), new System.Drawing.Size(128, 128), 4, 2, 3, 4);
            GameWindow.Instantiate(new AIControllerArcher(archer));
            GameWindow.Instantiate(archer);


            Samurai samurai = new Samurai(new Vector2D(64, (int)GameWindow.Instance.BackGround.Height - 600), new System.Drawing.Size(128, 128), 8, 1, 1, 20, health);
            GameWindow.Instantiate(new PlayerController(samurai));
            GameWindow.Instantiate(samurai);
        }

        private void CreateMap(byte[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != 0)
                    {
                        Game.Plane plane = CreatePlane(map, i, j);
                        GameWindow.Instantiate(plane);
                    }
                }
            }
        }

        private Game.Plane CreatePlane(byte[,] map, int i, int j)
        {
            Game.Plane plane;

            bool leftNeighbor = j > 0 && map[i, j - 1] != 0;
            bool rightNeighbor = j < map.GetLength(1) - 1 && map[i, j + 1] != 0;
            bool topNeighbor = i > 0 && map[i - 1, j] != 0;
            bool toptopNeighbor = i > 1 && map[i - 2, j] != 0;
            bool bottomNeighbor = i < map.GetLength(0) - 1 && map[i + 1, j] != 0;

            bool topLeftNeighbor = i > 0 && j > 0 && map[i - 1, j - 1] == 0;
            bool topRightNeighbor = i > 0 && j < map.GetLength(1) - 1 && map[i - 1, j + 1] != 0;

            if (
                leftNeighbor ||
                //topNeighbor && topLeftNeighbor ||
                //j == 0 && !rightNeighbor && !topRightNeighbor 
                topNeighbor && toptopNeighbor && !rightNeighbor
                )
            {

                plane = new Game.Plane(new Vector2D(j * 64, i * 64),
                      new Size(65, 65),
                    _planeDictionary[map[i, j]],
                    null);
            }
            else
            {
                int countRightNeighbors = CountRightNeighbors(map, i, j);
                int countDownNeighbors = 0;

                if (!rightNeighbor || j == map.GetLength(1) - 1)
                    countDownNeighbors = CountDownNeighbors(map, i, j);

                plane = new Game.Plane(new Vector2D(j * 64, i * 64),
                    new Size(65, 65),
                    _planeDictionary[map[i, j]],
                    new OffsetParam(0, -64 * countRightNeighbors, 0, -64 * countDownNeighbors));
            }
            return plane;
        }

        private int CountRightNeighbors(byte[,] map, int rowIndex, int colIndex)
        {
            int count = 0;
            int maxCols = map.GetLength(1);

            for (int j = colIndex + 1; j < maxCols; j++)
            {
                if (map[rowIndex, j] != 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private int CountDownNeighbors(byte[,] map, int rowIndex, int colIndex)
        {
            int count = 0;
            int maxRows = map.GetLength(0);

            for (int i = rowIndex + 1; i < maxRows; i++)
            {
                if (map[i, colIndex] != 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }
    }
}
