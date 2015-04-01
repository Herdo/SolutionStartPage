﻿namespace SolutionStartPage.Vs2010.Views.SolutionPageView
 {
     using System;
     using System.Windows.Input;
     using Shared.Extensions;
     using Shared.Views.SolutionPageView;

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

         #endregion
     }
 }