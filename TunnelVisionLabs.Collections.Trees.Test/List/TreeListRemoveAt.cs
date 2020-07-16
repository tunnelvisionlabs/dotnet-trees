// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.RemoveAt(int)"/>, derived from tests for
    /// <see cref="List{T}.RemoveAt(int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListRemoveAt
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int index = Generator.GetInt32(0, 10);
            listObject.RemoveAt(index);
            Assert.DoesNotContain(iArray[index], listObject);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string and the element at the beginning would be removed")]
        public void PosTest2()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.RemoveAt(0);
            Assert.Equal(6, listObject.Count);

            for (int i = 0; i < 6; i++)
            {
                Assert.Equal(strArray[i + 1], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type and the element to be removed is at the end of the list")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.RemoveAt(2);
            Assert.Equal(2, listObject.Count);
            Assert.DoesNotContain(myclass3, listObject);
        }

        [Fact(DisplayName = "NegTest1: The index is negative")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.RemoveAt(-1));
        }

        [Fact(DisplayName = "NegTest2: The index is greater than the range of the list")]
        public void NegTest2()
        {
            char?[] chArray = { 'a', 'b', ' ', 'c', null };
            TreeList<char?> listObject = new TreeList<char?>(chArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.RemoveAt(10));
        }

        public class MyClass
        {
        }
    }
}
