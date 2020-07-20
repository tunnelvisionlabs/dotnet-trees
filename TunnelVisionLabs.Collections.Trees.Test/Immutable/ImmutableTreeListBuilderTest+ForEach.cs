// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableTreeListBuilderTest
    {
        /// <summary>
        /// Tests for <see cref="ImmutableTreeList{T}.Builder.ForEach(Action{T})"/>.
        /// </summary>
        public class ForEach
        {
            [Fact(DisplayName = "PosTest1: The generic type is int")]
            public void PosTest1()
            {
                int[] iArray = { 1, 9, 3, 6, -1, 8, 7, 1, 2, 4 };
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                var myClass = new MyClass();
                var action = new Action<int>(myClass.SumCalc);
                listObject.ForEach(action);
                Assert.Equal(40, myClass.Sum);
            }

            [Fact(DisplayName = "PosTest2: The generic type is type of string")]
            public void PosTest2()
            {
                string[] strArray = { "Hello", "wor", "l", "d" };
                var listObject = ImmutableTreeList.Create(strArray).ToBuilder();
                var myClass = new MyClass();
                var action = new Action<string>(myClass.JoinStr);
                listObject.ForEach(action);
                Assert.Equal("Helloworld", myClass.Result);
            }

            [Fact(DisplayName = "PosTest3: The generic type is custom type")]
            public void PosTest3()
            {
                var myclass1 = new MyClass2('h');
                var myclass2 = new MyClass2('=');
                var myclass3 = new MyClass2('&');
                var mc = new MyClass2[3] { myclass1, myclass2, myclass3 };
                var listObject = ImmutableTreeList.Create(mc).ToBuilder();
                var myClass = new MyClass();
                var action = new Action<MyClass2>(myClass.DeleteValue);
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
                var listObject = ImmutableTreeList.Create(iArray).ToBuilder();
                Action<int>? action = null;
                Assert.Throws<ArgumentNullException>(() => listObject.ForEach(action!));
            }

            public class MyClass
            {
                public int Sum { get; set; } = 0;

                public string? Result
                {
                    get; set;
                }

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
}
