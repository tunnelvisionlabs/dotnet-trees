// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Count"/>, derived from tests for
    /// <see cref="List{T}.Count"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListCount
    {
        [Fact(DisplayName = "PosTest1: Calling count property of List,T is Value type.")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<int> myList = new TreeList<int>();
            int count = 10;

            int element = 0;
            for (int i = 1; i <= count; i++)
            {
                element = i * count;
                myList.Add(element);
            }

            if (myList.Count != count)
            {
                userMessage = " calling count property should return " + count;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: Calling count property of List,T is reference type.")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<string> myList = new TreeList<string>();
            int count = 10;
            string element = string.Empty;
            for (int i = 1; i <= count; i++)
            {
                element = i.ToString();
                myList.Add(element);
            }

            if (myList.Count != count)
            {
                userMessage = " calling count property should return " + count;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }
    }
}
