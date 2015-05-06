namespace SolutionStartPage.Shared.Views
{
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Interface for resource providers.
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Gets or sets the current culture used for resource requests.
        /// </summary>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets the localized string associated with the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the resource to get.</param>
        /// <exception cref="ResourceReferenceKeyNotFoundException">The given <paramref name="key"/> is not found in the resources.</exception>
        /// <returns>The associated resource or null, if it's not found.</returns>
        string this[string key] { get; }
    }
}