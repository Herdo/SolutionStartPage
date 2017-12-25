namespace StartPageDebugHelper.Models
{
    using System;

    public class FileData
    {
        /////////////////////////////////////////////////////////
        #region Properties 

        public FileType FileType { get; }
        public string SourcePath { get; }
        public string TargetPath { get; }
        public Version SourceVersion { get; }
        public Version TargetVersion { get; }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public FileData(FileType fileType, string sourcePath, string targetPath, Version sourceVersion, Version targetVersion)
        {
            FileType = fileType;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            SourceVersion = sourceVersion;
            TargetVersion = targetVersion;
        }

        #endregion
    }
}