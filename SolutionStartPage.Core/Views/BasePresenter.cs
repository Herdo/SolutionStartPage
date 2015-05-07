namespace SolutionStartPage.Core.Views
{
    using Shared.Views;

    public abstract class BasePresenter<TView, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        /////////////////////////////////////////////////////////
        #region Properties

        public TView View { get; }

        public TViewModel ViewModel { get; }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected BasePresenter(TView view, TViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }

        #endregion
    }
}