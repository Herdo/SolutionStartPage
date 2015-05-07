namespace SolutionStartPage.Vs2015.Views.SolutionPageView
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using Shared.Converter;
    using Shared.Models;
    using Shared.Views.SolutionPageView;

    /// <summary>
    /// Interaction logic for SolutionControl.xaml
    /// </summary>
    public partial class SolutionControl : ISolutionControl
    {
        /////////////////////////////////////////////////////////
        #region Properties

        private Solution Solution => DataContext as Solution;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionControl()
        {
            DataContextChanged += SolutionControl_DataContextChanged;

            InitializeComponent();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private void SetImageBinding()
        {
            if (Solution?.FileSystem == null)
                return;

            var converter = new PathToSystemImageConverter(Solution.FileSystem);
            var binding = new Binding("SolutionPath")
            {
                Source = Solution,
                Converter = converter
            };
            SolutionFileImage.SetBinding(Image.SourceProperty, binding);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        private void OpenSolution_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Solution?.TriggerOpenSolution_Executed(Solution, e);
        }

        private void OpenSolution_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Solution?.TriggerOpenSolution_CanExecute(Solution, e);
        }

        private void AlterSolution_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Solution?.TriggerAlterSolution_Executed(Solution, e);
        }

        private void AlterSolution_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Solution?.TriggerAlterSolution_CanExecute(Solution, e);
        }

        void SolutionControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Solution != null)
                SetImageBinding();
        }

        #endregion
    }
}
