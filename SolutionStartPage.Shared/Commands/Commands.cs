namespace SolutionStartPage.Shared.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;

    [ExcludeFromCodeCoverage]
    public static class Commands
    {
        #region OpenSolution

        private static RoutedUICommand _openSolution;
        public static RoutedCommand OpenSolution => _openSolution ?? (_openSolution = new RoutedUICommand("OpenSolution",
                                                                                                          "OpenSolution",
                                                                                                          typeof(Commands)));

        #endregion

        #region AlterSolution

        private static RoutedUICommand _alterSolution;
        public static RoutedCommand AlterSolution => _alterSolution ?? (_alterSolution = new RoutedUICommand("AlterSolution",
                                                                                                             "AlterSolution",
                                                                                                             typeof(Commands)));

        #endregion

        #region AlterSolutionGroup

        private static RoutedUICommand _alterSolutionGroup;
        public static RoutedCommand AlterSolutionGroup => _alterSolutionGroup ?? (_alterSolutionGroup = new RoutedUICommand("AlterSolutionGroup",
                                                                                                                            "AlterSolutionGroup",
                                                                                                                            typeof(Commands)));

        #endregion

        #region AlterPage

        private static RoutedUICommand _alterPage;
        public static RoutedCommand AlterPage => _alterPage ?? (_alterPage = new RoutedUICommand("AlterPage",
                                                                                                 "AlterPage",
                                                                                                 typeof(Commands)));

        #endregion
    }
}