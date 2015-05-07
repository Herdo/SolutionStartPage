﻿namespace SolutionStartPage.Vs2013.Views.SolutionPageView
 {
     using Shared.BLL.Provider;
     using Shared.Views.SolutionPageView;

     public sealed class SolutionPageViewModel : SolutionPageViewModelBase
     {
         /////////////////////////////////////////////////////////
         #region Constructors
         
         public SolutionPageViewModel(IViewStateProvider viewStateProvider, IResourceProvider resourceProvider)
             : base(viewStateProvider, resourceProvider)
         {
         }

         #endregion
     }
 }