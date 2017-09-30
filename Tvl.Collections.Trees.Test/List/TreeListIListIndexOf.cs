// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.IndexOf(object)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListIndexOf
    {
        [Fact(DisplayName = "PosTest1: Calling IndexOf method of IList,T is Value type.")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
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

                for (int j = 0; j < myIList.Count; j++)
                {
                    int current = myIList.IndexOf(myIList[j]);
                    if (current == -1)
                    {
                        userMessage = "calling IndexOf method return an error number.";
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: Calling IndexOf method of IList,T is reference type.")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
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

                for (int j = 0; j < myIList.Count; j++)
                {
                    int current = myIList.IndexOf(myIList[j]);
                    if (current == -1)
                    {
                        userMessage = "calling IndexOf method return an error number.";
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: Calling IndexOf method of IList,T is reference type and the item is not exist in the List.")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
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

                for (int j = 0; j < myIList.Count; j++)
                {
                    int current = myIList.IndexOf(null);
                    if (current != -1)
                    {
                        userMessage = "calling IndexOf method return an error number.";
                        retVal = false;
                    }
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
