namespace SolutionStartPage.UnitTests.Shared.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Shared.DAL;
    using SolutionStartPage.Shared.Models;
    using SolutionStartPage.Shared.Views;
    using Telerik.JustMock;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SolutionTest
    {
        [TestMethod]
        public void Ctor_Empty()
        {
            // Arrange
            var sln = new Solution();

            // Act
            // Do nothing

            // Assert
            Assert.IsNull(sln.ComputedSolutionDirectory);
            Assert.IsNull(sln.ViewStateProvider);
            Assert.IsNull(sln.FileSystem);
            Assert.IsNull(sln.ParentGroup);
            Assert.IsFalse(sln.SolutionAvailable);
            Assert.AreEqual(String.Empty, sln.SolutionDirectory);
            Assert.IsFalse(sln.SolutionDirectoryAvailable);
            Assert.IsNull(sln.SolutionPath);
        }

        [TestMethod]
        public void Ctor_Filled()
        {
            // Arrange
            var vsp = Mock.Create<IViewStateProvider>();
            Mock.Arrange(() => vsp.EditModeEnabled).Returns(true);
            var fileSystem = Mock.Create<IFileSystem>();
            var group = new SolutionGroup(vsp);
            var sln = new Solution(vsp, fileSystem, group, @"C:\Users\Administrator\foo.sln");

            // Act
            // Do nothing

            // Assert
            Assert.IsNotNull(sln.ComputedSolutionDirectory);
            Assert.IsTrue(sln.EditModeEnabled);
            Assert.IsNotNull(sln.FileSystem);
            Assert.IsNotNull(sln.ParentGroup);
            Assert.IsTrue(sln.SolutionAvailable);
            Assert.IsNotNull(sln.SolutionDirectory);
            Assert.IsTrue(sln.SolutionDirectoryAvailable);
            Assert.IsNotNull(sln.SolutionDisplayName);
            Assert.IsNotNull(sln.SolutionPath);
            Assert.IsNotNull(sln.ViewStateProvider);
        }

        [TestMethod]
        public void TriggerAlterSolution_CanExecute_Subscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.AlterSolutionCanExecute += (sender, args) => invoked = true;

            // Act
            sln.TriggerAlterSolution_CanExecute(this, null);
            
            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void TriggerAlterSolution_CanExecute_NotSubscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;

            // Act
            sln.TriggerAlterSolution_CanExecute(this, null);

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void TriggerAlterSolution_Executed_Subscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.AlterSolutionExecuted += (sender, args) => invoked = true;

            // Act
            sln.TriggerAlterSolution_Executed(this, null);

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void TriggerAlterSolution_Executed_NotSubscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;

            // Act
            sln.TriggerAlterSolution_Executed(this, null);

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void TriggerOpenSolution_CanExecute_Subscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.OpenSolutionCanExecute += (sender, args) => invoked = true;

            // Act
            sln.TriggerOpenSolution_CanExecute(this, null);

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void TriggerOpenSolution_CanExecute_NotSubscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;

            // Act
            sln.TriggerOpenSolution_CanExecute(this, null);

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void TriggerOpenSolution_Executed_Subscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.OpenSolutionExecuted += (sender, args) => invoked = true;

            // Act
            sln.TriggerOpenSolution_Executed(this, null);

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void TriggerOpenSolution_Executed_NotSubscribed()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;

            // Act
            sln.TriggerOpenSolution_Executed(this, null);

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var vsp = Mock.Create<IViewStateProvider>();

            // Act
            sln.ViewStateProvider = vsp;

            // Assert
            Assert.AreSame(vsp, sln.ViewStateProvider);
        }

        [TestMethod]
        public void FileSystem_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var fs = Mock.Create<IFileSystem>();

            // Act
            sln.FileSystem = fs;

            // Assert
            Assert.AreSame(fs, sln.FileSystem);
        }

        [TestMethod]
        public void ParentGroup_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var group = new SolutionGroup();

            // Act
            sln.ParentGroup = group;

            // Assert
            Assert.AreSame(group, sln.ParentGroup);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SolutionDisplayName_GetSet_ArgumentNullException()
        {
            // Arrange
            var sln = new Solution();

            // Act
            // Cause exception by getter call
            Assert.IsNotNull(sln.SolutionDisplayName);
        }

        [TestMethod]
        public void SolutionDisplayName_GetSet_ComputedByPath()
        {
            // Arrange
            var sln = new Solution {SolutionPath = @"C:\Users\Administrator\foo.sln"};

            // Act
            // Do nothing

            // Assert
            Assert.AreEqual("foo.sln", sln.SolutionDisplayName);
        }

        [TestMethod]
        public void SolutionDisplayName_GetSet()
        {
            // Arrange
            var sln = new Solution { SolutionPath = @"C:\Users\Administrator\foo.sln" };

            // Act
            sln.SolutionDisplayName = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionDisplayName);
        }

        [TestMethod]
        public void SolutionPath_GetSet()
        {
            // Arrange
            var sln = new Solution();

            // Act
            sln.SolutionPath = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionPath);
        }

        [TestMethod]
        public void SolutionDirectory_GetSet()
        {
            // Arrange
            var sln = new Solution();

            // Act
            sln.SolutionDirectory = @"C:\Users\Administrator\";

            // Assert
            Assert.AreEqual(@"C:\Users\Administrator\", sln.SolutionDirectory);
        }

        [TestMethod]
        public void EditModeEnabled_Get()
        {
            // Arrange
            var sln = new Solution();
            var vsp = Mock.Create<IViewStateProvider>();
            sln.ViewStateProvider = vsp;
            Mock.Arrange(() => vsp.EditModeEnabled).Returns(true);

            // Act
            // Do nothing

            // Assert
            Assert.IsTrue(sln.EditModeEnabled);
        }

        [TestMethod]
        public void SolutionAvailable_GetSet()
        {
            // Arrange
            var sln = new Solution();

            // Act
            sln.SolutionAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionAvailable);
        }

        [TestMethod]
        public void SolutionDirectoryAvailable_GetSet()
        {
            // Arrange
            var sln = new Solution();

            // Act
            sln.SolutionDirectoryAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionDirectoryAvailable);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_NothingSet()
        {
            // Arrange
            var sln = new Solution();

            // Act
            // Do nothing

            // Assert
            Assert.IsNull(sln.ComputedSolutionDirectory);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_OnlyNormalDirBySln()
        {
            // Arrange
            var sln = new Solution();
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";

            // Assert
            Assert.AreEqual(@"C:/Users/Administrator", sln.ComputedSolutionDirectory);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_TreatDirAsFile()
        {
            // Arrange
            var sln = new Solution();
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(false);

            // Act
            sln.SolutionDirectory = @"C:\Users\Administrator\foo.sln";

            // Assert
            Assert.AreEqual(@"C:/Users/Administrator", sln.ComputedSolutionDirectory);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_CombineRelativeNormal()
        {
            // Arrange
            var sln = new Solution();
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";
            sln.SolutionDirectory = @"..\";

            // Assert
            Assert.AreEqual(@"C:/Users/", sln.ComputedSolutionDirectory);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_CombineRelativeLong()
        {
            // Arrange
            var sln = new Solution();
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";
            sln.SolutionDirectory = @"..\..\..\..\";

            // Assert
            Assert.AreEqual(@"C:/", sln.ComputedSolutionDirectory);
        }
    }
}
