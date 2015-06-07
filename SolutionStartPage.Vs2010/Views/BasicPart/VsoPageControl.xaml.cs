namespace SolutionStartPage.Vs2010.Views.BasicPart
{
    using Shared.Views.BasicPart;

    /// <summary>
    /// Interaction logic for VisualStudioOverviewPageControl.xaml
    /// </summary>
    public partial class VsoPageControl : IVsoPageView
    {
        /////////////////////////////////////////////////////////

        #region Constructors

        public VsoPageControl()
        {
            InitializeComponent();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region IVsoPageView Members

        public void ConnectDataSource(IVsoPageViewModel vm)
        {
            // Ignore in VS 2010
        }

        #endregion
    }
}