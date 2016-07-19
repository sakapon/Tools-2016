using System;
using System.Collections.Generic;
using System.Linq;

namespace InkStrokes
{
    public static class Enumerable2
    {
        public static IEnumerable<TSource> DistinctConsecutively<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var elementCache = new ValueCache<TSource>();

            return source.Where(e => elementCache.SetValue(e));
        }

        public static IEnumerable<TSource> DistinctConsecutively<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            var keyCache = new ValueCache<TKey>();

            return source.Where(e => keyCache.SetValue(keySelector(e)));
        }
    }

    class ValueCache<T>
    {
        bool isInitialized;
        T value;

        public bool SetValue(T newValue)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                value = newValue;
                return true;
            }
            else
            {
                if (Equals(value, newValue)) return false;

                value = newValue;
                return true;
            }
        }
    }
}
