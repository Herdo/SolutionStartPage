namespace SolutionStartPage.Shared.BLL.Provider
{
    using System.ComponentModel;

    public interface IViewStateProvider : INotifyPropertyChanged
    {
        bool EditModeEnabled { get; set; }
        bool DisplayFolders { get; set; }
    }
}