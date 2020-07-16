// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.CopyTo(T[])"/>, derived from tests for
    /// <see cref="List{T}.CopyTo(T[])"/> in dotnet/coreclr.
    /// </summary>
    public class CopyTo1
    {
        [Fact(DisplayName = "PosTest1: The list is type of int")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[10];
            listObject.CopyTo(result);
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(listObject[i], result[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: The list is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "Tom", "Jack", "Mike" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] result = new string[3];
            listObject.CopyTo(result);
            Assert.Equal("Tom", result[0]);
            Assert.Equal("Jack", result[1]);
            Assert.Equal("Mike", result[2]);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.Add(myclass1);
            listObject.Add(myclass2);
            listObject.Add(myclass3);
            MyClass[] mc = new MyClass[3];
            listObject.CopyTo(mc);
            Assert.Equal(myclass1, mc[0]);
            Assert.Equal(myclass2, mc[1]);
            Assert.Equal(myclass3, mc[2]);
        }

        [Fact(DisplayName = "NegTest1: The array is a null reference")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentNullException>(() => listObject.CopyTo(null));
        }

        [Fact(DisplayName = "NegTest2: The number of elements in the source List is greater than the number of elements that the destination array can contain")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[1];
            Assert.Throws<ArgumentException>(() => listObject.CopyTo(result));
        }

        public class MyClass
        {
        }
    }
}
