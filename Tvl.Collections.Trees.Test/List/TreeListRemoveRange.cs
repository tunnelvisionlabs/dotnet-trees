// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.RemoveRange(int, int)"/>, derived from tests for
    /// <see cref="List{T}.RemoveRange(int, int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListRemoveRange
    {
        [Fact(DisplayName = "PosTest1: Remove all the elements in the int type list")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.RemoveRange(0, 10);
            if (listObject.Count != 0)
            {
                userMessage = "The result is not the value as expected,count is: " + listObject.Count;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.RemoveRange(3, 3);
            string[] expected = { "dog", "apple", "joke", "food" };
            for (int i = 0; i < 4; i++)
            {
                if (listObject[i] != expected[i])
                {
                    userMessage = "The result is not the value as expected,result is: " + listObject[i];
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The count argument is zero")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.RemoveRange(1, 0);
            for (int i = 0; i < 3; i++)
            {
                if (listObject[i] != mc[i])
                {
                    userMessage = "The result is not the value as expected,result is: " + listObject[i];
                    retVal = false;
                }
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
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.RemoveRange(-1, 3);
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
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.RemoveRange(3, -2);
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
                string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                listObject.RemoveRange(3, 10);
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

        public class MyClass
        {
        }
    }
}
