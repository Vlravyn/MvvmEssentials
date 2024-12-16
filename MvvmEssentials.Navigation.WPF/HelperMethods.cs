using MvvmEssentials.Navigation.WPF.Navigation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MvvmEssentials.Navigation.WPF
{
    /// <summary>
    /// Coontains all the helper methods for this project.
    /// </summary>
    internal static class HelperMethods
    {
        /// <summary>
        /// Checks if the type has attribute <typeparamref name="T"/> attached to it
        /// </summary>
        /// <param name="type">the type to check the attribute of</param>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <returns><see langword="true"/> if the type has the attribute <typeparamref name="T"/> attached to it</returns>
        public static bool HasAttribute<T>(this Type type)
            where T : Attribute
        {
            return Attribute.GetCustomAttribute(type, typeof(T)) != null;
        }

        /// <summary>
        /// Get the first attribute of specific type
        /// </summary>
        /// <typeparam name="T">the type of attribute to get</typeparam>
        /// <param name="value">the value whose attribute we need to get</param>
        /// <returns>the attribute. Returns null if attribute of given type is not assigned to the value</returns>
        public static T? GetAttribute<T>(this object value)
            where T : Attribute, new()
        {
            if (value is not null)
            {
                var type = value.GetType();
                var memInfo = type.GetMember(value.ToString());
                var attribute = memInfo[0].GetCustomAttributes(typeof(T), false).FirstOrDefault();
                return (T?)attribute;
            }

            return null;
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter or null if not found</returns>
        internal static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null)
                return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child
                if (child is not T)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Finds all the children of control with a specific type
        /// </summary>
        /// <typeparam name="T">the type of children to find</typeparam>
        /// <returns>list of controls. Returns empty list when no control of specified type exists</returns>
        internal static IEnumerable<T> FindChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null)
                yield return (T)Enumerable.Empty<T>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);

                if (ithChild == null)
                    continue;

                if (ithChild is T t)
                    yield return t;

                foreach (T childOfChild in FindChildren<T>(ithChild))
                    yield return childOfChild;
            }
        }

        /// <summary>
        /// Gets all the child <see cref="Frame"/> that have attached property <see cref="NavigationNamesAP.NavigationNameProperty"/> assingned a value.
        /// </summary>
        /// <param name="parent">the parent control that holds the frames</param>
        /// <returns>list of frames. Returns empty list when there is not frame that meets the criteria</returns>
        internal static IEnumerable<Frame> GetNavigationFrames(FrameworkElement parent)
        {
            var frames = FindChildren<Frame>(parent);

            var navigationFrames = new List<Frame>();
            if (frames == null)
                return new List<Frame>().AsEnumerable();

            if (!frames.Any())
                return new List<Frame>().AsEnumerable();

            foreach (var frame in frames)
            {
                if (frame is not null)
                {
                    var nameValue = frame.GetValue(NavigationNamesAP.NavigationNameProperty);
                    if (nameValue != null)
                        navigationFrames.Add(frame);
                }
            }

            return navigationFrames;
        }
    }
}