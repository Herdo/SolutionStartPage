namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Annotations;
    using Models;

    public interface ISolutionPageModel
    {
        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <returns>A <see cref="SolutionPageConfiguration"/> for the current Visual Studio version
        /// or a default configuration, if none is found</returns>
        [NotNull]
        Task<SolutionPageConfiguration> LoadConfiguration();

        /// <summary>
        /// Saves the configuration for the current Visual Studio instance.
        /// </summary>
        /// <param name="configuration">The configuration to save.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="configuration"/> is null.</exception>
        Task SaveConfiguration([NotNull] SolutionPageConfiguration configuration);

        /// <summary>
        /// Gets all files in the <paramref name="directory"/>, matching the <paramref name="pattern"/>.
        /// </summary>
        /// <param name="directory">The directory to search in.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>A collection of matching files.</returns>
        IEnumerable<FileInfo> GetFilesInDirectory(string directory, string pattern);

        /// <summary>
        /// Gets the parent directory of the <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">The directory to get the parent from.</param>
        /// <returns>The parent directory information.</returns>
        DirectoryInfo GetParentDirectory(string directory);

        /// <summary>
        /// Checks if the <paramref name="directory"/> exists.
        /// </summary>
        /// <param name="directory">The directory to check for existence.</param>
        /// <returns>True, if the <paramref name="directory"/> exists, otherwise false.</returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Checks if the <paramref name="file"/> exists.
        /// </summary>
        /// <param name="file">The file to check for existence.</param>
        /// <returns>True, if the <paramref name="file"/> exists, otherwise false.</returns>
        bool FileExists(string file);
    }
}