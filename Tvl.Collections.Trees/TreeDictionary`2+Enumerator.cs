// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System.Collections;
    using System.Collections.Generic;
    using IDictionaryEnumerator = System.Collections.IDictionaryEnumerator;

    public partial class TreeDictionary<TKey, TValue>
    {
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private readonly ReturnType _returnType;
            private TreeSet<KeyValuePair<TKey, TValue>>.Enumerator _enumerator;

            internal Enumerator(TreeSet<KeyValuePair<TKey, TValue>>.Enumerator enumerator, ReturnType returnType)
            {
                _returnType = returnType;
                _enumerator = enumerator;
            }

            internal enum ReturnType
            {
                /// <summary>
                /// The return value from the implementation of <see cref="IEnumerable.GetEnumerator"/> is
                /// <see cref="KeyValuePair{TKey, TValue}"/>. This is the return value for most instances of this
                /// enumerator.
                /// </summary>
                KeyValuePair,

                /// <summary>
                /// The return value from the implementation of <see cref="IEnumerable.GetEnumerator"/> is
                /// <see cref="DictionaryEntry"/>. This is the return value for instances of this enumerator created by
                /// the <see cref="IDictionary.GetEnumerator"/> implementation in
                /// <see cref="TreeDictionary{TKey, TValue}"/>.
                /// </summary>
                DictionaryEntry,
            }

            public KeyValuePair<TKey, TValue> Current => _enumerator.Current;

            object IEnumerator.Current => _returnType == ReturnType.DictionaryEntry ? (object)((IDictionaryEnumerator)this).Entry : Current;

            DictionaryEntry IDictionaryEnumerator.Entry => new DictionaryEntry(Current.Key, Current.Value);

            object IDictionaryEnumerator.Key => Current.Key;

            object IDictionaryEnumerator.Value => Current.Value;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext() => _enumerator.MoveNext();

            void IEnumerator.Reset() => InternalReset();

            internal void InternalReset() => _enumerator.InternalReset();
        }
    }
}
