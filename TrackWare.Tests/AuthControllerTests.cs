using Microsoft.AspNetCore.Mvc;
using Moq;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;
using TrackWare.EndPoint.Controllers;

namespace TrackWare.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsOk_WhenUserIsAuthenticated()
        {
            // Arrange
            var loginRequest = new LoginRequestDto { UserName = "test", Password = "pass" };

            var loginResult = new LoginResponseDto
            {
                IsAuthenticated = true,
                Token = "fake-jwt-token",
                UserName = "test"
            };

            var handlerMock = new Mock<IUserLoginHandler>();
            handlerMock.Setup(h => h.Handle(It.IsAny<LoginRequestDto>()))
                       .ReturnsAsync(loginResult);

            var controller = new AuthController(handlerMock.Object);

            // Act
            var result = await controller.Login(loginRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convert to a dictionary
            var json = okResult.Value;
            var dict = System.Text.Json.JsonSerializer.SerializeToElement(json);

            // Access properties
            var token = dict.GetProperty("token").GetString();
            var user = dict.GetProperty("user").GetString();

            Assert.Equal("fake-jwt-token", token);
            Assert.Equal("test", user);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var loginRequest = new LoginRequestDto { UserName = "test", Password = "wrongpass" };

            var loginResult = new LoginResponseDto
            {
                IsAuthenticated = false
            };

            var handlerMock = new Mock<IUserLoginHandler>();
            handlerMock.Setup(h => h.Handle(It.IsAny<LoginRequestDto>()))
                       .ReturnsAsync(loginResult);

            var controller = new AuthController(handlerMock.Object);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var data = unauthorizedResult.Value as dynamic;
            var dict = System.Text.Json.JsonSerializer.SerializeToElement(data);
            var msg = dict.GetProperty("message").GetString();
            Assert.Equal("Invalid User Name or Password", msg);
        }
    }
}