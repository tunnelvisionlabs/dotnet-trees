// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}(IEnumerable{T})"/>, derived from tests for
    /// <see cref="List{T}(IEnumerable{T})"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListCtor2
    {
        [Fact(DisplayName = "PosTest1: The genaric type is a value type")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] intArray = new int[5] { 1, 2, 3, 4, 5 };
                TreeList<int> listObject = new TreeList<int>(intArray);
                if (listObject == null)
                {
                    userMessage = "The constructor does not work well";
                    retVal = false;
                }

                if (listObject.Count != 5)
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

        [Fact(DisplayName = "PosTest2: The generic type is a reference type")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] stringArray = { "Hello", "world", "thanks", "school" };
                TreeList<string> listObject = new TreeList<string>(stringArray);
                if (listObject == null)
                {
                    userMessage = "The constructor does not work well";
                    retVal = false;
                }

                if (listObject.Count != 4)
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
                int length = Generator.GetByte(-55);
                MyClass[] myClass = new MyClass[length];
                TreeList<MyClass> listObject = new TreeList<MyClass>(myClass);
                if (listObject == null)
                {
                    userMessage = "The constructor does not work well";
                    retVal = false;
                }

                if (listObject.Count != length)
                {
                    userMessage = "The result is not the value as expected,the count is: " + listObject.Count + ",The length is: " + length;
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

        [Fact(DisplayName = "PosTest4: Using a list to construct another list")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                TreeList<int> listObject1 = new TreeList<int>(iArray);
                TreeList<int> listObject2 = new TreeList<int>(listObject1);
                if (listObject2 == null)
                {
                    userMessage = "The constructor does not work well";
                    retVal = false;
                }

                if (listObject2.Count != 10)
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

        [Fact(DisplayName = "NegTest1: The argument is a null reference")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                IEnumerable<char> i = null;
                TreeList<char> listObject = new TreeList<char>(i);
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

        public class MyClass
        {
        }
    }
}
