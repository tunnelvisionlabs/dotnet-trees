// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.Add(object)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListAdd
    {
        [Fact(DisplayName = "PosTest1: Calling Add method of IList,T is Value type.")]
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

            IEnumerator returnValue = myIList.GetEnumerator();
            int j = 0;
            for (IEnumerator itr = returnValue; itr.MoveNext();)
            {
                int current = (int)itr.Current;
                Assert.Equal(expectValue[j], current);

                j++;
            }
        }

        [Fact(DisplayName = "PosTest2: Calling Add method of IList,T is reference type.")]
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

            IEnumerator returnValue = myIList.GetEnumerator();
            int j = 0;
            for (IEnumerator itr = returnValue; itr.MoveNext();)
            {
                string current = (string)itr.Current;
                Assert.Equal(expectValue[j], current);

                j++;
            }
        }

        [Fact(DisplayName = "NegTest1: item is of a type that is not assignable to the IList.")]
        public void NegTest1()
        {
            TreeList<int> myList = new TreeList<int>();
            IList myIList = myList;

            // int type should be add. but add null ArgumentNullException should be caught.
            Assert.Throws<ArgumentNullException>(() => myIList.Add(null));
        }
    }
}
