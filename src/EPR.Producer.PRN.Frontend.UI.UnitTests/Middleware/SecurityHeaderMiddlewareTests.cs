using EPR.Producer.PRN.Frontend.UI.Middleware.Auth;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace EPR.Producer.PRN.Frontend.UI.UnitTests.Middleware
{
    [TestClass]
    public class SecurityHeaderMiddlewareTests
    {
        private readonly DefaultHttpContext _context = new DefaultHttpContext();
        private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _mockConfiguration = new();
        private SecurityHeaderMiddleware _middleware = default!;

        [TestInitialize]
        public void Setup()
        {
            _middleware = new SecurityHeaderMiddleware(next: (_) =>
            {
                return Task.CompletedTask;
            });
        }

        [DataRow("Content-Security-Policy")]
        [DataRow("Permissions-Policy")]
        [TestMethod]
        public async Task InvokeAsync_Returns_Expected_Header(string header)
        {
            // Arrange + Act
            await _middleware.InvokeAsync(_context, _mockConfiguration.Object);

            // Assert
            Assert.IsTrue(_context.Response.Headers.ContainsKey(header));
        }

        [DataRow("Cross-Origin-Embedder-Policy", "require-corp")]
        [DataRow("Cross-Origin-Opener-Policy", "same-origin")]
        [DataRow("Cross-Origin-Resource-Policy", "same-origin")]
        [DataRow("Referrer-Policy", "strict-origin-when-cross-origin")]
        [DataRow("X-Content-Type-Options", "nosniff")]
        [DataRow("X-Frame-Options", "deny")]
        [DataRow("X-Permitted-Cross-Domain-Policies", "none")]
        [DataRow("X-Robots-Tag", "noindex, nofollow")]
        [DataTestMethod]
        public async Task InvokeAsync_Returns_Expected_Header_And_Value(string header, string headerValue)
        {
            // Arrange + Act
            await _middleware.InvokeAsync(_context, _mockConfiguration.Object);

            // Assert
            Assert.IsTrue(_context.Response.Headers.ContainsKey(header));
            _context.Response.Headers.TryGetValue(header, out var actualHeaderValue);
            Assert.AreEqual(actualHeaderValue.ToString(), headerValue);
        }

        [TestMethod]
        public async Task InvokeAsync_Returns_Expected_Nonce_Item()
        {
            // Arrange + Act
            await _middleware.InvokeAsync(_context, _mockConfiguration.Object);

            // Assert
            Assert.IsTrue(_context.Items.ContainsKey("ScriptNonce"));
        }

        [TestMethod]
        public void GetContentSecurityPolicyHeader_Returns_Expected_Header_String()
        {
            // Arranage
            var nonce = "nonce";
            var whitelistedFormActionAddresses = "whitelistedFormActionAddresses";

            StringBuilder sb = new StringBuilder();
            sb.Append("default-src 'self'").Append(';');
            sb.Append("object-src 'none'").Append(';');
            sb.Append("frame-ancestors 'none'").Append(';');
            sb.Append("upgrade-insecure-requests").Append(';');
            sb.Append("block-all-mixed-content").Append(';');
            sb.Append($"script-src 'self' 'nonce-{nonce}' https://tagmanager.google.com https://*.googletagmanager.com").Append(';');
            sb.Append("img-src 'self' www.googletagmanager.com https://ssl.gstatic.com https://www.gstatic.com ");
            sb.Append("https://*.google-analytics.com https://*.googletagmanager.com").Append(';');
            sb.Append($"form-action 'self' {whitelistedFormActionAddresses}").Append(';');
            sb.Append("style-src 'self' https://tagmanager.google.com https://fonts.googleapis.com").Append(';');
            sb.Append("font-src 'self' https://fonts.gstatic.com data:").Append(';');
            sb.Append("connect-src 'self' https://*.google-analytics.com ");
            sb.Append("https://*.analytics.google.com https://*.googletagmanager.com");

            // Act
            string actual = SecurityHeaderMiddleware.GetContentSecurityPolicyHeader(nonce, whitelistedFormActionAddresses);

            // Assert
            Assert.AreEqual(actual, sb.ToString());
        }
    }
}