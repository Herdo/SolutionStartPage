namespace SolutionStartPage.Core.BLL.Interfaces
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Shared.BLL.Interfaces;
    using static Shared.Utilities;

    [ExcludeFromCodeCoverage]
    public class SystemInterface : ISystemInterface
    {
        /////////////////////////////////////////////////////////

        #region ISystemInterface Member

        void ISystemInterface.StartProcess(ProcessStartInfo startInfo, Action<Process> modifyAction)
        {
            ThrowIfNull(startInfo, nameof(startInfo));

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