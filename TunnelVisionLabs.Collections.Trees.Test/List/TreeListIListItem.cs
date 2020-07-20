// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.this[int]"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListItem
    {
        [Fact(DisplayName = "PosTest1: Calling Add method of IList,T is Value type.")]
        public void PosTest1()
        {
            TreeList<int> myList = new TreeList<int>();
            int count = 10;
            int[] expectValue = new int[10];
            IList myIList = myList;
            object? element = null;
            for (int i = 1; i <= count; i++)
            {
                element = i * count;
                myIList.Add(element);
                expectValue[i - 1] = (int)element;
            }

            for (int j = 0; j < myIList.Count; j++)
            {
                int current = (int)myIList[j]!;
                Assert.Equal(expectValue[j], current);
            }
        }

        [Fact(DisplayName = "PosTest2: Calling Add method of IList,T is reference type.")]
        public void PosTest2()
        {
            TreeList<string> myList = new TreeList<string>();
            int count = 10;
            string?[] expectValue = new string?[10];
            object? element = null;
            IList myIList = myList;
            for (int i = 1; i <= count; i++)
            {
                element = i.ToString();
                myIList.Add(element);
                expectValue[i - 1] = element.ToString();
            }

            for (int j = 0; j < myIList.Count; j++)
            {
                string current = (string)myIList[j]!;
                Assert.Equal(expectValue[j], current);
            }
        }

        [Fact(DisplayName = "NegTest1: item is of a type that is not assignable to the IList.")]
        public void NegTest1()
        {
            TreeList<int> myList = new TreeList<int>();
            IList myIList = myList;

            // int type should be add. but add null ArgumentNullException should be caught.
            Assert.Throws<ArgumentNullException>(() => myIList[0] = null);
        }

        [Fact(DisplayName = "NegTest2: index is not a valid index in the IList.")]
        public void NegTest2()
        {
            TreeList<int> myList = new TreeList<int>();
            IList myIList = myList;

            // int type should be add. but add null ArgumentException should be caught.
            Assert.Throws<ArgumentOutOfRangeException>(() => myIList[int.MaxValue] = 1);
        }
    }
}
