namespace SolutionStartPage.Shared.Views.BasicPart
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Extensions;

    public abstract class VsoPageViewModelBase : IVsoPageViewModel
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private string _fullEditionName;

        #endregion

        /////////////////////////////////////////////////////////
        #region IVsoPageViewModel Member

        public string FullEditionName
        {
            get { return _fullEditionName; }
            set
            {
                if (value == _fullEditionName) return;
                _fullEditionName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region INotifyPropertyChanged Members & Extension

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.SafeInvoke(this, propertyName);
        }

        #endregion
    }
}