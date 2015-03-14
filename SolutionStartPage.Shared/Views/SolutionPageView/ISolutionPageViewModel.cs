namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Models;

    public interface ISolutionPageViewModel : INotifyPropertyChanged
    {
        bool EditModeEnabled { get; set; }

        int Columns { get; set; }

        ObservableCollection<SolutionGroup> SolutionGroups { get; set; }

    }
}