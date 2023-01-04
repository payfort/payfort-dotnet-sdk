using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Tests.Commands.Responses
{
    public class TokenizationResponseCommandTests
    {
        readonly TokenizationResponseCommand tokenizationResponseCommandTest = new();

        [Test]
        public void BuildNotificationCommand_Tokenization_ReturnsErrorWhenNoCommand()
        {
            IDictionary<string, string>? responseObjectTest = new Dictionary<string, string>();

            //act
            //assert
            var expectedException = "Response does not contain any command. Please contact the SDK provider.";
            var actualException = Assert.Throws<InvalidNotification>(() => tokenizationResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Tokenization_ReturnsErrorWhenResponseCommandMismatch()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"service_command", "AUTHORIZATION" }
            };

            //act
            //assert
            var expectedException = "Invalid Command received from payment gateway:AUTHORIZATION";
            var actualException = Assert.Throws<InvalidNotification>(() => tokenizationResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Tokenization_ReturnsNoError()
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

                {"service_command", "TOKENIZATION" },
                {"merchant_reference", "TestMerchantReference"},
                {"expiry_date", "TestExpiryDate"},
                {"card_number", "TestCardNumber"},
                {"card_holder_name", "TestCardHolderName"},
                {"card_bin", "TestCardBin"},

                {"token_name", "TestTokenName"},

                {"return_url", "TestReturnUrl"},
                {"remember_me", "TestRememberMe"},
            };

            //act
            //assert
            Assert.DoesNotThrow(() => tokenizationResponseCommandTest.BuildNotificationCommand(responseObjectTest));
        }
    }
}
