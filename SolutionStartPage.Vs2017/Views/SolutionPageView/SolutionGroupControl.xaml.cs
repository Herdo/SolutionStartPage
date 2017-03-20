namespace SolutionStartPage.Vs2017.Views.SolutionPageView
{
    using System.Windows.Input;
    using Shared.Models;
    using Shared.Views.SolutionPageView;

    /// <summary>
    /// Interaction logic for SolutionGroupControl.xaml
    /// </summary>
    public partial class SolutionGroupControl : ISolutionGroupControl
    {
        /////////////////////////////////////////////////////////

        #region Properties

        private SolutionGroup SolutionGroup => DataContext as SolutionGroup;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        public SolutionGroupControl()
        {
            InitializeComponent();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Event Handler

        private void AlterSolutionGroup_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SolutionGroup?.TriggerAlterSolutionGroup_Executed(e);
        }

        private void AlterSolutionGroup_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            SolutionGroup?.TriggerAlterSolutionGroup_CanExecute(e);
        }

        #endregion
    }
}