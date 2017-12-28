// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.ForEach(Action{T})"/>, derived from tests for
    /// <see cref="List{T}.ForEach(Action{T})"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListForEach
    {
        [Fact(DisplayName = "PosTest1: The generic type is int")]
        public void PosTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            MyClass myClass = new MyClass();
            Action<int> action = new Action<int>(myClass.SumCalc);
            listObject.ForEach(action);
            Assert.Equal(40, myClass.Sum);
        }

        [Fact(DisplayName = "PosTest2: The generic type is type of string")]
        public void PosTest2()
        {
            string[] strArray = { "Hello", "wor", "l", "d" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            MyClass myClass = new MyClass();
            Action<string> action = new Action<string>(myClass.JoinStr);
            listObject.ForEach(action);
            Assert.Equal("Helloworld", myClass.Result);
        }

        [Fact(DisplayName = "PosTest3: The generic type is custom type")]
        public void PosTest3()
        {
            MyClass2 myclass1 = new MyClass2('h');
            MyClass2 myclass2 = new MyClass2('=');
            MyClass2 myclass3 = new MyClass2('&');
            MyClass2[] mc = new MyClass2[3] { myclass1, myclass2, myclass3 };
            TreeList<MyClass2> listObject = new TreeList<MyClass2>(mc);
            MyClass myClass = new MyClass();
            Action<MyClass2> action = new Action<MyClass2>(myClass.DeleteValue);
            listObject.ForEach(action);
            for (int i = 0; i < 3; i++)
            {
                Assert.Null(mc[i].Value);
            }
        }

        [Fact(DisplayName = "NegTest1: The action is a null reference")]
        public void NegTest1()
        {
            int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => listObject.ForEach(action));
        }

        public class MyClass
        {
            public int Sum { get; set; } = 0;

            public string Result { get; set; }

            public void SumCalc(int a)
            {
                Sum = Sum + a;
            }

            public void JoinStr(string a)
            {
                Result = Result + a;
            }

            public void DeleteValue(MyClass2 mc)
            {
                mc.Value = null;
            }
        }

        public class MyClass2
        {
            public MyClass2(char c)
            {
                Value = c;
            }

            public char? Value
            {
                get;
                set;
            }
        }
    }
}
