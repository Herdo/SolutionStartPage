namespace SolutionStartPage.Shared.Views
{
    using System.ComponentModel;
    using System.Windows;

    public interface IViewStateProvider : INotifyPropertyChanged
    {
        bool EditModeEnabled { get; set; }

        FontWeight GroupHeaderFontWeight { get; set; }
    }
}