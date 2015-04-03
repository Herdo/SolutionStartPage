﻿namespace SolutionStartPage.Core.Views.SolutionPageView
 {
     using System;
     using System.Collections.Generic;
     using System.Collections.ObjectModel;
     using System.ComponentModel;
     using System.Diagnostics;
     using System.IO;
     using System.Linq;
     using System.Windows.Input;
     using Shared.Commands;
     using Shared.DAL;
     using Shared.Models;
     using Shared.Views;
     using Shared.Views.SolutionPageView;

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

         #endregion

         /////////////////////////////////////////////////////////
         #region Constructors

         public SolutionPagePresenter(ISolutionPageView view, ISolutionPageViewModel viewModel, ISolutionPageModel model, IViewStateProvider viewStateProvider, IFileSystem fileSystem, IIde ide)
             : base(view, viewModel)
         {
             _model = model;
             _viewStateProvider = viewStateProvider;
             _fileSystem = fileSystem;
             _ide = ide;

             _configuration = _model.LoadConfiguration();
             
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
             var newGroup = new SolutionGroup(_viewStateProvider) {GroupName = groupName};
             newGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
             newGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
             ViewModel.SolutionGroups.Add(newGroup);
             return newGroup;
         }

         private void AddSolutionsBulk(bool singleGroup)
         {
             var selectedPath = View.BrowseBulkAddRootFolder();
             if (!String.IsNullOrEmpty(selectedPath))
                 AddSolutionsByBulk(selectedPath, singleGroup);
         }

         private void AddSolutionsByBulk(string selectedPath, bool singleGroup)
         {
             var files = _model.GetFilesInDirectory(selectedPath, "*.sln");

             // Order solution files into groups
             var groups = new Dictionary<string, List<FileInfo>>();
             foreach (var fileInfo in files)
             {
                 var rootDir = (singleGroup ? selectedPath : fileInfo.DirectoryName) ?? String.Empty;
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
             group.AlterSolutionGroupCanExecute -= solutionGroup_AlterSolutionGroupCanExecute;
             group.AlterSolutionGroupExecuted -= solutionGroup_AlterSolutionGroupExecuted;
             ViewModel.SolutionGroups.Remove(group);
         }

         private void DeleteGroups()
         {
             while (ViewModel.SolutionGroups.Count > 0)
                 RemoveGroup(ViewModel.SolutionGroups[0]);
         }

         private void AddSolution(SolutionGroup group, Solution solution = null)
         {
             if (solution == null)
             {
                 var path = View.BrowseSolution(group);
                 if (!String.IsNullOrEmpty(path))
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
             solution.OpenSolutionCanExecute -= solution_OpenSolutionCanExecute;
             solution.OpenSolutionExecuted -= solution_OpenSolutionExecuted;
             solution.AlterSolutionCanExecute -= solution_AlterSolutionCanExecute;
             solution.AlterSolutionExecuted -= solution_AlterSolutionExecuted;

             solution.ParentGroup.Solutions.Remove(solution);
         }

         private void OpenSolutionDirectoryExplorer(Solution solution)
         {
             if (Keyboard.Modifiers == ModifierKeys.Shift)
             {
                 if (_model.GetParentDirectory(solution.ComputedSolutionDirectory) == null) return;
                 // If shift, open the parent directory of the computed
                 // directory, and select the computed directory.
                 var argument = String.Format(@"/select,{0}", new Uri(solution.ComputedSolutionDirectory).LocalPath);
                 Process.Start("explorer", argument);
             }
             else
             {
                 if (_model.DirectoryExists(solution.ComputedSolutionDirectory))
                     // If no shift, just open the computed directory.
                     Process.Start(solution.ComputedSolutionDirectory);
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

         void solutionGroup_AlterSolutionGroupCanExecute(object sender, CanExecuteRoutedEventArgs e)
         {
             var param = e.Parameter.ToString();
             var group = (SolutionGroup)sender;

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

         void solutionGroup_AlterSolutionGroupExecuted(object sender, ExecutedRoutedEventArgs e)
         {
             var param = e.Parameter.ToString();
             var group = (SolutionGroup)sender;
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

         void view_AlterPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
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

         void view_AlterPageExecuted(object sender, ExecutedRoutedEventArgs e)
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

         void solution_OpenSolutionCanExecute(object sender, CanExecuteRoutedEventArgs e)
         {
             var param = e.Parameter.ToString();
             var solution = (Solution)sender;
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
                     OpenSolutionDirectoryExplorer(solution);
                     break;
             }
         }

         void solution_AlterSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
         {
             var param = e.Parameter.ToString();
             var solution = (Solution)sender;
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

         void solution_AlterSolutionCanExecute(object sender, CanExecuteRoutedEventArgs e)
         {
             var param = e.Parameter.ToString();
             var solution = (Solution)sender;
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