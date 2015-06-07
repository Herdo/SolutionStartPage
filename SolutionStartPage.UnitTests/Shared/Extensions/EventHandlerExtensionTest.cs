namespace SolutionStartPage.UnitTests.Shared.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Extensions;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventHandlerExtensionTest
    {
        private event EventHandler SaveInvokeNormalTestSubscribed;
        private event EventHandler<EventArgs> SaveInvokeGenericTestSubscribed;
        private event PropertyChangedEventHandler SaveInvokPropertyChangedTestSubscribed;

        [TestMethod]
        public void SaveInvoke_NormalEventHandler_Subscribed()
        {
            // Arrange
            var invoked = false;
            SaveInvokeNormalTestSubscribed += (sender, args) => invoked = true;

            // Act
            SaveInvokeNormalTestSubscribed.SafeInvoke(this, new EventArgs());

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SaveInvoke_NormalEventHandler_NotSubscribed()
        {
            // Arrange
            var invoked = false;

            // Act
            SaveInvokeNormalTestSubscribed.SafeInvoke(this, new EventArgs());

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SaveInvoke_GenericEventHandler_Subscribed()
        {
            // Arrange
            var invoked = false;
            SaveInvokeGenericTestSubscribed += (sender, args) => invoked = true;

            // Act
            SaveInvokeGenericTestSubscribed.SafeInvoke(this, new EventArgs());

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SaveInvoke_GenericEventHandler_NotSubscribed()
        {
            // Arrange
            var invoked = false;

            // Act
            SaveInvokeGenericTestSubscribed.SafeInvoke(this, new EventArgs());

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SaveInvoke_PropertyChangedEventHandler_Subscribed()
        {
            // Arrange
            var invoked = false;
            SaveInvokPropertyChangedTestSubscribed += (sender, args) => invoked = true;

            // Act
            SaveInvokPropertyChangedTestSubscribed.SafeInvoke(this, null);

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SaveInvoke_PropertyChangedEventHandler_NotSubscribed()
        {
            // Arrange
            var invoked = false;

            // Act
            SaveInvokPropertyChangedTestSubscribed.SafeInvoke(this, null);

            // Assert
            Assert.IsFalse(invoked);
        }
    }
}