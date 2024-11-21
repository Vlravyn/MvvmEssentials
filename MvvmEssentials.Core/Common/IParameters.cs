using System.Diagnostics.CodeAnalysis;

namespace MvvmEssentials.Core
{
    /// <summary>
    /// A basic interaface for storing parameters to send to other classes.
    /// </summary>
    public interface IParameters : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Adds a new value to the specified key
        /// </summary>
        /// <param name="key">the key for this value</param>
        /// <param name="value">the value</param>
        void Add(string key, object value);

        /// <summary>
        /// Checks if the parameters has a certain key
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns><see langword="true"/> if the parameters contain the key</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the number of parameters that are added to the parameter
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Stores all the keys are have been used.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Gets the value for the key
        /// </summary>
        /// <typeparam name="T">the type of value to get</typeparam>
        /// <param name="key">the key</param>
        /// <returns>the value</returns>
        T GetValue<T>(string key);

        /// <summary>
        /// Gets all the values for this key
        /// </summary>
        /// <typeparam name="T">the type of values</typeparam>
        /// <param name="key">the key whose values to get</param>
        /// <returns></returns>
        IEnumerable<T> GetValues<T>(string key);

        /// <summary>
        /// Tries to get the value for the key
        /// </summary>
        /// <typeparam name="T">the type of value to get</typeparam>
        /// <param name="key">the key</param>
        /// <param name="value">the value. Null when there is no value.</param>
        /// <returns><see langword="true"/> is the value is retrieved successfully, <see langword="false"/> when value is null</returns>
        bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value);
    }
}