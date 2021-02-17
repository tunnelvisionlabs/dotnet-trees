// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }
    }
}
