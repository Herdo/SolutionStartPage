namespace SolutionStartPage.Core.BLL.Interfaces
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Shared.BLL.Interfaces;

    [ExcludeFromCodeCoverage]
    public class SystemInterface : ISystemInterface
    {
        /////////////////////////////////////////////////////////
        #region ISystemInterface Member

        void ISystemInterface.StartProcess(ProcessStartInfo startInfo, Action<Process> modifyAction)
        {
            if (startInfo == null)
                throw new ArgumentNullException(nameof(startInfo));

            var process = new Process
            {
                StartInfo = startInfo
            };

            modifyAction?.Invoke(process);

            process.Start();
        }

        #endregion
    }
}