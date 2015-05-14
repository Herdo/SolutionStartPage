namespace SolutionStartPage.Shared.BLL.Interfaces
{
    using System;
    using System.Diagnostics;

    public interface ISystemInterface
    {
        /// <summary>
        /// Starts a process with a given <paramref name="startInfo"/>.
        /// </summary>
        /// <param name="startInfo">The info to pass to the process before starting it.</param>
        /// <param name="modifyAction">Any modify action to the process, before it get's started.</param>
        /// <exception cref="ArgumentNullException"><paramref name="startInfo"/> is null.</exception>
        void StartProcess(ProcessStartInfo startInfo, Action<Process> modifyAction = null);
    }
}