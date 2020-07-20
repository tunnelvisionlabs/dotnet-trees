// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

// This file contains tests for Contains...
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Contains(T)"/>, derived from tests for
    /// <see cref="List{T}.Contains(T)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListContains
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int i = Generator.GetInt32(0, 10);
            Assert.True(listObject.Contains(i));
        }

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.True(listObject.Contains("dog"));
        }

        [Fact(DisplayName = "PosTest3: The generic type is custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            Assert.True(listObject.Contains(myclass1));
        }

        [Fact(DisplayName = "PosTest4: The list does not contain the element")]
        public void PosTest4()
        {
            char[] chArray = { '1', '9', '3', '6', '5', '8', '7', '2', '4' };
            TreeList<char> listObject = new TreeList<char>(chArray);
            Assert.False(listObject.Contains('t'));
        }

        [Fact(DisplayName = "PosTest5: The argument is a null reference")]
        public void PosTest5()
        {
            string?[] strArray = { "apple", "banana", "chocolate", null, "food" };
            TreeList<string?> listObject = new TreeList<string?>(strArray);
            Assert.True(listObject.Contains(null));
        }

        public class MyClass
        {
        }
    }
}
