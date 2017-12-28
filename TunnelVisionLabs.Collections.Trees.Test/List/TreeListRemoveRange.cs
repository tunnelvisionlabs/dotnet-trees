// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.RemoveRange(int, int)"/>, derived from tests for
    /// <see cref="List{T}.RemoveRange(int, int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListRemoveRange
    {
        [Fact(DisplayName = "PosTest1: Remove all the elements in the int type list")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.RemoveRange(0, 10);
            Assert.Empty(listObject);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.RemoveRange(3, 3);
            string[] expected = { "dog", "apple", "joke", "food" };
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(expected[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest3: The count argument is zero")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.RemoveRange(1, 0);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(mc[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "NegTest1: The index is a negative number")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.RemoveRange(-1, 3));
        }

        [Fact(DisplayName = "NegTest2: The count is a negative number")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.RemoveRange(3, -2));
        }

        [Fact(DisplayName = "NegTest3: index and count do not denote a valid range of elements in the List")]
        public void NegTest3()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.Throws<ArgumentException>(() => listObject.RemoveRange(3, 10));
        }

        public class MyClass
        {
        }
    }
}
