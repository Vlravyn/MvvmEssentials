using MvvmEssentials.Core.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace MvvmEssentials.Navigation.WPF.Navigation
{
    /// <summary>
    /// Journal to store all the past navigations.
    /// </summary>
    internal class NavigationJournal : INavigationJournal
    {
        private readonly List<INavigationJournalEntry> entries = new();
        public IEnumerable<INavigationJournalEntry> JournalEntries => entries.AsEnumerable();

        public void AddNewEntry(object navigationTarget)
        {
            entries.Add(new NavigationJournalEntry(navigationTarget));
        }
    }
}