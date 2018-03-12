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
        private bool _displayIcons;
        private bool _displaySeparator;

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
            get => _editModeEnabled;
            set
            {
                if (value == _editModeEnabled) return;
                _editModeEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayFolders
        {
            get => _displayFolders;
            set
            {
                if (value == _displayFolders) return;
                _displayFolders = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayIcons
        {
            get => _displayIcons;
            set
            {
                if (value == _displayIcons) return;
                _displayIcons = value;
                OnPropertyChanged();
            }
        }

        public bool DisplaySeparator
        {
            get => _displaySeparator;
            set
            {
                if (value == _displaySeparator) return;
                _displaySeparator = value;
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