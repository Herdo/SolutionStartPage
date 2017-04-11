namespace StartPageDebugHelper.Actions.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
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
        private static readonly ReadOnlyCollection<string> ValidVisualStudioEditions;
        private static readonly ReadOnlyDictionary<string, VsVersionInformation> VisualStudioVersionMapping;

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
                "14.0",
                "15.0"
            });
            ValidVisualStudioEditions = new ReadOnlyCollection<string>(new[]
            {
                "Community",
                "Professional",
                "Enterprise"
            });
            VisualStudioVersionMapping = new ReadOnlyDictionary<string, VsVersionInformation>(new Dictionary<string, VsVersionInformation>
            {
                {"10.0", new VsVersionInformation
                {
                    InternalVersion = "10.0",
                    PublicVersion = "2010",
                    UseLegacyPath = true
                }},
                {"12.0", new VsVersionInformation
                {
                    InternalVersion = "12.0",
                    PublicVersion = "2013",
                    UseLegacyPath = true
                }},
                {"14.0", new VsVersionInformation
                {
                    InternalVersion = "14.0",
                    PublicVersion = "2015",
                    UseLegacyPath = true
                }},
                {"15.0", new VsVersionInformation
                {
                    InternalVersion = "15.0",
                    PublicVersion = "2017"
                }},
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
            var vsStartPageDir = Path.Combine(documentDir, $"Visual Studio {vsVersion.PublicVersion}", "StartPages");
            var vsPrivateAssembliesDir = ResolvePrivateAssemblyDirectory(vsVersion);

            // Compute xaml files
            var results = (from xamlFile in xamlFiles
                           let fileName = Path.GetFileName(xamlFile)
                           where fileName != null
                           select new FileData(FileType.XAML, xamlFile, Path.Combine(vsStartPageDir, fileName), null, null)).ToList();

            // Compute dll files
            results.AddRange(from dllFile in dllFiles
                             let fileName = Path.GetFileName(dllFile)
                             where fileName != null
                             let targetFile = Path.Combine(vsPrivateAssembliesDir, fileName)
                             let sourceVersion = GetVersionForFile(dllFile)
                             let targetVersion = GetVersionForFile(targetFile)
                             select new FileData(FileType.DLL, dllFile, targetFile, sourceVersion, targetVersion));

            // Compute pdb files
            results.AddRange(from pdbFile in pdbFiles
                             let fileName = Path.GetFileName(pdbFile)
                             where fileName != null
                             let targetFile = Path.Combine(vsPrivateAssembliesDir, fileName)
                             let sourceVersion = GetVersionForFile(pdbFile)
                             let targetVersion = GetVersionForFile(targetFile)
                             select new FileData(FileType.PDB, pdbFile, targetFile, sourceVersion, targetVersion));

            return results.ToArray();
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        /// <summary>
        /// Gets a pair for the Visual Studio version
        /// </summary>
        /// <returns>The version pair.</returns>
        private static VsVersionInformation GetVisualStudioVersion()
        {
            var version = GetUserSelection("Select a Visual Studio version", ValidVisualStudioVersions);
            return VisualStudioVersionMapping[version];
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

        private static string ResolvePrivateAssemblyDirectory(VsVersionInformation vsVersion)
        {
            if (vsVersion.UseLegacyPath)
                return PrivateAssemblyPathResolver_Legacy(vsVersion.InternalVersion);
            
            vsVersion.Edition = GetUserSelection("Select a Visual Studio edition", ValidVisualStudioEditions);
            return PrivateAssemblyPathResolver(vsVersion.PublicVersion, vsVersion.Edition);
        }

        private static string PrivateAssemblyPathResolver_Legacy(string internalVersion)
        {
            return Path.Combine(Program.ProgramFilesDirectory,
                $"Microsoft Visual Studio {internalVersion}\\Common7\\IDE\\PrivateAssemblies\\");
        }

        private static string PrivateAssemblyPathResolver(string publicVersion, string edition)
        {
            return Path.Combine(Program.ProgramFilesDirectory,
                $"Microsoft Visual Studio\\{publicVersion}\\{edition}\\Common7\\IDE\\PrivateAssemblies\\");
        }

        private static Version GetVersionForFile(string file)
        {
            if (!File.Exists(file))
                return null;

            var fiv = FileVersionInfo.GetVersionInfo(file);

            // Try product version before file version
            // If product version cannot be parsed, use the file version
            // If neither succeeds, return null (version cannot be determined)
            return Version.TryParse(fiv.ProductVersion, out Version version)
                || Version.TryParse(fiv.FileVersion, out version)
                    ? version
                    : null;
        }

        #endregion
    }
}