// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.BinarySearch(int, int, T, IComparer{T})"/>, derived from tests for
    /// <see cref="List{T}.BinarySearch(int, int, T, IComparer{T})"/> in dotnet/coreclr.
    /// </summary>
    public class BinarySearch3
    {
        [Fact(DisplayName = "PosTest1: The generic type is int and using custom IComparer")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Sort();
            IntClass intClass = new IntClass();
            int i = 7;
            Assert.Equal(i, listObject.BinarySearch(5, 4, i, intClass));
        }

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string and using the custom IComparer")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            StrClass strClass = new StrClass();
            listObject.Sort(strClass);
            Assert.Equal(-3, listObject.BinarySearch(2, 3, "egg", strClass));
        }

        [Fact(DisplayName = "PosTest3: There are many elements with the same value")]
        public void PosTest3()
        {
            string[] strArray = { "key", "keys", "key", "key", "sky", "key" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            StrClass strClass = new StrClass();
            Assert.Equal(0, listObject.BinarySearch(0, 1, "key", strClass));
        }

        [Fact(DisplayName = "PosTest4: The generic type is custom type")]
        public void PosTest4()
        {
            MyClass myclass1 = new MyClass(10);
            MyClass myclass2 = new MyClass(20);
            MyClass myclass3 = new MyClass(30);
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            MyClassIC myclassIC = new MyClassIC();
            listObject.Sort(myclassIC);
            Assert.Equal(2, listObject.BinarySearch(0, 3, new MyClass(10), myclassIC));
        }

        [Fact(DisplayName = "PosTest5: The item to be search is a null reference")]
        public void PosTest5()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.Sort();
            StrClass strClass = new StrClass();
            Assert.Equal(-1, listObject.BinarySearch(0, 3, null, strClass));
        }

        // Additional tests to cover code in StrClass not covered by previous tests
        [Fact]
        public void PosTest5Ext()
        {
            string[] strArray = { null, "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.Sort();
            StrClass strClass = new StrClass();
            Assert.Equal(~1, listObject.BinarySearch(0, 3, string.Empty, strClass));
            Assert.Equal(0, listObject.BinarySearch(0, 3, null, strClass));
        }

        [Fact(DisplayName = "PosTest6: The IComparer is a null reference")]
        public void PosTest6()
        {
            MyClass myclass1 = new MyClass(10);
            MyClass myclass2 = new MyClass(20);
            MyClass myclass3 = new MyClass(30);
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            MyClassIC myclassIC = new MyClassIC();
            listObject.Sort();
            Assert.Equal(0, listObject.BinarySearch(0, 3, new MyClass(10), null));
        }

        [Fact(DisplayName = "NegTest1: IComparable generic interface was not implemented")]
        public void NegTest1()
        {
            TestClass[] tc = new TestClass[2] { new TestClass(), new TestClass() };
            TreeList<TestClass> listObject = new TreeList<TestClass>(tc);
            Assert.Throws<InvalidOperationException>(() => listObject.BinarySearch(0, 2, new TestClass(), null));
        }

        [Fact(DisplayName = "NegTest2: Index is less than zero")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Sort();
            IntClass intClass = new IntClass();
            int i = 7;
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.BinarySearch(-1, 4, i, intClass));
        }

        [Fact(DisplayName = "NegTest3: Count is less than zero")]
        public void NegTest3()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Sort();
            IntClass intClass = new IntClass();
            int i = 7;
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.BinarySearch(1, -4, i, intClass));
        }

        [Fact(DisplayName = "NegTest4: index and count do not denote a valid range in the List")]
        public void NegTest4()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Sort();
            IntClass intClass = new IntClass();
            int i = 7;
            Assert.Throws<ArgumentException>(() => listObject.BinarySearch(6, 5, i, intClass));
        }

        public class MyClass : IComparable
        {
            private int _value;

            public MyClass(int a)
            {
                _value = a;
            }

            public int Value => _value;

            public int CompareTo(object obj)
            {
                return _value.CompareTo(((MyClass)obj)._value);
            }
        }

        public class TestClass
        {
        }

        public class IntClass : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }

        public class StrClass : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                {
                    if (x == null)
                    {
                        if (y == null)
                        {
                            // If x is null and y is null, they're
                            // equal.
                            return 0;
                        }
                        else
                        {
                            // If x is null and y is not null, y
                            // is greater.
                            return -1;
                        }
                    }
                    else
                    {
                        // If x is not null...
                        if (y == null)
                        {
                            // ...and y is null, x is greater.
                            return 1;
                        }
                        else
                        {
                            // ...and y is not null, compare the
                            // lengths of the two strings.
                            int retval = x.Length.CompareTo(y.Length);

                            if (retval != 0)
                            {
                                // If the strings are not of equal length,
                                // the longer string is greater.
                                return retval;
                            }
                            else
                            {
                                // If the strings are of equal length,
                                // sort them with ordinary string comparison.
                                return x.CompareTo(y);
                            }
                        }
                    }
                }
            }
        }

        public class MyClassIC : IComparer<MyClass>
        {
            public int Compare(MyClass x, MyClass y)
            {
                return (-1) * x.Value.CompareTo(y.Value);
            }
        }
    }
}
