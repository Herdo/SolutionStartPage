namespace SolutionStartPage.Proxies.Views
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.Unity;
    using Shared.Funtionality;
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

        public event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute
        {
            add { RealSubject.AlterPageCanExecute += value; }
            remove { RealSubject.AlterPageCanExecute -= value; }
        }

        public event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted
        {
            add { RealSubject.AlterPageExecuted += value; }
            remove { RealSubject.AlterPageExecuted -= value; }
        }

        public void ConnectDataSource(ISolutionPageViewModel vm)
        {
            RealSubject.ConnectDataSource(vm);
        }

        #endregion
    }
}