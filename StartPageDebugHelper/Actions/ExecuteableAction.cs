namespace StartPageDebugHelper.Actions
{
    using System;

    public class ExecuteableAction : BaseActionProvider,
                                     IActionProvider
    {
        /////////////////////////////////////////////////////////
        #region Fields
            
        private readonly Action _action;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteableAction"/> class with a specific <paramref name="action"/> to execute.
        /// </summary>
        /// <param name="displayName">The display name for the provider to display.</param>
        /// <param name="action">The action to execute, when <see cref="IActionProvider.ExecuteAction"/> is called.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public ExecuteableAction(string displayName, Action action)
            : base(displayName)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            _action = action;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IActionProvider Member

        string IActionProvider.DisplayName => DisplayName;

        void IActionProvider.ExecuteAction()
        {
            PrintActionBase();
            _action();
        }

        #endregion
    }
}