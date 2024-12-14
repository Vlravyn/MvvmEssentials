using MvvmEssentials.Core.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace MvvmEssentials.Navigation.WPF.Navigation
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

        /// <summary>
        /// Dependency property for <see cref="NavigationName"/>
        /// </summary>
        public static readonly DependencyProperty NavigationNameProperty = DependencyProperty.RegisterAttached(nameof(NavigationName), typeof(string), typeof(Frame), new PropertyMetadata());

        /// <summary>
        /// Setter method for dependency property <see cref="NavigationNameProperty"/>
        /// </summary>
        /// <param name="d">the frame whose attached property <see cref="NavigationNameProperty"/> value to set.</param>
        /// <param name="value">the value to set to.</param>
        public static void SetNavigationName(DependencyObject d, string value) => d.SetValue(NavigationNameProperty, value);

        /// <summary>
        /// Getter method for dependency property <see cref="NavigationNameProperty"/>
        /// </summary>
        /// <param name="d">the frame whose attached property <see cref="NavigationNameProperty"/> value to get.</param>
        public static string GetNavigationName(DependencyObject d) => (string)d.GetValue(NavigationNameProperty);
    }
}