// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test.List
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="TreeList{T}.CopyTo(T[], int)"/>, derived from tests for
    /// <see cref="List{T}.CopyTo(T[], int)"/> in dotnet/coreclr.
    /// </summary>
    public class CopyTo2
    {
        [Fact(DisplayName = "PosTest1: The list is type of int and get a random index")]
        public bool PosTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[100];
                int t = GetInt32(0, 90);
                listObject.CopyTo(result, t);
                for (int i = 0; i < 10; i++)
                {
                    if (listObject[i] != result[i + t])
                    {
                        userMessage = "The result is not the value as expected,i is: " + i;
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest2: The list is type of string and copy the date to the array whose beginning index is zero")]
        public bool PosTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                string[] strArray = { "Tom", "Jack", "Mike" };
                TreeList<string> listObject = new TreeList<string>(strArray);
                string[] result = new string[3];
                listObject.CopyTo(result, 0);
                if ((result[0] != "Tom") || (result[1] != "Jack") || (result[2] != "Mike"))
                {
                    userMessage = "The result is not the value as expected";
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest3: The generic type is a custom type")]
        public bool PosTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                MyClass myclass1 = new MyClass();
                MyClass myclass2 = new MyClass();
                MyClass myclass3 = new MyClass();
                TreeList<MyClass> listObject = new TreeList<MyClass>();
                listObject.Add(myclass1);
                listObject.Add(myclass2);
                listObject.Add(myclass3);
                MyClass[] mc = new MyClass[3];
                listObject.CopyTo(mc, 0);
                if ((mc[0] != myclass1) || (mc[1] != myclass2) || (mc[2] != myclass3))
                {
                    userMessage = "The result is not the value as expected";
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "PosTest4: Copy an empty list to the end of an array")]
        public bool PosTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                TreeList<MyClass> listObject = new TreeList<MyClass>();
                MyClass[] mc = new MyClass[3];
                listObject.CopyTo(mc, 2);
                for (int i = 0; i < 3; i++)
                {
                    if (mc[i] != null)
                    {
                        userMessage = "The result is not the value as expected";
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                userMessage = "Unexpected exception: " + e;
                retVal = false;
            }

            return retVal;
        }

        [Fact(DisplayName = "NegTest1: The array is a null reference")]
        public bool NegTest1()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                listObject.CopyTo(null, 0);
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

            return retVal;
        }

        [Fact(DisplayName = "NegTest2: The number of elements in the source List is greater than the number of elements that the destination array can contain")]
        public bool NegTest2()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[1];
                listObject.CopyTo(result, 0);
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

            return retVal;
        }

        [Fact(DisplayName = "NegTest3: arrayIndex is equal to the length of array")]
        public bool NegTest3()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                listObject.CopyTo(result, 20);
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

            return retVal;
        }

        [Fact(DisplayName = "NegTest4: arrayIndex is greater than the length of array")]
        public bool NegTest4()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                listObject.CopyTo(result, 300);
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

            return retVal;
        }

        [Fact(DisplayName = "NegTest5: arrayIndex is less than 0")]
        public bool NegTest5()
        {
            bool retVal = true;
            string userMessage = string.Empty;

            try
            {
                int[] iArray = { 1, 9, 3, 6, 5, 8, 7, 2, 4, 0 };
                TreeList<int> listObject = new TreeList<int>(iArray);
                int[] result = new int[20];
                listObject.CopyTo(result, -1);
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

            return retVal;
        }

        private int GetInt32(int minValue, int maxValue)
        {
            try
            {
                if (minValue == maxValue)
                {
                    return minValue;
                }

                if (minValue < maxValue)
                {
                    return minValue + (Generator.GetInt32(-55) % (maxValue - minValue));
                }
            }
            catch
            {
                throw;
            }

            return minValue;
        }

        public class MyClass
        {
        }
    }
}
