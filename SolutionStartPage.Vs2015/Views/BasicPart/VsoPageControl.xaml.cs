namespace SolutionStartPage.Vs2015.Views.BasicPart
{
    using Shared.Views.BasicPart;

    /// <summary>
    /// Interaction logic for VsoPageControl.xaml
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
        #region IVsoPageView Member

        public void ConnectDataSource(IVsoPageViewModel vm)
        {
            VisualStudioEditionTextBlock.DataContext = vm;
        }

        #endregion
    }
}
