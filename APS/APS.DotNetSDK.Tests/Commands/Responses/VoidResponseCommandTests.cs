using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Tests.Commands.Responses
{
    public class VoidResponseCommandTests
    {
        readonly VoidResponseCommand voidResponseCommandTest = new();

        [Test]
        public void BuildNotificationCommand_Void_ReturnsErrorWhenNoCommand()
        {
            IDictionary<string, string>? responseObjectTest = new Dictionary<string, string>();

            //act
            //assert
            var expectedException = "Response does not contain any command. Please contact the SDK provider.";
            var actualException = Assert.Throws<InvalidNotification>(() => voidResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Void_ReturnsErrorWhenResponseCommandMismatch()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"command", "AUTHORIZATION" }
            };

            //act
            //assert
            var expectedException = "Invalid Command received from payment gateway:AUTHORIZATION";
            var actualException = Assert.Throws<InvalidNotification>(() => voidResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Void_ReturnsNoError()
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

                {"command", "VOID_AUTHORIZATION" },
                {"merchant_reference", "TestMerchantReference"},
                {"fort_id", "TestFortId"},
                {"order_description", "TestOrderDescription"},
            };

            //act
            //assert
            Assert.DoesNotThrow(() => voidResponseCommandTest.BuildNotificationCommand(responseObjectTest));
        }
    }
}
