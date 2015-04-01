namespace SolutionStartPage.Core.Views.BasicPart
{
    using System;
    using Models;
    using Shared;
    using Shared.Funtionality;
    using Shared.Models;
    using Shared.Views;
    using Shared.Views.BasicPart;

    public class VsoPagePresenter : IVsoPagePresenter
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly IAppControlHost _appControlHost;
        private readonly VisualStudioVersion _vsVersion;
        private readonly IVsoPageView _view;
        private readonly IVsoPageViewModel _vm;

        private IIde _ide;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        private IIde Ide
        {
            get { return _ide ?? (_ide = UnityFactory.ResolveIfRegistered<IIde>(Constants.IIDE_REGISTRATION_NAME)); }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VsoPagePresenter(IAppControlHost appControlHost, VisualStudioVersion vsVersion, IVsoPageView view, IVsoPageViewModel vm)
        {
            _appControlHost = appControlHost;
            _vsVersion = vsVersion;
            _view = view;
            _vm = vm;

            if (Ide == null)
                _appControlHost.AppRegistrationCompleted += appControlHost_AppRegistrationCompleted;
            else
                LoadVmContent();

            _view.ConnectDataSource(_vm);
        }

        #endregion
        
        /////////////////////////////////////////////////////////
        #region Event Handler

        void appControlHost_AppRegistrationCompleted(object sender, EventArgs e)
        {
            _appControlHost.AppRegistrationCompleted -= appControlHost_AppRegistrationCompleted;
            LoadVmContent();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods
        
        private void LoadVmContent()
        {
            _vm.FullEditionName = String.Format("{0} {1}", Ide.Edition, _vsVersion.LongVersion);
        }

        #endregion
    }
}