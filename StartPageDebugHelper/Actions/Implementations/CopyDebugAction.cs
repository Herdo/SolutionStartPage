namespace StartPageDebugHelper.Actions.Implementations
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using Models;
    using static System.Console;
    using static Program;

    public class CopyDebugAction : DebugActionBase
    {
        /////////////////////////////////////////////////////////
        #region Public Methods

        public static void CopyData()
        {
            var files = GetFilesForAction();

            if (files.Length == 0)
            {
                PrintWarning("No action executed. No files found.");
                PrintWarning("Please generate the source files by building the solution again.");
                return;
            }

            foreach (var file in files)
                CopyFile(file);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Private Methods

        private static void CopyFile(FileData file)
        {
            try
            {
                if (File.Exists(file.SourcePath))
                {
                    if (File.Exists(file.TargetPath))
                    {
                        // Check MD5 hash prior to copying - only copy files that differ
                        var sourceHash = GetFileHash(file.SourcePath);
                        var targetHash = GetFileHash(file.TargetPath);
                        if (sourceHash == targetHash)
                        {
                            PrintWarning($"Skipped file copy of {file.SourcePath} to {file.TargetPath}, because they are equal.");
                            return;
                        }
                        // Check if existing version is higher than the current version
                        if (file.SourceVersion != null
                         && file.TargetVersion != null
                         && file.TargetVersion > file.SourceVersion)
                        {
                            PrintWarning($"Skipped file copy of {file.SourcePath} to {file.TargetPath}, because the existing version is newer.");
                            return;
                        }
                    }
                    WriteLine("Copying file {0} to {1}", file.SourcePath, file.TargetPath);
                    File.Copy(file.SourcePath, file.TargetPath, true);
                    if (File.Exists(file.TargetPath))
                        PrintSuccess("File copied.");
                    else
                        PrintError("Unknown error during copy process.");
                }
                else
                {
                    PrintWarning(
                        $"Skipped file copy of {file.SourcePath} to {file.TargetPath}, because the source file doesn't exist.");
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        private static string GetFileHash(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        #endregion
    }
}