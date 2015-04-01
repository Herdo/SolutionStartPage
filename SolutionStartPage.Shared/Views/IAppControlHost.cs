namespace SolutionStartPage.Shared.Views
{
    using System;
    using System.Windows;

    public interface IAppControlHost
    {
        event RoutedEventHandler Loaded;
        event EventHandler<EventArgs> AppRegistrationCompleted; 

        object DataContext { get; set; }
    }
}