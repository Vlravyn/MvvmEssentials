using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MvvmEssentials.Core.Common
{
    /// <summary>
    /// Basic Implementation of <see cref="IParameters"/>
    /// </summary>
    public class Parameters : IParameters
    {
        //internal list of parameters.
        private List<KeyValuePair<string, object>> pairs = new List<KeyValuePair<string, object>>();

        /// <inheritdoc/>
        public int Count => pairs.Count;

        /// <inheritdoc/>
        public IEnumerable<string> Keys => pairs.Distinct().ToList().ConvertAll(pair => pair.Key).AsEnumerable();

        /// <inheritdoc/>
        public void Add(string key, object value)
        {
            pairs.Add(new KeyValuePair<string, object>(key, value));
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return Keys.Contains(key);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            var pair = pairs.Where(pair => pair.Key == key);

            if (pair == null || pair.Any() is false)
                return default;

            return (T)pair.First().Value;
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetValues<T>(string key)
        {
            var ps = pairs.Where(pair => pair.Key == key);

            return ps.ToList().ConvertAll(pair => (T)pair.Value).AsEnumerable();
        }

        /// <inheritdoc/>
        public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            var pair = pairs.Where(pair => pair.Key == key && pair.GetType() == typeof(T));
            if (pair is null || pair.Any() is false)
            {
                value = default;
                return false;
            }
            else
            {
                value = (T)pair.First().Value;
                return true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pairs.GetEnumerator();
        }
    }
}