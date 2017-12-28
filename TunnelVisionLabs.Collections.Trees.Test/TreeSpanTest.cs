// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class TreeSpanTest
    {
        [Fact]
        public void TestConstructor()
        {
            var treeSpan = new TreeSpan(50, 50);
            Assert.Equal(50, treeSpan.Start);
            Assert.Equal(50, treeSpan.Count);
        }

        [Fact]
        public void TestProperties()
        {
            var treeSpan = new TreeSpan(50, 50);
            Assert.False(treeSpan.IsEmpty);
            Assert.Equal(50, treeSpan.Start);
            Assert.Equal(50, treeSpan.Count);
            Assert.Equal(100, treeSpan.EndExclusive);
            Assert.Equal(99, treeSpan.EndInclusive);

            Assert.True(new TreeSpan(50, 0).IsEmpty);
        }

        [Fact]
        public void TestOperators()
        {
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(TreeSpan.Invalid == TreeSpan.Invalid);
            Assert.False(TreeSpan.Invalid != TreeSpan.Invalid);
#pragma warning restore CS1718 // Comparison made to same variable

            Assert.True(new TreeSpan(50, 50) == TreeSpan.FromBounds(50, 100));
            Assert.False(new TreeSpan(50, 50) != TreeSpan.FromBounds(50, 100));

            Assert.False(new TreeSpan(0, 0) == TreeSpan.Invalid);
            Assert.True(new TreeSpan(0, 0) != TreeSpan.Invalid);
        }

        [Fact]
        public void TestFromBounds()
        {
            var treeSpan = TreeSpan.FromBounds(50, 100);
            Assert.Equal(50, treeSpan.Start);
            Assert.Equal(100, treeSpan.EndExclusive);
            Assert.Equal(50, treeSpan.Count);

            treeSpan = TreeSpan.FromBounds(50, 50);
            Assert.Equal(50, treeSpan.Start);
            Assert.Equal(0, treeSpan.Count);
            Assert.True(treeSpan.IsEmpty);
        }

        [Fact]
        public void TestFromReverseSpan()
        {
            var treeSpan = TreeSpan.FromReverseSpan(50, 1);
            Assert.Equal(50, treeSpan.Start);
            Assert.Equal(1, treeSpan.Count);
            Assert.Equal(51, treeSpan.EndExclusive);
            Assert.Equal(50, treeSpan.EndInclusive);

            treeSpan = TreeSpan.FromReverseSpan(50, 5);
            Assert.Equal(46, treeSpan.Start);
            Assert.Equal(5, treeSpan.Count);
            Assert.Equal(51, treeSpan.EndExclusive);
            Assert.Equal(50, treeSpan.EndInclusive);

            treeSpan = TreeSpan.FromReverseSpan(50, 0);
            Assert.Equal(51, treeSpan.Start);
            Assert.Equal(0, treeSpan.Count);
            Assert.Equal(51, treeSpan.EndExclusive);
            Assert.Equal(50, treeSpan.EndInclusive);
        }

        [Fact]
        public void TestIntersect()
        {
            var treeSpan = TreeSpan.Intersect(TreeSpan.Invalid, TreeSpan.Invalid);
            Assert.Equal(TreeSpan.Invalid, treeSpan);

            treeSpan = TreeSpan.Intersect(TreeSpan.FromBounds(45, 50), TreeSpan.FromBounds(50, 60));
            Assert.Equal(TreeSpan.FromBounds(50, 50), treeSpan);

            treeSpan = TreeSpan.Intersect(TreeSpan.FromBounds(40, 45), TreeSpan.FromBounds(50, 55));
            Assert.Equal(TreeSpan.Invalid, treeSpan);
        }

        [Fact]
        public void TestOffset()
        {
            var treeSpan = new TreeSpan(50, 50);
            Assert.Equal(treeSpan, treeSpan.Offset(0));

            var offset = treeSpan.Offset(5);
            Assert.Equal(new TreeSpan(55, 50), offset);
            Assert.Equal(treeSpan, offset.Offset(-5));
        }

        [Fact]
        public void TestEquals()
        {
            var treeSpan = new TreeSpan(50, 50);
            Assert.False(treeSpan.Equals(new object()));
            Assert.False(treeSpan.Equals(null));
            Assert.False(treeSpan.Equals((object)TreeSpan.Invalid));
            Assert.True(treeSpan.Equals((object)new TreeSpan(50, 50)));
        }

        [Fact]
        public void TestIEquatable()
        {
            IEquatable<TreeSpan> treeSpan = TreeSpan.Invalid;
            Assert.True(treeSpan.Equals(TreeSpan.Invalid));

            treeSpan = new TreeSpan(50, 50);
            Assert.True(treeSpan.Equals(TreeSpan.FromBounds(50, 100)));
        }

        [Fact]
        public void TestIsSubspanOf()
        {
            Assert.True(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(35, 55)));
            Assert.True(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(35, 50)));
            Assert.True(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(40, 55)));
            Assert.True(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(40, 50)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(35, 45)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(45, 55)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsSubspanOf(TreeSpan.FromBounds(41, 49)));
        }

        [Fact]
        public void TestIsProperSubspanOf()
        {
            Assert.True(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(35, 55)));
            Assert.True(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(35, 50)));
            Assert.True(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(40, 55)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(40, 50)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(35, 45)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(45, 55)));
            Assert.False(TreeSpan.FromBounds(40, 50).IsProperSubspanOf(TreeSpan.FromBounds(41, 49)));
        }

        [Fact]
        public void TestGetHashCode()
        {
            var treeSpan = new TreeSpan(14, 40);
            Assert.Equal(EqualityComparer<TreeSpan>.Default.GetHashCode(treeSpan), treeSpan.GetHashCode());

            // Verify that the start position impacts the hash code (may need adjustment if the hash code algorithm changes)
            Assert.NotEqual(treeSpan.GetHashCode(), new TreeSpan(treeSpan.Start + 1, treeSpan.Count).GetHashCode());

            // Verify that the count impacts the hash code (may need adjustment if the hash code algorithm changes)
            Assert.NotEqual(treeSpan.GetHashCode(), new TreeSpan(treeSpan.Start, treeSpan.Count + 1).GetHashCode());
        }

        [Fact]
        public void TestToString()
        {
            Assert.Equal("[50, 100)", new TreeSpan(50, 50).ToString());
            Assert.Equal("[50, 100)", TreeSpan.FromBounds(50, 100).ToString());
            Assert.Equal("[50, 100)", TreeSpan.FromReverseSpan(99, 50).ToString());
            Assert.Equal("[-1, -1)", TreeSpan.Invalid.ToString());
        }
    }
}
