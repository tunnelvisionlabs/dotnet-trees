// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableTreeListBuilderTest
    {
        /// <summary>
        /// Tests for <see cref="ImmutableTreeList{T}.Builder.GetRange(int, int)"/>.
        /// </summary>
        public class GetRange
        {
            [Fact(DisplayName = "PosTest1: The generic type is int")]
            public void PosTest1()
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                int startIdx = Generator.GetInt32(0, 9);       // The starting index of the section to make a shallow copy
                int endIdx = Generator.GetInt32(startIdx, 10); // The end index of the section to make a shallow copy
                int count = endIdx - startIdx + 1;
                ImmutableTreeList<int> listResult = listObject.GetRange(startIdx, count);
                for (int i = 0; i < count; i++)
                {
                    Assert.Equal(iArray[i + startIdx], listResult[i]);
                }
            }

            [Fact(DisplayName = "PosTest2: The generic type is type of string")]
            public void PosTest2()
            {
                string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
                var listObject = ImmutableTreeList.Create(strArray).ToBuilder();
                int startIdx = Generator.GetInt32(0, 4);      // The starting index of the section to make a shallow copy
                int endIdx = Generator.GetInt32(startIdx, 5); // The end index of the section to make a shallow copy
                int count = endIdx - startIdx + 1;
                ImmutableTreeList<string> listResult = listObject.GetRange(startIdx, count);
                for (int i = 0; i < count; i++)
                {
                    Assert.Equal(strArray[i + startIdx], listResult[i]);
                }
            }

            [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
            public void PosTest3()
            {
                var myclass1 = new MyClass();
                var myclass2 = new MyClass();
                var myclass3 = new MyClass();
                var mc = new MyClass[3] { myclass1, myclass2, myclass3 };
                var listObject = ImmutableTreeList.Create(mc).ToBuilder();
                int startIdx = Generator.GetInt32(0, 2);      // The starting index of the section to make a shallow copy
                int endIdx = Generator.GetInt32(startIdx, 3); // The end index of the section to make a shallow copy
                int count = endIdx - startIdx + 1;
                ImmutableTreeList<MyClass> listResult = listObject.GetRange(startIdx, count);
                for (int i = 0; i < count; i++)
                {
                    Assert.Equal(mc[i + startIdx], listResult[i]);
                }
            }

            [Fact(DisplayName = "PosTest4: Copy no elements to the new list")]
            public void PosTest4()
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                ImmutableTreeList<int> listResult = listObject.GetRange(5, 0);
                Assert.NotNull(listResult);
                Assert.Empty(listResult);
            }

            [Fact(DisplayName = "NegTest1: The index is a negative number")]
            public void NegTest1()
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                Assert.Throws<ArgumentOutOfRangeException>(() => listObject.GetRange(-1, 4));
            }

            [Fact(DisplayName = "NegTest2: The count is a negative number")]
            public void NegTest2()
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                Assert.Throws<ArgumentOutOfRangeException>(() => listObject.GetRange(6, -4));
            }

            [Fact(DisplayName = "NegTest3: index and count do not denote a valid range of elements in the List")]
            public void NegTest3()
            {
                char[] iArray = { '#', ' ', '&', 'c', '1', '_', 'A' };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                Assert.Throws<ArgumentException>(() => listObject.GetRange(4, 4));
            }

            public class MyClass
            {
            }
        }
    }
}
