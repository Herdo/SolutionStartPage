namespace SolutionStartPage.UnitTests.Shared.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Converter;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SolutionWidthToTextWidthConverterTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertBack_NotSupportedException()
        {
            // Arrange
            var converter = new SolutionWidthToTextWidthConverter();

            // Act
            converter.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        public void Convert_Success()
        {
            // Arrange
            var converter = new SolutionWidthToTextWidthConverter();

            // Act
            var result = converter.Convert(64.0, null, null, null);

            // Assert
            Assert.AreEqual(32.0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Convert_InvalidCastException_value()
        {
            // Arrange
            var converter = new SolutionWidthToTextWidthConverter();

            // Act
            converter.Convert("foo", null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Convert_NullReferenceException_value()
        {
            // Arrange
            var converter = new SolutionWidthToTextWidthConverter();

            // Act
            converter.Convert(null, null, null, null);
        }
    }
}
