namespace SolutionStartPage.Core.Views.BasicPart
{
    using System.Windows;
    using Annotations;
    using Shared.Models;
    using Shared.Views.BasicPart;
    using static Shared.Utilities;

    public class VsoPagePresenter : BasePresenter<IVsoPageView, IVsoPageViewModel>,
        IVsoPagePresenter
    {
        /////////////////////////////////////////////////////////

        #region Fields

        private readonly IVisualStudioVersion _vsVersion;
        private readonly IIde _ide;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VsoPagePresenter"/> class.
        /// </summary>
        /// <param name="vsVersion">The version of the current Visual Studio instance.</param>
        /// <param name="ide">Information about the current Visual Studio instance.</param>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="System.ArgumentNullException">Any parameter is null.</exception>
        public VsoPagePresenter([NotNull] IVisualStudioVersion vsVersion,
            [NotNull] IIde ide,
            [NotNull] IVsoPageView view,
            [NotNull] IVsoPageViewModel viewModel)
            : base(view, viewModel)
        {
            ThrowIfNull(vsVersion, nameof(vsVersion));
            ThrowIfNull(ide, nameof(ide));

            _vsVersion = vsVersion;
            _ide = ide;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Base Overrides

        protected override void View_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVmContent();
            View.ConnectDataSource(ViewModel);
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private void LoadVmContent()
        {
            ViewModel.StartPageHeaderTitle = $"{_ide.Edition} {_vsVersion.LongVersion}";
        }

        #endregion
    }
}