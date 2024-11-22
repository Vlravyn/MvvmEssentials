using MvvmEssentials.Core.Navigation;

namespace MvvmEssentials.Navigation.WPF.Navigation
{
    /// <summary>
    /// Basic implementation of <see cref="INavigationJournalEntry"/>
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