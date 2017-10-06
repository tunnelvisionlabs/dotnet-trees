// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.InsertRange(int, IEnumerable{T})"/>, derived from tests for
    /// <see cref="List{T}.InsertRange(int, IEnumerable{T})"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListInsertRange
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 0, 1, 2, 3, 8, 9, 10, 11, 12, 13, 14 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int[] insert = { 4, 5, 6, 7 };
            listObject.InsertRange(4, insert);
            for (int i = 0; i < 15; i++)
            {
                if (listObject[i] != i)
                {
                    userMessage = "The result is not the value as expected,listObject is: " + listObject[i];
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: Insert the collection to the beginning of the list")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "dog", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] insert = { "Hello", "World" };
            listObject.InsertRange(0, insert);
            if (listObject.Count != 8)
            {
                userMessage = "The result is not the value as expected,Count is: " + listObject.Count;
                retVal = false;
            }

            if ((listObject[0] != "Hello") || (listObject[1] != "World"))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: Insert custom class array to the end of the list")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass myclass4 = new MyClass();
            MyClass myclass5 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            MyClass[] insert = new MyClass[2] { myclass4, myclass5 };
            listObject.InsertRange(3, insert);
            for (int i = 0; i < 5; i++)
            {
                if (i < 3)
                {
                    if (listObject[i] != mc[i])
                    {
                        userMessage = "The result is not the value as expected,i is: " + i;
                        retVal = false;
                    }
                }
                else
                {
                    if (listObject[i] != insert[i - 3])
                    {
                        userMessage = "The result is not the value as expected,i is: " + i;
                        retVal = false;
                    }
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: The collection has null reference element")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "dog", "banana", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] insert = new string[2] { null, null };
            int index = GetInt32(0, 4);
            listObject.InsertRange(index, insert);
            if (listObject.Count != 6)
            {
                userMessage = "The result is not the value as expected,Count is: " + listObject.Count;
                retVal = false;
            }

            if ((listObject[index] != null) || (listObject[index + 1] != null))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest1: The collection is a null reference")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "apple", "dog", "banana", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                string[] insert = null;
                int index = GetInt32(0, 4);
                listObject.InsertRange(index, insert);
                userMessage = "The ArgumentNullException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentNullException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest2: The index is negative")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] insert = { -0, 90, 100 };
                listObject.InsertRange(-1, insert);
                userMessage = "The ArgumentOutOfRangeException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest3: The index is greater than the count of the list")]
        public void NegTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] insert = { -0, 90, 100 };
                listObject.InsertRange(11, insert);
                userMessage = "The ArgumentOutOfRangeException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentOutOfRangeException)
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

        public class MyClass
        {
        }
    }
}
