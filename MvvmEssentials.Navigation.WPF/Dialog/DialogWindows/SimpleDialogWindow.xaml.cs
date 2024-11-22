using MvvmEssentials.Core.Dialog;
using MvvmEssentials.Navigation.WPF.Dialog.DialogWindows;
using System.Windows;

namespace MvvmEssentials.Navigation.WPF.Dialog
{
    /// <summary>
    /// Interaction logic for SimpleDialogWindow.xaml
    /// </summary>
    public partial class SimpleDialogWindow : Window
    {
        public SimpleDialogWindow(IDialogParameters parameters)
        {
            InitializeComponent();
            DataContext = new SimpleDialogViewModel(parameters);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}