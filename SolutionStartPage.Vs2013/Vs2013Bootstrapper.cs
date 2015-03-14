namespace SolutionStartPage.Vs2013
{
    using Control.SolutionsPart;
    using Microsoft.Practices.Unity;
    using Models;
    using Shared;
    using Shared.Funtionality;
    using Shared.Models;
    using Shared.Views.BasicPart;
    using Shared.Views.PageRootView;
    using Shared.Views.SolutionPageView;
    using Views.BasicPart;
    using Views.PageRootView;
    using Views.SolutionPageView;

    public class Vs2013Bootstrapper : IBootstrapper
    {
        public void Configure()
        {
            UnityFactory.Configure(c => c
                // Register Models
                .RegisterType<IIdeModel, IdeModel>(new ContainerControlledLifetimeManager())
                .RegisterType<IIde, VsIde>(new ContainerControlledLifetimeManager())
                
                // Register Views & their components
                // Root View
                .RegisterType<IPageRootView, PageRootControl>()
                // Basic Part
                .RegisterType<IVisualStudioOverviewPageView, VisualStudioOverviewPageControl>()
                // Solution Page
                .RegisterType<ISolutionPageView, SolutionPageControl>()
                .RegisterType<ISolutionGroupControl, SolutionGroupControl>()
                .RegisterType<ISolutionControl, SolutionControl>()
                .RegisterType<ISolutionPageModel, SolutionPageModel>()
                .RegisterType<ISolutionPageViewModel, SolutionPageViewModel>()
                );
        }
    }
}