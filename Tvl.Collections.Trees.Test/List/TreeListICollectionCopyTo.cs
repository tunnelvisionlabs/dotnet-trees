// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for the <see cref="TreeList{T}"/> implementation of <see cref="ICollection.CopyTo(Array, int)"/>, derived from tests for
    /// <see cref="List{T}"/> in dotnet/coreclr.
    /// </summary>
    public class TreeListICollectionCopyTo
    {
        [Fact(DisplayName = "PosTest1: The list is type of int and get a random index")]
        public void PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            int count = 10;
            int arraySum = 100;
            int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
            TreeList<int> listObject = new TreeList<int>(iArray);
            int position = GetInt32(0, arraySum - count);
            int[] result = new int[arraySum];
            ((ICollection)listObject).CopyTo(result, position);
            for (int i = 0; i < count; i++)
            {
                if (listObject[i] != result[i + position])
                {
                    userMessage = "The result is not the value as expected,i is: " + i;
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest2: The list is type of string and copy the date to the array whose beginning index is zero")]
        public void PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            string[] strArray = { "Tom", "Jack", "Mike" };
            TreeList<string> listObject = new TreeList<string>(strArray);
            string[] result = new string[3];
            ((ICollection)listObject).CopyTo(result, 0);
            if (result[0] != "Tom" || result[1] != "Jack" || result[2] != "Mike")
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public void PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            MyClass myclass1 = new MyClass();
            MyClass myclass2 = new MyClass();
            MyClass myclass3 = new MyClass();
            TreeList<MyClass> listObject = new TreeList<MyClass>();
            listObject.Add(myclass1);
            listObject.Add(myclass2);
            listObject.Add(myclass3);
            MyClass[] mc = new MyClass[3];
            ((ICollection)listObject).CopyTo(mc, 0);
            if ((mc[0] != myclass1) || (mc[1] != myclass2) || (mc[2] != myclass3))
            {
                userMessage = "The result is not the value as expected";
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "PosTest4: Copy an empty list to the end of an array,the three int32 arguments are zero all")]
        public void PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            TreeList<MyClass> listObject = new TreeList<MyClass>();
            MyClass[] mc = new MyClass[3];
            ((ICollection)listObject).CopyTo(mc, 0);
            for (int i = 0; i < 3; i++)
            {
                if (mc[i] != null)
                {
                    userMessage = "The result is not the value as expected";
                    retVal = false;
                }
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest1: The array is a null reference")]
        public void NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                ((ICollection)listObject).CopyTo(null, 0);
                userMessage = "The ArgumentNullException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentNullException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest2: The number of elements in the source List is greater than the number of elements that the destination array can contain")]
        public void NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[1];
                ((ICollection)listObject).CopyTo(result, 0);
                userMessage = "The ArgumentException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest3: arrayIndex is equal to the length of array")]
        public void NegTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                ((ICollection)listObject).CopyTo(result, 20);
                userMessage = "The ArgumentException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest4: arrayIndex is greater than the length of array")]
        public void NegTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                ((ICollection)listObject).CopyTo(result, 300);
                userMessage = "The ArgumentException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        [Fact(DisplayName = "NegTest5: arrayIndex is less than 0")]
        public void NegTest5()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                ((ICollection)listObject).CopyTo(result, -1);
                userMessage = "The ArgumentOutOfRangeException was not thrown as expected";
                retVal = false;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            Assert.True(retVal, userMessage);
        }

        private int GetInt32(int minValue, int maxValue)
        {
            if (minValue == maxValue)
            {
                return minValue;
            }

            if (minValue < maxValue)
            {
                return minValue + (Generator.GetInt32(-55) % (maxValue - minValue));
            }

            return minValue;
        }

        public class MyClass
        {
        }
    }
}
