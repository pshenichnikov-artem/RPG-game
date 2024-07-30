using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Script.Engine.Interfaces
{
    internal interface IInitializable { }

    internal interface IAwake : IInitializable
    {
        void Awake();
    }

    internal interface IStart : IInitializable
    {
        void Start();
    }

    internal interface IUpdate : IInitializable
    {
        void Update();
    }

    internal interface IFixedUpdate : IInitializable
    {
        void FixedUpdate();
    }

    internal interface IRenderImage : IInitializable
    {
        void Render();
    }

    internal interface IFall : IInitializable
    {
        public void Landing();
    }
}
