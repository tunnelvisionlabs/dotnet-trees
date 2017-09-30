// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
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
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[10];
                listObject.CopyTo(result);
                for (int i = 0; i < 10; i++)
                {
                    if (listObject[i] != result[i])
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

        [Fact(DisplayName = "PosTest2: The list is type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "Tom", "Jack", "Mike" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                string[] result = new string[3];
                listObject.CopyTo(result);
                if ((result[0] != "Tom") || (result[1] != "Jack") || (result[2] != "Mike"))
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

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
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
                if ((mc[0] != myclass1) || (mc[1] != myclass2) || (mc[2] != myclass3))
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

        [Fact(DisplayName = "NegTest1: The array is a null reference")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.CopyTo(null);
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

        [Fact(DisplayName = "NegTest2: The number of elements in the source List is greater than the number of elements that the destination array can contain")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[1];
                listObject.CopyTo(result);
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
