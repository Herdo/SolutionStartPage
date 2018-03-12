namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using BLL.Provider;
    using Models;

    public abstract class SolutionPageViewModelBase : ISolutionPageViewModel
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly IViewStateProvider _viewStateProvider;
        private int _columns;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        public bool EditModeEnabled
        {
            get => _viewStateProvider.EditModeEnabled;
            set => _viewStateProvider.EditModeEnabled = value;
        }

        public bool DisplayFolders
        {
            get => _viewStateProvider.DisplayFolders;
            set => _viewStateProvider.DisplayFolders = value;
        }

        public bool DisplayIcons
        {
            get => _viewStateProvider.DisplayIcons;
            set => _viewStateProvider.DisplayIcons = value;
        }

        public bool DisplaySeparator
        {
            get => _viewStateProvider.DisplaySeparator;
            set => _viewStateProvider.DisplaySeparator = value;
        }

        public int Columns
        {
            get => _columns;
            set
            {
                if (value == _columns) return;
                _columns = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SolutionGroup> SolutionGroups { get; set; }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected SolutionPageViewModelBase(IViewStateProvider viewStateProvider)
        {
            _viewStateProvider = viewStateProvider;
            _viewStateProvider.PropertyChanged += viewStateProvider_PropertyChanged;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        private void viewStateProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
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