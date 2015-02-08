namespace SolutionStartPage.Controls.Commands
{
    using System.Windows.Input;

    public static class Commands
    {
        #region OpenSolution

        private static RoutedUICommand _openSolution;
        public static RoutedCommand OpenSolution
        {
            get
            {
                return _openSolution ?? (_openSolution = new RoutedUICommand("OpenSolution",
                                                                             "OpenSolution",
                                                                             typeof(Commands)));
            }
        }

        #endregion

        #region AlterSolution

        private static RoutedUICommand _alterSolution;
        public static RoutedCommand AlterSolution
        {
            get
            {
                return _alterSolution ?? (_alterSolution = new RoutedUICommand("AlterSolution",
                                                                               "AlterSolution",
                                                                               typeof(Commands)));
            }
        }

        #endregion

        #region AlterSolutionGroup

        private static RoutedUICommand _alterSolutionGroup;
        public static RoutedCommand AlterSolutionGroup
        {
            get
            {
                return _alterSolutionGroup ?? (_alterSolutionGroup = new RoutedUICommand("AlterSolutionGroup",
                                                                                         "AlterSolutionGroup",
                                                                                         typeof(Commands)));
            }
        }

        #endregion

        #region AlterPage

        private static RoutedUICommand _alterPage;
        public static RoutedCommand AlterPage
        {
            get
            {
                return _alterPage ?? (_alterPage = new RoutedUICommand("AlterPage",
                                                                       "AlterPage",
                                                                       typeof(Commands)));
            }
        }

        #endregion
    }
}