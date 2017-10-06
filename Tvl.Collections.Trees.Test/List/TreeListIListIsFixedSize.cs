// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="IList.IsFixedSize"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListIListIsFixedSize
    {
        [Fact(DisplayName = "PosTest1: this IsFixedSize property always returns false.")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            bool actualValue = ((IList)listObject).IsFixedSize;
            if (actualValue)
            {
                userMessage = "calling IsFixedSize property should return false.";
                retVal = false;
            }

            string[] sArray = { "1", "9", "3", "6", "5", "8", "7", "2", "4", "0" };
            TreeList<string> listObject1 = new TreeList<string>(sArray);
            actualValue = ((IList)listObject).IsFixedSize;
            if (actualValue)
            {
                userMessage = "calling IsFixedSize property should return false.";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }
    }
}
