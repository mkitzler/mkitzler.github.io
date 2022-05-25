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
            //Add / Insert
            int addAt = 0;
            for (; addAt < Subnodes.Count && value.CompareTo(Subnodes[addAt].Value) >= 0; addAt++) {}
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
                List<BTreeNode<T>>? ret = Subnodes.GetRange(0, firstHalfCount);
                Subnodes.RemoveRange(0, firstHalfCount);
                Value = Subnodes.Last().Value;
                return ret;
            }
            else
            {
                return null;
            }
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

        public void ForEach(Action<BTreeNode<T>, int> action, int level)
        {
            Subnodes.ForEach(x => x.ForEach(action, 0));
        }
    }
}
