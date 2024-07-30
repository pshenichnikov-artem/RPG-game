using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Drawing;
using WpfApp1.Script.Engine.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Script.Engine.Core
{
    internal class RenderManager
    {
        private static RenderManager? _instance = null;
        private List<IRenderImage> items;

        public void Clear()
        {
            items.Clear();
        }

        public RenderManager()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("");
            }
            items = new List<IRenderImage>();
        }

        public void Render()
        {
            for (int i = 0; i < items.Count; ++i)
            {
                items[i].Render();
            }
        }

        public void Add(IRenderImage obj)
        {
            items.Add(obj);
        }

        public void Remove(IRenderImage obj)
        {
            items.Remove(obj);
        }
    }
}
