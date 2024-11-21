using MvvmEssentials.Core.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MvvmEssentials.WPF.Navigation
{
    /// <summary>
    /// Implementation of <see cref="INavigationRegionStorer{T}"/> for WPF <see cref="Frame"/> Navigation.
    /// <inheritdoc />
    /// </summary>
    internal class NavigationFrameStorer : INavigationRegionStorer<INavigationRegionManager<Frame>>
    {
        private Dictionary<string, INavigationRegionManager<Frame>> pairs = new();

        public INavigationRegionManager<Frame> this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException($"key for getting the frame in {nameof(NavigationFrameStorer)} is null");

                var result = pairs.TryGetValue(key, out var value);
                if (result)
                    return value;

                throw new KeyNotFoundException("Navigation name is not registered.");
            }
            set => pairs[key] = value;
        }

        public ICollection<string> Keys => pairs.Keys;

        public ICollection<INavigationRegionManager<Frame>> Values => pairs.Values;

        public int Count => pairs.Count;

        public bool IsReadOnly => false;

        public void Add(string key, Frame value)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if(value == null)
                throw new ArgumentNullException(nameof(value));

            if (pairs.ContainsKey(key))
                throw new ArgumentException("Frame NavigationName is already used");

            pairs.Add(key, new NavigationFrameManager(value));
        }

        public void Add(KeyValuePair<string, INavigationRegionManager<Frame>> item) => pairs.Add(item.Key, item.Value);

        public void Add(string key, INavigationRegionManager<Frame> value) => pairs.Add(key, value);

        public void Clear() => pairs.Clear();

        public bool Contains(KeyValuePair<string, Frame> item)
        {
            if (pairs.TryGetValue(item.Key, out INavigationRegionManager<Frame>? value) && value.Region == item.Value)
                return true;

            return false;
        }

        public bool Contains(KeyValuePair<string, INavigationRegionManager<Frame>> item) => pairs.ContainsKey(item.Key);

        public bool ContainsKey(string key) => pairs.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, INavigationRegionManager<Frame>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool FindAndStoreRegions<ParentOfRegionContainer>(ParentOfRegionContainer parent)
        {
            if (parent is Window or Page or UserControl)
            {
                var frames = HelperMethods.GetNavigationFrames(parent as FrameworkElement);

                if (frames is null || frames.Any() is false)
                    return false;

                bool isAnyAdded = false;
                foreach (var frame in frames)
                {
                    //Checking for regions of navigation inside child content.
                    if (frame.Content is Page or UserControl)
                        FindAndStoreRegions(frame.Content);

                    isAnyAdded = true;

                    var navigationName = (string)frame.GetValue(NavigationNamesAP.NavigationNameProperty);
                    pairs.Add(navigationName, new NavigationFrameManager(frame));
                }
                return isAnyAdded;
            }

            throw new ArgumentOutOfRangeException("Unknown type of control passed in to find the child navigation frames");
        }

        public IEnumerator<KeyValuePair<string, INavigationRegionManager<Frame>>> GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        public bool Remove(string key) => pairs.Remove(key);

        public bool Remove(KeyValuePair<string, INavigationRegionManager<Frame>> item)
        {
            if (pairs.ContainsKey(item.Key))
                return pairs.Remove(item.Key);

            return false;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out INavigationRegionManager<Frame> value) => pairs.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pairs.GetEnumerator();
        }
    }
}