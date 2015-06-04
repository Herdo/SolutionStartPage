namespace SolutionStartPage.Shared.Models
{
    using System;

    public interface IVisualStudioVersion
    {
        /// <summary>
        /// Gets the full Visual Studio version number.
        /// </summary>
        Version FullVersion { get; }

        /// <summary>
        /// Gets a long string representation for the <see cref="FullVersion"/>.
        /// </summary>
        string LongVersion { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2015 or later.
        /// </summary>
        bool Vs2015OrLater { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2013 or later.
        /// </summary>
        bool Vs2013OrLater { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2012 or later.
        /// </summary>
        bool Vs2012OrLater { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2010 or later.
        /// </summary>
        bool Vs2010OrLater { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2008 or older.
        /// </summary>
        bool Vs2008OrOlder { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2005.
        /// </summary>
        bool Vs2005 { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2008.
        /// </summary>
        bool Vs2008 { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2010.
        /// </summary>
        bool Vs2010 { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2012.
        /// </summary>
        bool Vs2012 { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2013.
        /// </summary>
        bool Vs2013 { get; }

        /// <summary>
        /// Gets if the Visual Studio version is 2015.
        /// </summary>
        bool Vs2015 { get; }
    }
}