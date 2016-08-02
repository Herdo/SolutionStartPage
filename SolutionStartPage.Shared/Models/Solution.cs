namespace SolutionStartPage.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Annotations;
    using BLL.Provider;
    using DAL;
    using static System.String;

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
        private string _computedSolutionPath;
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
                ComputeSolutionPath();
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
            get { return _solutionDisplayName ?? new FileInfo(ComputedSolutionPath).Name; }
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
        /// <remarks>The path might contain environment variables, which will be resolved in the <see cref="ComputedSolutionPath"/>.</remarks>
        [XmlElement]
        public string SolutionPath
        {
            get { return _solutionPath; }
            set
            {
                if (value == _solutionPath) return;
                _solutionPath = value;
                OnPropertyChanged();
                if (IsNullOrWhiteSpace(ComputedSolutionPath))
                    ComputeSolutionPath();
                if (IsNullOrWhiteSpace(SolutionDirectory))
                    ComputeSolutionDirectory();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string ComputedSolutionPath
        {
            get { return _computedSolutionPath; }
            private set
            {
                if (value == _computedSolutionPath) return;
                _computedSolutionPath = Uri.UnescapeDataString(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The solution directory. Either relative to <see cref="ComputedSolutionPath"/>, or absolute. Default is the directory of <see cref="ComputedSolutionPath"/>.
        /// </summary>
        [XmlElement]
        public string SolutionDirectory
        {
            get { return _solutionDirectory ?? Empty; }
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
                _computedSolutionDirectory = Uri.UnescapeDataString(value);
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

        [XmlIgnore]
        public bool DisplayFolders
        {
            get { return ViewStateProvider.DisplayFolders; }
            set { ViewStateProvider.DisplayFolders = value; }
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
            SolutionDirectory = Empty;

            SolutionAvailable = true;
            SolutionDirectoryAvailable = true;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void ComputeSolutionPath()
        {
            var result = SolutionPath;
            
            // Resolve environment variables
            result = ResolvePath(result);

            ComputedSolutionPath = result;
        }

        private void ComputeSolutionDirectory()
        {
            string tempDir;

            // If nothing set, return empty
            if (FileSystem == null
             || (IsNullOrWhiteSpace(SolutionPath)
              && IsNullOrWhiteSpace(SolutionDirectory)))
            {
                tempDir = Empty;
            }
            else
            {
                var resolvedSolutionDirectory = ResolvePath(SolutionDirectory);

                // Make directory, even if file path given
                if (!IsNullOrWhiteSpace(resolvedSolutionDirectory)
                    && !FileSystem.DirectoryExists(resolvedSolutionDirectory)
                    && FileSystem.FileExists(resolvedSolutionDirectory))
                {
                    _solutionDirectory = Path.GetDirectoryName(resolvedSolutionDirectory);
                    resolvedSolutionDirectory = ResolvePath(SolutionDirectory);
                }

                // Default, use solution file directory
                if (IsNullOrWhiteSpace(resolvedSolutionDirectory))
                {
                    SolutionDirectory = Path.GetDirectoryName(ComputedSolutionPath);
                    resolvedSolutionDirectory = ResolvePath(SolutionDirectory);
                }

                // Check if relative or absolute
                tempDir = Path.IsPathRooted(resolvedSolutionDirectory)
                          && FileSystem.DirectoryExists(resolvedSolutionDirectory)
                    ? resolvedSolutionDirectory
                    : new Uri(Path.Combine(Path.GetDirectoryName(ComputedSolutionPath) ?? @"\\",
                        resolvedSolutionDirectory ?? Empty)).AbsolutePath;
            }

            ComputedSolutionDirectory = tempDir;
        }

        private string ResolvePath(string path)
        {
            return IsNullOrWhiteSpace(path)
                ? path
                : Environment.ExpandEnvironmentVariables(path);
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Public Methods

        public void TriggerOpenSolution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenSolutionExecuted?.Invoke(sender, e);
        }

        public void TriggerOpenSolution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            OpenSolutionCanExecute?.Invoke(sender, e);
        }

        public void TriggerAlterSolution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AlterSolutionExecuted?.Invoke(sender, e);
        }

        public void TriggerAlterSolution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            AlterSolutionCanExecute?.Invoke(sender, e);
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