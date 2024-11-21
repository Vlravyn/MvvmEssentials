namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// The the base attached property that has to be attached to the control which hosts the content that has to be navigated.
    /// </summary>
    public interface INavigationNamesAP
    {
        /// <summary>
        /// The name of the region for navigation
        /// </summary>
        public string NavigationName { get; set; }
    }
}