using System;
using System.Collections.Generic;
namespace ArchiNET.Droid.Assets
{
    public interface ITree<T>
    {
        IList<ITree<T>> Children { get; }
        void Add(ITree<T> newNode);
        bool Remove(ITree<T> existingNode);
    }
    public sealed class Tree<T> : ITree<T>
    {
        public IList<ITree<T>> Children { get; }
        public void Add(ITree<T> newNode)
        {
            Children.Add(newNode);
        }
        public bool Remove(ITree<T> node)
        {
            return Children.Remove(node);
        }
    }
}