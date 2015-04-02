namespace SolutionStartPage.Shared.DAL
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public interface IFileSystem
    {
        IEnumerable<FileInfo> GetFilesInDirectory(string directory, string pattern);

        DirectoryInfo GetParentDirectory(string directory);

        bool DirectoryExists(string directory);

        bool FileExists(string file);

        FileVersionInfo GetFileVersionInfo(string file);

        void WriteAllTextToFile(string file, string contents);

        void DeleteFile(string file);
    }
}