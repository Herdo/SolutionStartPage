﻿namespace SolutionStartPage.Vs2010.Views.SolutionPageView
 {
     using System.Collections.ObjectModel;
     using System.ComponentModel;
     using System.Runtime.CompilerServices;
     using Annotations;
     using Shared.Extensions;
     using Shared.Models;
     using Shared.Views;
     using Shared.Views.SolutionPageView;

     public sealed class SolutionPageViewModel : ISolutionPageViewModel
     {
         /////////////////////////////////////////////////////////
         #region Fields

         private readonly IViewStateProvider _viewStateProvider;
         private int _columns;

         #endregion

         /////////////////////////////////////////////////////////
         #region Properties

         public bool EditModeEnabled
         {
             get { return _viewStateProvider.EditModeEnabled; }
             set { _viewStateProvider.EditModeEnabled = value; }
         }

         public int Columns
         {
             get { return _columns; }
             set
             {
                 if (value == _columns) return;
                 _columns = value;
                 OnPropertyChanged();
             }
         }

         public ObservableCollection<SolutionGroup> SolutionGroups { get; set; }

         #endregion

         /////////////////////////////////////////////////////////
         #region Constructors

         // Design time constructor
         public SolutionPageViewModel()
         {
             
         }

         public SolutionPageViewModel(IViewStateProvider viewStateProvider, SolutionPageConfiguration config)
         {
             _viewStateProvider = viewStateProvider;
             _viewStateProvider.PropertyChanged += viewStateProvider_PropertyChanged;

             Columns = config.Columns;
             SolutionGroups = new ObservableCollection<SolutionGroup>(config.SolutionGroups);
         }

         #endregion

         /////////////////////////////////////////////////////////
         #region Event Handler
         
         void viewStateProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
             PropertyChanged.SafeInvoke(this, propertyName);
         }

         #endregion
     }
 }