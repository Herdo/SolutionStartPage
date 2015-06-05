namespace SolutionStartPage.Shared.Views
{
    using System.Windows;

    public interface IView<in TViewModel> where TViewModel : IViewModel
    {
        event RoutedEventHandler Loaded;

        void ConnectDataSource(TViewModel vm);
    }
}