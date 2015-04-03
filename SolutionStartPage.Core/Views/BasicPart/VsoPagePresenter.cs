namespace SolutionStartPage.Core.Views.BasicPart
{
    using System;
    using Models;
    using Shared.Models;
    using Shared.Views;
    using Shared.Views.BasicPart;

    public class VsoPagePresenter : BasePresenter<IVsoPageView, IVsoPageViewModel>,
                                    IVsoPagePresenter
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly VisualStudioVersion _vsVersion;
        private readonly IIde _ide;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VsoPagePresenter(VisualStudioVersion vsVersion, IIde ide, IVsoPageView view, IVsoPageViewModel viewModel)
            : base(view, viewModel)
        {
            _vsVersion = vsVersion;
            _ide = ide;

            LoadVmContent();

            View.ConnectDataSource(ViewModel);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods
        
        private void LoadVmContent()
        {
            ViewModel.FullEditionName = String.Format("{0} {1}", _ide.Edition, _vsVersion.LongVersion);
        }

        #endregion
    }
}