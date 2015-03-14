namespace SolutionStartPage.Vs2013.Views.SolutionPageView
{
    using Shared.Views.SolutionPageView;

    public class SolutionPageModel : SolutionPageModelBase
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const string _SETTINGS_FILE_NAME_2013 = "VS2013_SolutionStartPage.settings";

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageModel()
            : base(_SETTINGS_FILE_NAME_2013)
        {}

        #endregion
    }
}