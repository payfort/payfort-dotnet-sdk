using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Web
{
    public class HtmlProviderModal3dsTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        private readonly Mock<ILogger<HtmlProviderModal3dsTests>> _loggerMock = new();
        [SetUp]
        public void Setup()
        {
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                _loggerFactoryMock.Object);
        }

        [Test]
        public void GetHtmlModalFor3ds_ReturnSuccessful()
        {
            //arrange
            var service = new HtmlProvider();

            //act
            var actualJavascriptContent = service.Handle3dsSecure(@"https://www.google.com/", true);

            //assert
            var expectedJavascriptContent = File.ReadAllTextAsync($"Web{ Path.DirectorySeparatorChar.ToString() }ExpectedModal3ds.txt").Result;

            Assert.That(actualJavascriptContent, Is.EqualTo(expectedJavascriptContent));
        }

        [Test]
        public void GetJavaScriptToCloseModal_ReturnJavaScript()
        {
            //arrange
            const string expectedJavaScript = @"<script>var myModal = window.parent.document.getElementById('myModal'); if (!(myModal == null)) { myModal.style.display = 'none';}</script>";
            var service = new HtmlProvider();

            //act
            //assert
            var actualJavaScript = service.GetJavaScriptToCloseModal();
            Assert.That(actualJavaScript, Is.EqualTo(expectedJavaScript));
        }

        [Test]
        public void GetJavaScriptToCloseIframe_WithUrlNull_ReturnJavaScript()
        {
            //arrange
            const string expectedJavaScript = @"<script>var iframe = window.parent.document.getElementById('iframeDiv');if (!(iframe == null)) { iframe.innerHTML = '';}</script>";

            var service = new HtmlProvider();

            //act
            //assert
            var actualJavaScript = service.Handle3dsSecure();
            Assert.That(actualJavaScript, Is.EqualTo(expectedJavaScript));
        }

        [Test]
        public void GetJavaScriptToCloseIframe_WithProvidedUrl_ReturnJavaScript()
        {
            //arrange
            const string expectedJavaScript = @"<script>window.parent.location.href = 'http://test.com';</script><script>var iframe = window.parent.document.getElementById('iframeDiv');if (!(iframe == null)) { iframe.innerHTML = '';}</script>";
            var url = @"http://test.com";
            var service = new HtmlProvider();

            //act
            //assert
            var actualJavaScript = service.Handle3dsSecure(url);
            Assert.That(actualJavaScript, Is.EqualTo(expectedJavaScript));
        }

        [Test]
        public void GetJavaScriptToCloseIframe_WithProvidedUrl_ReturnJavaScriptWithModalContent()
        {
            //arrange
            var expectedJavaScript =@File.ReadAllTextAsync($"Web{ Path.DirectorySeparatorChar.ToString() }CloseIframeWithModalContent.txt").Result;
            const string url = @"http://test.com";
            var service = new HtmlProvider();
            //assert

            //act
            //assert
            var actualJavaScript = service.Handle3dsSecure(url, true, true);
            Assert.That(actualJavaScript, Is.EqualTo(expectedJavaScript));
        }
        
        [Test]
        public void GetJavaScriptForFingerPrint_ReturnSuccessful()
        {
            //arrange
            var service = new HtmlProvider();

            //act
            var actualJavascriptContent = service.GetJavaScriptForFingerPrint();

            //assert
            var expectedJavascriptContent = File.ReadAllTextAsync($"Web{ Path.DirectorySeparatorChar.ToString() }ExpectedFingerPrintJavaScript.txt").Result;

            Assert.That(actualJavascriptContent, Is.EqualTo(expectedJavascriptContent));
        }
    }
}
