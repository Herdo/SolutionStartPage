namespace SolutionStartPage.Vs2017.Views.SolutionPageView
{
    using Shared.DAL;
    using Shared.Views.SolutionPageView;

    public class SolutionPageModel : SolutionPageModelBase
    {
        /////////////////////////////////////////////////////////

        #region Constants

        private const string _SETTINGS_FILE_NAME_2017 = "VS2017_SolutionStartPage.settings";

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        public SolutionPageModel(IFileSystem fileSystem)
            : base(fileSystem, _SETTINGS_FILE_NAME_2017)
        {
        }

        #endregion
    }
}