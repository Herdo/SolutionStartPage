namespace StartPageDebugHelper.Actions
{
    public interface IActionProvider
    {
        /// <summary>
        /// Gets the display name for the action.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Executes a specific action.
        /// </summary>
        void ExecuteAction();
    }
}