// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Add(T)"/>, derived from tests for
    /// <see cref="List{T}.Add(T)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListAdd
    {
        [Fact(DisplayName = "PosTest1: The item to be added is type of byte")]
        public bool PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            byte[] byteObject = new byte[1000];
            Generator.GetBytes(-55, byteObject);
            TreeList<byte> listObject = new TreeList<byte>();
            for (int i = 0; i < 1000; i++)
            {
                listObject.Add(byteObject[i]);
            }

            for (int i = 0; i < 1000; i++)
            {
                if (listObject[i] != byteObject[i])
                {
                    userMessage = "The result is not the value as expected,i is: " + i;
                    retVal = false;
                }
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest2: The item to be added is type of string")]
        public bool PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "Hello" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string str1 = "World";
            listObject.Add(str1);
            if (listObject.Count != 2)
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            if (listObject[1] != "World")
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest3: The item to be added is a custom type")]
        public bool PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myClass = new MyClass();
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.Add(myClass);
            if (listObject[0] != myClass)
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest4: Add null object to the list")]
        public bool PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<string> listObject = new TreeList<string>();
            listObject.Add(null);
            if (listObject[0] != null)
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            return retVal;
        }

        public class MyClass
        {
        }
    }
}
