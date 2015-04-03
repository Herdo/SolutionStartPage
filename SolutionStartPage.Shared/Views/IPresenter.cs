namespace SolutionStartPage.Shared.Views
{
    public interface IPresenter<out TView, out TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        TView View { get; }
        TViewModel ViewModel { get; }
    }
}