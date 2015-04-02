namespace SolutionStartPage.Proxies.Views
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.Unity;
    using Shared.Funtionality;
    using Shared.Models;
    using Shared.Views.SolutionPageView;

    public class SolutionPageControlProxy : ContentProxyBaseControl<ISolutionPageView>,
                                            ISolutionPageView
    {
        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageControlProxy()
        {
            // Initialize presenter
            UnityFactory.Resolve<ISolutionPagePresenter>(new ParameterOverride("view", this));
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region ISolutionPageView Member

        event EventHandler<CanExecuteRoutedEventArgs> ISolutionPageView.AlterPageCanExecute
        {
            add { RealSubject.AlterPageCanExecute += value; }
            remove { RealSubject.AlterPageCanExecute -= value; }
        }

        event EventHandler<ExecutedRoutedEventArgs> ISolutionPageView.AlterPageExecuted
        {
            add { RealSubject.AlterPageExecuted += value; }
            remove { RealSubject.AlterPageExecuted -= value; }
        }

        void ISolutionPageView.ConnectDataSource(ISolutionPageViewModel vm)
        {
            RealSubject.ConnectDataSource(vm);
        }

        string ISolutionPageView.BrowseBulkAddRootFolder()
        {
            return RealSubject.BrowseBulkAddRootFolder();
        }

        Solution ISolutionPageView.BrowseSolution(SolutionGroup solutionGroup)
        {
            return RealSubject.BrowseSolution(solutionGroup);
        }

        #endregion
    }
}