﻿namespace SolutionStartPage.Control.SolutionsPart
 {
     using System;
     using System.Windows;
     using System.Windows.Input;
     using Extensions;

     /// <summary>
     /// Interaction logic for SolutionPageControl.xaml
     /// </summary>
     public partial class SolutionPageControl
     {
         /////////////////////////////////////////////////////////
         #region Events

         public event EventHandler<CanExecuteRoutedEventArgs> AlterPageCanExecute;
         public event EventHandler<ExecutedRoutedEventArgs> AlterPageExecuted;

         #endregion

         /////////////////////////////////////////////////////////
         #region Constructors

         public SolutionPageControl()
         {
             InitializeComponent();

             Loaded += SolutionPageControl_Loaded;
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Public Methods

         public void ConnectDataSource(SolutionPageViewModel vm)
         {
             DataContext = vm;
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Event Handler

         void SolutionPageControl_Loaded(object sender, RoutedEventArgs e)
         {
             new SolutionPagePresenter(this);
         }

         private void AlterPage_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
         {
             AlterPageCanExecute.SafeInvoke(sender, e);
         }

         private void AlterPage_OnExecuted(object sender, ExecutedRoutedEventArgs e)
         {
             AlterPageExecuted.SafeInvoke(sender, e);
         }

         #endregion
     }
 }