namespace StartPageDebugHelper.Actions
{
    using System;
    using System.Collections.Generic;
    using static System.Console;
    using static System.Int32;

    public class ActionSet : BaseActionProvider,
        IActionProvider
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly IDictionary<int, IActionProvider> _set;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSet"/> class with a given set of executeable actions.
        /// </summary>
        /// <param name="displayName">The display name for the provider to display.</param>
        /// <param name="set">A set of <see cref="IActionProvider"/>s.</param>
        /// <exception cref="ArgumentNullException"><paramref name="set"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="set.Count"/> is not greater than 0.</exception>
        public ActionSet(string displayName, IDictionary<int, IActionProvider> set)
            : base(displayName)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));
            if (set.Count <= 0)
                throw new ArgumentException("Count must be greater than 0.", nameof(set.Count));

            _set = set;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region IActionProvider Member

        string IActionProvider.DisplayName => DisplayName;

        void IActionProvider.ExecuteAction()
        {
            PrintActionBase();

            int resultAction;
            do
            {
                WriteLine("Select an action:");
                foreach (var actionProvider in _set)
                    WriteLine($"[{actionProvider.Key}] - {actionProvider.Value.DisplayName}");
            } while (!TryParse(ReadLine(), out resultAction)
                     && !_set.ContainsKey(resultAction));

            _set[resultAction].ExecuteAction();
        }

        #endregion
    }
}