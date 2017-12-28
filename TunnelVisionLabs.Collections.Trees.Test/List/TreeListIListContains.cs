// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.Contains(object)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListContains
    {
        [Fact(DisplayName = "PosTest1: Calling Contains method of IList,T is Value type.")]
        public void PosTest1()
        {
            TreeList<int> myList = new TreeList<int>();
            int count = 10;
            int[] expectValue = new int[10];
            IList myIList = myList;
            object element = null;
            for (int i = 1; i <= count; i++)
            {
                element = i * count;
                myIList.Add(element);
                expectValue[i - 1] = (int)element;
            }

            Assert.True(myIList.Contains(element));
        }

        [Fact(DisplayName = "PosTest2: Calling Contains method of IList,T is reference type.")]
        public void PosTest2()
        {
            TreeList<string> myList = new TreeList<string>();
            int count = 10;
            string[] expectValue = new string[10];
            object element = null;
            IList myIList = myList;
            for (int i = 1; i <= count; i++)
            {
                element = i.ToString();
                myIList.Add(element);
                expectValue[i - 1] = element.ToString();
            }

            Assert.True(myIList.Contains(element));
        }
    }
}
