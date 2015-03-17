namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System;
    using System.Windows.Input;

    public interface ISolutionPageView : IBasicControlSubject
    {
        event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute;
        event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted;
        void ConnectDataSource(ISolutionPageViewModel vm);
    }
}