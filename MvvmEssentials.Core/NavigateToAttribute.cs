namespace MvvmEssentials.Core
{
    /// <summary>
    /// Use on the enum values to set which enum refers to which content
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NavigateToAttribute : Attribute
    {
        /// <summary>
        /// The type of content to navigate to
        /// </summary>
        public Type DestinationType { get; set; }
    }
}