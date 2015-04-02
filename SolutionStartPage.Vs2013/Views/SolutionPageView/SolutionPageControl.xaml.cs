﻿namespace SolutionStartPage.Vs2013.Views.SolutionPageView
 {
     using System;
     using System.Windows.Forms;
     using System.Windows.Input;
     using Microsoft.Practices.Unity;
     using Shared.Extensions;
     using Shared.Funtionality;
     using Shared.Models;
     using Shared.Views.SolutionPageView;
     using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

     /// <summary>
     /// Interaction logic for SolutionPageControl.xaml
     /// </summary>
     public partial class SolutionPageControl : ISolutionPageView
     {
         /////////////////////////////////////////////////////////
         #region Constructors

         public SolutionPageControl()
         {
             InitializeComponent();
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Event Handler

         private void AlterPage_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
         {
             AlterPageCanExecute.SafeInvoke(sender, e);
         }

         private void AlterPage_OnExecuted(object sender, ExecutedRoutedEventArgs e)
         {
             AlterPageExecuted.SafeInvoke(sender, e);
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region ISolutionPageView Member

         public event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute;
         public event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted;

         void ISolutionPageView.ConnectDataSource(ISolutionPageViewModel vm)
         {
             DataContext = vm;
         }

         string ISolutionPageView.BrowseBulkAddRootFolder()
         {
             string selectedPath = null;
             var fbd = new FolderBrowserDialog
             {
                 Description = @"Browse for a root folder...",
                 ShowNewFolderButton = false,
                 RootFolder = Environment.SpecialFolder.Desktop
             };
             var dialogResult = fbd.ShowDialog();
             if (dialogResult == DialogResult.OK)
                 selectedPath = fbd.SelectedPath;

             return selectedPath;
         }

         Solution ISolutionPageView.BrowseSolution(SolutionGroup solutionGroup)
         {
             var ofd = new OpenFileDialog
             {
                 CheckFileExists = true,
                 CheckPathExists = true,
                 DefaultExt = ".sln",
                 Filter = @"Solution (*.sln)|*.sln|" +
                          @"All files (*.*)|*.*",
                 AddExtension = true,
                 Multiselect = false,
                 ValidateNames = true,
                 Title = @"Browse for solution or other file..."
             };

             if (ofd.ShowDialog() == true)
             {
                 return UnityFactory.Resolve<Solution>(new ParameterOverrides
                 {
                     {"group", solutionGroup},
                     {"solutionPath", ofd.FileName}
                 });
             }

             return null;
         }

         #endregion
     }
 }