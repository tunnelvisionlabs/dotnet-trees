// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.AddRange(IEnumerable{T})"/>, derived from tests for
    /// <see cref="List{T}.AddRange(IEnumerable{T})"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListAddRange
    {
        [Fact(DisplayName = "PosTest1: The item to be added is type of byte")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                byte[] byteObject = new byte[1000];
                Generator.GetBytes(-55, byteObject);
                TreeList<byte> listObject = new TreeList<byte>();
                listObject.AddRange(byteObject);
                for (int i = 0; i < 1000; i++)
                {
                    if (listObject[i] != byteObject[i])
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

        [Fact(DisplayName = "PosTest2: The item to be added is type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "Hello", "world", "Tom", "school" };
                TreeList<string> listObject = new TreeList<string>(1);
                listObject.AddRange(strArray);
                if (listObject.Count != 4)
                {
                    userMessage = "The result is not the value as expected";
                    retVal = false;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (listObject[i] != strArray[i])
                    {
                        userMessage = "The result is not the value as expected";
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

        [Fact(DisplayName = "PosTest3: The item to be added is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                MyClass myClass1 = new MyClass();
                MyClass myClass2 = new MyClass();
                MyClass[] mc = { myClass1, myClass2 };
                TreeList<MyClass> listObject = new TreeList<MyClass>();
                listObject.AddRange(mc);
                if (listObject[0] != myClass1 || (listObject[1] != myClass2))
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
                IEnumerable<string> i = null;
                TreeList<string> listObject = new TreeList<string>(100);
                listObject.AddRange(i);
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
