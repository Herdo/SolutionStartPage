namespace SolutionStartPage.Controls.SolutionsPart
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Extensions;
    using Models;

    public sealed class SolutionPageViewModel : INotifyPropertyChanged
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private bool _editModeEnabled;
        private int _columns;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        public bool EditModeEnabled
        {
            get { return _editModeEnabled; }
            set
            {
                if (value.Equals(_editModeEnabled)) return;
                _editModeEnabled = value;
                OnPropertyChanged();
            }
        }

        public int Columns
        {
            get { return _columns; }
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

        public SolutionPageViewModel()
        {
            EditModeEnabled = false;
        }

        public SolutionPageViewModel(SolutionPageConfiguration config)
            : this()
        {
            Columns = config.Columns;
            SolutionGroups = new ObservableCollection<SolutionGroup>(config.SolutionGroups);
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