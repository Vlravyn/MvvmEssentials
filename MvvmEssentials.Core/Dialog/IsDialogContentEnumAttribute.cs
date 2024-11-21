using System;

namespace MvvmEssentials.Core.Dialog
{
    /// <summary>
    /// Tells the dialog service that this enum is contains values with have <see cref="NavigateToAttribute"/> value for setting content for the dialog
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public class IsDialogContentEnumAttribute : Attribute
    {
    }
}