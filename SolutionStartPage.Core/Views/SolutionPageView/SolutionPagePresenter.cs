﻿namespace SolutionStartPage.Core.Views.SolutionPageView
 {
     using System;
     using System.Collections.Generic;
     using System.ComponentModel;
     using System.Diagnostics;
     using System.IO;
     using System.Linq;
     using System.Windows.Forms;
     using System.Windows.Input;
     using Microsoft.Practices.Unity;
     using Shared;
     using Shared.Commands;
     using Shared.Funtionality;
     using Shared.Models;
     using Shared.Views.SolutionPageView;
     using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

     public class SolutionPagePresenter : ISolutionPagePresenter
     {
         /////////////////////////////////////////////////////////
         #region Fields

         private readonly ISolutionPageView _view;
         private readonly ISolutionPageViewModel _vm;
         private readonly ISolutionPageModel _model;
         private readonly SolutionPageConfiguration _configuration;

         private IIde _ide;

         #endregion

         /////////////////////////////////////////////////////////
         #region Properties

         private IIde Ide
         {
             get { return _ide ?? (_ide = UnityFactory.Resolve<IIde>(Constants.IIDE_REGISTRATION_NAME)); }
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Constructors

         public SolutionPagePresenter(ISolutionPageView view, ISolutionPageModel model)
         {
             _view = view;
             _model = model;

             if (UnityFactory.IsRegistered<SolutionPageConfiguration>())
             {
                 _configuration = UnityFactory.Resolve<SolutionPageConfiguration>();
             }
             else
             {
                 _configuration = _model.LoadConfiguration();
                 UnityFactory.RegisterInstance(_configuration, new ContainerControlledLifetimeManager());
             }

             _vm = UnityFactory.Resolve<ISolutionPageViewModel>();

             _view.ConnectDataSource(_vm);

             PrepareLoadedData();
             ConnectEventHandler();

             FillDefault();
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Private Methods

         private void PrepareLoadedData()
         {
             foreach (var solutionGroup in _vm.SolutionGroups)
             {
                 // Connect loaded solution groups
                 solutionGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
                 solutionGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
                 foreach (var solution in solutionGroup.Solutions)
                 {
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
             _vm.PropertyChanged += vm_PropertyChanged;

             _view.AlterPageCanExecute += view_AlterPageCanExecute;
             _view.AlterPageExecuted += view_AlterPageExecuted;
         }

         private void FillDefault()
         {
             if (_vm.SolutionGroups.Count == 0)
                 AddGroup();
         }

         private SolutionGroup AddGroup(string groupName = "Group Name")
         {
             var newGroup = UnityFactory.Resolve<SolutionGroup>();
             newGroup.GroupName = groupName;
             newGroup.AlterSolutionGroupCanExecute += solutionGroup_AlterSolutionGroupCanExecute;
             newGroup.AlterSolutionGroupExecuted += solutionGroup_AlterSolutionGroupExecuted;
             _vm.SolutionGroups.Add(newGroup);
             return newGroup;
         }

         private void AddSolutionsBulk(bool singleGroup)
         {
             var fbd = new FolderBrowserDialog
             {
                 Description = @"Browse for a root folder...",
                 ShowNewFolderButton = false,
                 RootFolder = Environment.SpecialFolder.Desktop
             };
             var dialogResult = fbd.ShowDialog();
             if (dialogResult == DialogResult.OK)
                 AddSolutionsByBulk(fbd.SelectedPath, singleGroup);
         }

         private void AddSolutionsByBulk(string selectedPath, bool singleGroup)
         {
             // Get *.sln files
             var di = new DirectoryInfo(selectedPath);
             var files = di.GetFiles("*.sln", SearchOption.AllDirectories);

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
                     .Select(fileInfo => UnityFactory.Resolve<Solution>(new ParameterOverrides
                     {
                         {"group", g},
                         {"solutionPath", fileInfo.FullName}
                     })))
                     AddSolution(g, sln);
             }
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
                 solution = BrowseSolution(group);
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

         private Solution BrowseSolution(SolutionGroup group)
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
                 return UnityFactory.Resolve<Solution>(new ParameterOverrides
                 {
                     {"group", group},
                     {"solutionPath", ofd.FileName}
                 });
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
                 await _model.SaveConfiguration(_configuration.ApplyViewModel(_vm));
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
             var group = (SolutionGroup)sender;
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
                 case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_SINGLE:
                 case CommandParameter.ALTER_PAGE_ADD_GROUP_BULK_MULTIPLE:
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
                     if (Ide != null)
                         Ide.OpenSolution(solution.SolutionPath);
                     break;
                 case CommandParameter.OPEN_SOLUTION_OPEN_EXPLORER:
                     Process.Start(solution.ComputedSolutionDirectory);
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