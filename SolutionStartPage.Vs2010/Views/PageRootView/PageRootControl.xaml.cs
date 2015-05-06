namespace SolutionStartPage.Vs2010.Views.PageRootView
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using Shared.Models;
    using Shared.Views;
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
            Grid.SetRow(_vsoPageControl, 1);
            Grid.SetColumn(_vsoPageControl, 0);
            _vsoPageControl.Margin = new Thickness(15, -35, 15, 15);
            _vsoPageControl.VerticalAlignment = VerticalAlignment.Stretch;
            
            // Add Solution Page Control
            LayoutGrid.Children.Add(_solutionPageControl);
            Grid.SetRow(_solutionPageControl, 1);
            Grid.SetColumn(_solutionPageControl, 2);
            _solutionPageControl.Margin = new Thickness(0, -35, 15, 15);
        }

        #endregion
    }
}
