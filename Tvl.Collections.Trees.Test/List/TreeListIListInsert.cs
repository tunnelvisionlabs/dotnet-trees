// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.Insert(int, object)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListInsert
    {
        [Fact(DisplayName = "PosTest1: Calling Add method of IList,T is Value type.")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<int> myList = new TreeList<int>();
            int count = 10;
            int[] expectValue = new int[20];
            for (int z = 0; z < 20; z++)
            {
                expectValue[z] = 0;
            }

            IList myIList = myList;
            object element = null;
            for (int i = 1; i <= count; i++)
            {
                element = 0;
                myIList.Add(element);
            }

            for (int i = 1; i <= count; i++)
            {
                element = i * count;
                myIList.Insert(i - 1, element);
                expectValue[i - 1] = (int)element;
            }

            IEnumerator returnValue = myIList.GetEnumerator();
            int j = 0;
            for (IEnumerator itr = returnValue; itr.MoveNext();)
            {
                int current = (int)itr.Current;
                if (expectValue[j] != current)
                {
                    userMessage = " current value should be " + expectValue[j];
                    retVal = false;
                }

                j++;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: Calling Add method of IList,T is reference type.")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<string> myList = new TreeList<string>();
            int count = 10;
            string[] expectValue = new string[20];
            for (int z = 0; z < 20; z++)
            {
                expectValue[z] = string.Empty;
            }

            object element = null;
            IList myIList = myList;
            for (int i = 1; i <= count; i++)
            {
                element = string.Empty;
                myIList.Add(element);
            }

            for (int i = 1; i <= count; i++)
            {
                element = i.ToString();
                myIList.Insert(i - 1, element);
                expectValue[i - 1] = element.ToString();
            }

            IEnumerator returnValue = myIList.GetEnumerator();
            int j = 0;
            for (IEnumerator itr = returnValue; itr.MoveNext();)
            {
                string current = (string)itr.Current;
                if (expectValue[j] != current)
                {
                    userMessage = " current value should be " + expectValue[j];
                    retVal = false;
                }

                j++;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest1: item is of a type that is not assignable to the IList.")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TreeList<int> myList = new TreeList<int>();
                IList myIList = myList;

                // int type should be add. but add null ArgumentException should be caught.
                myIList.Add(0);
                myIList.Insert(0, null);
                userMessage = "ArgumentException should be caught.";
                retVal = false;
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest2: index is not a valid index in the IList.")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TreeList<int> myList = new TreeList<int>();
                IList myIList = myList;

                // int type should be add. but add null ArgumentException should be caught.
                myIList.Insert(int.MaxValue, 0);
                userMessage = "ArgumentOutOfRangeException should be caught.";
                retVal = false;
            }
            catch (ArgumentOutOfRangeException)
            {
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
