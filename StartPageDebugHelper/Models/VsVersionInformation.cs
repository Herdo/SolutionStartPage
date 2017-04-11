namespace StartPageDebugHelper.Models
{
    public class VsVersionInformation
    {
        public string InternalVersion { get; set; }

        public string PublicVersion { get; set; }

        public string Edition { get; set; }

        public bool UseLegacyPath { get; set; }
    }
}