namespace SolutionStartPage.Control.SolutionsPart
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;

    /// <summary>
    /// Interaction logic for SolutionGroupControl.xaml
    /// </summary>
    public partial class SolutionGroupControl
    {
        /////////////////////////////////////////////////////////
        #region Properties

        private SolutionGroup SolutionGroup
        {
            get { return DataContext as SolutionGroup; }
        }

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
            if (SolutionGroup != null)
                SolutionGroup.TriggerAlterSolutionGroup_Executed(e);
        }

        #endregion

        private void AlterSolutionGroup_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SolutionGroup != null)
                SolutionGroup.TriggerAlterSolutionGroup_CanExecute(e);
        }
    }
}
