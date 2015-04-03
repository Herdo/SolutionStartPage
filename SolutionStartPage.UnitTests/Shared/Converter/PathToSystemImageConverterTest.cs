namespace SolutionStartPage.UnitTests.Shared.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Converter;
    using SolutionStartPage.Shared.DAL;
    using Telerik.JustMock;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PathToSystemImageConverterTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertBack_NotSupportedException()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            converter.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        public void Convert_NoString_value()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            var result = converter.Convert(0.0, null, null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_NoExtension_value()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            var result = converter.Convert(@"D:\fileWithoutExtension", null, null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_Null_value()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            var result = converter.Convert(null, null, null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_WithExtension_value()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();
            Mock.Arrange(() => fileSystem.WriteAllTextToFile(Arg.AnyString, Arg.AnyString)).DoInstead<string, string>(File.WriteAllText);

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            var result = converter.Convert(@"D:\fileWithoutExtension.sln", null, null, null);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_RelativeWithExtension_value()
        {
            // Arrange
            var fileSystem = Mock.Create<IFileSystem>();
            Mock.Arrange(() => fileSystem.WriteAllTextToFile(Arg.AnyString, Arg.AnyString)).DoInstead<string, string>(File.WriteAllText);
            Mock.Arrange(() => fileSystem.DirectoryExists(@"\fileWithoutExtension.sln")).Returns(true);
            Mock.Arrange(() => fileSystem.FileExists(@"\fileWithoutExtension.sln")).Returns(true);

            var converter = new PathToSystemImageConverter(fileSystem);

            // Act
            converter.Convert(@"\fileWithoutExtension.sln", null, null, null);
        }
    }
}
