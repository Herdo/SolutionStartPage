namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Threading.Tasks;
    using Models;

    public interface ISolutionPageModel
    {
        SolutionPageConfiguration LoadConfiguration();

        Task SaveConfiguration(SolutionPageConfiguration groups);
    }
}