namespace SolutionStartPage.Vs2015.Views.SolutionPageView
{
    using Shared.DAL;
    using Shared.Views.SolutionPageView;

    public class SolutionPageModel : SolutionPageModelBase
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const string _SETTINGS_FILE_NAME_2015 = "VS2015_SolutionStartPage.settings";

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageModel(IFileSystem fileSystem)
            : base(fileSystem, _SETTINGS_FILE_NAME_2015)
        {}

        #endregion
    }
}