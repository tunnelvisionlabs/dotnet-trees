// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.List
{
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.Reverse()"/>, derived from tests for
    /// <see cref="List{T}.Reverse()"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListReverse
    {
        [Fact(DisplayName = "PosTest1: The generic type is byte")]
        public void PosTest1()
        {
            byte[] byArray = new byte[1000];
            Generator.GetBytes(-55, byArray);
            TreeList<byte> listObject = new TreeList<byte>(byArray);
            byte[] expected = Reverse<byte>(byArray);
            listObject.Reverse();
            for (int i = 0; i < 1000; i++)
            {
                Assert.Equal(expected[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "dog", "apple", "joke", "banana", "chocolate", "dog", "food", "Microsoft" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            listObject.Reverse();
            string[] expected = Reverse<string>(strArray);
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
            listObject.Reverse();
            MyClass[] expected = new MyClass[4] { myclass4, myclass3, myclass2, myclass1 };
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(expected[i], listObject[i]);
            }
        }

        [Fact(DisplayName = "PosTest4: The list has no element")]
        public void PosTest4()
        {
            TreeList<int> listObject = new TreeList<int>();
            listObject.Reverse();
            Assert.Empty(listObject);
        }

        private T[] Reverse<T>(T[] arrayT)
        {
            T temp;
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
