using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Threading;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Engine.Core
{
    internal class Time
    {
        private static Time? _instance;
        private Stopwatch stopwatchDeltaTime;
        private TimeSpan previousDeltaTime;

        private Stopwatch stopwatchTime;
        private TimeSpan previousTime;

        public static float DeltaTime { get; private set; }

        private static List<Timer> list = new List<Timer>();

        public static void Invoke(Action action, int millisecondsDelay)
        {
            Timer timer = null;
            timer = new Timer(_ =>
            {
                action.Invoke();
                list.Remove(timer);
            }, null, millisecondsDelay, Timeout.Infinite);
        
            list.Add(timer);
        }

        public Time()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("The object Time has already been created");
            }
            stopwatchTime = new Stopwatch();
            previousTime = stopwatchTime.Elapsed;

            stopwatchDeltaTime = new Stopwatch();
            stopwatchDeltaTime.Start();
            previousDeltaTime = stopwatchDeltaTime.Elapsed;
        }

        public void Start()
        {
            stopwatchTime.Restart();
        }

        public int Stop()
        {
            stopwatchTime.Stop();
            return (int)stopwatchTime.ElapsedMilliseconds;
        }

        public void UpdateDeltaTime()
        {
            TimeSpan currentTime = stopwatchDeltaTime.Elapsed;
            DeltaTime = (float)(currentTime - previousDeltaTime).TotalSeconds;
            previousDeltaTime = currentTime;
        }
    }
}
