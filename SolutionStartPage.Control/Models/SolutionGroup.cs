namespace SolutionStartPage.Control.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Extensions;
    using Annotations;

    public class SolutionGroup : INotifyPropertyChanged
    {
        /////////////////////////////////////////////////////////
        #region Events

        public event EventHandler<CanExecuteRoutedEventArgs> AlterSolutionGroupCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> AlterSolutionGroupExecuted;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

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

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionGroup()
        {
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