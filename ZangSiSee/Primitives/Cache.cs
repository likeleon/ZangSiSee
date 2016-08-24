using System;
using System.Collections;
using System.Collections.Generic;

namespace ZangSiSee.Primitives
{
    public class Cache<T, U> : IReadOnlyDictionary<T, U>
    {
        public U this[T key] => _cache.GetOrAdd(key, _loader);
        public int Count => _cache.Count;
        public IEnumerable<T> Keys => _cache.Keys;
        public IEnumerable<U> Values => _cache.Values;

        readonly Dictionary<T, U> _cache = new Dictionary<T, U>();
        readonly Func<T, U> _loader;

        public Cache(Func<T, U> loader, IEqualityComparer<T> c)
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            _loader = loader;
        }

        public Cache(Func<T, U> loader)
            : this(loader, EqualityComparer<T>.Default)
        {
        }

        public bool ContainsKey(T key) => _cache.ContainsKey(key);
        public IEnumerator<KeyValuePair<T, U>> GetEnumerator() => _cache.GetEnumerator();
        public bool TryGetValue(T key, out U value) => _cache.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _cache.GetEnumerator();
    }
}
