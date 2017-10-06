// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.GetRange(int, int)"/>, derived from tests for
    /// <see cref="List{T}.GetRange(int, int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListGetRange
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int startIdx = GetInt32(0, 9);       // The starting index of the section to make a shallow copy
            int endIdx = GetInt32(startIdx, 10); // The end index of the section to make a shallow copy
            int count = endIdx - startIdx + 1;
            TreeList<int> listResult = listObject.GetRange(startIdx, count);
            for (int i = 0; i < count; i++)
            {
                if (listResult[i] != iArray[i + startIdx])
                {
                    userMessage = "The result is not the value as expected,result is: " + listResult[i] + " expected value is: " + iArray[i + startIdx];
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            int startIdx = GetInt32(0, 4);      // The starting index of the section to make a shallow copy
            int endIdx = GetInt32(startIdx, 5); // The end index of the section to make a shallow copy
            int count = endIdx - startIdx + 1;
            TreeList<string> listResult = listObject.GetRange(startIdx, count);
            for (int i = 0; i < count; i++)
            {
                if (listResult[i] != strArray[i + startIdx])
                {
                    userMessage = "The result is not the value as expected,result is: " + listResult[i] + " expected value is: " + strArray[i + startIdx];
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            int startIdx = GetInt32(0, 2);      // The starting index of the section to make a shallow copy
            int endIdx = GetInt32(startIdx, 3); // The end index of the section to make a shallow copy
            int count = endIdx - startIdx + 1;
            TreeList<MyClass> listResult = listObject.GetRange(startIdx, count);
            for (int i = 0; i < count; i++)
            {
                if (listResult[i] != mc[i + startIdx])
                {
                    userMessage = "The result is not the value as expected,result is: " + listResult[i] + " expected value is: " + mc[i + startIdx];
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: Copy no elements to the new list")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            TreeList<int> listResult = listObject.GetRange(5, 0);
            if (listResult == null)
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            if (listResult.Count != 0)
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest1: The index is a negative number")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                TreeList<int> listResult = listObject.GetRange(-1, 4);
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

        [Fact(DisplayName = "NegTest2: The count is a negative number")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                TreeList<int> listResult = listObject.GetRange(6, -4);
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

        [Fact(DisplayName = "NegTest3: index and count do not denote a valid range of elements in the List")]
        public void NegTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                char[] iArray = { '#', ' ', '&', 'c', '1', '_', 'A' };
                TreeList<char> listObject = new TreeList<char>(iArray);
                TreeList<char> listResult = listObject.GetRange(4, 4);
                userMessage = "The ArgumentException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentException)
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
