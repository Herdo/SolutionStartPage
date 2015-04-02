namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Models;

    public interface ISolutionPageModel
    {
        SolutionPageConfiguration LoadConfiguration();

        Task SaveConfiguration(SolutionPageConfiguration groups);

        IEnumerable<FileInfo> GetFilesInDirectory(string directory, string pattern);

        DirectoryInfo GetParentDirectory(string directory);

        bool DirectoryExists(string directory);

        bool FileExists(string file);
    }
}