namespace SolutionStartPage.Shared.Views.BasicPart
{
    using System.ComponentModel;

    public interface IVsoPageViewModel : INotifyPropertyChanged
    {
        string FullEditionName { get; set; }
    }
}