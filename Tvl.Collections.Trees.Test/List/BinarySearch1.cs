// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.BinarySearch(T)"/>, derived from tests for
    /// <see cref="List{T}.BinarySearch(T)"/> in dotnet/coreclr.
    /// </summary>
    public class BinarySearch1
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Sort();
            int i = GetInt32(0, 10);
            Assert.Equal(i, listObject.BinarySearch(i));
        }

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.Equal(-5, listObject.BinarySearch("egg"));
        }

        [Fact(DisplayName = "PosTest3: There are many elements with the same value")]
        public void PosTest3()
        {
            string[] strArray = { "key", "keys", "key", "key", "sky", "key" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.True(listObject.BinarySearch("key") >= 0);
        }

        [Fact(DisplayName = "PosTest4: The generic type is custom type")]
        public void PosTest4()
        {
            MyClass myclass1 = new MyClass(10);
            MyClass myclass2 = new MyClass(20);
            MyClass myclass3 = new MyClass(30);
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.Sort();
            Assert.Equal(1, listObject.BinarySearch(new MyClass(20)));
        }

        [Fact(DisplayName = "PosTest5: The item to be search is a null reference")]
        public void PosTest5()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.Equal(-1, listObject.BinarySearch(null));
        }

        [Fact(DisplayName = "NegTest1: IComparable generic interface was not implemented")]
        public void NegTest1()
        {
            TestClass[] tc = new TestClass[2] { new TestClass(), new TestClass() };
            TreeList<TestClass> listObject = new TreeList<TestClass>(tc);
            Assert.Throws<InvalidOperationException>(() => listObject.BinarySearch(new TestClass()));
        }

        private int GetInt32(int minValue, int maxValue)
        {
            if (minValue == maxValue)
            {
                return minValue;
            }

            if (minValue < maxValue)
            {
                return minValue + (Generator.GetInt32(-55) % (maxValue - minValue));
            }

            return minValue;
        }

        public class MyClass : IComparable
        {
            private int _value;

            public MyClass(int a)
            {
                _value = a;
            }

            public int CompareTo(object obj)
            {
                return _value.CompareTo(((MyClass)obj)._value);
            }
        }

        public class TestClass
        {
        }
    }
}
