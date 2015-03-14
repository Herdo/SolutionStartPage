namespace SolutionStartPage.Shared
{
    public interface IBootstrapper
    {
        /// <summary>
        /// Configures the unity container for the specific bootstrapper case.
        /// </summary>
        void Configure();
    }
}