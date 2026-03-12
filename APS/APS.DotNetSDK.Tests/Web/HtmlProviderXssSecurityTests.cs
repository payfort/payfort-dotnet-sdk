using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Web
{
    /// <summary>
    /// Security tests to verify XSS protection in HtmlProvider.Handle3dsSecure.
    /// These tests ensure that:
    /// 1. Only http/https URL schemes are allowed (javascript:, data:, vbscript: are rejected)
    /// 2. String breakout payloads are safely encoded in JS string contexts
    /// 3. Valid URLs continue to work correctly in all modes
    /// </summary>
    public class HtmlProviderXssSecurityTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration/MerchantSdkConfiguration.json";
        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        private readonly Mock<ILogger<HtmlProviderXssSecurityTests>> _loggerMock = new();

        [SetUp]
        public void Setup()
        {
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactoryMock.Object);
        }

        #region Scheme validation tests - javascript: protocol must be rejected

        [Test]
        public void Handle3dsSecure_JavascriptProtocol_ThrowsArgumentException()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "javascript:alert(document.cookie)";

            // Act & Assert - javascript: URLs must be rejected
            var ex = Assert.Throws<ArgumentException>(() => service.Handle3dsSecure(maliciousUrl));
            Assert.That(ex.Message, Does.Contain("Only http and https URL schemes are allowed"));
        }

        [Test]
        public void Handle3dsSecure_JavascriptProtocol_Modal_ThrowsArgumentException()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "javascript:alert(1)";

            // Act & Assert - javascript: URLs must be rejected in modal mode too
            var ex = Assert.Throws<ArgumentException>(() => service.Handle3dsSecure(maliciousUrl, useModal: true));
            Assert.That(ex.Message, Does.Contain("Only http and https URL schemes are allowed"));
        }

        [Test]
        public void Handle3dsSecure_JavascriptProtocol_StandardCheckout_ThrowsArgumentException()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "javascript:alert(1)";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                service.Handle3dsSecure(maliciousUrl, useModal: true, standardCheckout: true));
            Assert.That(ex.Message, Does.Contain("Only http and https URL schemes are allowed"));
        }

        #endregion

        #region Scheme validation tests - data: protocol must be rejected

        [Test]
        public void Handle3dsSecure_DataProtocol_ThrowsArgumentException()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "data:text/html,<script>alert(1)</script>";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.Handle3dsSecure(maliciousUrl));
            Assert.That(ex.Message, Does.Contain("Only http and https URL schemes are allowed"));
        }

        [Test]
        public void Handle3dsSecure_VbscriptProtocol_ThrowsArgumentException()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "vbscript:MsgBox(1)";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.Handle3dsSecure(maliciousUrl));
            Assert.That(ex.Message, Does.Contain("Only http and https URL schemes are allowed"));
        }

        #endregion

        #region String breakout protection tests

        [Test]
        public void Handle3dsSecure_StringBreakoutPayload_IsEncoded()
        {
            // Arrange
            var service = new HtmlProvider();
            // This payload tries to break out of the JS string literal
            var maliciousUrl = "http://evil.com/path?x=1';alert('XSS');//";

            // Act
            var result = service.Handle3dsSecure(maliciousUrl);

            // Assert - Single quotes must be escaped, preventing string breakout
            Assert.That(result, Does.Not.Contain("alert('XSS')"));
            Assert.That(result, Does.Contain("\\'"));
            // The output should still contain a valid script tag structure
            Assert.That(result, Does.Contain("<script>window.parent.location.href = '"));
        }

        [Test]
        public void Handle3dsSecure_DoubleQuoteBreakout_IsEncoded()
        {
            // Arrange
            var service = new HtmlProvider();
            var maliciousUrl = "http://evil.com/path?x=\";alert(1);//";

            // Act
            var result = service.Handle3dsSecure(maliciousUrl);

            // Assert - Double quotes must be escaped with backslash
            // The raw " in the URL should become \" in the JS output
            Assert.That(result, Does.Contain("\\\""));
            // Verify the URL is contained but with proper escaping
            Assert.That(result, Does.Contain("window.parent.location.href"));
        }

        [Test]
        public void Handle3dsSecure_ScriptTagBreakout_IsEncoded()
        {
            // Arrange
            var service = new HtmlProvider();
            // This tries to close the script tag and inject a new one
            var maliciousUrl = "http://evil.com/</script><script>alert(1)</script>";

            // Act
            var result = service.Handle3dsSecure(maliciousUrl);

            // Assert - < and > must be encoded to prevent script tag breakout
            Assert.That(result, Does.Not.Contain("</script><script>alert(1)"));
            Assert.That(result, Does.Contain("\\u003c"));
            Assert.That(result, Does.Contain("\\u003e"));
        }

        #endregion

        #region Valid URL tests - ensure normal URLs still work

        [Test]
        public void Handle3dsSecure_ValidHttpsUrl_WorksCorrectly()
        {
            // Arrange
            var service = new HtmlProvider();
            var validUrl = "https://3ds.example.com/authenticate?id=12345";

            // Act
            var result = service.Handle3dsSecure(validUrl);

            // Assert - valid URLs should be accepted and work correctly
            Assert.That(result, Does.Contain("window.parent.location.href"));
            Assert.That(result, Does.Contain("https://3ds.example.com/authenticate?id=12345"));
        }

        [Test]
        public void Handle3dsSecure_ValidHttpUrl_WorksCorrectly()
        {
            // Arrange
            var service = new HtmlProvider();
            var validUrl = "http://3ds.example.com/authenticate";

            // Act
            var result = service.Handle3dsSecure(validUrl);

            // Assert
            Assert.That(result, Does.Contain("window.parent.location.href"));
            Assert.That(result, Does.Contain("http://3ds.example.com/authenticate"));
        }

        [Test]
        public void Handle3dsSecure_ValidHttpsUrl_Modal_WorksCorrectly()
        {
            // Arrange
            var service = new HtmlProvider();
            var validUrl = "https://3ds.example.com/authenticate";

            // Act
            var result = service.Handle3dsSecure(validUrl, useModal: true);

            // Assert - modal should contain the URL in an iframe src
            Assert.That(result, Does.Contain("myModalIframe"));
            Assert.That(result, Does.Contain("https://3ds.example.com/authenticate"));
        }

        [Test]
        public void Handle3dsSecure_ValidHttpsUrl_StandardCheckout_WorksCorrectly()
        {
            // Arrange
            var service = new HtmlProvider();
            var validUrl = "https://3ds.example.com/authenticate";

            // Act
            var result = service.Handle3dsSecure(validUrl, useModal: true, standardCheckout: true);

            // Assert
            Assert.That(result, Does.Contain("myModalIframe"));
            Assert.That(result, Does.Contain("3ds.example.com/authenticate"));
        }

        [Test]
        public void Handle3dsSecure_NullUrl_ReturnsCloseIframeOnly()
        {
            // Arrange
            var service = new HtmlProvider();

            // Act
            var result = service.Handle3dsSecure();

            // Assert - null URL should still work (returns close iframe JS)
            Assert.That(result, Does.Contain("script"));
            Assert.That(result, Does.Not.Contain("location.href"));
        }

        #endregion

        #region Modal iframe HTML encoding tests

        [Test]
        public void Handle3dsSecure_Modal_UrlWithHtmlSpecialChars_IsHtmlEncoded()
        {
            // Arrange
            var service = new HtmlProvider();
            var url = "https://3ds.example.com/auth?a=1&b=2";

            // Act
            var result = service.Handle3dsSecure(url, useModal: true);

            // Assert - & in the URL should be HTML-encoded in the iframe src attribute
            Assert.That(result, Does.Contain("&amp;"));
        }

        #endregion
    }
}
