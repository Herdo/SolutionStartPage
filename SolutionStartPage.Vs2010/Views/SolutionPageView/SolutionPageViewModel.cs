﻿namespace SolutionStartPage.Vs2010.Views.SolutionPageView
{
    using Shared.Models;
    using Shared.Views;
    using Shared.Views.SolutionPageView;

    public sealed class SolutionPageViewModel : SolutionPageViewModelBase
    {
        /////////////////////////////////////////////////////////
        #region Constructors
        
        public SolutionPageViewModel(IViewStateProvider viewStateProvider, SolutionPageConfiguration config)
            : base(viewStateProvider, config)
        {
        }

        #endregion
    }
 }