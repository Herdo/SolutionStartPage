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

        /// <summary>
        /// As the proxy is hosted, and not the RealSubject, the DataContext
        /// of the proxy should be used to resolve the DTE.
        /// </summary>
        public object Context
        {
            get { return DataContext; }
            set { DataContext = value; }
        }

        public void ConnectDataSource(ISolutionPageViewModel vm)
        {
            RealSubject.ConnectDataSource(vm);
        }

        #endregion
    }
}