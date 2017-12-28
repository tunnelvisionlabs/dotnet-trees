// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;

    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This code should not be reachable.");
    }
}
