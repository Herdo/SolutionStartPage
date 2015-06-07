namespace StartPageDebugHelper.Actions.Implementations
{
    using System;
    using System.IO;
    using Models;
    using static System.Console;
    using static Program;

    public class CleanDebugAction : DebugActionBase
    {
        /////////////////////////////////////////////////////////

        #region Public Methods

        public static void CleanData()
        {
            var files = GetFilesForAction();

            if (files.Length == 0)
            {
                PrintWarning("No action executed. No files found.");
                PrintWarning("Please generate the source files by building the solution again.");
                return;
            }

            foreach (var file in files)
                DeleteFile(file);
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private static void DeleteFile(FileData file)
        {
            try
            {
                if (File.Exists(file.TargetPath))
                {
                    WriteLine("Deleting file: {0}", file.TargetPath);
                    File.Delete(file.TargetPath);
                    if (!File.Exists(file.TargetPath))
                        PrintSuccess("File deleted.");
                    else
                        PrintError("Unknown error during file deletion.");
                }
                else
                {
                    PrintWarning($"Skipped file deletion of {file.TargetPath}, because it doesn't exist.");
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        #endregion
    }
}