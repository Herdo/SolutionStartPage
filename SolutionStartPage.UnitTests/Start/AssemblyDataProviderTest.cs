namespace SolutionStartPage.UnitTests.Start
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyDataProviderTest
    {
        [TestMethod]
        public void CheckCorrectAssemblyReferences()
        {
            // Arrange
            const int numberOfSupportedVersions = 2;
            var testAssembly = Assembly.GetAssembly(typeof(AssemblyDataProviderTest));
            var testAssemblyDirecotry = Path.GetDirectoryName(testAssembly.Location);
            Assert.IsNotNull(testAssemblyDirecotry);
            var solutionFilePath = Path.GetFullPath(Path.Combine(testAssemblyDirecotry, @"..\..\..\SolutionStartPage.sln"));
            var primaryProjectFilePath = Path.GetFullPath(Path.Combine(testAssemblyDirecotry, @"..\..\..\SolutionStartPage\SolutionStartPage.csproj"));
            var solutionRegex = new Regex("Project\\(\"\\{([\\w|-]{36})\\}\"\\) = \"(?<projectName>SolutionStartPage.Vs(?<version>\\d{4}))\".+");
            var projectRefRegex = new Regex("<ProjectReference Include=\"\\.\\.\\\\(?<projectName1>SolutionStartPage.Vs(?<version1>\\d{4}))\\\\(?<projectName2>SolutionStartPage.Vs(?<version2>\\d{4})).csproj\"\\>");

            // Act
            var solutionContent = File.ReadAllText(solutionFilePath);
            var projectContent = File.ReadAllText(primaryProjectFilePath);
            var solutionMatches = solutionRegex.Matches(solutionContent)
                .Cast<Match>()
                .OrderBy(m => int.Parse(m.Groups["version"].Value))
                .Select(m => m.Groups["projectName"].Value).ToArray();
            var projectMatches = projectRefRegex.Matches(projectContent)
                .Cast<Match>()
                .OrderBy(m => int.Parse(m.Groups["version1"].Value))
                .Select(m => (projectName1: m.Groups["projectName1"].Value, projectName2: m.Groups["projectName2"].Value)).ToArray();

            // Assert
            Assert.AreEqual(numberOfSupportedVersions, solutionMatches.Length);
            Assert.AreEqual(numberOfSupportedVersions, projectMatches.Length);
            foreach (var s in solutionMatches)
            {
                Assert.IsNotNull(projectMatches.SingleOrDefault(m => s == m.projectName1 && m.projectName1 == m.projectName2), $"The primary project is missing a project reference to the project '{s}'.");
            }
        }
    }
}