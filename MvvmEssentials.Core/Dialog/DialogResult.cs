namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// The results that the dialog can have.
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// Represents that there was no result for the dialog.
        /// </summary>
        None,

        /// <summary>
        /// Represents that the user has decided to cancel the task of this dialog.
        /// </summary>
        Cancel,

        /// <summary>
        /// Represents that the dialog did it's work and completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Represents that the user has selected OK.
        /// </summary>
        OK,


        /// <summary>
        /// Represents that the user has selected Ignore.
        /// </summary>
        Ignore,

        /// <summary>
        /// Represents that the user has decided to retry.
        /// </summary>
        Retry,


        /// <summary>
        /// Represents that the user agrees to the dialog.
        /// </summary>
        Yes,

        /// <summary>
        /// Represents that the user has refused to agree whatever the dialog was showing.
        /// </summary>
        No,

        /// <summary>
        /// Represents that the user wants to continue doing what they were doing.
        /// </summary>
        Continue
    }
}