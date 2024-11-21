namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Contains all the journal entries
    /// </summary>
    public interface INavigationJournal
    {
        /// <summary>
        /// Stores all the journal entries.
        /// Supports different types of enums
        /// </summary>
        IEnumerable<INavigationJournalEntry> JournalEntries { get; }

        /// <summary>
        /// Adds a new journal entry
        /// </summary>
        /// <param name="oldContent">the previous content of the region</param>
        void AddNewEntry(object oldContent);
    }
}