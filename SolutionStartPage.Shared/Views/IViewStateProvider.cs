namespace SolutionStartPage.Shared.Views
{
    using System.ComponentModel;

    public interface IViewStateProvider : INotifyPropertyChanged
    {
        bool EditModeEnabled { get; set; } 
    }
}