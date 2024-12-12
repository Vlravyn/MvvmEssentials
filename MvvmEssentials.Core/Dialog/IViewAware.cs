using MvvmEssentials.Core.Common;

namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// Makes the view model aware of view being opened and closed.
    /// Also allows accepting parameters when the view is opened.\
    /// Use this on the view model of the view that is not a dialog.
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Runs when the view is opened.
        /// </summary>
        /// <param name="parameters">the paramters the this view.</param>
        void OnOpened(IParameters? parameters);

        /// <summary>
        /// Runs when the view is closing.
        /// </summary>
        void OnClosing();
    }
}