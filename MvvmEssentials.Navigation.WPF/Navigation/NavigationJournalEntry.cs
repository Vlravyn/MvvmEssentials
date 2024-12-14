using MvvmEssentials.Core.Navigation;

namespace MvvmEssentials.Navigation.WPF.Navigation
{
    /// <summary>
    /// Implementation of <see cref="INavigationJournalEntry"/>
    /// </summary>
    internal class NavigationJournalEntry : INavigationJournalEntry
    {
        public object Content { get; private set; }

        public NavigationJournalEntry(object content)
        {
            Content = content;
        }
    }
}