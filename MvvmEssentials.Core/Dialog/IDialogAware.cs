using MvvmEssentials.Core.Common;

namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// Specifies the required properties and method for the view model of a dialog.
    /// </summary>
    public interface IDialogAware : IClosable
    {
        /// <summary>
        /// The title for this dialog
        /// </summary>
        object? Title { get; }

        /// <summary>
        /// The result of this dialog
        /// </summary>
        DialogResult DialogResult { get; set; }

        /// <summary>
        /// Creates the parameters that the dialog will send back to the caller
        /// </summary>
        /// <returns>The parameters to send back to the viewmodel which opened the dialog</returns>
        IDialogParameters? ResultParameters();

        /// <summary>
        /// Runs when the dialog is opened.
        /// </summary>
        /// <param name="parameters">the parameters</param>
        void OnOpened(IDialogParameters? parameters);
    }
}