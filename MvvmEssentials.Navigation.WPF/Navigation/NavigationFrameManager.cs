using MvvmEssentials.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MvvmEssentials.Navigation.WPF.Navigation
{
    /// <summary>
    /// Implementation of the <see cref="INavigationRegionManager{T}"/> for WPF <see cref="Frame"/> navigation.
    /// </summary>
    internal class NavigationFrameManager : INavigationRegionManager<Frame>
    {
        private readonly Stack<INavigationJournalEntry> goBackEntries = new();
        private readonly Stack<INavigationJournalEntry> goForwardEntries = new();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="region">the frame to manage</param>
        public NavigationFrameManager(Frame region)
        {
            Region = region;
            Journal = new NavigationJournal();
        }

        public INavigationJournal Journal { get; }

        public Frame Region { get; }

        public bool CanGoBack => goBackEntries.Count > 0;

        public bool CanGoForward => goForwardEntries.Count > 0;

        public object CurrentContent => Region.Content;

        public event EventHandler<NavigatedEventHandler> Navigated;

        public bool Navigate(object content)
        {
            Journal.AddNewEntry(Region.Content);
            Region.Content = content;
            return true;
        }

        public void GoBack()
        {
            if (CanGoBack)
                Region.Content = goBackEntries.Pop();
        }

        public void GoForward()
        {
            if (CanGoForward)
                Region.Content = goForwardEntries.Pop();
        }
    }
}