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
        public void PosTest1()
        {
            byte[] byteObject = new byte[1000];
            Generator.GetBytes(-55, byteObject);
            TreeList<byte> listObject = new TreeList<byte>();
            for (int i = 0; i < 1000; i++)
            {
                listObject.Add(byteObject[i]);
            }

            for (int i = 0; i < 1000; i++)
            {
                Assert.Equal(byteObject[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: The item to be added is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "Hello" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string str1 = "World";
            listObject.Add(str1);
            Assert.Equal(2, listObject.Count);
            Assert.Equal("World", listObject[1]);
        }

        [Fact(DisplayName = "PosTest3: The item to be added is a custom type")]
        public void PosTest3()
        {
            MyClass myClass = new MyClass();
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.Add(myClass);
            Assert.Equal(myClass, listObject[0]);
        }

        [Fact(DisplayName = "PosTest4: Add null object to the list")]
        public void PosTest4()
        {
            TreeList<string> listObject = new TreeList<string>();
            listObject.Add(null);
            Assert.Null(listObject[0]);
        }

        public class MyClass
        {
        }
    }
}
