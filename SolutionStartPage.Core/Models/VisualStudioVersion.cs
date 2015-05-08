namespace SolutionStartPage.Core.Models
{
    using System;
    using System.IO;
    using Shared.DAL;

    public class VisualStudioVersion
    {
        /////////////////////////////////////////////////////////
        #region Properties

        public Version FullVersion { get; }

        public string LongVersion { get; }

        public bool Vs2015OrLater => FullVersion >= new Version(14, 0);

        public bool Vs2013OrLater => FullVersion >= new Version(12, 0);

        public bool Vs2012OrLater => FullVersion >= new Version(11, 0);

        public bool Vs2010OrLater => FullVersion >= new Version(10, 0);

        public bool Vs2008OrOlder => FullVersion < new Version(9, 0);

        public bool Vs2005 => FullVersion.Major == 8;

        public bool Vs2008 => FullVersion.Major == 9;

        public bool Vs2010 => FullVersion.Major == 10;

        public bool Vs2012 => FullVersion.Major == 11;

        public bool Vs2013 => FullVersion.Major == 12;

        public bool Vs2015 => FullVersion.Major == 14;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VisualStudioVersion(IFileSystem fileSystem)
        {
            if (FullVersion == null)
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
                    FullVersion = new Version(verName);
                    LongVersion = GetLongVisualStudioVersion();
                }
                else
                    FullVersion = new Version(0, 0); // Not running inside Visual Studio!
            }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private string GetLongVisualStudioVersion()
        {
            switch (FullVersion.Major)
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
                default:
                    throw new NotSupportedException("Version is not supported.");
            }
        }

        #endregion
    }
}