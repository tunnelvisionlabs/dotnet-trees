// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.LastIndexOf(T)"/>, derived from tests for
    /// <see cref="List{T}.LastIndexOf(T)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListLastIndexOf1
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
            int result = listObject.LastIndexOf(ob);
            Assert.Equal(ob, result);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "dog", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int result = listObject.LastIndexOf("dog");
            Assert.Equal(4, result);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[5] { myclass1, myclass2, myclass3, myclass3, myclass2 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            int result = listObject.LastIndexOf(myclass3);
            Assert.Equal(3, result);
        }

        [Fact(DisplayName = "PosTest4: There are many element in the list with the same value")]
        public void PosTest4()
        {
            string[] strArray = { "apple", "banana", "chocolate", "banana", "banana", "dog", "banana", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int result = listObject.LastIndexOf("banana");
            Assert.Equal(6, result);
        }

        [Fact(DisplayName = "PosTest5: Do not find the element")]
        public void PosTest5()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int result = listObject.LastIndexOf(-10000);
            Assert.Equal(-1, result);
        }

        [Fact(DisplayName = "PosTest6: The argument is a null reference")]
        public void PosTest6()
        {
            string[] strArray = { "apple", "banana", "chocolate" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int result = listObject.LastIndexOf(null);
            Assert.Equal(-1, result);
        }

        public class MyClass
        {
        }
    }
}
