namespace SolutionStartPage.UnitTests.Core.Views.BasicPart
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolutionStartPage.Core.Views.BasicPart;
    using SolutionStartPage.Shared.Models;
    using SolutionStartPage.Shared.Views.BasicPart;
    using Telerik.JustMock;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class VsoPagePresenterTest
    {
        [TestMethod]
        public void Constructor()
        {
            // Arrange
            var vsVersion = Mock.Create<IVisualStudioVersion>();
            var ide = Mock.Create<IIde>();
            var view = Mock.Create<IVsoPageView>();
            var vm = Mock.Create<IVsoPageViewModel>();

            var dataSourceConnected = false;
            var startPageHeaderTitleSet = false;

            Mock.Arrange(() => view.ConnectDataSource(vm)).DoInstead(() => dataSourceConnected = true);
            Mock.ArrangeSet(() => vm.StartPageHeaderTitle = Arg.AnyString).DoInstead(() => startPageHeaderTitleSet = true);

            // Act
            var presenter = new VsoPagePresenter(vsVersion, ide, view, vm);

            // Assert
            Assert.IsTrue(dataSourceConnected);
            Assert.IsTrue(startPageHeaderTitleSet);
        }
    }
}
