using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSets
{
    public class SetCollection<T> where T: CSet
    {
        T[] SetCollections;
        public int Count { get; private set; }
        public SetCollection()
        {
            SetCollections = new T[0];
        }//SetCollection
        public SetCollection(int Length)
        {
            SetCollections = new T[Length];
        }//Initialize
        public SetCollection(T[] sets)
        {
            SetCollections = new T[sets.Length];
            SetCollections = sets;
            Count = sets.Length;
            //(T[])Convert.ChangeType(sets, typeof(T[]));
        }//

        public T this[int iIndex]
        {
            get
            {
                if (iIndex >= SetCollections.Length)
                    throw new IndexOutOfRangeException();
                return SetCollections[iIndex];
            }//get
        }//end Indexer of T

        public void Add(T set)
        {
            Array.Resize(ref SetCollections, SetCollections.Length + 1);
            SetCollections[SetCollections.Length - 1] = set;
            Count++;
        }//Add
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (item.ElementString == SetCollections[i].ElementString)
                    return i;
            }//end for
            return -1;
        }//IndexOf
        public void RemoveAt(int index)
        {
            Remove(index);
        }//Remove
        private void Remove(int iStart)
        {
            if (iStart >= Count)
                throw new IndexOutOfRangeException();
            for (int i = iStart; i < Count - 1; i++)
            {
                SetCollections[i] = SetCollections[i + 1];
            }//end for
            Count--;
            Array.Resize(ref SetCollections, Count);
        }//Remove
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in SetCollections)
            {
                yield return item;
            }//end foreach
        }// GetEnumerator
    }//class
}//namespace
