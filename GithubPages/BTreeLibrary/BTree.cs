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
            for (; addAt < Subnodes.Count && value.CompareTo(Subnodes[addAt].Value) > 0; addAt++) { }
            if (addAt < Subnodes.Count && Subnodes[addAt].HasSubnodes)
            {
                List<BTreeNode<T>>? ret = Subnodes[addAt].Add(value);
                if (ret != null)
                {
                    if (Subnodes.Count + 1 > _maxSize)
                    {
                        Split();
                        Add(value);
                    }
                    else
                        Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, ret));
                }
            }
            else
            {
                if (Subnodes.Count + 1 > _maxSize)
                {
                    Split();
                    Add(value);
                }
                else
                    Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, value));
            }
        }

        private void Split()
        {
            int firstHalfCount = (int)Math.Floor(Subnodes.Count / 2d);
            List<BTreeNode<T>>? firstH = Subnodes.GetRange(0, firstHalfCount);
            List<BTreeNode<T>>? secondH = Subnodes.GetRange(firstHalfCount, Subnodes.Count - firstHalfCount);
            Subnodes.Clear();
            Subnodes.Add(new BTreeNode<T>(_maxSize, firstH));
            Subnodes.Add(new BTreeNode<T>(_maxSize, secondH));
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

        public delegate void forEachDelegate(BTreeNode<T> node, double x, int y);

        public void ForEach(forEachDelegate action)
        {
            int btmC = 0;
            foreach (var node in Subnodes)
            {
                node.ForEach(action, 0, ref btmC);
            }
        }
    }
}
