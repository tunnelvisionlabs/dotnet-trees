// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Xunit;

    public abstract class AbstractImmutableDictionaryTest
    {
        [Fact]
        public void TestClear()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            Assert.Empty(dictionary);
            Assert.Same(dictionary, dictionary.Clear());

            dictionary = dictionary.Add(1, 1);
            Assert.Empty(dictionary.Clear());
        }

        [Fact]
        public void TestAdd()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            Assert.Empty(dictionary);

            dictionary = dictionary.Add(1, 1);
            Assert.Same(dictionary, dictionary.Add(1, 1));
            Assert.Throws<ArgumentException>(() => dictionary.Add(1, 2));
        }

        [Fact]
        public void TestAddRange()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            Assert.Empty(dictionary);
            Assert.Same(dictionary, dictionary.AddRange(Enumerable.Empty<KeyValuePair<int, int>>()));
            Assert.Throws<ArgumentNullException>("items", () => dictionary.AddRange(null));

            dictionary = dictionary.AddRange(Enumerable.Range(0, 10).Select(x => new KeyValuePair<int, int>(x, x + 1)));
            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(0, 1),
                    new KeyValuePair<int, int>(1, 2),
                    new KeyValuePair<int, int>(2, 3),
                    new KeyValuePair<int, int>(3, 4),
                    new KeyValuePair<int, int>(4, 5),
                    new KeyValuePair<int, int>(5, 6),
                    new KeyValuePair<int, int>(6, 7),
                    new KeyValuePair<int, int>(7, 8),
                    new KeyValuePair<int, int>(8, 9),
                    new KeyValuePair<int, int>(9, 10),
                },
                dictionary);
        }

        [Fact]
        public void TestSetItem()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            Assert.Empty(dictionary);

            dictionary = dictionary.SetItem(1, 1);
            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(1, 1),
                },
                dictionary);

            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(1, 2),
                },
                dictionary.SetItem(1, 2));

            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(1, 1),
                    new KeyValuePair<int, int>(2, 5),
                },
                dictionary.SetItem(2, 5));
        }

        [Fact]
        public void TestSetItems()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            dictionary = dictionary.SetItems(Enumerable.Range(0, 4).Select(x => new KeyValuePair<int, int>(x, x)));
            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(0, 0),
                    new KeyValuePair<int, int>(1, 1),
                    new KeyValuePair<int, int>(2, 2),
                    new KeyValuePair<int, int>(3, 3),
                },
                dictionary);

            IEnumerable<int> itemsToChange = dictionary.Keys.Where(i => (i & 1) == 0);
            dictionary = dictionary.SetItems(itemsToChange.Select(x => new KeyValuePair<int, int>(x, x + 1)));
            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(0, 1),
                    new KeyValuePair<int, int>(1, 1),
                    new KeyValuePair<int, int>(2, 3),
                    new KeyValuePair<int, int>(3, 3),
                },
                dictionary);

            Assert.Throws<ArgumentNullException>("items", () => dictionary.SetItems(null));
        }

        [Fact]
        public void TestRemoveRange()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            for (int i = 0; i < 10; i++)
                dictionary = dictionary.Add(i, i);

            IEnumerable<int> itemsToRemove = dictionary.Keys.Where(i => (i & 1) == 0);
            dictionary = dictionary.RemoveRange(itemsToRemove);
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }.Select(x => new KeyValuePair<int, int>(x, x)), dictionary);

            Assert.Same(dictionary, dictionary.RemoveRange(Enumerable.Empty<int>()));
            Assert.Throws<ArgumentNullException>("keys", () => dictionary.RemoveRange(null));
        }

        [Fact]
        public void TestRemove()
        {
            IImmutableDictionary<int, int> dictionary = CreateDictionary<int, int>();
            for (int i = 0; i < 4; i++)
                dictionary = dictionary.Add(i, i);

            Assert.Same(dictionary, dictionary.Remove(4));

            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(0, 0),
                    new KeyValuePair<int, int>(1, 1),
                    new KeyValuePair<int, int>(2, 2),
                },
                dictionary.Remove(3));

            Assert.Equal(
                new[]
                {
                    new KeyValuePair<int, int>(0, 0),
                    new KeyValuePair<int, int>(2, 2),
                    new KeyValuePair<int, int>(3, 3),
                },
                dictionary.Remove(1));
        }

        protected abstract IImmutableDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
            where TKey : notnull;
    }
}
