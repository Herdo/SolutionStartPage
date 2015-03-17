namespace SolutionStartPage.Shared.Views
{
    using System.Windows;

    public interface IAppControlHost
    {
        event RoutedEventHandler Loaded;

        object DataContext { get; set; }
    }
}