namespace SolutionStartPage.Shared.Views.BasicPart
{
    using System.ComponentModel;

    public abstract class VsoPageViewModelBase : IVsoPageViewModel
    {
        /////////////////////////////////////////////////////////
        #region IVsoPageViewModel Member

        public string StartPageHeaderTitle
        {
            get { return "Visual Studio"; }
            set { /* Ignore */ }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}