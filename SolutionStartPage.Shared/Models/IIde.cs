namespace SolutionStartPage.Shared.Models
{
    public interface IIde
    {
        /// <summary>
        /// Gets the full edition name of the Visual Studio instance.
        /// </summary>
        string Edition { get; }

        /// <summary>
        /// Sets the object to access the IDE interface.
        /// </summary>
        object IdeAccess { set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets the <c>Language Code ID</c> of the current IDE language.
        /// </summary>
        int LCID { get; }

        /// <summary>
        /// Opens a solution.
        /// </summary>
        /// <param name="path">The path to the solution to open.</param>
        void OpenSolution(string path);
    }
}