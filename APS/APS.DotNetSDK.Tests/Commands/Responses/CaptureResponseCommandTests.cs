using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Tests.Commands.Responses
{
    public class CaptureResponseCommandTests
    {
        readonly CaptureResponseCommand captureResponseCommandTest = new();

        [Test]
        public void BuildNotificationCommand_Capture_ReturnsErrorWhenNoCommand()
        {
            IDictionary<string, string>? responseObjectTest = new Dictionary<string, string>();

            //act
            //assert
            var expectedException = "Response does not contain any command. Please contact the SDK provider.";
            var actualException = Assert.Throws<InvalidNotification>(() => captureResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Capture_ReturnsErrorWhenResponseCommandMismatch()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"command", "AUTHORIZATION" }
            };

            //act
            //assert
            var expectedException = "Invalid Command received from payment gateway:AUTHORIZATION";
            var actualException = Assert.Throws<InvalidNotification>(() => captureResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Capture_ReturnsNoError()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"access_code", "TestAccessCode"},
                {"merchant_identifier", "TestMerchantIdentifier" },
                {"language", "TestLanguage"},
                {"signature", "TestSignature"},
                {"response_message", "TestResponseMessage"},
                {"response_code", "TestResponseCode"},
                {"status", "TestStatus"},
                {"command", "CAPTURE" },
                {"merchant_reference", "TestMerchantReference"},
                {"amount", "24.3"},
                {"currency", "TestCurrency"},
                {"fort_id", "TestFortId"},
                {"order_description", "TestOrderDescription"},
                {"acquirer_response_code","TestAcquirerResponseCode"},
                {"acquirer_response_message","TestAcquirerResponseMessage"}
            };

            //act
            //assert
            Assert.DoesNotThrow(() => captureResponseCommandTest.BuildNotificationCommand(responseObjectTest));
        }
    }
}
