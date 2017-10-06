// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}()"/>, derived from tests for
    /// <see cref="List{T}()"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListCtor1
    {
        [Fact(DisplayName = "PosTest1: The genaric type is a value type")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<int> listObject = new TreeList<int>();
            if (listObject == null)
            {
                userMessage = "The constructor does not work well";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The generic type is a reference type")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<string> listObject = new TreeList<string>();
            if (listObject == null)
            {
                userMessage = "The constructor does not work well";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<MyClass> listObject = new TreeList<MyClass>();
            if (listObject == null)
            {
                userMessage = "The constructor does not work well";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        public class MyClass
        {
        }
    }
}
