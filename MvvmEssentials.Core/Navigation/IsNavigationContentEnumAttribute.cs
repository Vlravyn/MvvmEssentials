using System;

namespace MvvmEssentials.Core.Navigation
{
    /// <summary>
    /// Specifies that the enum is a navigation content enum that can be used by <see cref="INavigationService.Navigate{T}"/>
    /// to specify the content to navigate to
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class IsNavigationContentEnumAttribute : Attribute
    {
    }
}