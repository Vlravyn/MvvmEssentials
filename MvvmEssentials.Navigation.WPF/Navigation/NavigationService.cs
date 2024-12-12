using MvvmEssentials.Core;
using MvvmEssentials.Core.Navigation;
using MvvmEssentials.Navigation.WPF.Dialog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MvvmEssentials.Navigation.WPF.Navigation
{
    /// <summary>
    /// Implementation of <see cref="INavigationService"/> for WPF <see cref="Frame"/> navigation.
    /// </summary>
    public class NavigationService : INavigationService
    {
        #region Private Members

        private readonly NavigationFrameStorer navigationFrameStorer;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// list of region contents that are active right now.
        /// </summary>
        private readonly List<FrameworkElement> activeRegionContent = new();

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Creates an instance for navigation
        /// </summary>
        /// <param name="service">the service provider</param>
        public NavigationService(IServiceProvider service)
        {
            navigationFrameStorer = new NavigationFrameStorer();
            serviceProvider = service;
        }

        #endregion Constructors

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">thrown when region was not found. Check if the <paramref name="regionName"/> is correct</exception>
        /// <exception cref="ArgumentNullException">thrown when the type specified in the <see cref="NavigateToAttribute"/> is not found in the <see cref="IServiceProvider"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the page type does not have <see cref="IsNavigationContentEnumAttribute"/> set on it</exception>
        /// <exception cref="InvalidOperationException">thrown when the <see cref="NavigateToAttribute"/> is not set on the <paramref name="contentName"/></exception>
        public bool Navigate<T>(string regionName, T contentName, INavigationParameters parameters)
            where T : Enum
        {
            if (!typeof(T).HasAttribute<IsNavigationContentEnumAttribute>())
                throw new ArgumentOutOfRangeException(nameof(contentName), "Invalid page type used for navigation");

            //Try to add regions of the active window if there are no registered regions found.
            if (navigationFrameStorer.Count == 0)
                CheckAndUpdateAllViewsForNavigationRegions();

            if (navigationFrameStorer.Count == 0)
                return false;

            navigationFrameStorer.TryGetValue(regionName, out var frameManager);

            if (frameManager == null)
                throw new ArgumentException($"{regionName} frame not found");

            var navigateToAttribute = contentName.GetAttribute<NavigateToAttribute>();

            if (navigateToAttribute == null)
                throw new InvalidOperationException($"The enum passed in to the {nameof(Navigate)} does not have {nameof(NavigateToAttribute)} set on it's value");

            object? content;

            if (navigateToAttribute.DestinationType is not null)
            {
                content = serviceProvider.GetService(navigateToAttribute.DestinationType);

                if (content is null)
                    throw new ArgumentNullException($"Could not find the content to navigate to. {nameof(contentName)}: {contentName}");
            }
            else
                content = null;

            var result = frameManager.Navigate(content);

            //Check if the current frame content is a view with a view model that implements INavigationAware interface
            //To Run the OnNavigatedFrom method
            if (frameManager.Region.Content != null && frameManager.Region.Content is FrameworkElement currentFrameContent && currentFrameContent.DataContext is INavigationAware currentContentViewModel)
                currentContentViewModel.OnNavigatedFrom();

            if (content is FrameworkElement newContent && newContent.DataContext is INavigationAware newContentViewModel)
            {
                newContentViewModel.OnNavigatedTo(parameters);

                //Keep an instance of the views that have been opened for searching of frames to navigate
                newContent.Loaded += (s, e) => activeRegionContent.Add(newContent);
                newContent.Unloaded += (s, e) => activeRegionContent.Remove(newContent);
            }

            return result;
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentException">thrown when the frame with the specified region was not found.</exception>
        /// <exception cref="InvalidOperationException">thrown when the passed in <paramref name="viewType"/> is a <see cref="Window"/>.</exception>
        public bool Navigate(string regionName, Type viewType, INavigationParameters parameters)
        {
            //Try to add regions of the active window if there are no registered regions found.
            if (navigationFrameStorer.Count == 0)
                CheckAndUpdateAllViewsForNavigationRegions();

            if (navigationFrameStorer.Count == 0)
                return false;

            navigationFrameStorer.TryGetValue(regionName, out var frameManager);

            if (frameManager == null)
                throw new ArgumentException($"{regionName} frame not found");

            object? content = serviceProvider.GetService(viewType);

            if (content is Window)
                throw new InvalidOperationException("Cannot navigate to a type of window");

            var result = frameManager.Navigate(content);

            //Check if the current frame content is a view with a view model that implements INavigationAware interface
            //To Run the OnNavigatedFrom method
            if (frameManager.Region.Content != null && frameManager.Region.Content is FrameworkElement currentFrameContent && currentFrameContent.DataContext is INavigationAware currentContentViewModel)
                currentContentViewModel.OnNavigatedFrom();

            if (content is FrameworkElement newContent && newContent.DataContext is INavigationAware newContentViewModel)
            {
                newContentViewModel.OnNavigatedTo(parameters);

                //Keep an instance of the views that have been opened for searching of frames to navigate
                newContent.Loaded += (s, e) => activeRegionContent.Add(newContent);
                newContent.Unloaded += (s, e) => activeRegionContent.Remove(newContent);
            }

            return result;
        }
        /// <summary>
        /// Checks all the active views and adds them for navigation
        /// </summary>
        private void CheckAndUpdateAllViewsForNavigationRegions()
        {
            foreach (var view in DialogService.ActiveViews)
                navigationFrameStorer.FindAndStoreRegions(view);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">thrwn when the stored region list in the navigation service is empty or the service could not find the region</exception>
        public void NavigateBack(string regionName)
        {
            if (navigationFrameStorer.Count == 0)
                throw new InvalidOperationException("Cannot navigate back on a region because no regions are stored in the navigation service.");

            navigationFrameStorer.TryGetValue(regionName, out var frameManager);

            if (frameManager == null)
                throw new InvalidOperationException("Unable to find the view on which the navigation back has to occur");

            frameManager.GoBack();
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">thrwn when the stored region list in the navigation service is empty or the service could not find the region</exception>
        public void NavigateForward(string regionName)
        {
            if (navigationFrameStorer.Count == 0)
                throw new InvalidOperationException("Cannot navigate back on a region because no regions are stored in the navigation service.");

            navigationFrameStorer.TryGetValue(regionName, out var frameManager);

            if (frameManager == null)
                throw new InvalidOperationException("Unable to find the view on which the navigation back has to occur");

            frameManager.GoForward();
        }
    }
}