namespace SolutionStartPage.Controls.SolutionsPart
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;

    /// <summary>
    /// Interaction logic for SolutionControl.xaml
    /// </summary>
    public partial class SolutionControl : UserControl
    {
        /////////////////////////////////////////////////////////
        #region Properties

        private Solution Solution
        {
            get { return DataContext as Solution; }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionControl()
        {
            InitializeComponent();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Event Handler

        private void OpenSolution_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Solution != null)
                Solution.TriggerOpenSolution_Executed(Solution, e);
        }

        private void OpenSolution_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Solution != null)
                Solution.TriggerOpenSolution_CanExecute(Solution, e);
        }

        #endregion

        private void AlterSolution_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Solution != null)
                Solution.TriggerAlterSolution_Executed(Solution, e);
        }

        private void AlterSolution_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Solution != null)
                Solution.TriggerAlterSolution_CanExecute(Solution, e);
        }
    }
}
