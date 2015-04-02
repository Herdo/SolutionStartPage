namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System;
    using System.Windows.Input;
    using Models;

    public interface ISolutionPageView : IBasicControlSubject
    {
        event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute;
        event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted;
        void ConnectDataSource(ISolutionPageViewModel vm);
        string BrowseBulkAddRootFolder();
        Solution BrowseSolution(SolutionGroup solutionGroup);
    }
}