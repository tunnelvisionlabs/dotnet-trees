// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.InsertRange(int, IEnumerable{T})"/>, derived from tests for
    /// <see cref="List{T}.InsertRange(int, IEnumerable{T})"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListInsertRange
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = { 0, 1, 2, 3, 8, 9, 10, 11, 12, 13, 14 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] insert = { 4, 5, 6, 7 };
            listObject.InsertRange(4, insert);
            for (int i = 0; i < 15; i++)
            {
                Assert.Equal(i, listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: Insert the collection to the beginning of the list")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "dog", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] insert = { "Hello", "World" };
            listObject.InsertRange(0, insert);
            Assert.Equal(8, listObject.Count);
            Assert.Equal("Hello", listObject[0]);
            Assert.Equal("World", listObject[1]);
        }

        [Fact(DisplayName = "PosTest3: Insert custom class array to the end of the list")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass myclass4 = new MyClass();
            MyClass myclass5 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            MyClass[] insert = new MyClass[2] { myclass4, myclass5 };
            listObject.InsertRange(3, insert);
            for (int i = 0; i < 5; i++)
            {
                if (i < 3)
                {
                    Assert.Equal(mc[i], listObject[i]);
                }
                else
                {
                    Assert.Equal(insert[i - 3], listObject[i]);
                }
            }
        }

        [Fact(DisplayName = "PosTest4: The collection has null reference element")]
        public void PosTest4()
        {
            string[] strArray = { "apple", "dog", "banana", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] insert = new string[2] { null, null };
            int index = Generator.GetInt32(0, 4);
            listObject.InsertRange(index, insert);
            Assert.Equal(6, listObject.Count);
            Assert.Null(listObject[index]);
            Assert.Null(listObject[index + 1]);
        }

        [Fact(DisplayName = "NegTest1: The collection is a null reference")]
        public void NegTest1()
        {
            string[] strArray = { "apple", "dog", "banana", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] insert = null;
            int index = Generator.GetInt32(0, 4);
            Assert.Throws<ArgumentNullException>(() => listObject.InsertRange(index, insert));
        }

        [Fact(DisplayName = "NegTest2: The index is negative")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] insert = { -0, 90, 100 };
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.InsertRange(-1, insert));
        }

        [Fact(DisplayName = "NegTest3: The index is greater than the count of the list")]
        public void NegTest3()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] insert = { -0, 90, 100 };
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.InsertRange(11, insert));
        }

        public class MyClass
        {
        }
    }
}
