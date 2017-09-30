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
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.Sort();
                int i = GetInt32(0, 10);
                int result = listObject.BinarySearch(i);
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

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                int result = listObject.BinarySearch("egg");
                if (result != -5)
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
                int result = listObject.BinarySearch("key");
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
                listObject.Sort();
                int result = listObject.BinarySearch(new MyClass(20));
                if (result != 1)
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
                int result = listObject.BinarySearch(null);
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

        [Fact(DisplayName = "NegTest1: IComparable generic interface was not implemented")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TestClass[] tc = new TestClass[2] { new TestClass(), new TestClass() };
                TreeList<TestClass> listObject = new TreeList<TestClass>(tc);
                int result = listObject.BinarySearch(new TestClass());
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
