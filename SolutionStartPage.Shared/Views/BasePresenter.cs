namespace SolutionStartPage.Shared.Views
{
    public abstract class BasePresenter<TView, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly TView _view;
        private readonly TViewModel _viewModel;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        public TView View { get { return _view; } }

        public TViewModel ViewModel { get { return _viewModel; } }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected BasePresenter(TView view, TViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        #endregion
    }
}