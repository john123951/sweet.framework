using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sweet.framework.Utility.Extention
{
    /// <summary>
    /// Containing extensions for the collection objects.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts a <see cref="NameValueCollection"/> to a <see cref="IDictionary{TKey,TValue}"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="NameValueCollection"/> to convert.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> instance.</returns>
        public static IDictionary<string, IEnumerable<string>> ToDictionary(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, source.GetValues);
        }

        /// <summary>
        /// Converts an <see cref="IDictionary{TKey,TValue}"/> instance to a <see cref="NameValueCollection"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{TKey,TValue}"/> instance to convert.</param>
        /// <returns>A <see cref="NameValueCollection"/> instance.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, IEnumerable<string>> source)
        {
            var collection = new NameValueCollection();

            foreach (var key in source.Keys)
            {
                foreach (var value in source[key])
                {
                    collection.Add(key, value);
                }
            }

            return collection;
        }
    }
}