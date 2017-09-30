// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.BinarySearch(T, IComparer{T})"/>, derived from tests for
    /// <see cref="List{T}.BinarySearch(T, IComparer{T})"/> in dotnet/coreclr.
    /// </summary>
    public class BinarySearch2
    {
        [Fact(DisplayName = "PosTest1: The generic type is int and using custom IComparer")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.Sort();
                IntClass intClass = new IntClass();
                int i = GetInt32(0, 10);
                int result = listObject.BinarySearch(i, intClass);
                if (result != i)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string and using the custom IComparer")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                StrClass strClass = new StrClass();
                listObject.Sort(strClass);
                int result = listObject.BinarySearch("egg", strClass);
                if (result != -2)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: There are many elements with the same value")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "key", "keys", "key", "key", "sky", "key" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                StrClass strClass = new StrClass();
                int result = listObject.BinarySearch("key", strClass);
                if (result < 0)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: The generic type is custom type")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                MyClass myclass1 = new MyClass(10);
                MyClass myclass2 = new MyClass(20);
                MyClass myclass3 = new MyClass(30);
                MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
                TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
                MyClassIC myclassIC = new MyClassIC();
                listObject.Sort(myclassIC);
                int result = listObject.BinarySearch(new MyClass(10), myclassIC);
                if (result != 2)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest5: The item to be search is a null reference")]
        public void PosTest5()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                listObject.Sort();
                StrClass strClass = new StrClass();
                int result = listObject.BinarySearch(null, strClass);
                if (result != -1)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest6: The IComparer is a null reference")]
        public void PosTest6()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                MyClass myclass1 = new MyClass(10);
                MyClass myclass2 = new MyClass(20);
                MyClass myclass3 = new MyClass(30);
                MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
                TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
                MyClassIC myclassIC = new MyClassIC();
                listObject.Sort();
                int result = listObject.BinarySearch(new MyClass(10), null);
                if (result != 0)
                {
                    userMessage = "The result is not the value as expected,The result is: " + result;
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest1: IComparable generic interface was not implemented")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TestClass[] tc = new TestClass[2] { new TestClass(), new TestClass() };
                TreeList<TestClass> listObject = new TreeList<TestClass>(tc);
                int result = listObject.BinarySearch(new TestClass(), null);
                userMessage = "The InvalidOperationException was not thrown as expected";
                retVal = false;
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        private int GetInt32(int minValue, int maxValue)
        {
            try
            {
                if (minValue == maxValue)
                {
                    return minValue;
                }

                if (minValue < maxValue)
                {
                    return minValue + (Generator.GetInt32(-55) % (maxValue - minValue));
                }
            }
            catch
            {
                throw;
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
