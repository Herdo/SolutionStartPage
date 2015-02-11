namespace SolutionStartPage.Control.IDE
{
    using System;
    using EnvDTE80;

    public class VsIde : IIde
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly DTE2 _dte;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VsIde(DTE2 dte)
        {
            _dte = dte;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IIde Member

        public void OpenSolution(string path)
        {
            if (path != null)
                _dte.ExecuteCommand("File.OpenProject", String.Format("\"{0}\"", path));
        }

        #endregion
    }
}