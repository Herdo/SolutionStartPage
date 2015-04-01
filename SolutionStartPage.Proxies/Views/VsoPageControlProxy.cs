namespace SolutionStartPage.Proxies.Views
{
    using Microsoft.Practices.Unity;
    using Shared.Funtionality;
    using Shared.Views.BasicPart;

    public class VsoPageControlProxy : ContentProxyBaseControl<IVsoPageView>,
                                       IVsoPageView
    {
        /////////////////////////////////////////////////////////
        #region Constructors

        public VsoPageControlProxy()
        {
            // Initialize presenter
            UnityFactory.Resolve<IVsoPagePresenter>(new ParameterOverride("view", this));
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IVsoPageView Member

        public void ConnectDataSource(IVsoPageViewModel vm)
        {
            RealSubject.ConnectDataSource(vm);
        }

        #endregion
    }
}