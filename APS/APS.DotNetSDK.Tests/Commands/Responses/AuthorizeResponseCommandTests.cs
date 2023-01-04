using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Exceptions;

namespace APS.DotNetSDK.Tests.Commands.Responses
{
    public class AuthorizeResponseCommandTests
    {
        readonly AuthorizeResponseCommand authorizeResponseCommandTest = new();

        [Test]
        public void BuildNotificationCommand_Authorization_ReturnsErrorWhenNoCommand()
        {
            IDictionary<string, string>? responseObjectTest = new Dictionary<string, string>();

            //act
            //assert
            var expectedException = "Response does not contain any command. Please contact the SDK provider.";
            var actualException = Assert.Throws<InvalidNotification>(() => authorizeResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Authorization_ReturnsErrorWhenResponseCommandMismatch()
        {
            IDictionary<string, string> responseObjectTest = new Dictionary<string, string>()
            {
                {"command", "PURCHASE" }
            };

            //act
            //assert
            var expectedException = "Invalid Command received from payment gateway:PURCHASE";
            var actualException = Assert.Throws<InvalidNotification>(() => authorizeResponseCommandTest.BuildNotificationCommand(responseObjectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void BuildNotificationCommand_Authorization_ReturnsNoError()
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

                {"command", "AUTHORIZATION" },
                {"merchant_reference", "TestMerchantReference"},
                {"amount", "24.3"},
                {"currency", "TestCurrency"},
                {"customer_email", "TestCustomerEmail"},
                {"token_name", "TestTokenName"},
                {"fort_id", "TestFortId"},
                {"payment_option", "TestPaymentOption"},
                {"sadad_olp", "TestSadadOlp"},
                {"knet_ref_number", "TestKnetRefNumber"},
                {"third_party_transaction_number", "TestThirdPartyTransactionNumber"},
                {"eci", "TestEci"},
                {"order_description", "TestCurrency"},
                {"customer_ip", "TestCustomerIp"},
                {"customer_name", "TestCustomerName"},
                {"merchant_extra", "TestMerchantExtra"},
                {"merchant_extra1", "TestMerchantExtra1"},
                {"merchant_extra2", "TestMerchantExtra2"},
                {"merchant_extra3", "TestMerchantExtra3"},
                {"merchant_extra4", "TestMerchantExtra4"},
                {"merchant_extra5", "TestMerchantExtra5"},
                {"authorization_code", "TestAuthorizationCode"},
                {"card_holder_name", "TestCardHolderName"},
                {"expiry_date", "TestExpiryDate"},
                {"card_number", "TestCardNumber"},
                {"3ds_url", "TestSecure3dsUrl"},
                {"remember_me", "TestRememberMe"},
                {"return_url", "TestReturnUrl"},
                {"phone_number", "TestPhoneNumber"},
                {"settlement_reference", "TestSettlementReference"},
                {"billing_stateProvince", "TestBillingStateProvince"},
                {"billing_provinceCode", "TestBillingProvinceCode"},
                {"billing_street", "TestBillingStreet"},
                {"billing_street2", "TestBillingStreet2"},
                {"billing_postcode", "TestBillingPostCode"},
                {"billing_country", "TestBillingCountry"},
                {"billing_company", "TestBillingCompany"},
                {"billing_city", "TestBillingCity"},
                {"shipping_stateProvince", "TestShippingStateProvince"},
                {"shipping_provinceCode", "TestShippingProvinceCode"},
                {"shipping_street", "TestShippingStreet"},
                {"shipping_street2", "TestShippingStreet2"},
                {"shipping_source", "TestShippingSource"},
                {"shipping_sameAsBilling", "TestShippingSameAsBilling"},
                {"shipping_postcode", "TestShippingPostCode"},
                {"shipping_country", "TestShippingCountry"},
                {"shipping_company", "TestShippingCompany"},
                {"shipping_city", "TestShippingCity"},
            };

            //act
            //assert
            Assert.DoesNotThrow(() => authorizeResponseCommandTest.BuildNotificationCommand(responseObjectTest));
        }
    }
}
