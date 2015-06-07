namespace SolutionStartPage.Vs2013.Models
{
    using EnvDTE80;
    using Shared.Models;

    public class VsIde : IIde
    {
        /////////////////////////////////////////////////////////

        #region Fields

        private DTE2 _dte;

        #endregion

        /////////////////////////////////////////////////////////

        #region IIde Member

        string IIde.Edition => _dte.Edition;

        object IIde.IdeAccess
        {
            set { _dte = value as DTE2; }
        }

        int IIde.LCID => _dte.LocaleID;

        void IIde.OpenSolution(string path)
        {
            if (path != null)
                _dte.ExecuteCommand("File.OpenProject", $"\"{path}\"");
        }

        #endregion
    }
}