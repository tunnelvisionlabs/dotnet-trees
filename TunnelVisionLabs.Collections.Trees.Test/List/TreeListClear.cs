// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Clear()"/>, derived from tests for
    /// <see cref="List{T}.Clear()"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListClear
    {
        [Fact(DisplayName = "PosTest1: Remove int elements from the list")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            listObject.Clear();
            Assert.Empty(listObject);
        }

        [Fact(DisplayName = "PosTest2: Remove string elements from the list")]
        public void PosTest2()
        {
            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.Clear();
            Assert.Empty(listObject);
        }

        [Fact(DisplayName = "PosTest3: Remove the elements from the list of custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.Clear();
            Assert.Empty(listObject);
        }

        [Fact(DisplayName = "PosTest4: Remove the elements from the empty list")]
        public void PosTest4()
        {
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.Clear();
            Assert.Empty(listObject);
        }

        public class MyClass
        {
        }
    }
}
