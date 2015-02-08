namespace SolutionStartPage.Controls.SolutionsPart
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Input;
    using Commands;
    using EnvDTE80;
    using IDE;
    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.Win32;
    using Models;

    public class SolutionPagePresenter
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly SolutionPageControl _view;
        private readonly SolutionPageViewModel _vm;
        private readonly SolutionPageModel _model;
        private readonly IIde _ide;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPagePresenter(SolutionPageControl view)
        {
            _view = view;
            _ide = GetIde(_view.DataContext);

            _model = new SolutionPageModel();
            var config = _model.LoadConfiguration();
            _vm = new SolutionPageViewModel(config);

            _view.ConnectDataSource(_vm);
            ConnectEventHandler();

            FillDefault();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void ConnectEventHandler()
        {
            _vm.PropertyChanged += vm_PropertyChanged;

            _view.AlterPageCanExecute += view_AlterPageCanExecute;
            _view.AlterPageExecuted += view_AlterPageExecuted;

            // Connect loaded solution groups and solutions
            foreach (var solutionGroup in _vm.SolutionGroups)
            {
                solutionGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
                solutionGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
                foreach (var solution in solutionGroup.Solutions)
                {
                    solution.OpenSolutionCanExecute += solution_OpenSolutionCanExecute;
                    solution.OpenSolutionExecuted += solution_OpenSolutionExecuted;
                }
            }
        }

        private static IIde GetIde(object dataContext)
        {
            var dte = GetDte(dataContext);
            return dte == null
                ? null
                : new VsIde(dte);
        }

        private static DTE2 GetDte(object dataContext)
        {
            if (dataContext == null)
                return null;
            var typeDescriptor = dataContext as ICustomTypeDescriptor;
            if (typeDescriptor != null)
            {
                PropertyDescriptorCollection propertyCollection = typeDescriptor.GetProperties();
                return propertyCollection.Find("DTE", false).GetValue(dataContext) as DTE2;
            }
            var dataSource = dataContext as DataSource;
            if (dataSource != null)
            {
                return dataSource.GetValue("DTE") as DTE2;
            }
            Debug.Assert(false, "Could not get DTE instance, was " + (dataContext == null ? "null" : dataContext.GetType().ToString()));
            return null;
        }

        private void FillDefault()
        {
            if (_vm.SolutionGroups.Count == 0)
                AddGroup();
        }

        private void AddGroup()
        {
            var newGroup = new SolutionGroup
            {
                GroupName = "Group Name"
            };
            newGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
            newGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
            _vm.SolutionGroups.Add(newGroup);
        }

        private void RemoveGroup(SolutionGroup group)
        {
            group.AlterSolutionGroupCanExecute -= solutionGroup_AlterSolutionGroupCanExecute;
            group.AlterSolutionGroupExecuted -= solutionGroup_AlterSolutionGroupExecuted;
            _vm.SolutionGroups.Remove(group);
        }

        private void DeleteGroups()
        {
            while (_vm.SolutionGroups.Count > 0)
                RemoveGroup(_vm.SolutionGroups[0]);
        }

        private void AddSolution(SolutionGroup group, Solution solution = null)
        {
            if (solution == null)
                solution = BrowseSolution();
            if (solution == null)
                return;

            solution.OpenSolutionCanExecute += solution_OpenSolutionCanExecute;
            solution.OpenSolutionExecuted += solution_OpenSolutionExecuted;
            group.Solutions.Add(solution);
        }

        private Solution BrowseSolution()
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".sln",
                Filter = "Solution (*.sln)|*.sln|" +
                         "All files (*.*)|*.*",
                AddExtension = true,
                Multiselect = false,
                ValidateNames = true,
                Title = "Browse for solution or other file..."
            };

            if (ofd.ShowDialog() == true)
            {
                return new Solution
                {
                    SolutionPath = ofd.FileName
                };
            }

            return null;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        private async void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditModeEnabled"
             && _vm.EditModeEnabled == false)
            {
                await _model.SaveConfiguration(new SolutionPageConfiguration(_vm));
            }
        }

        void solutionGroup_AlterSolutionGroupCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var group = (SolutionGroup)sender;

            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_BACK:
                    e.CanExecute = _vm.SolutionGroups.IndexOf(group) != 0;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_FORWARD:
                    e.CanExecute = _vm.SolutionGroups.IndexOf(group) != _vm.SolutionGroups.Count - 1;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_REMOVE_GROUP:
                    e.CanExecute = true;
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_ADD_SOLUTION:
                    e.CanExecute = true;
                    break;
            }
        }

        void solutionGroup_AlterSolutionGroupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var group = (SolutionGroup) sender;
            int oldIdx;
            int newIdx;

            switch (param)
            {
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_BACK:
                    oldIdx = _vm.SolutionGroups.IndexOf(group);
                    newIdx = oldIdx - 1;
                    _vm.SolutionGroups.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_MOVE_GROUP_FORWARD:
                    oldIdx = _vm.SolutionGroups.IndexOf(group);
                    newIdx = oldIdx + 1;
                    _vm.SolutionGroups.Move(oldIdx, newIdx);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_REMOVE_GROUP:
                    RemoveGroup(group);
                    break;
                case CommandParameter.ALTER_SOLUTION_GROUP_ADD_SOLUTION:
                    AddSolution(group);
                    break;
            }
        }

        void view_AlterPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case CommandParameter.ALTER_PAGE_ADD_GROUP:
                    e.CanExecute = true;
                    break;
                case CommandParameter.ALTER_PAGE_DELETE_ALL_GROUPS:
                    e.CanExecute = _vm.SolutionGroups.Count > 0;
                    break;
            }
        }

        void view_AlterPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case CommandParameter.ALTER_PAGE_ADD_GROUP:
                    AddGroup();
                    break;
                case CommandParameter.ALTER_PAGE_DELETE_ALL_GROUPS:
                    DeleteGroups();
                    break;
            }
        }

        void solution_OpenSolutionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution) sender;
            switch (param)
            {
                case CommandParameter.OPEN_SOLUTION_OPEN:
                    e.CanExecute = !_vm.EditModeEnabled
                                && File.Exists(solution.SolutionPath);
                    break;
                case CommandParameter.OPEN_SOLUTION_OPEN_EXPLORER:
                    e.CanExecute = !_vm.EditModeEnabled
                                && Directory.Exists(solution.ComputedSolutionDirectory);
                    break;
            }
        }

        void solution_OpenSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            var solution = (Solution)sender;
            switch (param)
            {
                case CommandParameter.OPEN_SOLUTION_OPEN:
                    if (_ide != null)
                        _ide.OpenSolution(solution.SolutionPath);
                    break;
                case CommandParameter.OPEN_SOLUTION_OPEN_EXPLORER:
                    Process.Start(solution.ComputedSolutionDirectory);
                    break;
            }
        }

        #endregion
    }
}