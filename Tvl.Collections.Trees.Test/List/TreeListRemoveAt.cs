// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.RemoveAt(int)"/>, derived from tests for
    /// <see cref="List{T}.RemoveAt(int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListRemoveAt
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int index = GetInt32(0, 10);
                listObject.RemoveAt(index);
                if (listObject.Contains(iArray[index]))
                {
                    userMessage = "The result is not the value as expected";
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

        [Fact(DisplayName = "PosTest2: The generic type is type of string and the element at the beginning would be removed")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                listObject.RemoveAt(0);
                if (listObject.Count != 6)
                {
                    userMessage = "The result is not the value as expected,count is: " + listObject.Count;
                    retVal = false;
                }

                for (int i = 0; i < 6; i++)
                {
                    if (listObject[i] != strArray[i + 1])
                    {
                        userMessage = "The result is not the value as expected,i is: " + i;
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type and the element to be removed is at the end of the list")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                MyClass myclass1 = new MyClass();
                MyClass myclass2 = new MyClass();
                MyClass myclass3 = new MyClass();
                MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
                TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
                listObject.RemoveAt(2);
                if (listObject.Count != 2)
                {
                    userMessage = "The result is not the value as expected,count is: " + listObject.Count;
                    retVal = false;
                }

                if (listObject.Contains(myclass3))
                {
                    userMessage = "The result is not the value as expected";
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

        [Fact(DisplayName = "NegTest1: The index is negative")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.RemoveAt(-1);
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

        [Fact(DisplayName = "NegTest2: The index is greater than the range of the list")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                char?[] chArray = { 'a', 'b', ' ', 'c', null };
                TreeList<char?> listObject = new TreeList<char?>(chArray);
                listObject.RemoveAt(10);
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

        public class MyClass
        {
        }
    }
}
