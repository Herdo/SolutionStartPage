namespace SolutionStartPage.Shared.Views
{
    public interface IView<in TViewModel> where TViewModel : IViewModel
    {
        void ConnectDataSource(TViewModel vm);
    }
}