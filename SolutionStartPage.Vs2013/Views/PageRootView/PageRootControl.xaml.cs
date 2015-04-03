namespace SolutionStartPage.Vs2013.Views.PageRootView
{
    using System.Windows;
    using System.Windows.Controls;
    using Shared.Views.BasicPart;
    using Shared.Views.PageRootView;
    using Shared.Views.SolutionPageView;

    /// <summary>
    /// Interaction logic for PageRootControl.xaml
    /// </summary>
    public partial class PageRootControl : IPageRootView
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly FrameworkElement _vsoPageControl;
        private readonly FrameworkElement _solutionPageControl;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public PageRootControl(IVsoPagePresenter vsoPagePresenter, ISolutionPagePresenter solutionPagePresenter)
        {
            InitializeComponent();

            _vsoPageControl = vsoPagePresenter.View as FrameworkElement;
            _solutionPageControl = solutionPagePresenter.View as FrameworkElement;

            LayoutViewComponents();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void LayoutViewComponents()
        {
            // Add VSO Page Control
            LayoutGrid.Children.Add(_vsoPageControl);

            // Add Solution Page Control
            LayoutGrid.Children.Add(_solutionPageControl);
            Grid.SetColumn(_solutionPageControl, 1);
        }

        #endregion
    }
}
