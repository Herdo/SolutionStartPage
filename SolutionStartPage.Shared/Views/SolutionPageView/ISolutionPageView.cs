namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System;
    using System.Windows.Input;
    using Models;

    public interface ISolutionPageView : IView<ISolutionPageViewModel>,
        IBasicControlSubject
    {
        event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute;
        event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted;
        string BrowseBulkAddRootFolder();
        string BrowseSolution(SolutionGroup solutionGroup);
    }
}