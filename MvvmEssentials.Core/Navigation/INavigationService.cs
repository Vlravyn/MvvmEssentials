using System;

namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Allows navigation to different views on the same window.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to the new view.
        /// <see cref="INavigationNamesAP.NavigationName"/> must be set on the control which hosts the content for the view.
        /// </summary>
        /// <typeparam name="T">The type of enum which has attributes <see cref="IsNavigationContentEnumAttribute"/> and <see cref="NavigateToAttribute"/> attached to it</typeparam>
        /// <param name="regionName">The <see cref="INavigationNamesAP.NavigationName"/> of the control</param>
        /// <param name="contentName">the enum which has the <see cref="NavigateToAttribute"/> which holds to type of the view to navigate to</param>
        /// <param name="parameters">the parameters to pass to the new view</param>
        /// <returns><see langword="true"/> when the navigation is completed successfully.</returns>
        bool Navigate<T>(string regionName, T contentName, INavigationParameters parameters)
            where T : Enum;

        /// <summary>
        /// Navigates to the new view.
        /// <see cref="INavigationNamesAP.NavigationName"/> must be set on the control which hosts the content for the view.
        /// </summary>
        /// <param name="regionName">The <see cref="INavigationNamesAP.NavigationName"/> of the control</param>
        /// <param name="viewType">The type of view to open.</param>
        /// <param name="parameters">the parameters to pass to the new view</param>
        /// <returns><see langword="true"/> when the navigation is completed successfully.</returns>
        bool Navigate(string regionName, Type viewType, INavigationParameters parameters);

        /// <summary>
        /// Navigates back to the previous view
        /// </summary>
        /// <param name="regionName">the registered name of the region.</param>
        void NavigateBack(string regionName);

        /// <summary>
        /// Navigates forward to the next view.
        /// </summary>
        /// <param name="regionName">The registered name of the region</param>
        void NavigateForward(string regionName);
    }
}