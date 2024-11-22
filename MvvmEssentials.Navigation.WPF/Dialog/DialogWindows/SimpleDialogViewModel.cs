using MvvmEssentials.Core;
using MvvmEssentials.Core.Commands;
using MvvmEssentials.Core.Dialog;
using System;
using System.Linq;
using System.Windows;

namespace MvvmEssentials.Navigation.WPF.Dialog.DialogWindows
{
    internal class SimpleDialogViewModel : ObservableObject, IDialogAware
    {
        private string _button1Content;
        private string _button2Content;
        private string _text;
        public object? Title { get; set; }
        public DialogResult DialogResult { get; set; }

        public object? Content { get; set; }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string Button1Content
        {
            get => _button1Content;
            set => SetProperty(ref _button1Content, value);
        }

        public string Button2Content
        {
            get => _button2Content;
            set => SetProperty(ref _button2Content, value);
        }

        public RelayCommand<Window> Button1Command => new RelayCommand<Window>((window) => ButtonClicked(DialogResult.Yes, window));
        public RelayCommand<Window> Button2Command => new RelayCommand<Window>((window) => ButtonClicked(DialogResult.No, window));

        private void ButtonClicked(DialogResult result, Window view)
        {
            DialogResult = result;
            view.Close();
        }

        public SimpleDialogViewModel(IDialogParameters parameters)
        {
            Title = parameters.FirstOrDefault(t => string.Equals(t.Key, "title", StringComparison.OrdinalIgnoreCase));
            Text = parameters
                    .FirstOrDefault(t => string.Equals(t.Key, "text", StringComparison.OrdinalIgnoreCase))
                    .Value
                    .ToString() ?? string.Empty;
            Button1Content = parameters
                                .FirstOrDefault(t => string.Equals(t.Key, "button1Content", StringComparison.OrdinalIgnoreCase))
                                .Value
                                .ToString() ?? string.Empty;
            Button2Content = parameters
                                .FirstOrDefault(t => string.Equals(t.Key, "button2Content", StringComparison.OrdinalIgnoreCase))
                                .Value
                                .ToString() ?? string.Empty;
        }

        public IDialogParameters? CreateDialogParameters()
        {
            return null;
        }

        public void OnDialogClosing()
        {
        }
    }
}