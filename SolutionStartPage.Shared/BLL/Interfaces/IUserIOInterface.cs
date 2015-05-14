namespace SolutionStartPage.Shared.BLL.Interfaces
{
    using System.Windows.Input;

    public interface IUserIOInterface
    {
        /// <summary>
        /// Checks if the <paramref name="key"/> is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        bool IsModifierKeyDown(ModifierKeys key);
    }
}