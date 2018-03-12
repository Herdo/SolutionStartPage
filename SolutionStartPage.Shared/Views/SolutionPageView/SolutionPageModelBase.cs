namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using DAL;
    using Models;
    using static Utilities;

    public abstract class SolutionPageModelBase : ISolutionPageModel
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const int _MAXIMUM_RETRIES = 15;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

        private readonly IFileSystem _fileSystem;
        private readonly string _settingsFilePath;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected SolutionPageModelBase(IFileSystem fileSystem, string settingsFileName)
        {
            _fileSystem = fileSystem;
            _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                settingsFileName);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region ISolutionPageModel Member

        async Task<SolutionPageConfiguration> ISolutionPageModel.LoadConfiguration()
        {
            if (!_fileSystem.FileExists(_settingsFilePath))
                return new SolutionPageConfiguration();

            return await Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(SolutionPageConfiguration));

                for (var i = 0; i <= _MAXIMUM_RETRIES; i++)
                {
                    try
                    {
                        using (var reader = new StreamReader(_settingsFilePath))
                            return (SolutionPageConfiguration)serializer.Deserialize(reader);
                    }
                    catch (Exception)
                    {
                        if (i >= _MAXIMUM_RETRIES)
                            return new SolutionPageConfiguration();
                    }
                }
                return new SolutionPageConfiguration();
            }).ConfigureAwait(false);
        }

        async Task ISolutionPageModel.SaveConfiguration(SolutionPageConfiguration configuration)
        {
            ThrowIfNull(configuration, nameof(configuration));

            await Task.Run(async () =>
            {
                var serializer = new XmlSerializer(typeof(SolutionPageConfiguration));

                for (var i = 0; i <= _MAXIMUM_RETRIES; i++)
                {
                    try
                    {
                        using (var writer = new StreamWriter(_settingsFilePath))
                        {
                            serializer.Serialize(writer, configuration);
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        if (i >= _MAXIMUM_RETRIES)
                            return;
                    }
                    await Task.Delay(200);
                }
            }).ConfigureAwait(false);
        }

        IEnumerable<FileInfo> ISolutionPageModel.GetFilesInDirectory(string directory, string pattern)
        {
            return _fileSystem.GetFilesInDirectory(directory, pattern);
        }

        DirectoryInfo ISolutionPageModel.GetParentDirectory(string directory)
        {
            return _fileSystem.GetParentDirectory(directory);
        }

        bool ISolutionPageModel.DirectoryExists(string directory)
        {
            return _fileSystem.DirectoryExists(directory);
        }

        bool ISolutionPageModel.FileExists(string file)
        {
            return _fileSystem.FileExists(file);
        }

        #endregion
    }
}