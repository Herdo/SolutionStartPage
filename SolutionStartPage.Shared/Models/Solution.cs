namespace SolutionStartPage.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
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
        private bool _hasError;
        private string _errorText;

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
                Debug.WriteLine($"{nameof(ComputedSolutionPath)} set to {_computedSolutionPath}");
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
                Debug.WriteLine($"{nameof(ComputedSolutionDirectory)} set to {_computedSolutionDirectory}");
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

        [XmlIgnore]
        public bool HasError
        {
            get { return _hasError; }
            private set
            {
                if (value == _hasError) return;
                _hasError = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public string ErrorText
        {
            get { return _errorText; }
            private set
            {
                if (value == _errorText) return;
                _errorText = value;
                HasError = _errorText != null;
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
            string directory;

            // If no information is given set, return empty
            if (FileSystem == null
             || (IsNullOrWhiteSpace(SolutionPath)
              && IsNullOrWhiteSpace(SolutionDirectory)))
            {
                directory = Empty;
            }
            else
            {
                directory = ResolvePath(SolutionDirectory);
                directory = MakeDirectoryIfFile(directory);
                directory = UseDirectoryOfSolutionIfEmpty(directory);
                directory = MakeAbsolutePath(directory);
            }

            ComputedSolutionDirectory = directory;
        }

        private static string ResolvePath(string path)
        {
            return IsNullOrWhiteSpace(path)
                ? path
                : Environment.ExpandEnvironmentVariables(path);
        }

        private string MakeDirectoryIfFile(string path)
        {
            return !IsNullOrWhiteSpace(path)
                && !FileSystem.DirectoryExists(path)
                && FileSystem.FileExists(path)
                       ? Path.GetDirectoryName(path)
                       : path;
        }

        private string UseDirectoryOfSolutionIfEmpty(string path)
        {
            if (!IsNullOrWhiteSpace(path))
                return path;

            var solutionDirectory = Path.GetDirectoryName(ComputedSolutionPath);
            SolutionDirectory = solutionDirectory;
            return solutionDirectory;
        }

        private string MakeAbsolutePath(string path)
        {
            return Path.IsPathRooted(path)
                && FileSystem.DirectoryExists(path)
                ? path
                : new Uri(Path.Combine(Path.GetDirectoryName(ComputedSolutionPath) ?? @"\\", path ?? Empty)).LocalPath;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Public Methods

        public void ComputeProperties()
        {
            try
            {
                ComputeSolutionPath();
                ComputeSolutionDirectory();
                ErrorText = null;
            }
            catch (Exception e)
            {
                // Prevent errors during calculation to crash the whole start page
                ErrorText = $"Error loading solution:{Environment.NewLine}"
                          + $"{e}{Environment.NewLine}{Environment.NewLine}"
                          + $"Advanced information:{Environment.NewLine}"
                          + $"Solution path: {SolutionPath}{Environment.NewLine}"
                          + $"Solution directory: {SolutionDirectory}";
            }
        }

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