namespace StartPageDebugHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Actions;
    using Actions.Implementations;
    using static System.Console;

    internal class Program
    {
        /////////////////////////////////////////////////////////

        #region Properties

        public static string ProgramFilesDirectory { get; private set; }

        public static string BinDirectory { get; private set; }

        #endregion

        /////////////////////////////////////////////////////////

        #region Main

        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                WriteLine("Invalid amount of arguments.");
                WriteLine("Expected arguments:");
                WriteLine("[0] - Program files directory (cointaining Visual Studio directories)");
                WriteLine("[1] - Working directory (bin directory of start page output project)");
            }
            else
            {
                if (CheckDirectoryExistence(args[0], dir => { ProgramFilesDirectory = dir; })
                    && CheckDirectoryExistence(args[1], dir => { BinDirectory = dir; }))
                {
                    var rootAction = ConstructActionTree();

                    while (true)
                    {
                        rootAction.ExecuteAction();

                        ResetColor();
                        WriteLine("Press any <Enter> to return to the main menu...");
                        ReadLine();
                    }
                }
            }
        }

        private static bool CheckDirectoryExistence(string dir, Action<string> targetPropertySetter)
        {
            if (!Directory.Exists(dir))
            {
                WriteLine("Cannot find direcotry '{0}'.", dir);
                return false;
            }
            targetPropertySetter(dir);
            return true;
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Private Methods

        private static IActionProvider ConstructActionTree()
        {
            return new ActionSet("Main Menu", new Dictionary<int, IActionProvider>
            {
                {0, new ExecuteableAction("Exit", () => Environment.Exit(0))},
                {1, new ExecuteableAction("Copy Debug Data", CopyDebugAction.CopyData)},
                {2, new ExecuteableAction("Clean Debug Data", CleanDebugAction.CleanData)},
            });
        }

        private static void PrintColored(string message, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine(message);
            ResetColor();
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region Internal Methods

        internal static void PrintSuccess(string successMessage)
        {
            PrintColored(successMessage, ConsoleColor.DarkGreen);
        }

        internal static void PrintWarning(string warningMessage)
        {
            PrintColored(warningMessage, ConsoleColor.DarkYellow);
        }

        internal static void PrintError(string errorMessage)
        {
            PrintColored(errorMessage, ConsoleColor.DarkRed);
        }

        #endregion
    }
}