// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="ICollection{T}.IsReadOnly"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListICollectionIsReadOnly
    {
        [Fact(DisplayName = "PosTest1: In the default implementation of List, this property IsReadOnly always returns false.")]
        public void PosTest1()
        {
            TreeList<int> myList = new TreeList<int>();
            bool expectValue = false;
            bool returnValue = ((ICollection<int>)myList).IsReadOnly;
            Assert.Equal(expectValue, returnValue);
        }

        [Fact(DisplayName = "PosTest2: In the user define implementation of List, this property IsReadOnly may return true.")]
        public void PosTest2()
        {
            MyTestListICollection<int> myList = new MyTestListICollection<int>();
            bool expectValue = true;
            bool returnValue = ((ICollection<int>)myList).IsReadOnly;
            Assert.Equal(expectValue, returnValue);
        }

        public class MyTestListICollection<T> : TreeList<T>, ICollection<T>
        {
            bool ICollection<T>.IsReadOnly => true;
        }
    }
}
