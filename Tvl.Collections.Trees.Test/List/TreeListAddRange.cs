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
            byte[] byteObject = new byte[1000];
            Generator.GetBytes(-55, byteObject);
            TreeList<byte> listObject = new TreeList<byte>();
            listObject.AddRange(byteObject);
            for (int i = 0; i < 1000; i++)
            {
                Assert.Equal(byteObject[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: The item to be added is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "Hello", "world", "Tom", "school" };
            TreeList<string> listObject = new TreeList<string>();
            listObject.AddRange(strArray);
            Assert.Equal(4, listObject.Count);

            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(strArray[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest3: The item to be added is a custom type")]
        public void PosTest3()
        {
            MyClass myClass1 = new MyClass();
            MyClass myClass2 = new MyClass();
            MyClass[] mc = { myClass1, myClass2 };
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.AddRange(mc);
            Assert.Equal(myClass1, listObject[0]);
            Assert.Equal(myClass2, listObject[1]);
        }

        [Fact(DisplayName = "NegTest1: The argument is a null reference")]
        public void NegTest1()
        {
            IEnumerable<string> i = null;
            TreeList<string> listObject = new TreeList<string>();
            Assert.Throws<ArgumentNullException>(() => listObject.AddRange(i));
        }

        public class MyClass
        {
        }
    }
}
