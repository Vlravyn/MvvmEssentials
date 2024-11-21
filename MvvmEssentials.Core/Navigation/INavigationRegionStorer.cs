using System.Collections.Generic;

namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Supports storing the regions that can hold the content.
    /// </summary>
    /// <typeparam name="T">the type of region that holds the content</typeparam>
    public interface INavigationRegionStorer<T> : IDictionary<string, T>
    {
        /// <summary>
        /// Finds and stores all the <typeparamref name="T"/> what are children of this view
        /// </summary>
        /// <typeparam name="TParentOfRegionContainer">the type of control that holds the Regions</typeparam>
        /// <param name="parent">the parent control that can potentially hold a region.</param>
        /// <returns><see langword="true"/> if any new regions were added</returns>
        bool FindAndStoreRegions<TParentOfRegionContainer>(TParentOfRegionContainer parent);
    }
}