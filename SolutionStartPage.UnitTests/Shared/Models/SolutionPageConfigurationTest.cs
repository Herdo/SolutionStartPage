namespace SolutionStartPage.UnitTests.Shared.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.Models;
    using SolutionStartPage.Shared.Views.SolutionPageView;
    using Telerik.JustMock;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SolutionPageConfigurationTest
    {
        [TestMethod]
        public void Columns_GetSet()
        {
            // Arrange
            var config = new SolutionPageConfiguration();

            // Act
            config.Columns = 2;
            var result = config.Columns;

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Columns_GetSet_SameValue()
        {
            // Arrange
            var config = new SolutionPageConfiguration {Columns = 2};

            // Act
            config.Columns = 2;
            var result = config.Columns;

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Columns_Minvalue()
        {
            // Arrange
            var config = new SolutionPageConfiguration();

            // Act
            config.Columns = -1;
            var result = config.Columns;

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Columns_Maxvalue()
        {
            // Arrange
            var config = new SolutionPageConfiguration();

            // Act
            config.Columns = 4;
            var result = config.Columns;

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ApplyViewModel_NullReferenceException()
        {
            // Arrange
            var config = new SolutionPageConfiguration();

            // Act
            config.ApplyViewModel(null);
        }

        [TestMethod]
        public void ApplyViewModel_Success()
        {
            // Arrange
            var config = new SolutionPageConfiguration();
            var vm = Mock.Create<ISolutionPageViewModel>();
            var group = new SolutionGroup();
            Mock.Arrange(() => vm.Columns).Returns(2);
            Mock.Arrange(() => vm.SolutionGroups).Returns(new ObservableCollection<SolutionGroup> { group });

            // Act
            var result = config.ApplyViewModel(vm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Columns);
            Assert.AreEqual(group, result.SolutionGroups.Single());
        }
    }
}
