using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Transactions;
using WpfApp1.Script.Engine.Core;

namespace WpfApp1.Script.Engine
{
    internal interface ICollider
    {
        Collider GetCollider();
    }

    internal interface IOnCollision : ICollider
    {
        void OnCollision(CollisionEventArgs args);
    }

    internal class CollisionEventArgs : EventArgs
    {
        public List<ICollider> collidedWith;
        public CollisionEventArgs(List<ICollider> list)
        {
            collidedWith = list;
        }
    }
}
