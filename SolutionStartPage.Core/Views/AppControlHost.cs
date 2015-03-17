namespace SolutionStartPage.Core.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Practices.Unity;
    using Models;
    using Shared;
    using Shared.Funtionality;
    using Shared.Models;
    using Shared.Views;
    using Shared.Views.PageRootView;
    using Shared.Views.SolutionPageView;
    using SolutionPageView;
    using Vs2010;
    using Vs2013;

    /// <summary>
    /// Basic app control host.
    /// Takes care about dependency injection and basic business logic.
    /// </summary>
    public class AppControlHost : UserControl,
                                  IAppControlHost
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const int _VERSION_STRING_DETAIL_SPECIFIC = 2;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public AppControlHost()
        {
            Loaded += AppControlHost_Loaded;

            PreConfigureApp();
            ConfigureApp();
            Content = UnityFactory.Resolve<IPageRootView>();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods
        
        private void PreConfigureApp()
        {
            UnityFactory.Initialize(c => c
                // Register Presenters
                .RegisterType<ISolutionPagePresenter, SolutionPagePresenter>()
                // Register Models
                .RegisterType<Solution>()
                .RegisterType<SolutionGroup>()
                // Determine Singletons
                .RegisterType<VisualStudioVersion>(new ContainerControlledLifetimeManager())
                .RegisterType<IViewStateProvider, ViewStateProvider>(new ContainerControlledLifetimeManager())
                // Register Bootstrappers for each VS Version
                .RegisterType<IBootstrapper, Vs2010Bootstrapper>(new Version(10, 0).ToString(_VERSION_STRING_DETAIL_SPECIFIC))
                .RegisterType<IBootstrapper, Vs2013Bootstrapper>(new Version(12, 0).ToString(_VERSION_STRING_DETAIL_SPECIFIC))
                );
        }

        private void ConfigureApp()
        {
            var version = UnityFactory.Resolve<VisualStudioVersion>();
            var bootstrapper =
                UnityFactory.ResolveIfRegistered<IBootstrapper>(
                    version.FullVersion.ToString(_VERSION_STRING_DETAIL_SPECIFIC));

            if (bootstrapper == null)
                throw new NotSupportedException(
                    String.Format("Visual Studio Version {0} is not supported by this extension!", version.FullVersion));

            bootstrapper.Configure();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        void AppControlHost_Loaded(object sender, RoutedEventArgs e)
        {
            if (UnityFactory.IsRegistered<IIde>(Constants.IIDE_REGISTRATION_NAME)) return;

            var ideModel = UnityFactory.Resolve<IIdeModel>();
            var ide = ideModel.GetIde(DataContext);
            UnityFactory.RegisterInstance(ide, Constants.IIDE_REGISTRATION_NAME, new ContainerControlledLifetimeManager());
        }
        
        #endregion
    }
}