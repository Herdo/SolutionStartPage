namespace StartPageDebugHelper.Actions
{
    using System;
    using static System.Console;
    using static System.String;

    public abstract class BaseActionProvider
    {
        /////////////////////////////////////////////////////////

        #region Fields

        protected readonly string DisplayName;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseActionProvider"/> class.
        /// </summary>
        /// <param name="displayName">The display name for the provider to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="displayName"/> is null, empty or whitespace.</exception>
        protected BaseActionProvider(string displayName)
        {
            if (IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            DisplayName = displayName;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Protected Methods

        protected void PrintActionBase()
        {
            Clear();
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine($"Executing '{DisplayName}'.");
            ResetColor();
        }

        #endregion
    }
}