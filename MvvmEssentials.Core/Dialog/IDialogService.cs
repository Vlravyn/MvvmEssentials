namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// Allows opening dialogs with custom content.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Shows a new view independently
        /// </summary>
        /// <param name="viewType">the type of view to show</param>
        /// <param name="parameters">the optional parameters to pass in to the new view</param>
        void Show(Type viewType, IParameters? parameters = null);

        /// <summary>
        /// Shows the dialog with the content that is specified to the <paramref name="dialogContentType"/> with the attribute <see cref="NavigateToAttribute"/>
        /// </summary>
        /// <typeparam name="T">The enum type must have set <see cref="IsDialogContentEnumAttribute"/> to it</typeparam>
        /// <param name="dialogContentType">the enum value that corressponds to the dialog content. Must have <see cref="NavigateToAttribute"/> attached to the value</param>
        /// <param name="parameters">the parameters for this dialog</param>
        /// <param name="callbackMethod">the method to run after dialog closes</param>
        /// <returns>the dialog result.</returns>
        DialogResult ShowDialog<T>(T dialogContentType, IDialogParameters parameters, Action<IDialogParameters?> callbackMethod)
            where T : Enum;

        /// <summary>
        /// Shows a custom window with its content as a dialog
        /// </summary>
        /// <param name="customView">the view to open</param>
        /// <param name="parameters">the parameters for this dialog</param>
        /// <param name="callbackMethod">the callback method to run after </param>
        /// <returns>the dialog  result</returns>
        DialogResult ShowDialog(Type customView, IDialogParameters parameters, Action<IDialogParameters?> callbackMethod);

        /// <summary>
        /// Shows a simple dialog with text and two buttons.
        /// </summary>
        /// <param name="title">the title of the dialog</param>
        /// <param name="content">the text to show</param>
        /// <param name="button1Content">the text for the first button</param>
        /// <param name="button2Content">the text for the second button</param>
        /// <returns><see langword="true"/> if Button 1 was clicked, <see langword="false"/> if button 2 was clicked</returns>
        bool ShowSimpleDialog(string title, string content, string button1Content, string button2Content);
    }
}