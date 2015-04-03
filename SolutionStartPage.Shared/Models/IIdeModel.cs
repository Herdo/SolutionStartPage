namespace SolutionStartPage.Shared.Models
{
    using System;

    public interface IIdeModel
    {
        /// <summary>
        /// Gets the VS Version specific <see cref="IIde"/> object.
        /// </summary>
        /// <param name="dataContext">The datacontext required to get the <see cref="IIde"/>.</param>
        /// <param name="ideResolver">The function to resolve the ide.</param>
        /// <returns>The Ide.</returns>
        IIde GetIde(object dataContext, Func<IIde> ideResolver);
    }
}