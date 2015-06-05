namespace SolutionStartPage.Core.Views
{
    using System.Windows;
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
            View.Loaded += View_Loaded;

            ViewModel = viewModel;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Protected Methods

        protected abstract void View_Loaded(object sender, RoutedEventArgs e);

        #endregion
    }
}