namespace SolutionStartPage.Vs2010.Models
{
    using System;
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

        string IIde.Edition
        {
            get { return _dte.Edition; }
        }

        object IIde.IdeAccess
        {
            set { _dte = value as DTE2; }
        }

        int IIde.LCID
        {
            get { return _dte.LocaleID; }
        }

        void IIde.OpenSolution(string path)
        {
            if (path != null)
                _dte.ExecuteCommand("File.OpenProject", String.Format("\"{0}\"", path));
        }

        #endregion
    }
}