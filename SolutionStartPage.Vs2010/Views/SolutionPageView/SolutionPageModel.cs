namespace SolutionStartPage.Vs2010.Views.SolutionPageView
{
    using Shared.DAL;
    using Shared.Views.SolutionPageView;

    public class SolutionPageModel : SolutionPageModelBase
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const string _SETTINGS_FILE_NAME_2010 = "VS2010_SolutionStartPage.settings";

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageModel(IFileSystem fileSystem)
            : base(fileSystem, _SETTINGS_FILE_NAME_2010)
        {}

        #endregion
    }
}