namespace SolutionStartPage.UnitTests.Shared.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Converter;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BoolToVisibilityConverterTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertBack_NotSupportedException()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            converter.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        public void Convert_VisibleToVisible()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            var result = converter.Convert(true, null, null, null);

            // Assert
            Assert.AreEqual(result, Visibility.Visible);
        }

        [TestMethod]
        public void Convert_VisibleToCollapsed()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            var result = converter.Convert(true, null, "invert", null);

            // Assert
            Assert.AreEqual(result, Visibility.Collapsed);
        }

        [TestMethod]
        public void Convert_CollapsedToCollapsed()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            var result = converter.Convert(false, null, null, null);

            // Assert
            Assert.AreEqual(result, Visibility.Collapsed);
        }

        [TestMethod]
        public void Convert_CollapsedToVisible()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            var result = converter.Convert(false, null, "invert", null);

            // Assert
            Assert.AreEqual(result, Visibility.Visible);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Convert_InvalidCastException_value()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            converter.Convert(2.0, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Convert_InvalidCastException_parameter()
        {
            // Arrange
            var converter = new BoolToVisibilityConverter();

            // Act
            converter.Convert(true, null, 2.0, null);
        }
    }
}
