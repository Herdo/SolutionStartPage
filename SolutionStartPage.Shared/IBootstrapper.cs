namespace SolutionStartPage.Shared
{
    using Unity;

    public interface IBootstrapper
    {
        /// <summary>
        /// Configures the unity container for the specific bootstrapper case.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        void Configure(IUnityContainer container);
    }
}