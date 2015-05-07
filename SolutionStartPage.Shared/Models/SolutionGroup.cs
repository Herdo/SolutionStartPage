namespace SolutionStartPage.Shared.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Annotations;
    using BLL.Provider;
    using Extensions;

    public class SolutionGroup : INotifyPropertyChanged
    {
        /////////////////////////////////////////////////////////
        #region Events

        public event EventHandler<CanExecuteRoutedEventArgs> AlterSolutionGroupCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> AlterSolutionGroupExecuted;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

        private IViewStateProvider _viewStateProvider;

        private string _groupName;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        [XmlIgnore]
        public IViewStateProvider ViewStateProvider
        {
            get { return _viewStateProvider; }
            set
            {
                if (_viewStateProvider != null)
                    _viewStateProvider.PropertyChanged -= viewStateProvider_PropertyChanged;

                _viewStateProvider = value;

                if (_viewStateProvider != null)
                    _viewStateProvider.PropertyChanged += viewStateProvider_PropertyChanged;
            }
        }

        [XmlElement]
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (value == _groupName) return;
                _groupName = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public ObservableCollection<Solution> Solutions { get; set; }

        [XmlIgnore]
        public bool EditModeEnabled
        {
            get { return _viewStateProvider.EditModeEnabled; }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// <see cref="XmlSerializer"/> constructor.
        /// </summary>
        public SolutionGroup()
        {}

        public SolutionGroup(IViewStateProvider viewStateProvider)
        {
            ViewStateProvider = viewStateProvider;

            GroupName = String.Empty;
            Solutions = new ObservableCollection<Solution>();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Public Methods

        public void TriggerAlterSolutionGroup_CanExecute(CanExecuteRoutedEventArgs args)
        {
            AlterSolutionGroupCanExecute.SafeInvoke(this, args);
        }

        public void TriggerAlterSolutionGroup_Executed(ExecutedRoutedEventArgs args)
        {
            AlterSolutionGroupExecuted.SafeInvoke(this, args);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        void viewStateProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            PropertyChanged.SafeInvoke(this, propertyName);
        }

        #endregion
    }
}