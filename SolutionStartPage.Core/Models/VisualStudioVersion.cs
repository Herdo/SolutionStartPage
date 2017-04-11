namespace SolutionStartPage.Core.Models
{
    using System;
    using System.IO;
    using Annotations;
    using Shared.DAL;
    using Shared.Models;
    using static Shared.Utilities;

    public class VisualStudioVersion : IVisualStudioVersion
    {
        /////////////////////////////////////////////////////////

        #region Fields

        private readonly Version _fullVersion;
        private readonly string _longVersion;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        public VisualStudioVersion([NotNull] IFileSystem fileSystem)
        {
            ThrowIfNull(fileSystem, nameof(fileSystem));

            if (_fullVersion == null)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "devenv.exe");

                if (fileSystem.FileExists(path))
                {
                    var fvi = fileSystem.GetFileVersionInfo(path);

                    var verName = fvi.ProductVersion;

                    for (var i = 0; i < verName.Length; i++)
                    {
                        // Iterate until end of version string
                        if (!char.IsDigit(verName, i) && verName[i] != '.')
                        {
                            verName = verName.Substring(0, i);
                            break;
                        }
                    }
                    _fullVersion = new Version(verName);
                    _longVersion = GetLongVisualStudioVersion();
                }
                else
                    _fullVersion = new Version(0, 0); // Not running inside Visual Studio!
            }
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private string GetLongVisualStudioVersion()
        {
            switch (_fullVersion.Major)
            {
                case 8:
                    return "2005";
                case 9:
                    return "2008";
                case 10:
                    return "2010";
                case 11:
                    return "2012";
                case 12:
                    return "2013";
                case 14:
                    return "2015";
                case 15:
                    return "2017";
                default:
                    throw new NotSupportedException("Version is not supported.");
            }
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region IVisualStudioVersion Member

        Version IVisualStudioVersion.FullVersion => _fullVersion;

        string IVisualStudioVersion.LongVersion => _longVersion;

        bool IVisualStudioVersion.Vs2015OrLater => _fullVersion >= new Version(14, 0);

        bool IVisualStudioVersion.Vs2013OrLater => _fullVersion >= new Version(12, 0);

        bool IVisualStudioVersion.Vs2012OrLater => _fullVersion >= new Version(11, 0);

        bool IVisualStudioVersion.Vs2010OrLater => _fullVersion >= new Version(10, 0);

        bool IVisualStudioVersion.Vs2008OrOlder => _fullVersion < new Version(9, 0);

        bool IVisualStudioVersion.Vs2005 => _fullVersion.Major == 8;

        bool IVisualStudioVersion.Vs2008 => _fullVersion.Major == 9;

        bool IVisualStudioVersion.Vs2010 => _fullVersion.Major == 10;

        bool IVisualStudioVersion.Vs2012 => _fullVersion.Major == 11;

        bool IVisualStudioVersion.Vs2013 => _fullVersion.Major == 12;

        bool IVisualStudioVersion.Vs2015 => _fullVersion.Major == 14;

        #endregion
    }
}