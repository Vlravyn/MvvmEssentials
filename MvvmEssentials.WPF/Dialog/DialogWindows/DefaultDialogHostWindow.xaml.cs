using System.Windows;

namespace MvvmEssentials.WPF.Dialog
{
    /// <summary>
    /// Interaction logic for DefaultDialogHostWindow.xaml
    /// </summary>
    internal partial class DefaultDialogHostWindow : Window
    {
        internal DefaultDialogHostWindow(object? title, object? content)
        {
            InitializeComponent();
            DataContext = new DefaultDialogHostViewModel(title, content);
        }
    }
}