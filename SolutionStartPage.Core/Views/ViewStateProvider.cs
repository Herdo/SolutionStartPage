namespace SolutionStartPage.Core.Views
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Control.Annotations;
    using Shared.Extensions;
    using Shared.Views;

    public class ViewStateProvider : IViewStateProvider
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private bool _editModeEnabled;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public ViewStateProvider()
        {
            _editModeEnabled = false;
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