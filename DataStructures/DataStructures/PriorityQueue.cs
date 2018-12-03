using System;
using System.Linq;

namespace DataStructures
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private const int INITIAL_CAPACITY = 16;
        private T[] storage;

        public PriorityQueue()
        {
            this.storage = new T[INITIAL_CAPACITY];
            this.Count = 0;
        }

        public int Count { get; private set; }

        public void Clear()
        {
            this.storage = new T[this.storage.Length];
            this.Count = 0;
        }

        public T Peek()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            return this.storage[0];
        }

        public void Enqueue(T element)
        {
            this.storage[this.Count] = element;
            CheckParentPosition(this.Count);
            this.Count++;
            if (this.Count >= this.storage.Length)
            {
                DoubleTheStorageSize();
            }
        }

        public T Dequeue()
        {
            T result = this.Peek();

            if (this.Count > 1)
            {
                T last = this.storage[this.Count - 1];
                this.storage[0] = last;
            }

            this.storage[this.Count - 1] = default(T);
            this.Count--;

            RearrangeTheHeap(0);

            return result;
        }

        public override string ToString()
        {
            return string.Join(", ", this.storage);
        }

        private void DoubleTheStorageSize()
        {
            var cashedStorage = this.storage;
            this.storage = new T[2 * cashedStorage.Length];
            for (int i = 0; i < cashedStorage.Length; i++)
            {
                this.storage[i] = cashedStorage[i];
            }
        }

        private void CheckParentPosition(int childIndex)
        {
            int parentIndex = (int)Math.Floor((double)(childIndex - 1) / 2);
            if (parentIndex >= 0)
            {
                T parent = this.storage[parentIndex];
                T child = this.storage[childIndex];
                if (parent.CompareTo(child) > 0)
                {
                    this.storage[parentIndex] = child;
                    this.storage[childIndex] = parent;
                    CheckParentPosition(parentIndex);
                }
            }
        }

        private void RearrangeTheHeap(int parentTndex)
        {
            int leftChildIndex = 2 * parentTndex + 1;
            int rightChildIndex = leftChildIndex + 1;
            int largestIndex = parentTndex;

            if (leftChildIndex < this.Count)
            {
                T leftChild = this.storage[leftChildIndex];
                T largest = this.storage[largestIndex];
                if (largest.CompareTo(leftChild) > 0)
                {
                    largestIndex = leftChildIndex;
                }
            }

            if (rightChildIndex < this.Count)
            {
                T rightChild = this.storage[rightChildIndex];
                T largest = this.storage[largestIndex];
                if (largest.CompareTo(rightChild) > 0)
                {
                    largestIndex = rightChildIndex;
                }
            }

            if (largestIndex != parentTndex)
            {
                T parent = this.storage[parentTndex];
                this.storage[parentTndex] = this.storage[largestIndex];
                this.storage[largestIndex] = parent;

                RearrangeTheHeap(largestIndex);
            }
        }

        public string Print(Func<T, object> action)
        {
            return string.Join("|", this.storage.Where(a => a != null).Select(action));
        }
    }
}
