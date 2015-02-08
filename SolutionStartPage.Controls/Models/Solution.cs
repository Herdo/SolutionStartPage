namespace SolutionStartPage.Controls.Models
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Annotations;
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

        private readonly SolutionGroup _parentGroup;
        private string _solutionDisplayName;
        private string _solutionPath;
        private string _solutionDirectory;
        private string _computedSolutionDirectory;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        [XmlIgnore]
        public SolutionGroup ParentGroup { get { return _parentGroup; } }

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
                _computedSolutionDirectory = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors
        
        /// <summary>
        /// Design-time constructor.
        /// </summary>
        public Solution()
        {}

        public Solution(SolutionGroup group)
        {
            _parentGroup = group;
            SolutionDisplayName = null;
            SolutionPath = String.Empty;
            SolutionDirectory = String.Empty;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void ComputeSolutionDirectory()
        {
            // If nothing set, return empty
            if (String.IsNullOrWhiteSpace(SolutionPath)
             && String.IsNullOrWhiteSpace(SolutionDirectory))
            {
                ComputedSolutionDirectory = String.Empty;
            }
            else
            {
                // Make directory, even if file path given
                if (!String.IsNullOrWhiteSpace(SolutionDirectory)
                 && !Directory.Exists(SolutionDirectory)
                 && File.Exists(SolutionDirectory))
                    _solutionDirectory = Path.GetDirectoryName(SolutionDirectory);

                // Default, use solution file directory
                if (String.IsNullOrWhiteSpace(SolutionDirectory))
                    ComputedSolutionDirectory = Path.GetDirectoryName(SolutionPath);

                // Check if relative or absolute
                ComputedSolutionDirectory = Path.IsPathRooted(SolutionDirectory)
                                         && Directory.Exists(SolutionDirectory)
                    ? SolutionDirectory
                    : new Uri(Path.Combine(Path.GetDirectoryName(SolutionPath) ?? @"\\", SolutionDirectory)).AbsolutePath;
            }
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