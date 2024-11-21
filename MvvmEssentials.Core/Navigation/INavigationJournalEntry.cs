namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Represents a journal entry
    /// </summary>
    public interface INavigationJournalEntry
    {
        /// <summary>
        /// The old content of the the region.
        /// </summary>
        object Content { get; }
    }
}