namespace SolutionStartPage.UnitTests.Shared.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Converter;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InvertedBoolToOpacityConverterTest
    {
        [TestMethod]
        [ExpectedException(typeof (NotSupportedException))]
        public void ConvertBack_NotSupportedException()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            converter.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        public void Convert_TrueToHalfOpacity()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            var result = converter.Convert(true, null, "0.5", null);

            // Assert
            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void Convert_FalseToFullOpacity()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            var result = converter.Convert(false, null, null, null);

            // Assert
            Assert.AreEqual(1.0, result);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void Convert_InvalidCastException_value()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            converter.Convert(2.0, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof (FormatException))]
        public void Convert_FormatException_parameter()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            converter.Convert(true, null, "foo", null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Convert_NullReferenceException_value()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            converter.Convert(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Convert_NullReferenceException_parameter()
        {
            // Arrange
            var converter = new InvertedBoolToOpacityConverter();

            // Act
            converter.Convert(true, null, null, null);
        }
    }
}