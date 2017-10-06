// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
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
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<int> myList = new TreeList<int>();
            bool expectValue = false;
            bool returnValue = ((ICollection<int>)myList).IsReadOnly;
            if (expectValue != returnValue)
            {
                userMessage = " Calling IsReadOnly property should return false";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: In the user define implementation of List, this property IsReadOnly may return true.")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyTestListICollection<int> myList = new MyTestListICollection<int>();
            bool expectValue = true;
            bool returnValue = ((ICollection<int>)myList).IsReadOnly;
            if (expectValue != returnValue)
            {
                userMessage = " Calling IsReadOnly property should return true";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        public class MyTestListICollection<T> : TreeList<T>, ICollection<T>
        {
            bool ICollection<T>.IsReadOnly => true;

            int ICollection<T>.Count => throw new NotImplementedException();

            void ICollection<T>.Add(T item) => throw new NotImplementedException();

            void ICollection<T>.Clear() => throw new NotImplementedException();

            bool ICollection<T>.Contains(T item) => throw new NotImplementedException();

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();

            bool ICollection<T>.Remove(T item) => throw new NotImplementedException();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw new NotImplementedException();

            IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        }
    }
}
