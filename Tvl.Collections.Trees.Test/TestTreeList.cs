// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TreeCollectionTests
{
    using System;
    using System.Linq;
    using Tvl.Collections.Trees;
    using Xunit;

    public class TestTreeList
    {
        [Fact]
        public void TestTreeListConstructor()
        {
            TreeList<int> list = new TreeList<int>();
            Assert.Empty(list);
        }

        [Fact]
        public void TestTreeListBranchingFactorConstructor()
        {
            TreeList<int> list = new TreeList<int>(8);
            Assert.Empty(list);

            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(1));
        }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            TreeList<int> list = new TreeList<int>();
            Assert.Empty(list);
            list.Add(Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            TreeList<int> list = new TreeList<int>(branchingFactor: 3);
            foreach (var item in expected)
            {
                list.Add(item);
                Assert.Equal(item, list[list.Count - 1]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestInsert()
        {
            const int Value = 600;

            TreeList<int> list = new TreeList<int>();
            Assert.Empty(list);
            list.Insert(0, Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestInsertMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            TreeList<int> list = new TreeList<int>(branchingFactor: 3);
            foreach (var item in expected.Reverse())
            {
                list.Insert(0, item);
                Assert.Equal(item, list[0]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }
    }
}
