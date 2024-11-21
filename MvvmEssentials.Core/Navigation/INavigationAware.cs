namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Makes the view model aware of the navigation.
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Runs when the view model is navigated to.
        /// </summary>
        /// <param name="parameters"></param>
        void OnNavigatedTo(INavigationParameters parameters);

        /// <summary>
        /// Run when the view model is being navigated away from.
        /// </summary>
        void OnNavigatedFrom();
    }
}