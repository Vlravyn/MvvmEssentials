using MvvmEssentials.Core;
using MvvmEssentials.Core.Dialog;
using System;

namespace MvvmEssentials.Navigation.WPF.Dialog.DialogWindows
{
    internal class DefaultDialogHostViewModel : ObservableObject, IDialogAware
    {
        #region Private Members

        private object? _title;
        private DialogResult _dialogResult;
        private object? _content;

        #endregion Private Members

        #region Public Properties

        public object? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DialogResult DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        public object? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        public Action Close { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Default Constructor for design time.
        /// DO NOT USE.
        /// </summary>
        public DefaultDialogHostViewModel()
        {
        }

        public DefaultDialogHostViewModel(object? title, object? content)
        {
            Title = title;
            Content = content;
        }

        #endregion Constructors

        public IDialogParameters ResultParameters()
        {
            return new DialogParameters();
        }

        public void OnClosing()
        {
        }

        public bool CanClose()
        {
            return true;
        }
    }
}