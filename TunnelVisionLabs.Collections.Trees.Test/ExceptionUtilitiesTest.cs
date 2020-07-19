// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using Xunit;

    public class ExceptionUtilitiesTest
    {
        [Fact]
        public void TestUnreachable()
        {
            // Make sure it has the correct type
            Assert.IsAssignableFrom<InvalidOperationException>(ExceptionUtilities.Unreachable);

            // Make sure a new instance is created each time it's accessed
            Assert.NotSame(ExceptionUtilities.Unreachable, ExceptionUtilities.Unreachable);
        }
    }
}
