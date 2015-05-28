namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    partial class TreeList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly TreeList<T> _list;
            private int _version;

            private int _index;
            private T _current;

            public Enumerator(TreeList<T> list)
            {
                _list = list;
                _version = list._version;
                _index = -1;
                _current = default(T);
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                        throw new InvalidOperationException();

                    return _current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < -1)
                {
                    // Past the end of the list.
                    return false;
                }

                if (_list._version != _version)
                    throw new InvalidOperationException();

                if (_index == _list.Count - 1)
                {
                    _index = int.MinValue;
                    return false;
                }

                _index++;
                _current = _list[_index];
                return true;
            }

            public void Reset()
            {
                _version = _list._version;
                _index = -1;
            }
        }
    }
}
