using System;
using System.Collections.Generic;
using System.Linq;

namespace sweet.framework.Utility.Extention
{
    public static class LinqExtentions
    {
        /// <summary>
        /// Filters a collection based on a provided key selector.
        /// </summary>
        /// <param name="source">The collection filter.</param>
        /// <param name="keySelector">The predicate to filter by.</param>
        /// <typeparam name="TSource">The type of the collection to filter.</typeparam>
        /// <typeparam name="TKey">The type of the key to filter by.</typeparam>
        /// <returns>A <see cref="IEnumerable{T}"/> instance with the filtered values.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Merges a collection of <see cref="IDictionary{TKey,TValue}"/> instances into a single one.
        /// </summary>
        /// <param name="dictionaries">The list of <see cref="IDictionary{TKey,TValue}"/> instances to merge.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> instance containing the keys and values from the other instances.</returns>
        public static IDictionary<string, string> Merge(this IEnumerable<IDictionary<string, string>> dictionaries, bool ignoreCase = false)
        {
            var output = new Dictionary<string, string>(ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

            foreach (var dictionary in dictionaries.Where(d => d != null))
            {
                foreach (var kvp in dictionary)
                {
                    if (!output.ContainsKey(kvp.Key))
                    {
                        output.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return output;
        }

        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }
    }
}