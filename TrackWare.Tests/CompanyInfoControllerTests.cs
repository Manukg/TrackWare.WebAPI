using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrackWare.Application.Interfaces;
using TrackWare.EndPoint.Controllers;
using Xunit;
namespace TrackWare.Tests
{
    

    public class CompanyInfoControllerTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithCompanyList()
        {
            // Arrange
            var mockCompanyHandler = new Mock<ICompanyInfoHandle>();

            var fakeList = new Dictionary<string, string>
        {
            { "1", "ABC Company" },
            { "2", "XYZ Company" }
        };

            mockCompanyHandler
                .Setup(h => h.LoadCompanyList())
                .ReturnsAsync(fakeList);

            var controller = new CompanyInfoController(mockCompanyHandler.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedValue = okResult.Value as Dictionary<string, string>;
            returnedValue.Should().BeEquivalentTo(fakeList);

            mockCompanyHandler.Verify(h => h.LoadCompanyList(), Times.Once);
        }
    }

}
