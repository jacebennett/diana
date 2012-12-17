using System;
using System.Collections;
using System.Collections.Generic;

namespace Diana.Util
{
    public interface IImmutableBag<T> : IEnumerable<T> where T : class 
    {
        bool Empty { get; }
        int Count { get; }
        
        T this[int index] { get; set; }

        bool Contains(T item);

    }

    public interface IBag<T> : IImmutableBag<T> where T : class 
    {
        int Capacity { get; }
        void EnsureCapacity(int size);

        void Add(T item);
        void AddAll(IImmutableBag<T> other);
        
        T Remove(int index);
        bool Remove(T item);
        T RemoveLast();
        bool RemoveAll(IImmutableBag<T> other);

        void Clear();
    }

    public class Bag<T> : IBag<T> where T : class 
    {
        private T[] _data;
        private int _count = 0;

        public Bag(int capacity = 64)
        {
            _data = new T[capacity];    
        }

        public T this[int index]
        {
            get { return _data[index]; }
            set
            {
                if (index >= _data.Length)
                {
                    Grow(index * 2);
                }

                _count = index + 1;
                _data[index] = value;
            }
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (item == _data[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(T item)
        {
            if (_count == _data.Length)
            {
                Grow();
            }

            _data[_count++] = item;
        }

        public void AddAll(IImmutableBag<T> other)
        {
            for (int i = 0; i < other.Count; i++)
            {
                Add(other[i]);
            }
        }

        public T Remove(int index)
        {
            var item = _data[index];
            _data[index] = _data[--_count];
            _data[_count] = null;
            return item;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                T cursor = _data[i];

                if (item == cursor)
                {
                    _data[i] = _data[--_count]; // overwrite item to remove with last element
                    _data[_count] = null; // null last element, so gc can do its work
                    return true;
                }
            }

            return false;
        }

        public T RemoveLast()
        {
            if (_count > 0)
            {
                var val = _data[--_count];
                _data[_count] = null;
                return val;
            }
            
            return null;
        }

        public bool RemoveAll(IImmutableBag<T> other)
        {
            var modified = false;

            for (int i = 0; i < other.Count; i++)
            {
                var otherItem = other[i];

                for (int j = 0; j < _count; j++)
                {
                    var localItem = _data[j];

                    if (localItem == otherItem)
                    {
                        Remove(j);
                        modified = true;
                        break;
                    }
                }
            }

            return modified;
        }

        public void Clear()
        {
            // null all elements so gc can clean up
            for (int i = 0; i < _count; i++)
            {
                _data[i] = null;
            }

            _count = 0;
        }
        
        public bool Empty    { get { return _count == 0; } }
        public int  Count    { get { return _count; } }
        public int  Capacity { get { return _data.Length; } }

        private void Grow() {
            int newCapacity = (_data.Length * 3) / 2 + 1;
		    Grow(newCapacity);
	    }

        private void Grow(int newCapacity)
        {
		    T[] oldData = _data;
            _data = new T[newCapacity];
            oldData.CopyTo(_data, 0);
        }
	
	    public void EnsureCapacity(int index) {
		    if(index >= _data.Length) {
			    Grow(index*2);
		    }
	    }

        public IEnumerable<T> AsEnumerable()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _data[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}