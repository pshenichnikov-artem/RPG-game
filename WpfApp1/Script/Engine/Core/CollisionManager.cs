using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace WpfApp1.Script.Engine.Core
{
    internal class CollisionManager
    {
        private static CollisionManager? _instance = null;
        private static List<ICollider> items;

        public void Clear()
        {
            items.Clear();
        }
        public CollisionManager()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new Exception("CollisionManager was been created");
            }
            items = new List<ICollider>();
        }

        public void CheckCollison()
        {
            List<Task> collisionTasks = new List<Task>();

            for (int i = 0; i < items.Count; i++)
            {
                int index = i;
                collisionTasks.Add(Task.Run(() => HandleCollision(items[index])));
            }

            Task.WaitAll(collisionTasks.ToArray());
        }

        private void HandleCollision(ICollider currentItem)
        {
            List<ICollider> collidedWith = new List<ICollider>();

            for (int i = 0; i < items.Count; i++)
            {
                if (currentItem != items[i] && 
                    currentItem.GetCollider().CheckColliderIntersection(items[i].GetCollider().GetColliderRectangle()))
                {

                    collidedWith.Add(items[i]);      
                }
            }

            if (currentItem is IOnCollision currentItemOnCollision)
            {
                currentItemOnCollision.OnCollision(new CollisionEventArgs(collidedWith));
            }
            else if (true)
            {

            }
        }

        public static List<T> CheckCollisionWithColliders<T>(Rectangle rectangle, object? thisObject = null)
        {
            List<T> collidedWith = new List<T>();

            for (int j = 0; j < items.Count; j++)
            {
                ICollider otherItem = items[j];
                if (otherItem != thisObject && otherItem is T item && rectangle.IntersectsWith(otherItem.GetCollider().GetColliderRectangle()))
                {
                    collidedWith.Add(item);
                }
            }
            return collidedWith;
        }

        public void Add(ICollider obj)
        {
            if(obj.GetCollider() != null)
                items.Add(obj);
        }

        public void Remove(ICollider obj)
        {
            if(items.Contains(obj))
                items.Remove(obj);
        }

        public static (int xOffset, int yOffset) GetOffsetColliderRectangle(Rectangle first, Rectangle second,
            float paddingX = 0, float paddingY = 0)
        {
            if (paddingX == 0)
            {
                paddingX = first.Width;
            }
            if (paddingY == 0)
            {
                paddingY = first.Height;
            }

            int xOffset = 0;
            int yOffset = 0;

            if (first.Left <= second.Right && first.Left + paddingX >= second.Right)
            {
                xOffset = first.Left - second.Right;
            }
            if (first.Right >= second.Left && first.Right - paddingX <= second.Left)
            {
                xOffset = first.Right - second.Left;
            }

            if (first.Top <= second.Bottom && first.Top + paddingY >= second.Bottom)
            {
                yOffset = first.Top - second.Bottom;
            }
            if (first.Bottom >= second.Top && first.Bottom - paddingY <= second.Top)
            {
                yOffset = first.Bottom - second.Top;
            }

            return (xOffset, yOffset);
        }
    }
}
