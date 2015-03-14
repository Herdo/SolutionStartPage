namespace SolutionStartPage.Vs2013.Models
{
    using System.ComponentModel;
    using System.Diagnostics;
    using EnvDTE80;
    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.Practices.Unity;
    using Shared.Funtionality;
    using Shared.Models;

    public class IdeModel : IIdeModel
    {
        /////////////////////////////////////////////////////////
        #region Private Methods

        private static DTE2 GetDte(object dataContext)
        {
            if (dataContext == null)
                return null;
            var typeDescriptor = dataContext as ICustomTypeDescriptor;
            if (typeDescriptor != null)
            {
                PropertyDescriptorCollection propertyCollection = typeDescriptor.GetProperties();
                return propertyCollection.Find("DTE", false).GetValue(dataContext) as DTE2;
            }
            var dataSource = dataContext as DataSource;
            if (dataSource != null)
            {
                return dataSource.GetValue("DTE") as DTE2;
            }
            Debug.Assert(false, "Could not get DTE instance, was " + (dataContext == null ? "null" : dataContext.GetType().ToString()));
            return null;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IIdeModel Member

        public IIde GetIde(object dataContext)
        {
            var dte = GetDte(dataContext);
            return dte == null
                ? null
                : UnityFactory.Resolve<IIde>(new ParameterOverride("dte", dte));
        }

        #endregion
    }
}