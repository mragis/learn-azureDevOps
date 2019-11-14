using Arana.Learn.AzureDevOps.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Arana.Learn.AzureDevOps {
    public class ControllerTests {
        [Fact]
        public void Index() {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;

            result.Should().NotBeNull();
        }

        [Fact]
        public void Privacy() {
            var controller = new HomeController();
            var result = controller.Privacy() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}