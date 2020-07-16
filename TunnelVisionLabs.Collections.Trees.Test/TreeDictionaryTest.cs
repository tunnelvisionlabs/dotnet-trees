// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class TreeDictionaryTest
    {
        [Fact]
        public void TestTreeDictionaryConstructor()
        {
            TreeDictionary<int, int> dictionary = new TreeDictionary<int, int>();
            Assert.Empty(dictionary);
        }

        [Fact]
        public void TestCollectionConstructors()
        {
            Assert.Throws<ArgumentNullException>("collection", () => new TreeDictionary<int, int>(collection: null));
            Assert.Throws<ArgumentNullException>("collection", () => new TreeDictionary<int, int>(branchingFactor: 4, collection: null, comparer: null));

            Assert.Throws<ArgumentException>(() => new TreeDictionary<int, int>(new[] { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(1, 2) }));
            Assert.Throws<ArgumentException>(() => new TreeDictionary<int, int>(branchingFactor: 4, new[] { new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(1, 2) }, comparer: null));

            var dictionary = new TreeDictionary<int, int>(new[] { new KeyValuePair<int, int>(1, 1) });
            Assert.Single(dictionary);
            Assert.Equal(new[] { new KeyValuePair<int, int>(1, 1) }, dictionary);

            dictionary = new TreeDictionary<int, int>(branchingFactor: 4, new[] { new KeyValuePair<int, int>(1, 1) }, comparer: null);
            Assert.Single(dictionary);
            Assert.Equal(new[] { new KeyValuePair<int, int>(1, 1) }, dictionary);
        }

        [Fact]
        public void TestEmptyDictionary()
        {
            var dictionary = new TreeDictionary<int, int>();
            TreeDictionary<int, int>.KeyCollection keys = dictionary.Keys;
            TreeDictionary<int, int>.ValueCollection values = dictionary.Values;

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
        }

        [Fact]
        public void TestIDictionaryT()
        {
            IDictionary<int, int> dictionary = new TreeDictionary<int, int>(branchingFactor: 4);
            for (int i = 0; i < 10; i++)
                dictionary.Add(i, i);

            Assert.Equal(10, dictionary.Count);
            Assert.False(dictionary.IsReadOnly);
            Assert.Equal(10, dictionary.Keys.Count);
            Assert.False(dictionary.Keys.IsReadOnly);
            Assert.Equal(10, dictionary.Values.Count);
            Assert.False(dictionary.Values.IsReadOnly);

            Assert.Equal(Enumerable.Range(0, 10), dictionary.Keys);
            Assert.Equal(Enumerable.Range(0, 10), dictionary.Values);

            Assert.Throws<NotSupportedException>(() => dictionary.Values.Remove(9));
            Assert.False(dictionary.Keys.Remove(10));
            Assert.True(dictionary.Keys.Remove(9));

            Assert.Equal(9, dictionary.Count);
            Assert.Equal(9, dictionary.Keys.Count);
            Assert.False(dictionary.Keys.IsReadOnly);
            Assert.Equal(9, dictionary.Values.Count);
            Assert.False(dictionary.Values.IsReadOnly);

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

            IReadOnlyDictionary<int, int> readOnlyDictionary = (IReadOnlyDictionary<int, int>)dictionary;
            Assert.Equal(dictionary.Keys, readOnlyDictionary.Keys);
            Assert.Equal(dictionary.Values, readOnlyDictionary.Values);

            dictionary.Add(new KeyValuePair<int, int>(11, 11));
            Assert.Equal(11, dictionary[11]);
            Assert.True(dictionary.Contains(new KeyValuePair<int, int>(11, 11)));
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(11, 12)));
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(12, 12)));
            Assert.False(dictionary.Remove(new KeyValuePair<int, int>(11, 12)));
            Assert.True(dictionary.Contains(new KeyValuePair<int, int>(11, 11)));
            Assert.True(dictionary.Remove(new KeyValuePair<int, int>(11, 11)));
            Assert.False(dictionary.Contains(new KeyValuePair<int, int>(11, 11)));

            Assert.NotEmpty(dictionary);
            dictionary.Keys.Clear();
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);

            dictionary[0] = 1;
            Assert.NotEmpty(dictionary);
            dictionary.Values.Clear();
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);
        }

        [Fact]
        public void TestIDictionary()
        {
            IDictionary dictionary = new TreeDictionary<int, int>(branchingFactor: 4);
            Assert.False(dictionary.IsFixedSize);
            Assert.False(dictionary.IsReadOnly);
            Assert.False(dictionary.IsSynchronized);

            Assert.Throws<ArgumentNullException>("key", () => dictionary.Add(key: null, value: 1));
            Assert.Throws<ArgumentException>("value", () => dictionary.Add(key: 1, value: null));
            Assert.Throws<ArgumentException>("key", () => dictionary.Add(key: "string value", value: 0));
            Assert.Throws<ArgumentException>("value", () => dictionary.Add(key: 0, value: "string value"));

            for (int i = 0; i < 11; i++)
                dictionary.Add(i, i + 1);

            Assert.Throws<ArgumentNullException>("key", () => dictionary[key: null]);
            Assert.Null(dictionary["string key"]);
            Assert.Equal(11, dictionary[10]);

            Assert.Throws<ArgumentNullException>("key", () => dictionary[key: null] = 12);
            Assert.Throws<ArgumentException>("key", () => dictionary["string key"] = 12);
            Assert.Throws<ArgumentException>("value", () => dictionary[10] = null);
            Assert.Throws<ArgumentException>("value", () => dictionary[10] = "string value");
            dictionary[10] = 12;
            Assert.Equal(12, dictionary[10]);
            dictionary[10] = 11;

            TestCollection(dictionary, i => new KeyValuePair<int, int>(i, i + 1));

            var entries = new DictionaryEntry[dictionary.Count];
            dictionary.CopyTo(entries, 0);
            Assert.Equal(entries.Select(i => i.Key), dictionary.Keys.Cast<object>());
            Assert.Equal(entries.Select(i => i.Value), dictionary.Values.Cast<object>());

            Assert.Throws<ArgumentNullException>(() => dictionary.Contains(null));
            Assert.False(dictionary.Contains("string value"));
            Assert.True(dictionary.Contains(10));

            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
            Assert.Equal(11, dictionary.Count);
            dictionary.Remove("string value");
            Assert.Equal(11, dictionary.Count);
            dictionary.Remove(10);
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

            dictionary.Clear();
            Assert.Empty(dictionary);
            Assert.Empty(keys);
            Assert.Empty(values);

            void TestCollection<T>(ICollection collection, Func<int, T> indexToExpectedValue)
            {
                Assert.NotNull(collection);
                Assert.False(collection.IsSynchronized);
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

                var objects = new object[collection.Count];
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
        public void TestTreeSetBranchingFactorConstructor()
        {
            TreeDictionary<int, int> set = new TreeDictionary<int, int>(8);
            Assert.Empty(set);

            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeDictionary<int, int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeDictionary<int, int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeDictionary<int, int>(1));
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>().Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>().Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>().Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(Enumerable.Empty<KeyValuePair<object, object>>()).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(Enumerable.Empty<KeyValuePair<int, int>>()).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(Enumerable.Empty<KeyValuePair<IComparable, IComparable>>()).Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(comparer: null).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(comparer: null).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(comparer: null).Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(Enumerable.Empty<KeyValuePair<object, object>>(), comparer: null).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(Enumerable.Empty<KeyValuePair<int, int>>(), comparer: null).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(Enumerable.Empty<KeyValuePair<IComparable, IComparable>>(), comparer: null).Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(4).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(4).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(4).Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(4, comparer: null).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(4, comparer: null).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(4, comparer: null).Comparer);

            Assert.Same(EqualityComparer<object>.Default, new TreeDictionary<object, object>(4, Enumerable.Empty<KeyValuePair<object, object>>(), comparer: null).Comparer);
            Assert.Same(EqualityComparer<int>.Default, new TreeDictionary<int, int>(4, Enumerable.Empty<KeyValuePair<int, int>>(), comparer: null).Comparer);
            Assert.Same(EqualityComparer<IComparable>.Default, new TreeDictionary<IComparable, IComparable>(4, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>(), comparer: null).Comparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = ZeroHashCodeEqualityComparer<object>.Default;
            var intComparer = ZeroHashCodeEqualityComparer<int>.Default;
            var comparableComparer = ZeroHashCodeEqualityComparer<IComparable>.Default;

            Assert.Same(objComparer, new TreeDictionary<object, object>(comparer: objComparer).Comparer);
            Assert.Same(intComparer, new TreeDictionary<int, int>(comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new TreeDictionary<IComparable, IComparable>(comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new TreeDictionary<object, object>(Enumerable.Empty<KeyValuePair<object, object>>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new TreeDictionary<int, int>(Enumerable.Empty<KeyValuePair<int, int>>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new TreeDictionary<IComparable, IComparable>(Enumerable.Empty<KeyValuePair<IComparable, IComparable>>(), comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new TreeDictionary<object, object>(4, comparer: objComparer).Comparer);
            Assert.Same(intComparer, new TreeDictionary<int, int>(4, comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new TreeDictionary<IComparable, IComparable>(4, comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new TreeDictionary<object, object>(4, Enumerable.Empty<KeyValuePair<object, object>>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new TreeDictionary<int, int>(4, Enumerable.Empty<KeyValuePair<int, int>>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new TreeDictionary<IComparable, IComparable>(4, Enumerable.Empty<KeyValuePair<IComparable, IComparable>>(), comparer: comparableComparer).Comparer);
        }
    }
}
