namespace StartPageDebugHelper.Actions.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Models;
    using static System.Console;
    using static System.String;

    public abstract class DebugActionBase
    {
        /////////////////////////////////////////////////////////

        #region Fields

        private static readonly ReadOnlyCollection<string> ValidBuildConfigurations;
        private static readonly ReadOnlyCollection<string> ValidVisualStudioVersions;
        private static readonly ReadOnlyDictionary<string, string> VisualStudioVersionMapping;

        #endregion

        /////////////////////////////////////////////////////////

        #region Constructors

        static DebugActionBase()
        {
            ValidBuildConfigurations = new ReadOnlyCollection<string>(new[]
            {
                "Debug",
                "Release"
            });
            ValidVisualStudioVersions = new ReadOnlyCollection<string>(new[]
            {
                "10.0",
                "12.0",
                "14.0"
            });
            VisualStudioVersionMapping = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
            {
                {"10.0", "2010"},
                {"12.0", "2013"},
                {"14.0", "2015"}
            });
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Protected Methods

        /// <summary>
        /// Gets a collection of file path pairs, used in the derived action.
        /// </summary>
        /// <returns>A collection of file path pairs.</returns>
        /// <remarks>The <see cref="Tuple"/>s consist of the following parts: file-extension, source-file, target-file.</remarks>
        protected static FileData[] GetFilesForAction()
        {
            var vsVersion = GetVisualStudioVersion();
            var sourceDirectory = GetSourceDirectory();

            var xamlFiles = Directory.GetFiles(sourceDirectory, "*.xaml", SearchOption.TopDirectoryOnly);
            var dllFiles = Directory.GetFiles(sourceDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            var pdbFiles
                = Directory.GetFiles(sourceDirectory, "*.pdb", SearchOption.TopDirectoryOnly);

            var documentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var vsStartPageDir = Path.Combine(documentDir, $"Visual Studio {vsVersion.Item2}", "StartPages");
            var vsPrivateAssembliesDir = Path.Combine(Program.ProgramFilesDirectory,
                $"Microsoft Visual Studio {vsVersion.Item1}/Common7/IDE/PrivateAssemblies/");

            // Compute xaml files
            var results = (from xamlFile in xamlFiles
                let fileName = Path.GetFileName(xamlFile)
                where fileName != null
                select new FileData(FileType.XAML, xamlFile, Path.Combine(vsStartPageDir, fileName))).ToList();

            // Compute dll files
            results.AddRange(from dllFile in dllFiles
                let fileName = Path.GetFileName(dllFile)
                where fileName != null
                select new FileData(FileType.DLL, dllFile, Path.Combine(vsPrivateAssembliesDir, fileName)));

            // Compute pdb files
            results.AddRange(from pdbFile in pdbFiles
                let fileName = Path.GetFileName(pdbFile)
                where fileName != null
                select new FileData(FileType.PDB, pdbFile, Path.Combine(vsPrivateAssembliesDir, fileName)));

            return results.ToArray();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        /// <summary>
        /// Gets a pair for the Visual Studio version
        /// </summary>
        /// <returns>The version pair.</returns>
        /// <remarks>The <see cref="Tuple"/> consists of the following parts: short/internal version, display version/year.</remarks>
        private static Tuple<string, string> GetVisualStudioVersion()
        {
            var version = GetUserSelection("Select a Visual Studio version", ValidVisualStudioVersions);
            return new Tuple<string, string>(version, VisualStudioVersionMapping[version]);
        }

        private static string GetSourceDirectory()
        {
            string sourceDirectory;
            do
            {
                var buildConfiguration = GetBuildConfiguration();
                sourceDirectory = Path.Combine(Program.BinDirectory, buildConfiguration);

                if (Directory.Exists(sourceDirectory)) continue;
                WriteLine("Directory '{0}' doesn't exist.", sourceDirectory);
                sourceDirectory = null;
            } while (sourceDirectory == null);
            return sourceDirectory;
        }

        private static string GetBuildConfiguration()
        {
            return GetUserSelection("Select a build configuration", ValidBuildConfigurations);
        }

        private static string GetUserSelection(string message, ReadOnlyCollection<string> options)
        {
            string result;

            do
            {
                Write($"{message} [ ");
                for (var i = 0; i < options.Count; i++)
                {
                    Write(options[i]);
                    if (i != options.Count - 1)
                        Write(" | ");
                }
                WriteLine(" ]:");
                result = ReadLine();
            } while (!IsNullOrWhiteSpace(result)
                     && !options.Contains(result));

            return result;
        }

        #endregion
    }
}