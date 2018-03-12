namespace SolutionStartPage.Core.Views
{
    using System.Threading.Tasks;
    using System.Windows;
    using Annotations;
    using Shared.Views;
    using static Shared.Utilities;

    public abstract class BasePresenter<TView, TViewModel> : IPresenter<TView, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        /////////////////////////////////////////////////////////
        #region Properties

        /// <summary>
        /// Gets the view.
        /// </summary>
        public TView View { get; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        public TViewModel ViewModel { get; }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePresenter{TView,TViewModel}"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="System.ArgumentNullException">Any parameter is null.</exception>
        protected BasePresenter([NotNull] TView view,
            [NotNull] TViewModel viewModel)
        {
            ThrowIfNull(view, nameof(view));
            ThrowIfNull(viewModel, nameof(viewModel));

            View = view;
            View.Loaded += async (sender, e) => await View_Loaded(sender, e);

            ViewModel = viewModel;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Protected Methods

        protected abstract Task View_Loaded(object sender, RoutedEventArgs e);

        #endregion
    }
}