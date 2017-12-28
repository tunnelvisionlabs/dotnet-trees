// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Reverse(int, int)"/>, derived from tests for
    /// <see cref="List{T}.Reverse(int, int)"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListReverse2
    {
        [Fact(DisplayName = "PosTest1: The generic type is byte")]
        public void PosTest1()
        {
            byte[] byArray = new byte[100];
            Generator.GetBytes(-55, byArray);
            TreeList<byte> listObject = new TreeList<byte>(byArray);
            byte[] expected = Reverse<byte>(byArray);
            listObject.Reverse(10, 80);
            for (int i = 0; i < 100; i++)
            {
                if ((i < 10) || (i > 89))
                {
                    Assert.Equal(byArray[i], listObject[i]);
                }
                else
                {
                    Assert.Equal(expected[i], listObject[i]);
                }
            }
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food", "Microsoft" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.Reverse(2, 5);
            string[] expected = { "dog", "apple", "food", "dog", "chocolate", "banana", "joke", "Microsoft" };
            for (int i = 0; i < 8; i++)
            {
                Assert.Equal(expected[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            MyClass myclass4 = new MyClass();
            MyClass[] mc = new MyClass[4] { myclass1, myclass2, myclass3, myclass4 };
            TreeList<MyClass> listObject = new TreeList<MyClass>(mc);
            listObject.Reverse(0, 2);
            MyClass[] expected = new MyClass[4] { myclass2, myclass1, myclass3, myclass4 };
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(expected[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest4: The list has no element")]
        public void PosTest4()
        {
            TreeList<int> listObject = new TreeList<int>();
            listObject.Reverse(0, 0);
            Assert.Empty(listObject);
        }

        [Fact(DisplayName = "NegTest1: The index is a negative number")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.Reverse(-1, 3));
        }

        [Fact(DisplayName = "NegTest2: The count is a negative number")]
        public void NegTest2()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 10, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Assert.Throws<ArgumentOutOfRangeException>(() => listObject.Reverse(3, -2));
        }

        [Fact(DisplayName = "NegTest3: index and count do not denote a valid range of elements in the List")]
        public void NegTest3()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            Assert.Throws<ArgumentException>(() => listObject.Reverse(3, 10));
        }

        private T[] Reverse<T>(T[] array)
        {
            T temp;
            T[] arrayT = new T[array.Length];
            array.CopyTo(arrayT, 0);
            int times = arrayT.Length / 2;
            for (int i = 0; i < times; i++)
            {
                temp = arrayT[i];
                arrayT[i] = arrayT[arrayT.Length - 1 - i];
                arrayT[arrayT.Length - 1 - i] = temp;
            }

            return arrayT;
        }

        public class MyClass
        {
        }
    }
}
