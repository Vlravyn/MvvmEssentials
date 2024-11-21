using MvvmEssentials.Core.Navigation;

namespace MvvmEssentials.WPF.Navigation
{
    /// <summary>
    /// Basic implementation of <see cref="INavigationJournalEntry"/>
    /// </summary>
    internal record NavigationJournalEntry : INavigationJournalEntry
    {
        public required object Content { get; init; }
    }
}