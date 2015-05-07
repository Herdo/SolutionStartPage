namespace SolutionStartPage.UnitTests.Shared.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.BLL.Provider;
    using SolutionStartPage.Shared.Models;
    using Telerik.JustMock;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SolutionGroupTest
    {
        [TestMethod]
        public void Ctor_Empty()
        {
            // Arrange
            var group = new SolutionGroup();

            // Act
            // Do nothing

            // Assert
            Assert.IsNull(group.ViewStateProvider);
            Assert.IsNull(group.GroupName);
            Assert.IsNull(group.Solutions);
        }

        [TestMethod]
        public void Ctor_Filled()
        {
            // Arrange
            var vsp = Mock.Create<IViewStateProvider>();
            var group = new SolutionGroup(vsp);

            // Act
            // Do nothing

            // Assert
            Assert.AreEqual(vsp, group.ViewStateProvider);
            Assert.AreEqual(String.Empty, group.GroupName);
            Assert.IsNotNull(group.Solutions);
        }

        [TestMethod]
        public void TriggerAlterSolutionGroup_CanExecute_Subscribed()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.AlterSolutionGroupCanExecute += (sender, args) => invoked = true;

            // Act
            group.TriggerAlterSolutionGroup_CanExecute(null);
            
            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void TriggerAlterSolutionGroup_Executed_Subscribed()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.AlterSolutionGroupExecuted += (sender, args) => invoked = true;

            // Act
            group.TriggerAlterSolutionGroup_Executed(null);

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;
            var vsp = Mock.Create<IViewStateProvider>();

            // Act
            group.ViewStateProvider = vsp;

            // Assert
            Assert.AreSame(vsp, group.ViewStateProvider);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet_PropertyChanged()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;
            var vsp = Mock.Create<IViewStateProvider>();
            group.ViewStateProvider = vsp;

            // Act
            Mock.Raise(() => vsp.PropertyChanged += null, new PropertyChangedEventArgs(null));

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet_PropertyChangedWithOldVsp()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;
            var vsp1 = Mock.Create<IViewStateProvider>();
            var vsp2 = Mock.Create<IViewStateProvider>();
            group.ViewStateProvider = vsp1;
            group.ViewStateProvider = vsp2;

            // Act
            Mock.Raise(() => vsp1.PropertyChanged += null, new PropertyChangedEventArgs(null));

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void GroupName_GetSet()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;

            // Act
            group.GroupName = "foo";

            // Assert
            Assert.AreEqual("foo", group.GroupName);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void GroupName_GetSet_SameValue()
        {
            // Arrange
            var group = new SolutionGroup {GroupName = "foo"};
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;

            // Act
            group.GroupName = "foo";

            // Assert
            Assert.AreEqual("foo", group.GroupName);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void Solutions_GetSet()
        {
            // Arrange
            var group = new SolutionGroup();
            var invoked = false;
            group.PropertyChanged += (sender, args) => invoked = true;
            var sln = new Solution();

            // Act
            group.Solutions = new ObservableCollection<Solution>{sln};

            // Assert
            Assert.AreSame(sln, group.Solutions.Single());
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void EditModeEnabled_Get()
        {
            // Arrange
            var vsp = Mock.Create<IViewStateProvider>();
            var group = new SolutionGroup(vsp);
            Mock.Arrange(() => vsp.EditModeEnabled).Returns(true);

            // Act
            // Do nothing

            // Assert
            Assert.IsTrue(group.EditModeEnabled);
        }
    }
}
