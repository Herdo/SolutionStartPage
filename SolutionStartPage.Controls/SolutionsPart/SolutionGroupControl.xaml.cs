using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolutionStartPage.Controls.SolutionsPart
{
    using Models;

    /// <summary>
    /// Interaction logic for SolutionGroupControl.xaml
    /// </summary>
    public partial class SolutionGroupControl : UserControl
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
