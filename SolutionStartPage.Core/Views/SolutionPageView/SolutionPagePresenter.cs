namespace SolutionStartPage.Core.Views.SolutionPageView
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Annotations;
    using Shared.BLL.Interfaces;
    using Shared.BLL.Provider;
    using Shared.Commands;
    using Shared.DAL;
    using Shared.Models;
    using Shared.Views.SolutionPageView;
    using static System.String;
    using static Shared.Utilities;

    public class SolutionPagePresenter : BasePresenter<ISolutionPageView, ISolutionPageViewModel>,
        ISolutionPagePresenter
    {
        /////////////////////////////////////////////////////////

        #region Fields

        private readonly ISolutionPageModel _model;
        private readonly SolutionPageConfiguration _configuration;
        private readonly IIde _ide;
        private readonly IViewStateProvider _viewStateProvider;
        private readonly IFileSystem _fileSystem;
        private readonly ISystemInterface _systemInterface;
        private readonly IUserIOInterface _userIOInterface;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionPagePresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewStateProvider">A common <see cref="IViewStateProvider"/>.</param>
        /// <param name="fileSystem">The file sytem access.</param>
        /// <param name="systemInterface">The interface for system operations.</param>
        /// <param name="userIOInterface">The user IO interface.</param>
        /// <param name="ide">Information about the current Visual Studio instance.</param>
        /// <exception cref="ArgumentNullException">Any parameter is null.</exception>
        public SolutionPagePresenter([NotNull] ISolutionPageView view,
            [NotNull] ISolutionPageViewModel viewModel,
            [NotNull] ISolutionPageModel model,
            [NotNull] IViewStateProvider viewStateProvider,
            [NotNull] IFileSystem fileSystem,
            [NotNull] ISystemInterface systemInterface,
            [NotNull] IUserIOInterface userIOInterface,
            [NotNull] IIde ide)
            : base(view, viewModel)
        {
            ThrowIfNull(model, nameof(model));
            ThrowIfNull(viewStateProvider, nameof(viewStateProvider));
            ThrowIfNull(fileSystem, nameof(fileSystem));
            ThrowIfNull(systemInterface, nameof(systemInterface));
            ThrowIfNull(userIOInterface, nameof(userIOInterface));
            ThrowIfNull(ide, nameof(ide));

            _model = model;
            _viewStateProvider = viewStateProvider;
            _fileSystem = fileSystem;
            _systemInterface = systemInterface;
            _userIOInterface = userIOInterface;
            _ide = ide;
            _configuration = _model.LoadConfiguration();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Base Overrides

        protected override void View_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareLoadedData();
            ConnectEventHandler();

            View.ConnectDataSource(ViewModel);

            FillDefault();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private void PrepareLoadedData()
        {
            ViewModel.Columns = _configuration.Columns;
            ViewModel.DisplayFolders = _configuration.DisplayFolders;
            ViewModel.SolutionGroups = new ObservableCollection<SolutionGroup>(_configuration.SolutionGroups);

            foreach (var solutionGroup in ViewModel.SolutionGroups)
            {
                // Apply view state provider
                solutionGroup.ViewStateProvider = _viewStateProvider;

                // Connect loaded solution groups
                solutionGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
                solutionGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
                foreach (var solution in solutionGroup.Solutions)
                {
                    // Apply view state provider
                    solution.ViewStateProvider = _viewStateProvider;
                    solution.FileSystem = _fileSystem;

                    // Apply parent group - is not set by XmlSerializer when deserializing
                    solution.ParentGroup = solutionGroup;

                    // Connect loaded solutions
                    solution.OpenSolutionCanExecute += solution_OpenSolutionCanExecute;
                    solution.OpenSolutionExecuted += solution_OpenSolutionExecuted;
                    solution.AlterSolutionCanExecute += solution_AlterSolutionCanExecute;
                    solution.AlterSolutionExecuted += solution_AlterSolutionExecuted;
                }
            }
        }

        private void ConnectEventHandler()
        {
            ViewModel.PropertyChanged += vm_PropertyChanged;

            View.AlterPageCanExecute += view_AlterPageCanExecute;
            View.AlterPageExecuted += view_AlterPageExecuted;
        }

        private void FillDefault()
        {
            if (ViewModel.SolutionGroups.Count == 0)
                AddGroup();
        }

        private SolutionGroup AddGroup(string groupName = "Group Name")
        {
            var newGroup = new SolutionGroup(_viewStateProvider) {GroupName = groupName ?? Empty};
            newGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
            newGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
            ViewModel.SolutionGroups.Add(newGroup);
            return newGroup;
        }

        private void AddSolutionsBulk(bool singleGroup)
        {
            var selectedPath = View.BrowseBulkAddRootFolder();
            if (!IsNullOrEmpty(selectedPath))
                AddSolutionsByBulk(selectedPath, singleGroup);
        }

        private void AddSolutionsByBulk(string selectedPath, bool singleGroup)
        {
            var files = _model.GetFilesInDirectory(selectedPath, "*.sln");

            // Order solution files into groups
            var groups = new Dictionary<string, List<FileInfo>>();
            foreach (var fileInfo in files)
            {
                var rootDir = (singleGroup ? selectedPath : fileInfo.DirectoryName) ?? Empty;
                if (!groups.ContainsKey(rootDir))
                    groups.Add(rootDir, new List<FileInfo>());
                var group = groups[rootDir];
                group.Add(fileInfo);
            }

            // Create groups
            foreach (var group in groups)
            {
                var g = AddGroup(group.Key);
                foreach (var sln in group.Value
                    .Select(fileInfo => new Solution(_viewStateProvider, _fileSystem, g, fileInfo.FullName)))
                    AddSolution(g, sln);
            }
        }

        private void RemoveGroup(SolutionGroup group)
        {
            ThrowIfNull(group, nameof(group));

            group.AlterSolutionGroupCanExecute -= solutionGroup_AlterSolutionGroupCanExecute;
            group.AlterSolutionGroupExecuted -= solutionGroup_AlterSolutionGroupExecuted;
            ViewModel.SolutionGroups.Remove(group);
        }

        private void DeleteGroups()
        {
            while (ViewModel.SolutionGroups.Count > 0)
                RemoveGroup(ViewModel.SolutionGroups[0]);
        }

        private void AddSolution([NotNull] SolutionGroup group, Solution solution = null)
        {
            ThrowIfNull(group, nameof(group));

            if (solution == null)
            {
                var path = View.BrowseSolution(group);
                if (!IsNullOrEmpty(path))
                    solution = new Solution(_viewStateProvider, _fileSystem, group, path);
            }
            if (solution == null)
                return;

            solution.OpenSolutionCanExecute += solution_OpenSolutionCanExecute;
            solution.OpenSolutionExecuted += solution_OpenSolutionExecuted;
            solution.AlterSolutionCanExecute += solution_AlterSolutionCanExecute;
            solution.AlterSolutionExecuted += solution_AlterSolutionExecuted;
            group.Solutions.Add(solution);
        }

        private void RemoveSolution(Solution solution)
        {
            ThrowIfNull(solution, nameof(solution));

            solution.OpenSolutionCanExecute -= solution_OpenSolutionCanExecute;
            solution.OpenSolutionExecuted -= solution_OpenSolutionExecuted;
            solution.AlterSolutionCanExecute -= solution_AlterSolutionCanExecute;
            solution.AlterSolutionExecuted -= solution_AlterSolutionExecuted;

            solution.ParentGroup.Solutions.Remove(solution);
        }

        private void OpenSolutionDirectoryExplorer(Solution solution)
        {
            var depth = 0;
            if (_userIOInterface.IsModifierKeyDown(ModifierKeys.Shift))
                depth += 1;
            if (_userIOInterface.IsModifierKeyDown(ModifierKeys.Control))
                depth += 1;

            string targetDirectory;

            switch (depth)
            {
                case 0:
                    if (_model.DirectoryExists(solution.ComputedSolutionDirectory))
                        _systemInterface.StartProcess(new ProcessStartInfo(solution.ComputedSolutionDirectory));
                    break;
                case 1:
                    targetDirectory = solution.ComputedSolutionDirectory;
                    if (_model.GetParentDirectory(targetDirectory) == null) return;

                    var parentArgument = $@"/select,{new Uri(targetDirectory).LocalPath}";
                    _systemInterface.StartProcess(new ProcessStartInfo("explorer", parentArgument));
                    break;
                case 2:
                    targetDirectory = solution.ComputedSolutionDirectory;
                    var parent = _model.GetParentDirectory(targetDirectory);
                    if (parent == null) return;

                    targetDirectory = parent.FullName;
                    parent = _model.GetParentDirectory(targetDirectory);
                    // if no 2nd-level parent exists, use the first-level parent
                    targetDirectory = parent?.FullName ?? solution.ComputedSolutionDirectory;

                    var parentParentArgument = $@"/select,{new Uri(targetDirectory).LocalPath}";
                    _systemInterface.StartProcess(new ProcessStartInfo("explorer", parentParentArgument));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Event Handler

        private async void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditModeEnabled"
                && ViewModel.EditModeEnabled == false)
            {
                await _model.SaveConfiguration(_configuration.ApplyViewModel(ViewModel));
            }
        }

        private void solutionGroup_AlterSolutionGroupCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var group = (SolutionGroup) sender;

            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_BACK:
                    e.CanExecute = ViewModel.SolutionGroups.IndexOf(group) != 0;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_FORWARD:
                    e.CanExecute = ViewModel.SolutionGroups.IndexOf(group) != ViewModel.SolutionGroups.Count - 1;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_REMOVE_GROUP:
                    e.CanExecute = true;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_ADD_SOLUTION:
                    e.CanExecute = true;
                    break;
            }
        }

        private void solutionGroup_AlterSolutionGroupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var group = (SolutionGroup) sender;
            int oldIdx;
            int newIdx;

            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_BACK:
                    oldIdx = ViewModel.SolutionGroups.IndexOf(group);
                    newIdx = oldIdx - 1;
                    ViewModel.SolutionGroups.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_FORWARD:
                    oldIdx = ViewModel.SolutionGroups.IndexOf(group);
                    newIdx = oldIdx + 1;
                    ViewModel.SolutionGroups.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_REMOVE_GROUP:
                    RemoveGroup(group);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_ADD_SOLUTION:
                    AddSolution(group);
                    break;
            }
        }

        private void view_AlterPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case CommandParameter.ALTER_PAGE_ADD_GROUP:
                    e.CanExecute = true;
                    break;
                case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_SINGLE:
                case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_MULTIPLE:
                    e.CanExecute = true;
                    break;
                case CommandParameter.ALTER_PAGE_DELETE_ALL_GROUPS:
                    e.CanExecute = ViewModel.SolutionGroups.Count > 0;
                    break;
            }
        }

        private void view_AlterPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case CommandParameter.ALTER_PAGE_ADD_GROUP:
                    AddGroup();
                    break;
                case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_SINGLE:
                    AddSolutionsBulk(true);
                    break;
                case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_MULTIPLE:
                    AddSolutionsBulk(false);
                    break;
                case CommandParameter.ALTER_PAGE_DELETE_ALL_GROUPS:
                    DeleteGroups();
                    break;
            }
        }

        private void solution_OpenSolutionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution) sender;
            switch (param)
            {
                case CommandParameter.OPEN_SOLUTION_OPEN:
                    solution.SolutionAvailable =
                        e.CanExecute = !ViewModel.EditModeEnabled
                                       && _model.FileExists(solution.SolutionPath);
                    break;
                case CommandParameter.OPEN_SOLUTION_OPEN_EXPLORER:
                    solution.SolutionDirectoryAvailable =
                        e.CanExecute = !ViewModel.EditModeEnabled
                                       && _model.DirectoryExists(solution.ComputedSolutionDirectory);
                    break;
            }
        }

        private void solution_OpenSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution) sender;
            switch (param)
            {
                case CommandParameter.OPEN_SOLUTION_OPEN:
                    _ide?.OpenSolution(solution.SolutionPath);
                    break;
                case CommandParameter.OPEN_SOLUTION_OPEN_EXPLORER:
                    OpenSolutionDirectoryExplorer(solution);
                    break;
            }
        }

        private void solution_AlterSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution) sender;
            int oldIdx;
            int newIdx;

            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_MOVE_UP:
                    oldIdx = solution.ParentGroup.Solutions.IndexOf(solution);
                    newIdx = oldIdx - 1;
                    solution.ParentGroup.Solutions.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_MOVE_DOWN:
                    oldIdx = solution.ParentGroup.Solutions.IndexOf(solution);
                    newIdx = oldIdx + 1;
                    solution.ParentGroup.Solutions.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_REMOVE_SOLUTION:
                    RemoveSolution(solution);
                    break;
            }
        }

        private void solution_AlterSolutionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution) sender;
            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_MOVE_UP:
                    e.CanExecute = solution.ParentGroup.Solutions.IndexOf(solution) != 0;
                    break;
                case CommandParameter.ALTER_SOLUTION_MOVE_DOWN:
                    e.CanExecute = solution.ParentGroup.Solutions.IndexOf(solution) !=
                                   solution.ParentGroup.Solutions.Count - 1;
                    break;
                case CommandParameter.ALTER_SOLUTION_REMOVE_SOLUTION:
                    e.CanExecute = true;
                    break;
            }
        }

        #endregion
    }
}