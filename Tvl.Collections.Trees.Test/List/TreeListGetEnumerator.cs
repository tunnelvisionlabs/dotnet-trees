// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.GetEnumerator()"/>, derived from tests for
    /// <see cref="List{T}.GetEnumerator()"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListGetEnumerator
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            TreeList<int>.Enumerator enumerator = listObject.GetEnumerator();
            for (int i = 0; i < 10; i++)
            {
                enumerator.MoveNext();
                if (enumerator.Current != iArray[i])
                {
                    userMessage = "The result is not the value as expected,i is: " + i;
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            TreeList<string>.Enumerator enumerator = listObject.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != strArray[i])
                {
                    userMessage = "The result is not the value as expected,The i is: " + i;
                    retVal = false;
                }

                i++;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            TreeList<MyClass>.Enumerator enumerator = listObject.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != mc[i])
                {
                    userMessage = "The result is not the value as expected,The i is: " + i;
                    retVal = false;
                }

                i++;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: The List is empty")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<string> listObject = new TreeList<string>();
            TreeList<string>.Enumerator enumerator = listObject.GetEnumerator();
            if (enumerator.MoveNext())
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        public class MyClass
        {
        }
    }
}
