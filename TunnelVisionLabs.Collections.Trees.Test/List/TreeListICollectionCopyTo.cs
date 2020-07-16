// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="ICollection.CopyTo(Array, int)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListICollectionCopyTo
    {
        [Fact(DisplayName = "PosTest1: The list is type of int and get a random index")]
        public void PosTest1()
        {
            int count = 10;
            int arraySum = 100;
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int position = Generator.GetInt32(0, arraySum - count);
            int[] result = new int[arraySum];
            ((ICollection)listObject).CopyTo(result, position);
            for (int i = 0; i < count; i++)
            {
                Assert.Equal(result[i + position], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: The list is type of string and copy the date to the array whose beginning index is zero")]
        public void PosTest2()
        {
            string[] strArray = { "Tom", "Jack", "Mike" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] result = new string[3];
            ((ICollection)listObject).CopyTo(result, 0);
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
            ((ICollection)listObject).CopyTo(mc, 0);
            Assert.Equal(myclass1, mc[0]);
            Assert.Equal(myclass2, mc[1]);
            Assert.Equal(myclass3, mc[2]);
        }

        [Fact(DisplayName = "PosTest4: Copy an empty list to the end of an array,the three int32 arguments are zero all")]
        public void PosTest4()
        {
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            MyClass[] mc = new MyClass[3];
            ((ICollection)listObject).CopyTo(mc, 0);
            for (int i = 0; i < 3; i++)
            {
                Assert.Null(mc[i]);
            }
        }

        [Fact(DisplayName = "NegTest1: The array is a null reference")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentNullException>(() => ((ICollection)listObject).CopyTo(null, 0));
        }

        [Fact(DisplayName = "NegTest2: The number of elements in the source List is greater than the number of elements that the destination array can contain")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[1];
            Assert.Throws<ArgumentException>(() => ((ICollection)listObject).CopyTo(result, 0));
        }

        [Fact(DisplayName = "NegTest3: arrayIndex is equal to the length of array")]
        public void NegTest3()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[20];
            Assert.Throws<ArgumentException>(() => ((ICollection)listObject).CopyTo(result, 20));
        }

        [Fact(DisplayName = "NegTest4: arrayIndex is greater than the length of array")]
        public void NegTest4()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[20];
            Assert.Throws<ArgumentException>(() => ((ICollection)listObject).CopyTo(result, 300));
        }

        [Fact(DisplayName = "NegTest5: arrayIndex is less than 0")]
        public void NegTest5()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] result = new int[20];
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection)listObject).CopyTo(result, -1));
        }

        public class MyClass
        {
        }
    }
}
