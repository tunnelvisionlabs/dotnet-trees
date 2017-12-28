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
            int[] intArray = new int[5] { 1, 2, 3, 4, 5 };
            TreeList<int> listObject = new TreeList<int>(intArray);
            Assert.NotNull(listObject);
            Assert.Equal(5, listObject.Count);
        }

        [Fact(DisplayName = "PosTest2: The generic type is a reference type")]
        public void PosTest2()
        {
            string[] stringArray = { "Hello", "world", "thanks", "school" };
            TreeList<string> listObject = new TreeList<string>(stringArray);
            Assert.NotNull(listObject);
            Assert.Equal(4, listObject.Count);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            int length = Generator.GetByte(-55);
            MyClass[] myClass = new MyClass[length];
            TreeList<MyClass> listObject = new TreeList<MyClass>(myClass);
            Assert.NotNull(listObject);
            Assert.Equal(length, listObject.Count);
        }

        [Fact(DisplayName = "PosTest4: Using a list to construct another list")]
        public void PosTest4()
        {
            int[] iArray = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            TreeList<int> listObject1 = new TreeList<int>(iArray);
            TreeList<int> listObject2 = new TreeList<int>(listObject1);
            Assert.NotNull(listObject2);
            Assert.Equal(10, listObject2.Count);
        }

        [Fact(DisplayName = "NegTest1: The argument is a null reference")]
        public void NegTest1()
        {
            IEnumerable<char> i = null;
            Assert.Throws<ArgumentNullException>(() => new TreeList<char>(i));
        }

        public class MyClass
        {
        }
    }
}
