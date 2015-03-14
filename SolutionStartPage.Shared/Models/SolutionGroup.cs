namespace SolutionStartPage.Shared.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Annotations;
    using Funtionality;
    using Extensions;
    using Views;

    public class SolutionGroup : INotifyPropertyChanged
    {
        /////////////////////////////////////////////////////////
        #region Events

        public event EventHandler<CanExecuteRoutedEventArgs> AlterSolutionGroupCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> AlterSolutionGroupExecuted;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

        private readonly IViewStateProvider _viewStateProvider;
        private string _groupName;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

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
            set { _viewStateProvider.EditModeEnabled = value; }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// <see cref="XmlSerializer"/> constructor.
        /// </summary>
        public SolutionGroup()
        {
            _viewStateProvider = UnityFactory.Resolve<IViewStateProvider>();
            _viewStateProvider.PropertyChanged += viewStateProvider_PropertyChanged;
        }

        public SolutionGroup(IViewStateProvider viewStateProvider)
        {
            _viewStateProvider = viewStateProvider;
            _viewStateProvider.PropertyChanged += viewStateProvider_PropertyChanged;

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