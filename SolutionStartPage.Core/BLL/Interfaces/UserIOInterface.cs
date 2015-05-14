namespace SolutionStartPage.Core.BLL.Interfaces
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Shared.BLL.Interfaces;

    [ExcludeFromCodeCoverage]
    public class UserIOInterface : IUserIOInterface
    {
        /////////////////////////////////////////////////////////
        #region IUserIOInterface Member

        bool IUserIOInterface.IsModifierKeyDown(ModifierKeys key)
        {
            return Keyboard.Modifiers.HasFlag(key);
        }

        #endregion
    }
}