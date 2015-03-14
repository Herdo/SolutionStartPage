namespace SolutionStartPage.Vs2013.Models
{
    using System;
    using EnvDTE80;
    using Shared.Models;

    public class VsIde : IIde
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly DTE2 _dte;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VsIde(object dte)
        {
            var dteAccessor = dte as DTE2;
            if (dteAccessor == null)
                throw new ArgumentException(@"Invalid dte object.", "dte");

            _dte = dteAccessor;
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