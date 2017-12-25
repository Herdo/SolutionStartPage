namespace SolutionStartPage.UnitTests.Shared
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared;

    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void ThrowIfNull_NotNull()
        {
            // Arrange
            var testObject1 = 1;
            var testObject2 = DateTime.Now;
            var testObject3 = "foo";
            var testObject4 = new Uri("http://www.google.com", UriKind.Absolute);

            // Act
            Utilities.ThrowIfNull(testObject1, nameof(testObject1));
            Utilities.ThrowIfNull(testObject2, nameof(testObject2));
            Utilities.ThrowIfNull(testObject3, nameof(testObject3));
            Utilities.ThrowIfNull(testObject4, nameof(testObject4));

            // Assert
            // Nothing to assert / No exception thrown
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowIfNull_ArgumentNullException_Integer()
        {
            // Arrange
            int? testObject = null;

            // Act
            Utilities.ThrowIfNull(testObject, nameof(testObject));

            // Assert
            // Nothing to assert / Exception thrown
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowIfNull_ArgumentNullException_DateTime()
        {
            // Arrange
            DateTime? testObject = null;

            // Act
            Utilities.ThrowIfNull(testObject, nameof(testObject));

            // Assert
            // Nothing to assert / Exception thrown
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowIfNull_ArgumentNullException_String()
        {
            // Arrange
            string testObject = null;

            // Act
            Utilities.ThrowIfNull(testObject, nameof(testObject));

            // Assert
            // Nothing to assert / Exception thrown
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ThrowIfNull_ArgumentNullException_Uri()
        {
            // Arrange
            Uri testObject = null;

            // Act
            Utilities.ThrowIfNull(testObject, nameof(testObject));

            // Assert
            // Nothing to assert / Exception thrown
        }
    }
}