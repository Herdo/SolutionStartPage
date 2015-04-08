namespace SolutionStartPage.UnitTests.Shared.Models
{
    using System;
    using System.ComponentModel;
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
        public void ViewStateProvider_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var vsp = Mock.Create<IViewStateProvider>();

            // Act
            sln.ViewStateProvider = vsp;

            // Assert
            Assert.AreSame(vsp, sln.ViewStateProvider);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet_PropertyChanged()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var vsp = Mock.Create<IViewStateProvider>();
            sln.ViewStateProvider = vsp;

            // Act
            Mock.Raise(() => vsp.PropertyChanged += null, new PropertyChangedEventArgs(null));

            // Assert
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ViewStateProvider_GetSet_PropertyChangedWithOldVsp()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var vsp1 = Mock.Create<IViewStateProvider>();
            var vsp2 = Mock.Create<IViewStateProvider>();
            sln.ViewStateProvider = vsp1;
            sln.ViewStateProvider = vsp2;

            // Act
            Mock.Raise(() => vsp1.PropertyChanged += null, new PropertyChangedEventArgs(null));

            // Assert
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void FileSystem_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var fs = Mock.Create<IFileSystem>();

            // Act
            sln.FileSystem = fs;

            // Assert
            Assert.AreSame(fs, sln.FileSystem);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ParentGroup_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var group = new SolutionGroup();

            // Act
            sln.ParentGroup = group;

            // Assert
            Assert.AreSame(group, sln.ParentGroup);
            Assert.IsFalse(invoked);
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
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDisplayName = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionDisplayName);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SolutionDisplayName_GetSet_SameValue()
        {
            // Arrange
            var sln = new Solution { SolutionDisplayName = @"foo" };
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDisplayName = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionDisplayName);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SolutionPath_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionPath = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionPath);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SolutionPath_GetSet_SameValue()
        {
            // Arrange
            var sln = new Solution {SolutionPath = "foo"};
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionPath = "foo";

            // Assert
            Assert.AreEqual("foo", sln.SolutionPath);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SolutionDirectory_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDirectory = @"C:\Users\Administrator\";

            // Assert
            Assert.AreEqual(@"C:\Users\Administrator\", sln.SolutionDirectory);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SolutionDirectory_GetSet_SameValue()
        {
            // Arrange
            var sln = new Solution { SolutionDirectory = @"C:\Users\Administrator\" };
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDirectory = @"C:\Users\Administrator\";

            // Assert
            Assert.AreEqual(@"C:\Users\Administrator\", sln.SolutionDirectory);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SolutionDirectory_Get_BySolutionPath()
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
            Assert.AreEqual(@"C:\Users\Administrator", sln.SolutionDirectory);
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
        public void EditModeEnabled_Set()
        {
            // Arrange
            var expected = false;
            var sln = new Solution();
            var vsp = Mock.Create<IViewStateProvider>();
            sln.ViewStateProvider = vsp;
            Mock.ArrangeSet(() => vsp.EditModeEnabled = Arg.AnyBool)
                .DoInstead(() => expected = true);

            // Act
            sln.EditModeEnabled = true;

            // Assert
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void SolutionAvailable_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionAvailable);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SolutionAvailable_GetSet_SameValue()
        {
            // Arrange
            var sln = new Solution {SolutionAvailable = true};
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionAvailable);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SolutionDirectoryAvailable_GetSet()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDirectoryAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionDirectoryAvailable);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void SolutionDirectoryAvailable_GetSet_SameValue()
        {
            // Arrange
            var sln = new Solution {SolutionDirectoryAvailable = true};
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;

            // Act
            sln.SolutionDirectoryAvailable = true;

            // Assert
            Assert.IsTrue(sln.SolutionDirectoryAvailable);
            Assert.IsFalse(invoked);
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
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";

            // Assert
            Assert.AreEqual(@"C:\Users\Administrator", sln.ComputedSolutionDirectory);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_SameValue()
        {
            // Arrange
            var sln = new Solution();
            var invokeCount = 0;
            sln.PropertyChanged += (sender, args) => invokeCount++;
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";
            sln.SolutionPath = @"C:\Users\Administrator\bar.sln";

            // Assert
            Assert.AreEqual(5, invokeCount);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_TreatDirAsFile()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(false);

            // Act
            sln.SolutionDirectory = @"C:\Users\Administrator\foo.sln";

            // Assert
            Assert.AreEqual(@"C:/Users/Administrator", sln.ComputedSolutionDirectory);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_CombineRelativeNormal()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";
            sln.SolutionDirectory = @"..\";

            // Assert
            Assert.AreEqual(@"C:/Users/", sln.ComputedSolutionDirectory);
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ComputedSolutionDirectory_Get_CombineRelativeLong()
        {
            // Arrange
            var sln = new Solution();
            var invoked = false;
            sln.PropertyChanged += (sender, args) => invoked = true;
            var fs = Mock.Create<IFileSystem>();
            sln.FileSystem = fs;
            Mock.Arrange(() => fs.FileExists(Arg.AnyString)).Returns(true);
            Mock.Arrange(() => fs.DirectoryExists(Arg.AnyString)).Returns(true);

            // Act
            sln.SolutionPath = @"C:\Users\Administrator\foo.sln";
            sln.SolutionDirectory = @"..\..\..\..\";

            // Assert
            Assert.AreEqual(@"C:/", sln.ComputedSolutionDirectory);
            Assert.IsTrue(invoked);
        }
    }
}
