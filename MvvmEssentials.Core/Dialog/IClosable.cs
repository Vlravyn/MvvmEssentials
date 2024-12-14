using System;

namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// Allows the view to be closed from code.
    /// </summary>
    public interface IClosable
    {
        /// <summary>
        /// Allows the view model to close the view.
        /// </summary>
        Action Close { get; set; }

        /// <summary>
        /// Checks whether the view is closeable or not.
        /// </summary>
        /// <returns><see langword="true"/> if the view can be closed.</returns>
        bool CanClose();
    }
}
