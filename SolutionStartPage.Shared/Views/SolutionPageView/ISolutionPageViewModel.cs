namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Collections.ObjectModel;
    using Models;

    public interface ISolutionPageViewModel : IViewModel
    {
        bool EditModeEnabled { get; set; }

        int Columns { get; set; }

        ObservableCollection<SolutionGroup> SolutionGroups { get; set; }
    }
}