using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Script.Engine.Interfaces;

namespace WpfApp1.Script.Engine.Core
{
    internal class LifecycleManager
    {
        private static LifecycleManager _instance = null;

        private static Dictionary<Type, List<IInitializable>> _dictionary;

        public void Clear()
        {
            foreach (var items in _dictionary.Values.ToList())
            {
                foreach (var item in items)
                {
                    GameWindow.Destroy(item);
                }
            }
        }

        public LifecycleManager()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("GamaObjectFactory has been created");
            }

            _dictionary = new Dictionary<Type, List<IInitializable>>();
        }
       

        public void InvokeAsync<T>(Action<T> action)
        {
            var key = typeof(T);

            if (!_dictionary.ContainsKey(key)) return;

            List<IInitializable> list = _dictionary[key];

            List<Task> updateTasks = new List<Task>();

            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                if (list[index] is T item)
                {
                    updateTasks.Add(Task.Run(() => action(item)));
                }
            }

            Task.WaitAll(updateTasks.ToArray());
        }

        public void Invoke<T>(Action<T> action)
        {
            var key = typeof(T);

            if (!_dictionary.ContainsKey(key)) return;

            List<IInitializable> list = _dictionary[key];

            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                if (list[index] is T item)
                {
                    action(item);
                }
            }
        }

        public void Add<T>(IInitializable initializable)
        {
            var key = typeof (T);
            if( _dictionary.ContainsKey(key))
            {
                _dictionary[key].Add(initializable);
            }
            else
            {
                _dictionary.Add(key, new List<IInitializable>());
                _dictionary[key].Add(initializable);
            }
        }

        public void Remove<T>(IInitializable initializable)
        {
            var key = typeof(T);
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key].Remove(initializable);
            }
        }
    }
}
