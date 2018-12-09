using System;
using System.Diagnostics;
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
        public void Add_Example_Left_Test()
        {
            int[] data = new int[] { 43, 18, 22, 9, 21, 6, 8, 20, 63, 50, 62 };

            var tree = new BinaryTree<int>(data);

            Assert.AreEqual("22:{18:{8:{6,9},21+:{20,~}},50-:{43,63+:{62,~}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_Example_Full_Test()
        {
            int[] data = new int[] { 43, 18, 22, 9, 21, 6, 8, 20, 63, 50, 62, 51 };

            var tree = new BinaryTree<int>(data);

            Assert.AreEqual("22:{18:{8:{6,9},21+:{20,~}},50-:{43,62:{51,63}}}", tree.ToString());
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
            var tree = new BinaryTree<int>(new[] { 2, 1, 4, 8 });

            Assert.AreEqual("2-:{1,4-:{~,8}}", tree.ToString());

            tree.Add(20);

            Assert.AreEqual("2-:{1,8:{4,20}}", tree.ToString());
        }

        [TestMethod]
        public void Remove_ParentNullAndReplaceWithRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 1, 2 });

            Assert.AreEqual("1-:{~,2}", tree.ToString());

            bool isDeleted = tree.Remove(1);

            Assert.AreEqual("2", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_ParentNullAndReplaceWithLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 2, 1 });

            Assert.AreEqual("2+:{1,~}", tree.ToString());

            bool isDeleted = tree.Remove(2);

            Assert.AreEqual("1", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_Empty_Test()
        {
            var tree = new BinaryTree<int>(new[] { 1 });

            Assert.AreEqual("1", tree.ToString());

            bool isDeleted = tree.Remove(1);

            Assert.AreEqual(string.Empty, tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_NotFound_Test()
        {
            var tree = new BinaryTree<int>(new[] { 1 });

            Assert.AreEqual("1", tree.ToString());

            bool isDeleted = tree.Remove(2);

            Assert.AreEqual("1", tree.ToString());
            Assert.IsFalse(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 2, 5, 3 });

            Assert.AreEqual("4+:{2-:{~,3},5}", tree.ToString());

            bool isDeleted = tree.Remove(5);

            Assert.AreEqual("3:{2,4}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 2, 5, 6 });

            Assert.AreEqual("4-:{2,5-:{~,6}}", tree.ToString());

            bool isDeleted = tree.Remove(2);

            Assert.AreEqual("5:{4,6}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 2, 6, 5 });

            Assert.AreEqual("4-:{2,6+:{5,~}}", tree.ToString());

            bool isDeleted = tree.Remove(2);

            Assert.AreEqual("5:{4,6}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 4, 2, 5, 1 });

            Assert.AreEqual("4+:{2+:{1,~},5}", tree.ToString());

            bool isDeleted = tree.Remove(1);

            Assert.AreEqual("4:{2,5}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightNull_NonNullParentToLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 9, 4, 20, 3, 19, 21, 27 });

            Assert.AreEqual("9-:{4+:{3,~},20-:{19,21-:{~,27}}}", tree.ToString());

            bool isDeleted = tree.Remove(4);

            Assert.AreEqual("20:{9:{3,19},21-:{~,27}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightNull_NonNullParentToRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 9, 4, 20, 3, 5, 19, 2 });

            Assert.AreEqual("9+:{4+:{3+:{2,~},5},20+:{19,~}}", tree.ToString());

            bool isDeleted = tree.Remove(20);

            Assert.AreEqual("4:{3+:{2,~},9:{5,19}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftNull_NonNullParentToRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 9, 4, 20, 3, 5, 26, 2 });

            Assert.AreEqual("9+:{4+:{3+:{2,~},5},20-:{~,26}}", tree.ToString());

            bool isDeleted = tree.Remove(20);

            Assert.AreEqual("4:{3+:{2,~},9:{5,26}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftNull_NonNullParentToLeft_Test()
        {
            var tree = new BinaryTree<int>(new[] { 9, 4, 20, 5, 19, 21, 27 });

            Assert.AreEqual("9-:{4-:{~,5},20-:{19,21-:{~,27}}}", tree.ToString());

            bool isDeleted = tree.Remove(4);

            Assert.AreEqual("20:{9:{5,19},21-:{~,27}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_SuccessorLeftNull_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26 });

            Assert.AreEqual("20:{4,26}", tree.ToString());

            bool isDeleted = tree.Remove(20);

            Assert.AreEqual("26+:{4,~}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftParentSuccessorLeftNull_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 40, 30, 50 });

            Assert.AreEqual("20-:{10,40:{30,50}}", tree.ToString());

            bool isDeleted = tree.Remove(40);

            Assert.AreEqual("20-:{10,50+:{30,~}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightParentSuccessorLeftNull_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 40, 5, 15 });

            Assert.AreEqual("20+:{10:{5,15},40}", tree.ToString());

            bool isDeleted = tree.Remove(10);

            Assert.AreEqual("20+:{15+:{5,~},40}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_SuccessorRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 4, 26, 25 });

            Assert.AreEqual("20-:{4,26+:{25,~}}", tree.ToString());

            bool isDeleted = tree.Remove(20);

            Assert.AreEqual("25:{4,26}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_RightParentSuccessorRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 40, 15, 30, 50, 45 });

            Assert.AreEqual("20-:{10-:{~,15},40-:{30,50+:{45,~}}}", tree.ToString());

            bool isDeleted = tree.Remove(40);

            Assert.AreEqual("20:{10-:{~,15},45:{30,50}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_LeftParentSuccessorRight_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 40, 5, 15, 30, 14 });

            Assert.AreEqual("20+:{10-:{5,15+:{14,~}},40+:{30,~}}", tree.ToString());

            bool isDeleted = tree.Remove(10);

            Assert.AreEqual("20:{14:{5,15},40+:{30,~}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }


        [TestMethod]
        public void Remove_ExitRebalanceRightEarly_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 30, 5, 15, 35, 4, 16 });

            Assert.AreEqual("20+:{10:{5+:{4,~},15-:{~,16}},30-:{~,35}}", tree.ToString());

            bool isDeleted = tree.Remove(35);

            Assert.AreEqual("10-:{5+:{4,~},20+:{15-:{~,16},30}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Remove_ExitRebalanceLeftEarly_Test()
        {
            var tree = new BinaryTree<int>(new[] { 20, 10, 30, 5, 25, 35, 24, 36 });

            Assert.AreEqual("20-:{10+:{5,~},30:{25+:{24,~},35-:{~,36}}}", tree.ToString());

            bool isDeleted = tree.Remove(5);

            Assert.AreEqual("30+:{20-:{10,25+:{24,~}},35-:{~,36}}", tree.ToString());
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Add_05_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 5; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("2-:{1,4:{3,5}}", tree.ToString());
        }

        [TestMethod]
        public void Add_10_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 10; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("4-:{2:{1,3},8:{6:{5,7},9-:{~,10}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_15_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 15; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("8:{4:{2:{1,3},6:{5,7}},12:{10:{9,11},14:{13,15}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_20_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 20; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("8-:{4:{2:{1,3},6:{5,7}},16:{12:{10:{9,11},14:{13,15}},18-:{17,19-:{~,20}}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_23_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 23; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("8-:{4:{2:{1,3},6:{5,7}},16:{12:{10:{9,11},14:{13,15}},20:{18:{17,19},22:{21,23}}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_24_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 24; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("16:{8:{4:{2:{1,3},6:{5,7}},12:{10:{9,11},14:{13,15}}},20-:{18:{17,19},22-:{21,23-:{~,24}}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_25_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 25; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("16:{8:{4:{2:{1,3},6:{5,7}},12:{10:{9,11},14:{13,15}}},20-:{18:{17,19},22-:{21,24:{23,25}}}}", tree.ToString());
        }

        [TestMethod]
        public void Add_30_Elements_Test()
        {
            var tree = new BinaryTree<int>();
            for (int i = 1; i <= 30; i++)
            {
                tree.Add(i);
            }

            Assert.AreEqual("16:{8:{4:{2:{1,3},6:{5,7}},12:{10:{9,11},14:{13,15}}},24:{20:{18:{17,19},22:{21,23}},28:{26:{25,27},29-:{~,30}}}}", tree.ToString());
        }

        [TestMethod]
        public void Insertion_Forth_Test()
        {
            var stopWatch = new Stopwatch();

            var tree = new BinaryTree<int>();
            stopWatch.Start();

            for (int i = 1; i <= 10000; i++)
            {
                tree.Add(i);
            }

            stopWatch.Stop();
            TimeSpan elapsed = stopWatch.Elapsed;
        }

        [TestMethod]
        public void Insertion_Back_Test()
        {
            var stopWatch = new Stopwatch();

            var tree = new BinaryTree<int>();
            stopWatch.Start();

            for (int i = 10000; i >= 1; i--)
            {
                tree.Add(i);
            }

            stopWatch.Stop();
            TimeSpan elapsed = stopWatch.Elapsed;
        }
    }
}
