// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Diagnostics;

    internal static class Generator
    {
        private static Random _rand = new Random();
        private static int? _seed = null;

        public static int? Seed
        {
            get
            {
                if (_seed.HasValue)
                {
                    return _seed.Value;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (!_seed.HasValue)
                {
                    _seed = value;
                    if (_seed.HasValue)
                    {
                        Debug.WriteLine("Seeding Random with: " + _seed.Value.ToString());
                        _rand = new Random(_seed.Value);
                    }
                }
                else
                {
                    Debug.WriteLine("Attempt to seed Random to " + value.ToString() + " rejected it was already seeded to: " + _seed.Value.ToString());
                }
            }
        }

        // returns a byte array of random data
        public static void GetBytes(int new_seed, byte[] buffer)
        {
            Seed = new_seed;
            GetBytes(buffer);
        }

        public static void GetBytes(byte[] buffer)
        {
            _rand.NextBytes(buffer);
            Debug.WriteLine("Random Byte[] produced: " + Convert.ToBase64String(buffer));
        }

        // returns a non-negative Int32 between 0 and Int32.MaxValue
        private static int GetInt32(int new_seed)
        {
            Seed = new_seed;
            return GetInt32();
        }

        private static int GetInt32()
        {
            int i = _rand.Next();
            Debug.WriteLine("Random Int32 produced: " + i.ToString());
            return i;
        }

        public static int GetInt32(int minValue, int maxValue)
        {
            if (minValue == maxValue)
            {
                return minValue;
            }

            if (minValue < maxValue)
            {
                return minValue + (GetInt32(-55) % (maxValue - minValue));
            }

            return minValue;
        }

        // returns a non-negative Byte between 0 and Byte.MaxValue
        public static byte GetByte(int new_seed)
        {
            Seed = new_seed;
            return GetByte();
        }

        public static byte GetByte()
        {
            byte i = Convert.ToByte(_rand.Next() % (1 + byte.MaxValue));
            Debug.WriteLine("Random Byte produced: " + i.ToString());
            return i;
        }
    }
}
