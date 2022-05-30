using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeLibrary
{
    internal class BTreeNode<T> where T : IComparable<T>
    {
        private int _maxSize;
        public bool HasSubnodes {
            get
            {
                return Subnodes.Count > 0;
            }
        }
        public T Value { get; private set; }
        public List<BTreeNode<T>> Subnodes { get; private set; }


        public BTreeNode(int maxSize, T value)
        {
            _maxSize = maxSize;
            Value = value;
            Subnodes = new List<BTreeNode<T>>();
        }
        public BTreeNode(int maxSize, List<BTreeNode<T>> list) : this(maxSize, list.Last().Value)
        {
            Subnodes = list;
        }


        public int Depth()
        {
            if (HasSubnodes)
                return Subnodes[0].Depth() + 1;
            else
                return 0;
        }

        public List<BTreeNode<T>>? Add(T value)
        {
            List<BTreeNode<T>>? ret = null;

            //Add / Insert
            int addAt = 0;
            for (; addAt < Subnodes.Count && value.CompareTo(Subnodes[addAt].Value) > 0; addAt++) {}
            if (addAt < Subnodes.Count && Subnodes[addAt].HasSubnodes)  // If within List and append to subnode available
            {   // Add as subnode
                List<BTreeNode<T>>? res = Subnodes[addAt].Add(value);
                if (res != null)
                {
                    if (Subnodes.Count + 1 > _maxSize)
                    {
                        ret = Split();
                        Add(value);
                    }
                    else
                        Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, res));
                }
            }
            else
            {   // Create new node
                if (Subnodes.Count + 1 > _maxSize)
                {
                    ret = Split();
                    Add(value);
                }
                else
                    Subnodes.Insert(addAt, new BTreeNode<T>(_maxSize, value));
                //if (HasSubnodes)    //TODO
                //{
                //    node.Add(value);
                //}
            }

            return ret;
        }

        private List<BTreeNode<T>> Split()
        {
            int firstHalfCount = (int)Math.Floor(Subnodes.Count / 2d);
            List<BTreeNode<T>> ret = Subnodes.GetRange(0, firstHalfCount);
            Subnodes.RemoveRange(0, firstHalfCount);
            Value = Subnodes.Last().Value;
            return ret;
        }

        public int ToList(List<List<T[]>> valueCounts)
        {
            int level = 0;
            if (Subnodes[0].HasSubnodes)
            {
                foreach(BTreeNode<T> node in Subnodes)
                {
                    level = node.ToList(valueCounts);
                }
            }

            if (valueCounts.Count <= level)
            {
                valueCounts.Add(new List<T[]>());
            }
            valueCounts[level].Add(Subnodes.Select(x => x.Value).ToArray());

            return level + 1;
        }

        public double ForEach(BTree<T>.forEachDelegate action, int level, ref int bottomCount)
        {
            double x = 0;
            if (HasSubnodes)
            {
                double x1 = Subnodes[0].ForEach(action, level + 1, ref bottomCount);
                for (int i = 1; i < Subnodes.Count - 1; i++)
                {
                    Subnodes[i].ForEach(action, level + 1, ref bottomCount);
                }
                double x2 = Subnodes[Subnodes.Count - 1].ForEach(action, level + 1, ref bottomCount);
                x = (x1 + x2) / 2;
            }
            else
            {
                x = bottomCount++;
            }
            action(this, x, level);
            return x;
        }

        public override string ToString()
        {
            return Value.ToString()!;
        }
    }
}
