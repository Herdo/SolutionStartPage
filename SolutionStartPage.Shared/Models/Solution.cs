namespace SolutionStartPage.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Annotations;
    using BLL.Provider;
    using DAL;
    using Extensions;

    public class Solution : INotifyPropertyChanged
    {
        /////////////////////////////////////////////////////////

        #region Events

        public event EventHandler<CanExecuteRoutedEventArgs> OpenSolutionCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> OpenSolutionExecuted;
        public event EventHandler<CanExecuteRoutedEventArgs> AlterSolutionCanExecute;
        public event EventHandler<ExecutedRoutedEventArgs> AlterSolutionExecuted;

        #endregion

        /////////////////////////////////////////////////////////

        #region Fields

        private IViewStateProvider _viewStateProvider;

        private string _solutionDisplayName;
        private string _solutionPath;
        private string _solutionDirectory;
        private string _computedSolutionDirectory;
        private bool _solutionAvailable;
        private bool _solutionDirectoryAvailable;
        private IFileSystem _fileSystem;

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

        [XmlIgnore]
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
            set
            {
                _fileSystem = value;
                ComputeSolutionDirectory();
            }
        }

        [XmlIgnore]
        public SolutionGroup ParentGroup { get; set; }

        /// <summary>
        /// The name for the solution, displayed in the GUI.
        /// </summary>
        [XmlElement]
        public string SolutionDisplayName
        {
            get { return _solutionDisplayName ?? new FileInfo(SolutionPath).Name; }
            set
            {
                if (value == _solutionDisplayName) return;
                _solutionDisplayName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The path to the solution or project file.
        /// </summary>
        [XmlElement]
        public string SolutionPath
        {
            get { return _solutionPath; }
            set
            {
                if (value == _solutionPath) return;
                _solutionPath = value;
                OnPropertyChanged();
                if (String.IsNullOrWhiteSpace(SolutionDirectory))
                    ComputeSolutionDirectory();
            }
        }

        /// <summary>
        /// The solution directory. Either relative to <see cref="SolutionPath"/>, or absolute. Default is the directory of <see cref="SolutionPath"/>.
        /// </summary>
        [XmlElement]
        public string SolutionDirectory
        {
            get { return _solutionDirectory ?? String.Empty; }
            set
            {
                if (value == _solutionDirectory) return;
                _solutionDirectory = value;
                OnPropertyChanged();
                ComputeSolutionDirectory();
            }
        }

        /// <summary>
        /// The actual solution directory, depending on the value of <see cref="SolutionDirectory"/>.
        /// </summary>
        [XmlIgnore]
        public string ComputedSolutionDirectory
        {
            get { return _computedSolutionDirectory; }
            private set
            {
                if (value == _computedSolutionDirectory) return;
                _computedSolutionDirectory = HttpUtility.UrlDecode(value);
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public bool EditModeEnabled
        {
            get { return ViewStateProvider.EditModeEnabled; }
            set { ViewStateProvider.EditModeEnabled = value; }
        }

        [XmlIgnore]
        public bool SolutionAvailable
        {
            get { return _solutionAvailable; }
            set
            {
                if (value.Equals(_solutionAvailable)) return;
                _solutionAvailable = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public bool SolutionDirectoryAvailable
        {
            get { return _solutionDirectoryAvailable; }
            set
            {
                if (value.Equals(_solutionDirectoryAvailable)) return;
                _solutionDirectoryAvailable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        /// <summary>
        /// <see cref="XmlSerializer"/> constructor.
        /// </summary>
        public Solution()
        {
        }

        public Solution(IViewStateProvider viewStateProvider, IFileSystem fileSystem, SolutionGroup group,
            string solutionPath)
        {
            ViewStateProvider = viewStateProvider;
            FileSystem = fileSystem;

            ParentGroup = group;
            SolutionPath = solutionPath;

            SolutionDisplayName = null;
            SolutionDirectory = String.Empty;

            SolutionAvailable = true;
            SolutionDirectoryAvailable = true;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private void ComputeSolutionDirectory()
        {
            string tempDir;

            // If nothing set, return empty
            if ((String.IsNullOrWhiteSpace(SolutionPath)
                 && String.IsNullOrWhiteSpace(SolutionDirectory))
                || FileSystem == null)
            {
                tempDir = String.Empty;
            }
            else
            {
                // Make directory, even if file path given
                if (!String.IsNullOrWhiteSpace(SolutionDirectory)
                    && !FileSystem.DirectoryExists(SolutionDirectory)
                    && FileSystem.FileExists(SolutionDirectory))
                    _solutionDirectory = Path.GetDirectoryName(SolutionDirectory);

                // Default, use solution file directory
                if (String.IsNullOrWhiteSpace(SolutionDirectory))
                    SolutionDirectory = Path.GetDirectoryName(SolutionPath);

                // Check if relative or absolute
                tempDir = Path.IsPathRooted(SolutionDirectory)
                          && FileSystem.DirectoryExists(SolutionDirectory)
                    ? SolutionDirectory
                    : new Uri(Path.Combine(Path.GetDirectoryName(SolutionPath) ?? @"\\",
                        SolutionDirectory ?? String.Empty)).AbsolutePath;
            }

            ComputedSolutionDirectory = tempDir;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Public Methods

        public void TriggerOpenSolution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenSolutionExecuted.SafeInvoke(sender, e);
        }

        public void TriggerOpenSolution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            OpenSolutionCanExecute.SafeInvoke(sender, e);
        }

        public void TriggerAlterSolution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AlterSolutionExecuted.SafeInvoke(sender, e);
        }

        public void TriggerAlterSolution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            AlterSolutionCanExecute.SafeInvoke(sender, e);
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
            PropertyChanged.SafeInvoke(this, propertyName);
        }

        #endregion
    }
}