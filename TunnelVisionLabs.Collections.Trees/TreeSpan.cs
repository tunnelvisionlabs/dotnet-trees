// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Diagnostics;

    internal struct TreeSpan : IEquatable<TreeSpan>
    {
        public static readonly TreeSpan Invalid = new TreeSpan(-1, 0);

        public TreeSpan(int start, int count)
        {
            Debug.Assert(count >= 0, $"Assertion failed: {nameof(count)} >= 0");

            Start = start;
            Count = count;
        }

        public int Start
        {
            get;
        }

        public int Count
        {
            get;
        }

        public int EndExclusive => Start + Count;

        public int EndInclusive => Start + Count - 1;

        public bool IsEmpty => Count == 0;

        public static bool operator ==(TreeSpan span1, TreeSpan span2) => span1.Equals(span2);

        public static bool operator !=(TreeSpan span1, TreeSpan span2) => !span1.Equals(span2);

        public static TreeSpan FromBounds(int start, int endExclusive)
        {
            Debug.Assert(endExclusive >= start, $"Assertion failed: {nameof(endExclusive)} >= {nameof(start)}");

            return new TreeSpan(start, endExclusive - start);
        }

        public static TreeSpan FromReverseSpan(int start, int count)
        {
            return new TreeSpan(start - count + 1, count);
        }

        public static TreeSpan Intersect(TreeSpan left, TreeSpan right)
        {
            int start = Math.Max(left.Start, right.Start);
            int endExclusive = Math.Min(left.EndExclusive, right.EndExclusive);
            if (endExclusive < start)
                return Invalid;

            return FromBounds(start, endExclusive);
        }

        public TreeSpan Offset(int distance) => new TreeSpan(Start + distance, Count);

        public bool IsSubspanOf(TreeSpan other) => Start >= other.Start && EndExclusive <= other.EndExclusive;

        public bool IsProperSubspanOf(TreeSpan other) => Count < other.Count && IsSubspanOf(other);

        public override bool Equals(object obj)
        {
            if (!(obj is TreeSpan other))
                return false;

            return Equals(other);
        }

        public override int GetHashCode() => (Start * -1369216789) + Count;

        public bool Equals(TreeSpan other)
        {
            return Start == other.Start
                && Count == other.Count;
        }

        public override string ToString() => $"[{Start}, {EndExclusive})";
    }
}
