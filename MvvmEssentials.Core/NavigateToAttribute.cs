using System;

namespace MvvmEssentials.Core
{
    /// <summary>
    /// Use on the enum values to set which enum refers to which content
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NavigateToAttribute : Attribute
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        /// <summary>
        /// The type of content to navigate to
        /// </summary>
        public Type DestinationType { get; set; }
    }
}