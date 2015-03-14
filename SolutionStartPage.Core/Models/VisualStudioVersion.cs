namespace SolutionStartPage.Core.Models
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class VisualStudioVersion
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly Version _vsVersion;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        public Version FullVersion
        {
            get { return _vsVersion; }
        }

        public bool Vs2013OrLater
        {
            get { return FullVersion >= new Version(12, 0); }
        }

        public bool Vs2012OrLater
        {
            get { return FullVersion >= new Version(11, 0); }
        }

        public bool Vs2010OrLater
        {
            get { return FullVersion >= new Version(10, 0); }
        }

        public bool Vs2008OrOlder
        {
            get { return FullVersion < new Version(9, 0); }
        }

        public bool Vs2005
        {
            get { return FullVersion.Major == 8; }
        }

        public bool Vs2008
        {
            get { return FullVersion.Major == 9; }
        }

        public bool Vs2010
        {
            get { return FullVersion.Major == 10; }
        }

        public bool Vs2012
        {
            get { return FullVersion.Major == 11; }
        }

        public bool Vs2013
        {
            get { return FullVersion.Major == 12; }
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public VisualStudioVersion()
        {
            if (_vsVersion == null)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "devenv.exe");

                if (File.Exists(path))
                {
                    var fvi = FileVersionInfo.GetVersionInfo(path);

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
                    _vsVersion = new Version(verName);
                }
                else
                    _vsVersion = new Version(0, 0); // Not running inside Visual Studio!
            }
        }

        #endregion
    }
}