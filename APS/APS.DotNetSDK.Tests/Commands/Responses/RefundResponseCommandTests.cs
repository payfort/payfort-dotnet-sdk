using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Tests.Commands.Responses
{
    public class RefundResponseCommandTests
    {
        readonly RefundResponseCommand refundResponseCommandTest = new();

        [Test]
        public void BuildNotificationCommand_Refund_ReturnsErrorWhenNoCommand()
        {
            IDictionary<string, string>? responseObjectTest = new Dictionary<string, string>();

            //act
            //assert
            var expectedException = "Response does not contain any command. Please contact the SDK provider.";
            var actualException = Assert.Throws<InvalidNotification>(() => refundResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Refund_ReturnsErrorWhenResponseCommandMismatch()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"command", "AUTHORIZATION" }
            };

            //act
            //assert
            var expectedException = "Invalid Command received from payment gateway:AUTHORIZATION";
            var actualException = Assert.Throws<InvalidNotification>(() => refundResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Refund_ReturnsNoError()
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

                {"command", "REFUND" },
                {"merchant_reference", "TestMerchantReference"},
                {"amount", "24.3"},
                {"currency", "TestCurrency"},
                {"maintenance_reference", "TestMaintenanceReference"},
                {"fort_id", "TestFortId"},
                {"order_description", "TestOrderDescription"},
            };

            //act
            //assert
            Assert.DoesNotThrow(() => refundResponseCommandTest.BuildNotificationCommand(responseObjectTest));
        }
    }
}
