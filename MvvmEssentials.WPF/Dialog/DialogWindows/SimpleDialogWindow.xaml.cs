using MvvmEssentials.Core.Dialog;
using System.Windows;

namespace MvvmEssentials.WPF.Dialog
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