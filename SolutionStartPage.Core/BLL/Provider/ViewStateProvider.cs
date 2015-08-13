namespace SolutionStartPage.Core.BLL.Provider
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Shared.BLL.Provider;

    public class ViewStateProvider : IViewStateProvider
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private bool _editModeEnabled;
        private bool _displayFolders;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public ViewStateProvider()
        {
            _editModeEnabled = false;
            _displayFolders = true;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IViewStateProvider Member

        public bool EditModeEnabled
        {
            get { return _editModeEnabled; }
            set
            {
                if (value == _editModeEnabled) return;
                _editModeEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayFolders
        {
            get { return _displayFolders; }
            set
            {
                if (value == _displayFolders) return;
                _displayFolders = value;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}