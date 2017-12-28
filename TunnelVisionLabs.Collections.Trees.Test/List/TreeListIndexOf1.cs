// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.IndexOf(T)"/>, derived from tests for
    /// <see cref="List{T}.IndexOf(T)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIndexOf1
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = new int[1000];
            for (int i = 0; i < 1000; i++)
            {
                iArray[i] = i;
            }

            TreeList<int> listObject = new TreeList<int>(iArray);
            int ob = Generator.GetInt32(0, 1000);
            int result = listObject.IndexOf(ob);
            Assert.Equal(ob, result);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int result = listObject.IndexOf("dog");
            Assert.Equal(3, result);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            int result = listObject.IndexOf(myclass3);
            Assert.Equal(2, result);
        }

        [Fact(DisplayName = "PosTest4: There are many element in the list with the same value")]
        public void PosTest4()
        {
            string[] strArray = { "apple", "banana", "chocolate", "banana", "banana", "dog", "banana", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int result = listObject.IndexOf("banana");
            Assert.Equal(1, result);
        }

        [Fact(DisplayName = "PosTest5: Do not find the element")]
        public void PosTest5()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int result = listObject.IndexOf(-10000);
            Assert.Equal(-1, result);
        }

        public class MyClass
        {
        }
    }
}
