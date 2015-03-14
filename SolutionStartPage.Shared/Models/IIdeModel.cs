namespace SolutionStartPage.Shared.Models
{
    public interface IIdeModel
    {
        /// <summary>
        /// Gets the VS Version specific <see cref="IIde"/> object.
        /// </summary>
        /// <param name="dataContext">The datacontext required to get the <see cref="IIde"/>.</param>
        /// <returns>The Ide.</returns>
        IIde GetIde(object dataContext);
    }
}