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
            TreeList<int> listObject = new TreeList<int>();
            Assert.NotNull(listObject);
        }

        [Fact(DisplayName = "PosTest2: The generic type is a reference type")]
        public void PosTest2()
        {
            TreeList<string> listObject = new TreeList<string>();
            Assert.NotNull(listObject);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            Assert.NotNull(listObject);
        }

        public class MyClass
        {
        }
    }
}
