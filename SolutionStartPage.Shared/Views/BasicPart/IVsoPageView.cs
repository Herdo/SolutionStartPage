namespace SolutionStartPage.Shared.Views.BasicPart
{
    public interface IVsoPageView : IBasicControlSubject
    {
        void ConnectDataSource(IVsoPageViewModel vm);
    }
}