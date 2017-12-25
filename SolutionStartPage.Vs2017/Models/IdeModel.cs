namespace SolutionStartPage.Vs2017.Models
{
    using System;
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell.Interop;
    using Shared.Models;

    public class IdeModel : IIdeModel
    {
        /////////////////////////////////////////////////////////

        #region Private Methods

        private static DTE2 GetDte(object dataContext)
        {
            var dataSource = dataContext as IVsUIDataSource;
            if (dataSource != null)
            {
                IVsUIObject obj;
                object result;
                if (dataSource.GetValue("DTE", out obj) == 0
                    && obj.get_Data(out result) == 0)
                {
                    return result as DTE2;
                }
            }
            return null;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region IIdeModel Member

        IIde IIdeModel.GetIde(object dataContext, Func<IIde> ideResolver)
        {
            var dte = GetDte(dataContext);
            if (dte == null)
                return null;

            var ide = ideResolver();
            ide.IdeAccess = dte;
            return ide;
        }

        #endregion
    }
}