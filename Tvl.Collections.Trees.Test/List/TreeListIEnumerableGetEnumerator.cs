﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IEnumerable{T}.GetEnumerator()"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIEnumerableGetEnumerator
    {
        [Fact(DisplayName = "PosTest1: Calling GetEnumerator method of IEnumerable,T is Value type.")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TreeList<int> myList = new TreeList<int>();
                int count = 10;
                int[] expectValue = new int[10];

                for (int i = 1; i <= count; i++)
                {
                    myList.Add(i * count);
                    expectValue[i - 1] = i * count;
                }

                IEnumerator<int> returnValue = ((IEnumerable<int>)myList).GetEnumerator();
                int j = 0;
                for (IEnumerator<int> itr = returnValue; itr.MoveNext();)
                {
                    int current = itr.Current;
                    if (expectValue[j] != current)
                    {
                        userMessage = " current value should be " + expectValue[j];
                        retVal = false;
                    }

                    j++;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: Calling GetEnumerator method of IEnumerable,T is reference type.")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TreeList<string> myList = new TreeList<string>();
                int count = 10;
                string[] expectValue = new string[10];
                string element = string.Empty;
                for (int i = 1; i <= count; i++)
                {
                    element = i.ToString();
                    myList.Add(element);
                    expectValue[i - 1] = element;
                }

                IEnumerator<string> returnValue = ((IEnumerable<string>)myList).GetEnumerator();
                int j = 0;
                for (IEnumerator<string> itr = returnValue; itr.MoveNext();)
                {
                    string current = itr.Current;
                    if (expectValue[j] != current)
                    {
                        userMessage = " current value should be " + expectValue[j];
                        retVal = false;
                    }

                    j++;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }
    }
}