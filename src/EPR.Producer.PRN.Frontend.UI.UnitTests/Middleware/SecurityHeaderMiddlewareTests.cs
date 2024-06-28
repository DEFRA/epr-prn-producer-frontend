using EPR.Producer.PRN.Frontend.UI.Middleware.Auth;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EPR.Producer.PRN.Frontend.UI.UnitTests.Middleware
{
    [TestClass]
    public class SecurityHeaderMiddlewareTests
    {
        [TestMethod]
        public async Task InvokeAsync_Returns_Expected_X_Content_Type_Options_Header()
        {
            // Arrange
            var middleware = new SecurityHeaderMiddleware(next: (_) =>
            {
                return Task.CompletedTask;
            });

            // Create the DefaultHttpContext
            var context = new DefaultHttpContext();
            var mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            // Act
            await middleware.InvokeAsync(context, mockConfiguration.Object);

            // Assert
            Assert.IsTrue(context.Response.Headers.ContainsKey("X-Content-Type-Options"));

            Microsoft.Extensions.Primitives.StringValues expectedValue;
            context.Response.Headers.TryGetValue("X-Content-Type-Options", out expectedValue);
            Assert.AreEqual(expectedValue.ToString(), "nosniff");
        }
    }
}
