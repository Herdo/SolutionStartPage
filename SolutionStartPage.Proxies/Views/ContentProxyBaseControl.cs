namespace SolutionStartPage.Proxies.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Shared.Funtionality;
    using Shared.Views;

    /// <summary>
    /// Basic proxy implementation.
    /// </summary>
    /// <typeparam name="TSubject">The common subject type for the proxy and the actual implementation.</typeparam>
    public abstract class ContentProxyBaseControl<TSubject> : UserControl
        where TSubject : IBasicControlSubject
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly TSubject _realSubject;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        protected TSubject RealSubject
        {
            get { return _realSubject; }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected ContentProxyBaseControl()
        {
            _realSubject = UnityFactory.Resolve<TSubject>();
            Content = _realSubject;
        }

        #endregion
    }
}
