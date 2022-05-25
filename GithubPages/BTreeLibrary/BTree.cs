using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeLibrary
{
    internal class BTree<T> where T : IComparable<T>
    {
        private int _maxSize;
        public List<BTreeNode<T>> Subnodes { get; private set; }

        public BTree(int maxSize)
        {
            _maxSize = maxSize;
            Subnodes = new List<BTreeNode<T>>();
        }
        public BTree(int maxSize, T value) : this(maxSize)
        {
            Subnodes.Add(new BTreeNode<T>(_maxSize, value));
        }

        public int Depth()
        {
            if (Subnodes.Count > 0)
                return Subnodes[0].Depth() + 1;
            else
                return 0;
        }

        public void Add(T value)
        {
            //Add / Insert
            int addAt = 0;
            for (; addAt < Subnodes.Count && value.CompareTo(Subnodes[addAt].Value) >= 0; addAt++) { }
            if (addAt < Subnodes.Count && Subnodes[addAt].HasSubnodes)
            {
                List<BTreeNode<T>>? ret = Subnodes[addAt].Add(value);
                if (ret != null)
                {
                    Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, ret));
                }
            }
            else
            {
                Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, value));
            }

            // Split
            if (Subnodes.Count > _maxSize)
            {
                int firstHalfCount = (int)Math.Floor(Subnodes.Count / 2d);
                List<BTreeNode<T>>? firstH = Subnodes.GetRange(0, firstHalfCount);
                Subnodes.RemoveRange(0, firstHalfCount);
                List<BTreeNode<T>>? secondH = Subnodes.GetRange(0, Subnodes.Count - firstHalfCount);
                Subnodes.RemoveRange(0, Subnodes.Count - firstHalfCount);
                Subnodes.Add(new BTreeNode<T>(_maxSize, firstH));
                Subnodes.Add(new BTreeNode<T>(_maxSize, secondH));
            }
        }

        public List<List<T[]>> ToList()
        {
            List<List<T[]>> ret = new List<List<T[]>>();
            foreach (var node in Subnodes)
            {
                node.ToList(ret);
            }
            return ret;
        }

        public void ForEach(Action<BTreeNode<T>, int> action)
        {
            Subnodes.ForEach(x => x.ForEach(action, 0));
        }
    }
}
