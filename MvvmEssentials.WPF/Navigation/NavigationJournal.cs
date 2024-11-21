using MvvmEssentials.Core.Navigation;

namespace MvvmEssentials.WPF.Navigation
{
    /// <summary>
    /// Journal to store all the past navigations.
    /// </summary>
    internal class NavigationJournal : INavigationJournal
    {
        private List<INavigationJournalEntry> entries = new();
        public IEnumerable<INavigationJournalEntry> JournalEntries => entries.AsEnumerable();

        public void AddNewEntry(object navigationTarget)
        {
            entries.Add(new NavigationJournalEntry()
            {
                Content = navigationTarget
            });
        }
    }
}