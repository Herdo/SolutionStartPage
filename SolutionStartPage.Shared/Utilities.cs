namespace SolutionStartPage.Shared
{
    using System;
    using Annotations;
    using static System.String;

    public static class Utilities
    {
        /// <summary>
        /// Checks if the <paramref name="obj"/> is null and throws an exception, if so.
        /// </summary>
        /// <param name="obj">The object to validate for null.</param>
        /// <param name="name">The name of the object in the calling location.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
        /// <example><code>
        /// view.ThrowIfNull(nameof(view));
        /// </code></example>
        public static void ThrowIfNull([CanBeNull] object obj,
            [CanBeNull] string name)
        {
            if (obj == null)
                throw new ArgumentNullException(name ?? Empty, "Expected [NotNull] parameter was null.");
        }
    }
}