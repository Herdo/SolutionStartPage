namespace SolutionStartPage.Core.DAL
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Shared.DAL;

    [ExcludeFromCodeCoverage]
    public class FileSystem : IFileSystem
    {
        IEnumerable<FileInfo> IFileSystem.GetFilesInDirectory(string directory, string pattern)
        {
            var di = new DirectoryInfo(directory);
            return di.GetFiles(pattern, SearchOption.AllDirectories);
        }

        DirectoryInfo IFileSystem.GetParentDirectory(string directory)
        {
            return Directory.GetParent(directory);
        }

        bool IFileSystem.DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        bool IFileSystem.FileExists(string file)
        {
            return File.Exists(file);
        }

        FileVersionInfo IFileSystem.GetFileVersionInfo(string file)
        {
            return FileVersionInfo.GetVersionInfo(file);
        }

        void IFileSystem.WriteAllTextToFile(string file, string contents)
        {
            File.WriteAllText(file, contents);
        }

        void IFileSystem.DeleteFile(string file)
        {
            File.Delete(file);
        }
    }
}