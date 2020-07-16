// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;
    using DictionaryEntry = System.Collections.DictionaryEntry;
    using ICollection = System.Collections.ICollection;
    using IDictionary = System.Collections.IDictionary;
    using IDictionaryEnumerator = System.Collections.IDictionaryEnumerator;
    using IEnumerator = System.Collections.IEnumerator;

    public class ImmutableSortedTreeDictionaryTest : AbstractImmutableDictionaryTest
    {
        [Fact]
        public void TestEmptyDictionary()
        {
            var dictionary = ImmutableSortedTreeDictionary.Create<int, int>();
            Assert.Same(ImmutableSortedTreeDictionary<int, int>.Empty, dictionary);

            ImmutableSortedTreeDictionary<int, int>.KeyCollection keys = dictionary.Keys;
            ImmutableSortedTreeDictionary<int, int>.ValueCollection values = dictionary.Values;

            Assert.Empty(dictionary);
            Assert.Empty(keys);
            Assert.Empty(values);

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
            Assert.Equal(0, dictionary.Count);
            Assert.Equal(0, keys.Count);
            Assert.Equal(0, values.Count);
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.

            Assert.False(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsValue(0));
            Assert.False(dictionary.TryGetValue(0, out _));
            Assert.Throws<KeyNotFoundException>(() => dictionary[0]);

#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
            Assert.False(keys.Contains(0));
            Assert.False(values.Contains(0));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection

            Assert.Same(dictionary, dictionary.Clear());
        }

        [Fact]
        public void TestSingleElementDictionary()
        {
            var key = Generator.GetInt32().ToString();
            var value = Generator.GetInt32();
            ImmutableSortedTreeDictionary<string, int> dictionary = ImmutableSortedTreeDictionary.Create<string, int>().Add(key, value);
            Assert.Equal(new[] { new KeyValuePair<string, int>(key, value) }, dictionary);

            dictionary = ImmutableSortedTreeDictionary.Create<string, int>(keyComparer: null).Add(key, value);
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(new[] { new KeyValuePair<string, int>(key, value) }, dictionary);

            dictionary = ImmutableSortedTreeDictionary.Create<string, int>(StringComparer.OrdinalIgnoreCase).Add(key, value);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.KeyComparer);
            Assert.Equal(new[] { new KeyValuePair<string, int>(key, value) }, dictionary);
        }

        [Fact]
        public void TestMultipleElementDictionary()
        {
            KeyValuePair<string, int>[] pairs =
            {
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
            };

            // Construction using ImmutableSortedTreeDictionary.Create
            var dictionary = ImmutableSortedTreeDictionary.CreateRange(pairs);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = ImmutableSortedTreeDictionary.CreateRange<string, int>(keyComparer: null, pairs);
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = ImmutableSortedTreeDictionary.CreateRange(StringComparer.OrdinalIgnoreCase, pairs);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, new KeyOfPairComparer<string, int>(StringComparer.OrdinalIgnoreCase)), dictionary);

            // Construction using ImmutableSortedTreeDictionary.ToImmutableSortedTreeDictionary
            dictionary = pairs.ToImmutableSortedTreeDictionary();
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = pairs.ToImmutableSortedTreeDictionary(keyComparer: null);
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = pairs.ToImmutableSortedTreeDictionary(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, new KeyOfPairComparer<string, int>(StringComparer.OrdinalIgnoreCase)), dictionary);

            // Construction using ImmutableSortedTreeDictionary.ToImmutableSortedTreeDictionary, where the source is already an
            // ImmutableSortedTreeDictionary<TKey, TValue>
            dictionary = pairs.ToImmutableSortedTreeDictionary().ToImmutableSortedTreeDictionary();
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = pairs.ToImmutableSortedTreeDictionary().ToImmutableSortedTreeDictionary(keyComparer: null);
            Assert.Same(Comparer<string>.Default, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);

            dictionary = pairs.ToImmutableSortedTreeDictionary().ToImmutableSortedTreeDictionary(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.KeyComparer);
            Assert.Equal(pairs.OrderBy(x => x, new KeyOfPairComparer<string, int>(StringComparer.OrdinalIgnoreCase)), dictionary);
        }

        [Fact]
        public void TestImmutableSortedTreeDictionaryCreateRange()
        {
            KeyValuePair<string, int>[] pairs =
            {
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
                new KeyValuePair<string, int>(Generator.GetInt32().ToString(), Generator.GetInt32()),
            };

            var dictionary = ImmutableSortedTreeDictionary.CreateRange(pairs);
            Assert.Equal(pairs.OrderBy(x => x, KeyOfPairComparer<string, int>.Default), dictionary);
        }

        [Fact]
        public void TestImmutableSortedTreeDictionaryCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeDictionary.CreateRange<string, int>(null));
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeDictionary.CreateRange<string, int>(Comparer<string>.Default, null));
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeDictionary.CreateRange<string, int>(Comparer<string>.Default, EqualityComparer<int>.Default, null));

            Assert.Throws<ArgumentNullException>("items", () => default(IEnumerable<KeyValuePair<string, int>>).ToImmutableSortedTreeDictionary());
            Assert.Throws<ArgumentNullException>("items", () => default(IEnumerable<KeyValuePair<string, int>>).ToImmutableSortedTreeDictionary(Comparer<string>.Default));
            Assert.Throws<ArgumentNullException>("items", () => default(IEnumerable<KeyValuePair<string, int>>).ToImmutableSortedTreeDictionary(Comparer<string>.Default, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<KeyValuePair<string, int>>).ToImmutableSortedTreeDictionary(x => x.Key, x => x.Value, Comparer<string>.Default));
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<KeyValuePair<string, int>>).ToImmutableSortedTreeDictionary(x => x.Key, x => x.Value, Comparer<string>.Default, EqualityComparer<int>.Default));

            Assert.Throws<ArgumentNullException>("keySelector", () => Enumerable.Empty<KeyValuePair<string, int>>().ToImmutableSortedTreeDictionary(keySelector: null, x => x.Value, Comparer<string>.Default));
            Assert.Throws<ArgumentNullException>("keySelector", () => Enumerable.Empty<KeyValuePair<string, int>>().ToImmutableSortedTreeDictionary(keySelector: null, x => x.Value, Comparer<string>.Default, EqualityComparer<int>.Default));

            Assert.Throws<ArgumentNullException>("elementSelector", () => Enumerable.Empty<KeyValuePair<string, int>>().ToImmutableSortedTreeDictionary<KeyValuePair<string, int>, string, int>(x => x.Key, elementSelector: null, Comparer<string>.Default));
            Assert.Throws<ArgumentNullException>("elementSelector", () => Enumerable.Empty<KeyValuePair<string, int>>().ToImmutableSortedTreeDictionary(x => x.Key, elementSelector: null, Comparer<string>.Default, EqualityComparer<int>.Default));
        }

        [Fact]
        public void TestCollectionConstructorUsesCorrectComparer()
        {
            var key1 = new StrongBox<int>(1);
            var key2 = new StrongBox<int>(2);
            KeyValuePair<StrongBox<int>, int>[] pairs =
            {
                new KeyValuePair<StrongBox<int>, int>(key1, 1),
                new KeyValuePair<StrongBox<int>, int>(key2, 2),
            };

            var comparer = new ComparisonComparer<StrongBox<int>>((x, y) => Comparer<int>.Default.Compare(x.Value, y.Value));
            var objectDictionary = ImmutableSortedTreeDictionary.CreateRange(comparer, pairs);
            Assert.Same(comparer, objectDictionary.KeyComparer);
            Assert.Equal(2, objectDictionary.Count);
            Assert.Equal(new[] { new KeyValuePair<StrongBox<int>, int>(key1, 1), new KeyValuePair<StrongBox<int>, int>(key2, 2) }, objectDictionary);

            var stringDictionary = ImmutableSortedTreeDictionary.Create<string, int>();
            Assert.Same(Comparer<string>.Default, stringDictionary.KeyComparer);

            stringDictionary = ImmutableSortedTreeDictionary.Create<string, int>(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringDictionary.KeyComparer);

            KeyValuePair<StrongBox<int>, int>[] pairsWithDuplicateKey =
            {
                new KeyValuePair<StrongBox<int>, int>(key1, 1),
                new KeyValuePair<StrongBox<int>, int>(key2, 2),
                new KeyValuePair<StrongBox<int>, int>(key1, 3),
            };

            Assert.Throws<ArgumentException>(() => ImmutableSortedTreeDictionary.CreateRange(comparer, pairsWithDuplicateKey));
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>().KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>().ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>().KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>().ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>().KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>().ValueComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(Enumerable.Empty<KeyValuePair<object, object>>()).KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(Enumerable.Empty<KeyValuePair<object, object>>()).ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(Enumerable.Empty<KeyValuePair<int, int>>()).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(Enumerable.Empty<KeyValuePair<int, int>>()).ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).ValueComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null).ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>(keyComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>(keyComparer: null).ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>(keyComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>(keyComparer: null).ValueComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(keyComparer: null, Enumerable.Empty<KeyValuePair<object, object>>()).KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(keyComparer: null, Enumerable.Empty<KeyValuePair<object, object>>()).ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(keyComparer: null, Enumerable.Empty<KeyValuePair<int, int>>()).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(keyComparer: null, Enumerable.Empty<KeyValuePair<int, int>>()).ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(keyComparer: null, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(keyComparer: null, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).ValueComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null, valueComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null, valueComparer: null).ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>(keyComparer: null, valueComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.Create<int, int>(keyComparer: null, valueComparer: null).ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>(keyComparer: null, valueComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.Create<IComparable, IComparable>(keyComparer: null, valueComparer: null).ValueComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<object, object>>()).KeyComparer);
            Assert.Same(EqualityComparer<object>.Default, ImmutableSortedTreeDictionary.CreateRange<object, object>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<object, object>>()).ValueComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<int, int>>()).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableSortedTreeDictionary.CreateRange<int, int>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<int, int>>()).ValueComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableSortedTreeDictionary.CreateRange<IComparable, IComparable>(keyComparer: null, valueComparer: null, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).ValueComparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            ZeroHashCodeEqualityComparer<object> objValueComparer = ZeroHashCodeEqualityComparer<object>.Default;

            Assert.Same(objComparer, ImmutableSortedTreeDictionary.Create<object, int>(keyComparer: objComparer).KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeDictionary.Create<int, int>(keyComparer: intComparer).KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeDictionary.Create<IComparable, int>(keyComparer: comparableComparer).KeyComparer);

            Assert.Same(objComparer, ImmutableSortedTreeDictionary.CreateRange<object, int>(keyComparer: objComparer, Enumerable.Empty<KeyValuePair<object, int>>()).KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeDictionary.CreateRange<int, int>(keyComparer: intComparer, Enumerable.Empty<KeyValuePair<int, int>>()).KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeDictionary.CreateRange<IComparable, int>(keyComparer: comparableComparer, Enumerable.Empty<KeyValuePair<IComparable, int>>()).KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null, valueComparer: objValueComparer).KeyComparer);
            Assert.Same(objValueComparer, ImmutableSortedTreeDictionary.Create<object, object>(keyComparer: null, valueComparer: objValueComparer).ValueComparer);
            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeDictionary.Create<object, object>().Add(new object(), null).WithComparers(keyComparer: null, valueComparer: objValueComparer).KeyComparer);
            Assert.Same(objValueComparer, ImmutableSortedTreeDictionary.Create<object, object>().Add(new object(), null).WithComparers(keyComparer: null, valueComparer: objValueComparer).ValueComparer);
        }

        [Fact]
        public void TestIDictionaryT()
        {
            IDictionary<int, int> dictionary = Enumerable.Range(0, 10).ToImmutableSortedTreeDictionary(x => x, x => x);
            Assert.Equal(Enumerable.Range(0, 10).Select(x => new KeyValuePair<int, int>(x, x)), dictionary);

            Assert.Equal(10, dictionary.Count);
            Assert.True(dictionary.IsReadOnly);
            Assert.Equal(10, dictionary.Keys.Count);
            Assert.True(dictionary.Keys.IsReadOnly);
            Assert.Equal(10, dictionary.Values.Count);
            Assert.True(dictionary.Values.IsReadOnly);

            Assert.Equal(Enumerable.Range(0, 10), dictionary.Keys);
            Assert.Equal(Enumerable.Range(0, 10), dictionary.Values);

            Assert.Throws<NotSupportedException>(() => dictionary.Keys.Remove(9));
            Assert.Throws<NotSupportedException>(() => dictionary.Values.Remove(9));
            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Remove(9);

            Assert.Equal(9, dictionary.Count);
            Assert.Equal(9, dictionary.Keys.Count);
            Assert.True(dictionary.Keys.IsReadOnly);
            Assert.Equal(9, dictionary.Values.Count);
            Assert.True(dictionary.Values.IsReadOnly);

            Assert.Equal(Enumerable.Range(0, 9), dictionary.Keys);
            Assert.Equal(Enumerable.Range(0, 9), dictionary.Values);

            Assert.Throws<NotSupportedException>(() => dictionary.Keys.Add(0));
            Assert.Throws<ArgumentNullException>("array", () => dictionary.Keys.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => dictionary.Keys.CopyTo(new int[dictionary.Count], -1));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => dictionary.Keys.CopyTo(new int[dictionary.Count], dictionary.Count + 1));
            Assert.Throws<ArgumentException>(() => dictionary.Keys.CopyTo(new int[dictionary.Count], 1));

            IEnumerator<int> keyEnumerator = dictionary.Keys.GetEnumerator();
            Assert.NotNull(keyEnumerator);
            Assert.True(keyEnumerator.MoveNext());
            Assert.Equal(0, keyEnumerator.Current);

            Assert.True(keyEnumerator.MoveNext());
            Assert.Equal(1, keyEnumerator.Current);

            keyEnumerator.Reset();
            Assert.True(keyEnumerator.MoveNext());
            Assert.Equal(0, keyEnumerator.Current);

            Assert.Throws<NotSupportedException>(() => dictionary.Values.Add(0));
            Assert.Throws<ArgumentNullException>("array", () => dictionary.Values.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => dictionary.Values.CopyTo(new int[dictionary.Count], -1));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => dictionary.Values.CopyTo(new int[dictionary.Count], dictionary.Count + 1));
            Assert.Throws<ArgumentException>(() => dictionary.Values.CopyTo(new int[dictionary.Count], 1));

            IEnumerator<int> valueEnumerator = dictionary.Values.GetEnumerator();
            Assert.NotNull(valueEnumerator);
            Assert.True(valueEnumerator.MoveNext());
            Assert.Equal(0, valueEnumerator.Current);

            Assert.True(valueEnumerator.MoveNext());
            Assert.Equal(1, valueEnumerator.Current);

            valueEnumerator.Reset();
            Assert.True(valueEnumerator.MoveNext());
            Assert.Equal(0, valueEnumerator.Current);

            var readOnlyDictionary = (IReadOnlyDictionary<int, int>)dictionary;
            Assert.Equal(dictionary.Keys, readOnlyDictionary.Keys);
            Assert.Equal(dictionary.Values, readOnlyDictionary.Values);

            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Add(11, 11);
            Assert.Equal(11, dictionary[11]);
            Assert.True(dictionary.Contains(new KeyValuePair<int, int>(11, 11)));
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(11, 12)));
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(12, 12)));
            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Remove(11);
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(11, 11)));

            Assert.NotEmpty(dictionary);
            Assert.Throws<NotSupportedException>(() => dictionary.Keys.Clear());
            Assert.Throws<NotSupportedException>(() => dictionary.Values.Clear());

            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Clear();
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);

            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).SetItem(0, 1);
            Assert.NotEmpty(dictionary);
            Assert.NotEmpty(dictionary.Keys);
            Assert.NotEmpty(dictionary.Values);
        }

        [Fact]
        public void TestIDictionary()
        {
            IDictionary dictionary = ImmutableSortedTreeDictionary.Create<int, int>();
            Assert.True(dictionary.IsFixedSize);
            Assert.True(dictionary.IsReadOnly);
            Assert.True(dictionary.IsSynchronized);

            dictionary = Enumerable.Range(0, 11).ToImmutableSortedTreeDictionary(x => x, x => x + 1);

            Assert.Throws<ArgumentNullException>("key", () => dictionary[key: null]);
            Assert.Null(dictionary["string key"]);
            Assert.Equal(11, dictionary[10]);

            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).SetItem(10, 12);
            Assert.Equal(12, dictionary[10]);
            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).SetItem(10, 11);

            TestCollection(dictionary, i => new KeyValuePair<int, int>(i, i + 1));

            var entries = new DictionaryEntry[dictionary.Count];
            dictionary.CopyTo(entries, 0);
            Assert.Equal(entries.Select(i => i.Key), dictionary.Keys.Cast<object>());
            Assert.Equal(entries.Select(i => i.Value), dictionary.Values.Cast<object>());

            Assert.Throws<ArgumentNullException>(() => dictionary.Contains(null));
            Assert.False(dictionary.Contains("string value"));
            Assert.True(dictionary.Contains(10));

            Assert.Equal(11, dictionary.Count);
            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Remove(10);
            Assert.Equal(10, dictionary.Count);
            Assert.False(dictionary.Contains(10));

            IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
            Assert.NotNull(enumerator);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Key);
            Assert.Equal(1, enumerator.Value);
            Assert.Equal(enumerator.Key, enumerator.Entry.Key);
            Assert.Equal(enumerator.Value, enumerator.Entry.Value);
            Assert.Equal(enumerator.Entry, enumerator.Current);

            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Key);
            Assert.Equal(2, enumerator.Value);
            Assert.Equal(enumerator.Key, enumerator.Entry.Key);
            Assert.Equal(enumerator.Value, enumerator.Entry.Value);
            Assert.Equal(enumerator.Entry, enumerator.Current);

            enumerator.Reset();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Key);
            Assert.Equal(1, enumerator.Value);
            Assert.Equal(enumerator.Key, enumerator.Entry.Key);
            Assert.Equal(enumerator.Value, enumerator.Entry.Value);
            Assert.Equal(enumerator.Entry, enumerator.Current);

            ICollection keys = dictionary.Keys;
            TestCollection(keys, i => i);

            ICollection values = dictionary.Values;
            TestCollection(values, i => i + 1);

            dictionary = ((ImmutableSortedTreeDictionary<int, int>)dictionary).Clear();
            Assert.Empty(dictionary);
            Assert.NotEmpty(keys);
            Assert.NotEmpty(values);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);

            void TestCollection<T>(ICollection collection, Func<int, T> indexToExpectedValue)
            {
                Assert.NotNull(collection);
                Assert.True(collection.IsSynchronized);
                Assert.Same(dictionary, collection.SyncRoot);

                Assert.Throws<ArgumentNullException>("array", () => collection.CopyTo(null, 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(new int[collection.Count, 1], 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(Array.CreateInstance(typeof(int), lengths: new[] { collection.Count }, lowerBounds: new[] { 1 }), 0));
                Assert.Throws<ArgumentOutOfRangeException>("index", () => collection.CopyTo(new int[collection.Count], -1));
                Assert.Throws<ArgumentOutOfRangeException>("index", () => collection.CopyTo(new int[collection.Count], collection.Count + 1));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(new int[collection.Count], 1));

                var elements = new T[collection.Count];
                collection.CopyTo(elements, 0);
                Assert.Equal(elements, collection);

                object[] objects = new object[collection.Count];
                collection.CopyTo(objects, 0);
                Assert.Equal(objects, collection);

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[collection.Count], 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(new byte[collection.Count], 0));

                IEnumerator collectionEnumerator = collection.GetEnumerator();
                Assert.NotNull(collectionEnumerator);
                Assert.True(collectionEnumerator.MoveNext());
                Assert.Equal(indexToExpectedValue(0), collectionEnumerator.Current);

                Assert.True(collectionEnumerator.MoveNext());
                Assert.Equal(indexToExpectedValue(1), collectionEnumerator.Current);

                collectionEnumerator.Reset();
                Assert.True(collectionEnumerator.MoveNext());
                Assert.Equal(indexToExpectedValue(0), collectionEnumerator.Current);
            }
        }

        [Fact]
        public void TestUnsupportedIDictionaryOperations()
        {
            IDictionary<string, int> dictionary = ImmutableSortedTreeDictionary.Create<string, int>();
            Assert.Throws<NotSupportedException>(() => dictionary[string.Empty] = 3);
            Assert.Throws<NotSupportedException>(() => dictionary.Add(string.Empty, 3));
            Assert.Throws<NotSupportedException>(() => dictionary.Add(new KeyValuePair<string, int>(string.Empty, 3)));
            Assert.Throws<NotSupportedException>(() => dictionary.Remove(string.Empty));
            Assert.Throws<NotSupportedException>(() => dictionary.Remove(new KeyValuePair<string, int>(string.Empty, 3)));
            Assert.Throws<NotSupportedException>(() => dictionary.Clear());

            IDictionary legacyDictionary = ImmutableSortedTreeDictionary.Create<string, int>();
            Assert.Throws<NotSupportedException>(() => legacyDictionary[string.Empty] = 3);
            Assert.Throws<NotSupportedException>(() => legacyDictionary.Add(string.Empty, 3));
            Assert.Throws<NotSupportedException>(() => legacyDictionary.Remove(string.Empty));
            Assert.Throws<NotSupportedException>(() => legacyDictionary.Clear());
        }

        [Fact]
        public void TestEnumerator()
        {
            var dictionary = ImmutableSortedTreeDictionary.Create<int, int>();
            ImmutableSortedTreeDictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);

            dictionary = dictionary.Add(1, 2);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);

            enumerator = dictionary.GetEnumerator();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);

            // Reset has no effect due to boxing the value type
            ((IEnumerator<KeyValuePair<int, int>>)enumerator).Reset();
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
        }

        [Fact]
        public void TestIEnumeratorT()
        {
            var dictionary = ImmutableSortedTreeDictionary.Create<int, int>();
            IEnumerator<KeyValuePair<int, int>> enumerator = dictionary.GetEnumerator();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);

            // Adding an item to the list doesn't affect the enumerator
            dictionary = dictionary.Add(1, 2);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            enumerator.Reset();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);

            enumerator = dictionary.GetEnumerator();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);

            enumerator.Reset();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
            enumerator.Reset();
            Assert.Equal(new KeyValuePair<int, int>(0, 0), enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(new KeyValuePair<int, int>(1, 2), enumerator.Current);
        }

        protected override IImmutableDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
        {
            return ImmutableSortedTreeDictionary.Create<TKey, TValue>();
        }
    }
}
