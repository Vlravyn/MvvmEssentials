using MvvmEssentials.Core.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace MvvmEssentials.WPF.Navigation
{
    /// <summary>
    /// The property to register the name of the frame.
    /// The application will look for frames with this attached property to store and use for navigation.
    /// </summary>
    public class NavigationNamesAP : Frame, INavigationNamesAP
    {
        /// <summary>
        /// The name of the frame for navigation
        /// </summary>
        public string NavigationName
        {
            get => (string)GetValue(NavigationNameProperty);
            set => SetValue(NavigationNameProperty, value);
        }

        public static readonly DependencyProperty NavigationNameProperty = DependencyProperty.RegisterAttached(nameof(NavigationName), typeof(string), typeof(Frame), new PropertyMetadata());

        public static void SetNavigationName(DependencyObject d, string value) => d.SetValue(NavigationNameProperty, value);

        public static string GetNavigationName(DependencyObject d) => (string)d.GetValue(NavigationNameProperty);
    }
}