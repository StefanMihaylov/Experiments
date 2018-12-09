using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    public class BinaryTree<T> : ICollection<T> where T : struct
    {
        private const int INITIAL_CAPACITY = 16;
        private Node<T>[] storage;

        private readonly Func<T, T, int> compare;

        public int Count { get; private set; }
        public int Capacity => this.storage.Length;
        public bool IsReadOnly => false;

        #region Constructors

        public BinaryTree() : this(compare: null)
        {
        }

        public BinaryTree(IComparer<T> comparer) : this(comparer.Compare)
        {
        }

        public BinaryTree(Func<T, T, int> compare)
        {
            if (compare == null)
            {
                Type type = typeof(T);
                if (typeof(IComparable<T>).IsAssignableFrom(type) ||
                   typeof(IComparable).IsAssignableFrom(type))
                {
                    this.compare = Comparer<T>.Default.Compare;
                }
                else
                {
                    throw new InvalidOperationException($"The type {type.FullName} cannot be compared. It must implement IComparable.");
                }
            }
            else
            {
                this.compare = compare;
            }

            this.Clear();
        }

        public BinaryTree(IEnumerable<T> values) : this()
        {
            this.AddRange(values);
        }

        #endregion

        #region Public

        public T FindMin()
        {
            int nodeIndex = FindExtremeValueIndex(0, GetLeftChildIndex);
            return this.storage[nodeIndex].Value;
        }

        public T FindMax()
        {
            int nodeIndex = FindExtremeValueIndex(0, GetRightChildIndex);
            return this.storage[nodeIndex].Value;
        }

        public T Search(T value)
        {
            return Search(value, 0);
        }

        public void AddRange(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (T item in values)
            {
                this.Add(item);
            }
        }

        public void Add(T value)
        {
            this.Add(value, 0, 0);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            Description(builder, 0);

            return builder.ToString();
        }

        public string Print()
        {
            var builder = new StringBuilder();

            int idents = 0;
            int previousShift = 0;
            int step = 1;

            for (int i = 0; i < this.storage.Length; i++)
            {
                string direction = i != 0 ? (i % 2 != 0 ? "L" : "R") : "T";
                builder.AppendLine($"{new string('\t', idents)}|{direction}| {this.storage[i]} ");

                if (i == 0 || (i - previousShift) >= step)
                {
                    idents++;
                    previousShift = i;
                    step += step;
                }
            }

            return builder.ToString();
        }

        public void Clear()
        {
            this.storage = new Node<T>[INITIAL_CAPACITY];
        }

        public bool Contains(T item)
        {
            return !this.Search(item).Equals(default(T));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.storage.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Remove(item, 0);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.storage)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Private

        private T Search(T value, int nodeIndex)
        {
            Node<T> currentNode = this.storage.ElementAtOrDefault(nodeIndex);
            if (currentNode == null)
            {
                return default(T);
            }

            int compareResult = this.compare(value, currentNode.Value);
            if (compareResult < 0)
            {
                return Search(value, GetLeftChildIndex(nodeIndex));
            }
            else if (compareResult > 0)
            {
                return Search(value, GetRightChildIndex(nodeIndex));
            }
            else
            {
                return currentNode.Value;
            }
        }

        private void Add(T newElement, int nodeIndex, int balance)
        {
            if (nodeIndex >= this.storage.Length)
            {
                DoubleArraySize(ref this.storage);
            }

            Node<T> currentNode = this.storage.ElementAtOrDefault(nodeIndex);
            if (currentNode == null)
            {
                this.storage[nodeIndex] = new Node<T> { Value = newElement, Balance = 0 };
                this.Count++;

                int parentIndex = GetParentIndex(nodeIndex);
                Balance(parentIndex, balance);

                return;
            }

            int compareResult = this.compare(newElement, currentNode.Value);
            if (compareResult < 0)
            {
                Add(newElement, GetLeftChildIndex(nodeIndex), 1);
            }
            else if (compareResult > 0)
            {
                Add(newElement, GetRightChildIndex(nodeIndex), -1);
            }
            else
            {
                throw new ArgumentException($"Overlapping between {newElement} and {currentNode.Value}");
            }
        }

        private bool Remove(T value, int nodeIndex)
        {
            Node<T> currentNode = this.storage.ElementAtOrDefault(nodeIndex);
            if (currentNode == null)
            {
                return false;
            }

            int compareResult = this.compare(value, currentNode.Value);
            if (compareResult < 0)
            {
                return Remove(value, GetLeftChildIndex(nodeIndex));
            }
            else if (compareResult > 0)
            {
                return Remove(value, GetRightChildIndex(nodeIndex));
            }
            else
            {
                int leftIndex = this.GetLeftChildIndex(nodeIndex);
                int rightIndex = this.GetRightChildIndex(nodeIndex);
                Node<T> left = this.storage.ElementAtOrDefault(leftIndex);
                Node<T> right = this.storage.ElementAtOrDefault(rightIndex);

                if (left == null && right == null)
                {
                    this.storage[nodeIndex] = null;

                    int parentIndex = this.GetParentIndex(nodeIndex);
                    DeleteBalance(parentIndex, -GetParentBalance(nodeIndex));
                }
                else if (left == null && right != null)
                {
                    Node<T>[] rightSubTree = new Node<T>[INITIAL_CAPACITY / 2];
                    GetSubTree(rightIndex, ref rightSubTree, 0);
                    AddSubTree(nodeIndex, rightSubTree, 0);

                    DeleteBalance(nodeIndex, 0);
                }
                else if (left != null && right == null)
                {
                    Node<T>[] leftSubTree = new Node<T>[INITIAL_CAPACITY / 2];
                    GetSubTree(leftIndex, ref leftSubTree, 0);
                    AddSubTree(nodeIndex, leftSubTree, 0);

                    DeleteBalance(nodeIndex, 0);
                }
                else // left != null && right != null
                {
                    int successorIndex = FindExtremeValueIndex(rightIndex, GetLeftChildIndex);
                    this.storage[successorIndex].Balance = this.storage[nodeIndex].Balance;
                    this.storage[nodeIndex] = this.storage[successorIndex];
                    this.storage[successorIndex] = null;

                    int successorParentIndex = this.GetParentIndex(successorIndex);
                    int successotRightIndex = this.GetRightChildIndex(successorIndex);
                    Node<T>[] rightSubTree = new Node<T>[INITIAL_CAPACITY / 2];
                    GetSubTree(successotRightIndex, ref rightSubTree, 0);
                    AddSubTree(successorIndex, rightSubTree, 0);

                    if (successorIndex == rightIndex)
                    {
                        DeleteBalance(nodeIndex, 1);
                    }
                    else
                    {
                        DeleteBalance(successorParentIndex, -1);
                    }
                }

                return true;
            }
        }

        private void DeleteBalance(int nodeIndex, int balance)
        {
            while (nodeIndex >= 0)
            {
                Node<T> node = this.storage.ElementAtOrDefault(nodeIndex);

                int currentBalance = node.Balance + balance;
                node.Balance = currentBalance;

                if (currentBalance == 2)
                {
                    int leftIndex = this.GetLeftChildIndex(nodeIndex);
                    if (this.storage[leftIndex].Balance >= 0)
                    {
                        // RotateRight
                        RotateSingle(nodeIndex, GetLeftChildIndex, GetRightChildIndex, -1);
                    }
                    else
                    {
                        // RotateLeftRight
                        RotateDouble(nodeIndex, GetLeftChildIndex, GetRightChildIndex, -1);
                    }
                }
                else if (currentBalance == -2)
                {
                    int rightIndex = this.GetRightChildIndex(nodeIndex);
                    if (this.storage[rightIndex].Balance <= 0)
                    {
                        // RotateLeft
                        RotateSingle(nodeIndex, GetRightChildIndex, GetLeftChildIndex, +1);
                    }
                    else
                    {
                        // RotateRightLeft
                        RotateDouble(nodeIndex, GetRightChildIndex, GetLeftChildIndex, +1);
                    }
                }
                else if (currentBalance != 0)
                {
                    return;
                }

                int parentIndex = GetParentIndex(nodeIndex);
                if (parentIndex >= 0)
                {
                    balance = -GetParentBalance(nodeIndex);
                }

                nodeIndex = parentIndex;
            }
        }

        private void Balance(int nodeIndex, int balance)
        {
            while (nodeIndex >= 0)
            {
                int currentBalance = this.storage[nodeIndex].Balance + balance;
                this.storage[nodeIndex].Balance = currentBalance;

                if (currentBalance == 0)
                {
                    return;
                }
                else if (currentBalance == 2)
                {
                    int leftChildIndex = GetLeftChildIndex(nodeIndex);
                    if (this.storage[leftChildIndex].Balance == 1)
                    {
                        // RotateRight
                        RotateSingle(nodeIndex, GetLeftChildIndex, GetRightChildIndex, -1);
                    }
                    else
                    {
                        // RotateLeftRight
                        RotateDouble(nodeIndex, GetLeftChildIndex, GetRightChildIndex, -1);
                    }

                    return;
                }
                else if (currentBalance == -2)
                {
                    int rightChildIndex = GetRightChildIndex(nodeIndex);
                    if (this.storage[rightChildIndex].Balance == -1)
                    {
                        // RotateLeft
                        RotateSingle(nodeIndex, GetRightChildIndex, GetLeftChildIndex, +1);
                    }
                    else
                    {
                        // RotateRightLeft
                        RotateDouble(nodeIndex, GetRightChildIndex, GetLeftChildIndex, +1);
                    }

                    return;
                }

                int parentIndex = GetParentIndex(nodeIndex);
                if (parentIndex >= 0)
                {
                    balance = GetParentBalance(nodeIndex);
                }

                nodeIndex = parentIndex;

            }
        }

        private void RotateSingle(int nodeIndex, Func<int, int> getChildIndex, Func<int, int> getSubTreeIndex, int balanceCorrection)
        {
            Node<T> current = this.storage[nodeIndex];

            int childIndex = getChildIndex(nodeIndex);
            Node<T> child = this.storage[childIndex];

            int subTreeIndex = getSubTreeIndex(nodeIndex);
            Node<T>[] subTree = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(subTreeIndex, ref subTree, 0);

            int childLeftIndex = getChildIndex(childIndex);
            Node<T>[] childLeft = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(childLeftIndex, ref childLeft, 0);

            int childRightIndex = getSubTreeIndex(childIndex);
            Node<T>[] childRight = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(childRightIndex, ref childRight, 0);

            this.storage[nodeIndex] = child;
            AddSubTree(childIndex, childLeft, 0);
            this.storage[subTreeIndex] = current;

            int rightLeftIndex = getChildIndex(subTreeIndex);
            int rightRightIndex = getSubTreeIndex(subTreeIndex);

            AddSubTree(rightLeftIndex, childRight, 0);
            AddSubTree(rightRightIndex, subTree, 0);

            child.Balance += balanceCorrection;
            current.Balance = -child.Balance;
        }

        private void RotateDouble(int nodeIndex, Func<int, int> getChildIndex, Func<int, int> getSubTreeIndex, int balanceCorrection)
        {
            Node<T> current = this.storage[nodeIndex];

            int leftIndex = getChildIndex(nodeIndex);
            Node<T> left = this.storage[leftIndex];

            int rightIndex = getSubTreeIndex(nodeIndex);
            Node<T>[] right = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(rightIndex, ref right, 0);

            int leftLeftIndex = getChildIndex(leftIndex);
            Node<T>[] leftLeft = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(leftLeftIndex, ref leftLeft, 0);

            int leftRightIndex = getSubTreeIndex(leftIndex);
            Node<T> leftRight = this.storage[leftRightIndex];

            int leftRightLeftIndex = getChildIndex(leftRightIndex);
            Node<T>[] leftRightLeft = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(leftRightLeftIndex, ref leftRightLeft, 0);

            int leftRightRightIndex = getSubTreeIndex(leftRightIndex);
            Node<T>[] leftRightRight = new Node<T>[INITIAL_CAPACITY / 2];
            GetSubTree(leftRightRightIndex, ref leftRightRight, 0);

            int rightLeftIndex = getChildIndex(rightIndex);
            int rightRightIndex = getSubTreeIndex(rightIndex);

            this.storage[nodeIndex] = leftRight;
            this.storage[leftIndex] = left;
            this.storage[rightIndex] = current;
            AddSubTree(leftLeftIndex, leftLeft, 0);
            AddSubTree(leftRightIndex, leftRightLeft, 0);
            AddSubTree(rightLeftIndex, leftRightRight, 0);
            AddSubTree(rightRightIndex, right, 0);

            if (leftRight.Balance == balanceCorrection)
            {
                current.Balance = 0;
                left.Balance = -balanceCorrection;
            }
            else if (leftRight.Balance == 0)
            {
                current.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                current.Balance = balanceCorrection;
                left.Balance = 0;
            }

            leftRight.Balance = 0;
        }

        private void AddSubTree(int nodeIndex, Node<T>[] subTree, int subTreeIndex)
        {
            if (nodeIndex >= this.storage.Length)
            {
                DoubleArraySize(ref this.storage);
            }

            this.storage[nodeIndex] = subTree[subTreeIndex];

            int subTreeLeftIndex = GetLeftChildIndex(subTreeIndex);
            int subTreeRigtIndex = GetRightChildIndex(subTreeIndex);

            Node<T> left = subTree.ElementAtOrDefault(subTreeLeftIndex);
            Node<T> right = subTree.ElementAtOrDefault(subTreeRigtIndex);
            if (left == null && right == null)
            {
                return;
            }

            int leftIndex = GetLeftChildIndex(nodeIndex);
            int rigtIndex = GetRightChildIndex(nodeIndex);

            AddSubTree(leftIndex, subTree, subTreeLeftIndex);
            AddSubTree(rigtIndex, subTree, subTreeRigtIndex);
        }

        private void GetSubTree(int nodeIndex, ref Node<T>[] subTree, int subTreeIndex)
        {
            if (subTreeIndex >= subTree.Length)
            {
                DoubleArraySize(ref subTree);
            }

            Node<T> current = this.storage.ElementAtOrDefault(nodeIndex);

            if (current != null)
            {
                subTree[subTreeIndex] = current;
                this.storage[nodeIndex] = null;
            }

            int leftIndex = GetLeftChildIndex(nodeIndex);
            int rigtIndex = GetRightChildIndex(nodeIndex);

            Node<T> left = this.storage.ElementAtOrDefault(leftIndex);
            Node<T> right = this.storage.ElementAtOrDefault(rigtIndex);
            if (left == null && right == null)
            {
                return;
            }

            int subTreeLeftIndex = GetLeftChildIndex(subTreeIndex);
            int subTreeRigtIndex = GetRightChildIndex(subTreeIndex);

            GetSubTree(leftIndex, ref subTree, subTreeLeftIndex);
            GetSubTree(rigtIndex, ref subTree, subTreeRigtIndex);
        }

        private int FindExtremeValueIndex(int nodeIndex, Func<int, int> getChildIndex)
        {
            Node<T> current = this.storage.ElementAtOrDefault(nodeIndex);
            int childIndex = getChildIndex(nodeIndex);
            Node<T> child = this.storage.ElementAtOrDefault(childIndex);

            if (current != null && child == null)
            {
                return nodeIndex;
            }

            return FindExtremeValueIndex(childIndex, getChildIndex);
        }

        private void DoubleArraySize<P>(ref P[] array)
        {
            P[] cachedArray = array;
            array = new P[2 * cachedArray.Length];
            Array.Copy(cachedArray, array, cachedArray.Length);
        }

        private int GetParentIndex(int nodeIndex)
        {
            return (int)Math.Floor((double)(nodeIndex - 1) / 2);
        }

        private int GetParentBalance(int nodeIndex)
        {
            return (nodeIndex % 2 == 0) ? -1 : +1;
        }

        private int GetLeftChildIndex(int nodeIndex)
        {
            return nodeIndex + nodeIndex + 1;
        }

        private int GetRightChildIndex(int nodeIndex)
        {
            return GetLeftChildIndex(nodeIndex) + 1;
        }

        private void Description(StringBuilder builder, int nodeIndex)
        {
            Node<T> node = this.storage.ElementAtOrDefault(nodeIndex);
            if (node != null)
            {
                builder.Append(node.Value);

                if (node.Balance >= 0)
                {
                    builder.Append(new string('+', node.Balance));
                }
                else
                {
                    builder.Append(new string('-', -node.Balance));
                }

                int leftIndex = this.GetLeftChildIndex(nodeIndex);
                int rightIndex = this.GetRightChildIndex(nodeIndex);
                Node<T> left = this.storage.ElementAtOrDefault(leftIndex);
                Node<T> right = this.storage.ElementAtOrDefault(rightIndex);

                if (left != null || right != null)
                {
                    builder.Append(":{");

                    if (left == null)
                    {
                        builder.Append("~");
                    }
                    else
                    {
                        Description(builder, leftIndex);
                    }

                    builder.Append(",");

                    if (right == null)
                    {
                        builder.Append("~");
                    }
                    else
                    {
                        Description(builder, rightIndex);
                    }

                    builder.Append("}");
                }
            }
        }

        private class Node<N>
        {
            public N Value { get; set; }

            public int Balance { get; set; }

            public override string ToString()
            {
                string value = this != null ? this.Value.ToString() : "<null>";
                string balance = this != null ? this.Balance.ToString() : "-";
                return $"{value} ({balance})";
            }
        }

        #endregion
    }
}
