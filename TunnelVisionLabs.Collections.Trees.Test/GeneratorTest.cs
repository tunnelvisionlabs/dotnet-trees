// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System.Reflection;
    using Xunit;

    public class GeneratorTest
    {
        [Fact]
        public void TestSeed()
        {
            ResetSeed(null);
            Assert.Null(Generator.Seed);
            Generator.Seed = null;
            Assert.Null(Generator.Seed);

            Generator.Seed = 50;
            Assert.Equal(50, Generator.Seed);
            Generator.Seed = 64;
            Assert.Equal(50, Generator.Seed);

            void ResetSeed(int? seed)
            {
                FieldInfo seedField = typeof(Generator).GetField("_seed", BindingFlags.Static | BindingFlags.NonPublic);
                seedField.SetValue(null, seed);
            }
        }

        [Fact]
        public void TestGetInt32SingleElement()
        {
            Assert.Equal(13, Generator.GetInt32(13, 13));
            Assert.Equal(20, Generator.GetInt32(20, 20));
        }

        [Fact]
        public void TestGetInt32NegativeRange()
        {
            Assert.Equal(13, Generator.GetInt32(13, 10));
            Assert.Equal(-2, Generator.GetInt32(-2, -5));
        }
    }
}
