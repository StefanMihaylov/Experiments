using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructures.Tests
{
    [TestClass]
    public class BinaryTreeTests
    {
        [TestMethod]
        public void Search_MinValue_Test()
        {
            int[] data = new int[] { 5, 2, 7, 4, 1, 3, 8, 6, 9 };

            var tree = new BinaryTree<int>(data);

            var expected = data.Min();
            var min = tree.FindMin();

            Assert.AreEqual(expected, min);
        }

        [TestMethod]
        public void Search_MaxValue_Test()
        {
            int[] data = new int[] { 5, 2, 7, 4, 1, 3, 8, 6, 9 };

            var tree = new BinaryTree<int>(data);

            var expected = data.Max();
            var max = tree.FindMax();

            Assert.AreEqual(expected, max);
        }

        [TestMethod]
        public void Search_Found_Test()
        {
            int[] data = new int[] { 5, 2, 7, 4, 1, 3, 8, 6, 9 };

            var tree = new BinaryTree<int>(data);

            int expected = 6;
            var actual = tree.Search(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Search_NotFound_Test()
        {
            int[] data = new int[] { 5, 2, 7, 4, 1, 3, 8, 6, 9 };

            var tree = new BinaryTree<int>(data);

            var actual = tree.Search(-5);

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void Add_Example_LeftRight_Test()
        {
            int[] data = new int[] { 43, 18, 22 };

            var tree = new BinaryTree<int>(data);

            Assert.AreEqual(data[2], tree[0]);
            Assert.AreEqual(data[1], tree[1]);
            Assert.AreEqual(data[0], tree[2]);
            Assert.AreEqual("22:{18,43}", tree.ToString());
        }

        [TestMethod]
        public void Add_Example_Right_Test()
        {
            int[] data = new int[] { 43, 18, 22, 9, 21, 6 };

            var tree = new BinaryTree<int>(data);

            Assert.AreEqual("18:{9+:{6,~},22:{21,43}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Example_RightLeft_Test()
        {
            int[] data = new int[] { 43, 18, 22, 9, 21, 6, 8, 20, 63, 50 };

            var tree = new BinaryTree<int>(data);

            Assert.AreEqual("18-:{8:{6,9},22:{21+:{20,~},50:{43,63}}}", tree.ToString());
        }



        [TestMethod]
        public void Count_Empty()
        {
            var tree = new BinaryTree<int>();

            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void Count_CountMultiple()
        {
            var tree = new BinaryTree<int>(new[] { 20, 8, 22 });

            Assert.AreEqual(3, tree.Count);
        }

        [TestMethod]
        public void Add_Rotation_LeftLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 8 });

            Assert.AreEqual("20+:{8,~}", tree.ToString());

            tree.Add(4);

            Assert.AreEqual("8:{4,20}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_LeftRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4 });

            Assert.AreEqual("20+:{4,~}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("8:{4,20}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 20 });

            Assert.AreEqual("4-:{~,20}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("8:{4,20}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 8 });

            Assert.AreEqual("4-:{~,8}", tree.ToString());

            tree.Add(20);

            Assert.AreEqual("8:{4,20}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RootLeftRightRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26, 3, 9 });

            Assert.AreEqual("20+:{4:{3,9},26}", tree.ToString());

            tree.Add(15);

            Assert.AreEqual("9:{4+:{3,~},20:{15,26}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RootLeftRightLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26, 3, 9 });

            Assert.AreEqual("20+:{4:{3,9},26}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("9:{4:{3,8},20-:{~,26}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RootRightLeftLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26, 24, 30 });

            Assert.AreEqual("20-:{4,26:{24,30}}", tree.ToString());

            tree.Add(23);

            Assert.AreEqual("24:{20:{4,23},26-:{~,30}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RootRightLeftRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26, 24, 30 });

            Assert.AreEqual("20-:{4,26:{24,30}}", tree.ToString());

            tree.Add(25);

            Assert.AreEqual("24:{20+:{4,~},26:{25,30}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightParentedLeftLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 2, 1, 20, 8 });

            Assert.AreEqual("2-:{1,20+:{8,~}}", tree.ToString());

            tree.Add(4);

            Assert.AreEqual("2-:{1,8:{4,20}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_LeftParentedLeftLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 24, 28, 20, 8 });

            Assert.AreEqual("24+:{20+:{8,~},28}", tree.ToString());

            tree.Add(4);

            Assert.AreEqual("24+:{8:{4,20},28}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_LeftParentedLeftRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 24, 28, 20, 4 });

            Assert.AreEqual("24+:{20+:{4,~},28}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("24+:{8:{4,20},28}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightParentedLeftRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 2, 1, 20, 4 });

            Assert.AreEqual("2-:{1,20+:{4,~}}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("2-:{1,8:{4,20}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_LeftParentedRightLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 24, 28, 4, 20 });

            Assert.AreEqual("24+:{4-:{~,20},28}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("24+:{8:{4,20},28}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightParentedRightLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 2, 1, 4, 20 });

            Assert.AreEqual("2-:{1,4-:{~,20}}", tree.ToString());

            tree.Add(8);

            Assert.AreEqual("2-:{1,8:{4,20}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_LeftParentedRightRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 24, 28, 4, 8 });

            Assert.AreEqual("24+:{4-:{~,8},28}", tree.ToString());

            tree.Add(20);

            Assert.AreEqual("24+:{8:{4,20},28}", tree.ToString());
        }

        [TestMethod]
        public void Add_Rotation_RightParentedRightRight_Test()
        {
            var tree = new BinaryTree<int>(new [] { 2, 1, 4, 8 });

            Assert.AreEqual("2-:{1,4-:{~,8}}", tree.ToString());

            tree.Add(20);

            Assert.AreEqual("2-:{1,8:{4,20}}", tree.ToString());
        }
    }
}
