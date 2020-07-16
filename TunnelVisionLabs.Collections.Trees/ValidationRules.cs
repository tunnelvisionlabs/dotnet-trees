// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees
{
    using System;

    [Flags]
    internal enum ValidationRules
    {
        /// <summary>
        /// No special validation rules apply.
        /// </summary>
        None = 0,

        /// <summary>
        /// The tree is packed (i.e. there is empty space except at the end of each level).
        /// </summary>
        RequirePacked = 0b0001,
    }
}
