using MvvmEssentials.Core.Navigation;

namespace MvvmEssentials.WPF.Navigation
{
    /// <summary>
    /// Container for the region and stores past navigation data for going back/forward
    /// </summary>
    /// <typeparam name="T">the region that is being used for navigation</typeparam>
    public interface INavigationRegionManager<T>
    {
        /// <summary>
        /// the journal for the region
        /// </summary>
        INavigationJournal Journal { get; }

        /// <summary>
        /// The region that he being used for navigation
        /// </summary>
        T Region { get; }

        /// <summary>
        /// The current content that is being shown in the region
        /// </summary>
        object CurrentContent { get; }

        /// <summary>
        /// Checks whether the region can go back to the preview content
        /// </summary>
        public bool CanGoBack { get; }

        /// <summary>
        /// Checks whether the reggion can forward to the view that was being shown before going back.
        /// </summary>
        public bool CanGoForward { get; }

        /// <summary>
        /// Navigates to the new content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool Navigate(object content);

        /// <summary>
        /// Goes back to the previous view.
        /// </summary>
        public void GoBack();

        /// <summary>
        /// Go forward in the view
        /// </summary>
        public void GoForward();
    }
}