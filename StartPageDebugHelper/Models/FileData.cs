namespace StartPageDebugHelper.Models
{
    public class FileData
    {
        /////////////////////////////////////////////////////////

        #region Properties 

        public FileType FileType { get; }
        public string SourcePath { get; }
        public string TargetPath { get; }

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        public FileData(FileType fileType, string sourcePath, string targetPath)
        {
            FileType = fileType;
            SourcePath = sourcePath;
            TargetPath = targetPath;
        }

        #endregion
    }
}