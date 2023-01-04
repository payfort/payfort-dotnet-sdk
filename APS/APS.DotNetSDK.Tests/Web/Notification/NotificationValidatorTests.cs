using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Web.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Text;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Web.Notification
{
    public class NotificationValidatorTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly Mock<ILogger<NotificationValidator>> _loggerMock = new();
        private Mock<IFormCollection> _formCollection = new();
        private readonly Mock<HttpRequest> _request = new();

        [SetUp]
        public void Setup()
        {
            _formCollection = new Mock<IFormCollection>();
            _formCollection.Setup(x => x.Keys).Returns(new List<string>
            {
                "amount", "response_code", "signature", "merchant_identifier", "access_code",
                "language", "command", "response_message", "merchant_reference", "customer_email",
                "return_url", "currency", "status", "app_programming", "app_plugin", "app_plugin_version", "app_ver",
                "app_framework"
            });

            LoggingConfiguration loggingConfiguration = new(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration);
        }


        [Test]
        public void ValidateFormPostPurchase_IsValid_InputIsValid()
        {
            //arrange
            _formCollection.SetupGet(p => p["amount"]).Returns("243");
            _formCollection.SetupGet(p => p["response_code"]).Returns("00006");
            _formCollection.SetupGet(p => p["signature"])
                .Returns("b1869e119965f0b56be7d6401964003ce9ab65ed02b6ec2110eb3f94d136bd79");
            _formCollection.SetupGet(p => p["merchant_identifier"]).Returns(SdkConfiguration.MerchantIdentifier);
            _formCollection.SetupGet(p => p["access_code"]).Returns(SdkConfiguration.AccessCode);
            _formCollection.SetupGet(p => p["language"]).Returns("en");
            _formCollection.SetupGet(p => p["command"]).Returns("PURCHASE");
            _formCollection.SetupGet(p => p["response_message"]).Returns("Technical problem");
            _formCollection.SetupGet(p => p["merchant_reference"]).Returns("merchant_reference");
            _formCollection.SetupGet(p => p["customer_email"]).Returns("test@test.com");
            _formCollection.SetupGet(p => p["return_url"]).Returns("http%3A%2F%2Ftest.com");
            _formCollection.SetupGet(p => p["currency"]).Returns("USD");
            _formCollection.SetupGet(p => p["status"]).Returns("00");

            _request.Setup(x => x.Form).Returns(_formCollection.Object);
            _request.Setup(x => x.Method).Returns("POST");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));
            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
          
            var expectedKeys = new List<string>
            {
                "amount", "response_code", "signature", "merchant_identifier",
                "access_code", "language", "command", "response_message", "merchant_reference", "customer_email",
                "return_url", "currency", "status", "app_programming", "app_plugin", "app_plugin_version", "app_ver",
                "app_framework"
            };

            CollectionAssert.AreEquivalent(expectedKeys, validationResult.RequestData.Keys);
            Assert.Multiple(() =>
            {
                Assert.That(validationResult.RequestData["amount"], Is.EqualTo("243"));
                Assert.That(validationResult.RequestData["response_code"], Is.EqualTo("00006"));
                Assert.That(validationResult.RequestData["signature"], Is.EqualTo("b1869e119965f0b56be7d6401964003ce9ab65ed02b6ec2110eb3f94d136bd79"));
                Assert.That(validationResult.RequestData["merchant_identifier"], Is.EqualTo(SdkConfiguration.MerchantIdentifier));
                Assert.That(validationResult.RequestData["access_code"], Is.EqualTo(SdkConfiguration.AccessCode));
                Assert.That(validationResult.RequestData["language"], Is.EqualTo("en"));
                Assert.That(validationResult.RequestData["command"], Is.EqualTo("PURCHASE"));
                Assert.That(validationResult.RequestData["response_message"], Is.EqualTo("Technical problem"));
                Assert.That(validationResult.RequestData["merchant_reference"], Is.EqualTo("merchant_reference"));
                Assert.That(validationResult.RequestData["customer_email"], Is.EqualTo("test@test.com"));
                Assert.That(validationResult.RequestData["return_url"], Is.EqualTo("http%3A%2F%2Ftest.com"));
                Assert.That(validationResult.RequestData["currency"], Is.EqualTo("USD"));
                Assert.That(validationResult.RequestData["status"], Is.EqualTo("00"));
            });
        }

        [Test]
        public void ValidateFormPostAuthorization_IsValid_InputIsValid()
        {
            //arrange
            _formCollection.SetupGet(p => p["amount"]).Returns("243");
            _formCollection.SetupGet(p => p["response_code"]).Returns("00006");
            _formCollection.SetupGet(p => p["signature"])
                .Returns("1300e9a2ee36a76b458d3bf305ff8db7dd6647e14861bde573c09828729c602b");
            _formCollection.SetupGet(p => p["merchant_identifier"]).Returns(SdkConfiguration.MerchantIdentifier);
            _formCollection.SetupGet(p => p["access_code"]).Returns(SdkConfiguration.AccessCode);
            _formCollection.SetupGet(p => p["language"]).Returns("en");
            _formCollection.SetupGet(p => p["command"]).Returns("AUTHORIZATION");
            _formCollection.SetupGet(p => p["response_message"]).Returns("Technical problem");
            _formCollection.SetupGet(p => p["merchant_reference"]).Returns("merchant_reference");
            _formCollection.SetupGet(p => p["customer_email"]).Returns("test@test.com");
            _formCollection.SetupGet(p => p["return_url"]).Returns("http%3A%2F%2Ftest.com");
            _formCollection.SetupGet(p => p["currency"]).Returns("USD");
            _formCollection.SetupGet(p => p["status"]).Returns("00");

            _request.Setup(x => x.Form).Returns(_formCollection.Object);
            _request.Setup(x => x.Method).Returns("POST");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
        }

        [Test]
        public void ValidateFormPostCapture_IsValid_InputIsValid()
        {
            //arrange
            _formCollection.SetupGet(p => p["command"]).Returns("CAPTURE");
            _formCollection.SetupGet(p => p["merchant_reference"]).Returns("merchant_reference");
            _formCollection.SetupGet(p => p["amount"]).Returns("243");
            _formCollection.SetupGet(p => p["currency"]).Returns("USD");
            _formCollection.SetupGet(p => p["merchant_identifier"]).Returns(SdkConfiguration.MerchantIdentifier);
            _formCollection.SetupGet(p => p["access_code"]).Returns(SdkConfiguration.AccessCode);
            _formCollection.SetupGet(p => p["language"]).Returns("en");
            _formCollection.SetupGet(p => p["signature"])
                .Returns("d36985eaa48b46ab36f10d9c42ed9c977b7eeaf46628789d2804129e327f8569");

            _request.Setup(x => x.Form).Returns(_formCollection.Object);
            _request.Setup(x => x.Method).Returns("POST");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
        }

        [Test]
        public void ValidateFormPostRefund_IsValid_InputIsValid()
        {
            //arrange
            _formCollection.SetupGet(p => p["command"]).Returns("REFUND");
            _formCollection.SetupGet(p => p["merchant_reference"]).Returns("merchant_reference");
            _formCollection.SetupGet(p => p["maintenance_reference"]).Returns("maintenance_reference");
            _formCollection.SetupGet(p => p["amount"]).Returns("243");
            _formCollection.SetupGet(p => p["currency"]).Returns("USD");
            _formCollection.SetupGet(p => p["merchant_identifier"]).Returns(SdkConfiguration.MerchantIdentifier);
            _formCollection.SetupGet(p => p["access_code"]).Returns(SdkConfiguration.AccessCode);
            _formCollection.SetupGet(p => p["language"]).Returns("en");
            _formCollection.SetupGet(p => p["signature"])
                .Returns("16edc7836bff2f2177bf62de779b45f437860262a808b7dc53da578d615755cc");

            _request.Setup(x => x.Form).Returns(_formCollection.Object);
            _request.Setup(x => x.Method).Returns("POST");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
        }


        [Test]
        public void ValidateFormPostVoid_IsValid_InputIsValid()
        {
            //arrange
            _formCollection.SetupGet(p => p["command"]).Returns("VOID_AUTHORIZATION");
            _formCollection.SetupGet(p => p["merchant_reference"]).Returns("merchant_reference");
            _formCollection.SetupGet(p => p["merchant_identifier"]).Returns(SdkConfiguration.MerchantIdentifier);
            _formCollection.SetupGet(p => p["access_code"]).Returns(SdkConfiguration.AccessCode);
            _formCollection.SetupGet(p => p["language"]).Returns("en");
            _formCollection.SetupGet(p => p["signature"])
                .Returns("ee073e1a786869b88e7668864157f4694451b039d7bdc47704a28c9c16ab14e5");

            _request.Setup(x => x.Form).Returns(_formCollection.Object);
            _request.Setup(x => x.Method).Returns("POST");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
        }

        [Test]
        public void ValidateQueryStringTokenization_IsValid_InputIsValid()
        {
            _request.Setup(x => x.QueryString).Returns(new QueryString(
                "?response_code=18000&" +
                "card_number=****************&" +
                "card_holder_name=*******&" +
                "signature=b1d40a8be03f78d1c1e0e8bdf359dde1c82784fbb1f2d4bab959fb1f73211357&" +
                "merchant_identifier=merchant_identifier&" +
                "expiry_date=****&" +
                "access_code=access_code&" +
                "language=en&" +
                "service_command=TOKENIZATION&" +
                "response_message=Success&" +
                "merchant_reference=merchant_reference&" +
                "token_name=token_name&" +
                "return_url=http://test.com&" +
                "card_bin=400555&" +
                "status=18"));
            _request.Setup(x => x.Method).Returns("GET");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.Validate(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);
            var expectedKeys = new List<string>
            {
                "response_code", "card_number", "card_holder_name",
                "signature", "merchant_identifier", "expiry_date", "access_code", "language" , "service_command",
                "response_message", "merchant_reference", "token_name", "return_url", "card_bin", "status"
            };

            CollectionAssert.AreEquivalent(expectedKeys, validationResult.RequestData.Keys);
            Assert.Multiple(() =>
            {
                Assert.That(validationResult.RequestData["response_code"], Is.EqualTo("18000"));
                Assert.That(validationResult.RequestData["card_number"], Is.EqualTo("****************"));
                Assert.That(validationResult.RequestData["card_holder_name"], Is.EqualTo("*******"));
                Assert.That(validationResult.RequestData["signature"], Is.EqualTo("b1d40a8be03f78d1c1e0e8bdf359dde1c82784fbb1f2d4bab959fb1f73211357"));
                Assert.That(validationResult.RequestData["merchant_identifier"], Is.EqualTo("merchant_identifier"));
                Assert.That(validationResult.RequestData["expiry_date"], Is.EqualTo("****"));
                Assert.That(validationResult.RequestData["access_code"], Is.EqualTo("access_code"));
                Assert.That(validationResult.RequestData["language"], Is.EqualTo("en"));
                Assert.That(validationResult.RequestData["service_command"], Is.EqualTo("TOKENIZATION"));
                Assert.That(validationResult.RequestData["response_message"], Is.EqualTo("Success"));
                Assert.That(validationResult.RequestData["merchant_reference"], Is.EqualTo("merchant_reference"));
                Assert.That(validationResult.RequestData["token_name"], Is.EqualTo("token_name"));
                Assert.That(validationResult.RequestData["return_url"], Is.EqualTo("http://test.com"));
                Assert.That(validationResult.RequestData["card_bin"], Is.EqualTo("400555"));
                Assert.That(validationResult.RequestData["status"], Is.EqualTo("18"));
            });
        }

        [Test]
        public void ValidateQueryStringTokenization_QueryStringHasNoValue_ExceptionIsThrown()
        {
            _request.Setup(x => x.QueryString).Returns(null);
            _request.Setup(x => x.Method).Returns("GET");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            //assert
            var actualError =
                Assert.Throws<InvalidNotification>(() => notificationValidator.Validate(_request.Object));

            var expectedError = "HttpRequest query string has no value";
            Assert.That(expectedError, Is.EqualTo(actualError.Message));
        }

        [Test]
        public void Validate_ThrowsException_InvalidCommandIsReceived()
        {
            //arrange
            _formCollection.SetupGet(p => p["command"]).Returns("test");
            _request.Setup(x => x.Form).Returns(_formCollection.Object);

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            //assert
            Assert.Throws<InvalidNotification>(() => notificationValidator.Validate(_request.Object));
        }


        [Test]
        public void ValidateAsyncNotification_IsValid_InputIsValid()
        {
            var json =
                "{\"response_code\": \"02000\"," +
                "\"card_holder_name\": \"t\"," +
                "\"signature\": \"3eea9b9c4ebdbf3e033846ad0597629edad01945ca91faa9fb0df08ced609437\"," +
                "\"merchant_identifier\": \"merchant_identifier\"," +
                "\"access_code\": \"access_code\"," +
                "\"customer_ip\": \"172.254.12.5\"," +
                "\"language\": \"en\"," +
                "\"eci\": \"ECOMMERCE\"," +
                "\"merchant_reference\": \"merchant_reference\"," +
                "\"authorization_code\": \"320170\"," +
                "\"token_name\": \"9759c937251548c8b558991ddc90e664\"," +
                "\"currency\": \"AED\"," +
                "\"acquirer_response_code\": \"00\"," +
                "\"amount\": \"24300\"," +
                "\"card_number\": \"****************\"," +
                "\"payment_option\": \"VISA\"," +
                "\"expiry_date\": \"2505\"," +
                "\"fort_id\": \"12432222\"," +
                "\"command\": \"AUTHORIZATION\"," +
                "\"response_message\": \"Success\"," +
                "\"customer_email\": \"TestCustomerEmail\"," +
                "\"phone_number\": \"+1 (201)-234-6789\"," +
                "\"customer_name\": \"test customer&#39;s name\"," +
                "\"status\": \"02\"" +
                "}";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            _request.Setup(x => x.Method).Returns("POST");
            _request.Setup(x => x.Body).Returns(stream);
            _request.Setup(x => x.ContentType).Returns("application/json");

            var notificationValidator =
                new NotificationValidator(new SignatureValidator(new SignatureProvider()));

            //act
            var validationResult = notificationValidator.ValidateAsyncNotification(_request.Object);

            //assert
            Assert.That(validationResult.IsValid, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(validationResult.RequestData["response_code"], Is.EqualTo("02000"));
                Assert.That(validationResult.RequestData["card_holder_name"], Is.EqualTo("t"));
                Assert.That(validationResult.RequestData["signature"], Is.EqualTo("3eea9b9c4ebdbf3e033846ad0597629edad01945ca91faa9fb0df08ced609437"));
                Assert.That(validationResult.RequestData["merchant_identifier"], Is.EqualTo("merchant_identifier"));
                Assert.That(validationResult.RequestData["access_code"], Is.EqualTo("access_code"));
                Assert.That(validationResult.RequestData["customer_ip"], Is.EqualTo("172.254.12.5"));
                Assert.That(validationResult.RequestData["language"], Is.EqualTo("en"));
                Assert.That(validationResult.RequestData["eci"], Is.EqualTo("ECOMMERCE"));
                Assert.That(validationResult.RequestData["merchant_reference"], Is.EqualTo("merchant_reference"));
                Assert.That(validationResult.RequestData["authorization_code"], Is.EqualTo("320170"));
                Assert.That(validationResult.RequestData["token_name"], Is.EqualTo("9759c937251548c8b558991ddc90e664"));
                Assert.That(validationResult.RequestData["currency"], Is.EqualTo("AED"));
                Assert.That(validationResult.RequestData["acquirer_response_code"], Is.EqualTo("00"));
                Assert.That(validationResult.RequestData["amount"], Is.EqualTo("24300"));
                Assert.That(validationResult.RequestData["card_number"], Is.EqualTo("****************"));
                Assert.That(validationResult.RequestData["payment_option"], Is.EqualTo("VISA"));
                Assert.That(validationResult.RequestData["expiry_date"], Is.EqualTo("2505"));
                Assert.That(validationResult.RequestData["fort_id"], Is.EqualTo("12432222"));
                Assert.That(validationResult.RequestData["command"], Is.EqualTo("AUTHORIZATION"));
                Assert.That(validationResult.RequestData["response_message"], Is.EqualTo("Success"));
                Assert.That(validationResult.RequestData["customer_email"], Is.EqualTo("TestCustomerEmail"));
                Assert.That(validationResult.RequestData["phone_number"], Is.EqualTo("+1 (201)-234-6789"));
                Assert.That(validationResult.RequestData["customer_name"], Is.EqualTo("test customer&#39;s name"));
                Assert.That(validationResult.RequestData["status"], Is.EqualTo("02"));
            });

        }
    }
}