// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
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
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int i = GetInt32(0, 10);
            if (!listObject.Contains(i))
            {
                userMessage = "The result is not the value as expected,The i is: " + i;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is a referece type of string")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            if (!listObject.Contains("dog"))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass[] mc = new MyClass[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            if (!listObject.Contains(myclass1))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: The list does not contain the element")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            char[] chArray = { '1', '9', '3', '6', '5', '8', '7', '2', '4' };
            TreeList<char> listObject = new TreeList<char>(chArray);
            if (listObject.Contains('t'))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest5: The argument is a null reference")]
        public void PosTest5()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "apple", "banana", "chocolate", null, "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            if (!listObject.Contains(null))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        private int GetInt32(int minValue, int maxValue)
        {
            if (minValue == maxValue)
            {
                return minValue;
            }

            if (minValue < maxValue)
            {
                return minValue + (Generator.GetInt32(-55) % (maxValue - minValue));
            }

            return minValue;
        }

        public class MyClass
        {
        }
    }
}
