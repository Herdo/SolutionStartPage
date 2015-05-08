namespace SolutionStartPage.Core.Views
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using BasicPart;
    using BLL.Provider;
    using DAL;
    using Microsoft.Practices.Unity;
    using Models;
    using Shared;
    using Shared.BLL.Provider;
    using Shared.DAL;
    using Shared.Models;
    using Shared.Views;
    using Shared.Views.BasicPart;
    using Shared.Views.PageRootView;
    using Shared.Views.SolutionPageView;
    using SolutionPageView;
    using Vs2010;
    using Vs2013;
    using Vs2015;

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
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void Initialize()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                // Always register self, before doing anything else
                container.RegisterInstance<IAppControlHost>(this);
                PreConfigureApp(container);
                ConfigureApp(container);
                ConfigureSelf(container);
                PrepareUi(container);

                Content = container.Resolve<IPageRootView>();
            }
        }

        private void PrepareUi(IUnityContainer container)
        {
            var resourceProvider = container.Resolve<IResourceProvider>();
            var ideAccess = container.Resolve<IIde>();
            var resources = new Dictionary<object, object>
            {
                {"Texts", resourceProvider}
            };

            RegisterGlobalResources(resourceProvider, ideAccess, resources);
        }

        private void RegisterGlobalResources(IResourceProvider resourceProvider, IIde ideAccess, Dictionary<object, object> resources)
        {
            resourceProvider.Culture = new CultureInfo(ideAccess.LCID);

            foreach (var resource in resources.Where(resource => !Application.Current.Resources.Contains(resource.Key)))
                Application.Current.Resources.Add(resource.Key, resource.Value);
        }

        private void ConfigureSelf(IUnityContainer container)
        {
            var ideModel = container.Resolve<IIdeModel>();
            var ide = ideModel.GetIde(DataContext, container.Resolve<Func<IIde>>());
            container.RegisterInstance(ide, new ContainerControlledLifetimeManager());
        }

        private void PreConfigureApp(IUnityContainer container)
        {
            container
                // Register Presenters
                .RegisterType<ISolutionPagePresenter, SolutionPagePresenter>()
                .RegisterType<IVsoPagePresenter, VsoPagePresenter>()
                // Register Models
                .RegisterType<Solution>()
                .RegisterType<SolutionGroup>()
                // Determine Singletons
                .RegisterType<VisualStudioVersion>(new ContainerControlledLifetimeManager())
                .RegisterType<IViewStateProvider, ViewStateProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IResourceProvider, MainResourceProvider>(new ContainerControlledLifetimeManager())
                // Register DAL
                .RegisterType<IFileSystem, FileSystem>(new ContainerControlledLifetimeManager())
                // Register Bootstrappers for each VS Version
                .RegisterType<IBootstrapper, Vs2010Bootstrapper>(new Version(10, 0).ToString(_VERSION_STRING_DETAIL_SPECIFIC))
                .RegisterType<IBootstrapper, Vs2013Bootstrapper>(new Version(12, 0).ToString(_VERSION_STRING_DETAIL_SPECIFIC))
                .RegisterType<IBootstrapper, Vs2015Bootstrapper>(new Version(14, 0).ToString(_VERSION_STRING_DETAIL_SPECIFIC))
            ;
        }

        private void ConfigureApp(IUnityContainer container)
        {
            var version = container.Resolve<VisualStudioVersion>();
            IBootstrapper bootstrapper;
            try
            {
                bootstrapper = container.Resolve<IBootstrapper>(version.FullVersion.ToString(_VERSION_STRING_DETAIL_SPECIFIC));
            }
            catch (ResolutionFailedException)
            {
                throw new NotSupportedException($"Visual Studio Version {version.FullVersion} is not supported by this extension!");
            }
            
            bootstrapper.Configure(container);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        void AppControlHost_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }
        
        #endregion
    }
}