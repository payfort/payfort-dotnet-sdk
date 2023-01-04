using APS.DotNetSDK.Configuration;

namespace APS.DotNetSDK.Tests.Configuration
{
    public class ApsConfigurationTests
    {
        [Test]
        public void GetConfiguration_ReturnConfig_JsonIsValidAndEnvironmentIsTest()
        {
            ApsConfiguration configuration = new ApsConfiguration(true);

            var expectedConfig = new ApsEnvironmentConfiguration()
            {
                Name = "Test",
                MaintenanceOperations = new OperationsConfiguration()
                {
                    BaseUrl = "https://sbpaymentservices.payfort.com",
                    RequestUri = "/FortAPI/paymentApi",
                },
                RedirectUrl = "https://sbcheckout.payfort.com/FortAPI/paymentPage"
            };
            var actualConfig = configuration.GetEnvironmentConfiguration();

            Assert.Multiple(() =>
            {
                Assert.That(actualConfig.Name, Is.EqualTo(expectedConfig.Name));
                Assert.That(actualConfig.RedirectUrl, Is.EqualTo(expectedConfig.RedirectUrl));
                Assert.That(actualConfig.MaintenanceOperations.BaseUrl,
                    Is.EqualTo(expectedConfig.MaintenanceOperations.BaseUrl));
                Assert.That(actualConfig.MaintenanceOperations.RequestUri,
                    Is.EqualTo(expectedConfig.MaintenanceOperations.RequestUri));
            });
        }

        [Test]
        public void GetConfiguration_ReturnConfig_JsonIsValidAndEnvironmentIsProduction()
        {
            ApsConfiguration configuration = new ApsConfiguration(false);

            var expectedConfig = new ApsEnvironmentConfiguration()
            {
                Name = "Production",
                MaintenanceOperations = new OperationsConfiguration()
                {
                    BaseUrl = "https://paymentservices.payfort.com",
                    RequestUri = "/FortAPI/paymentApi",
                },
                RedirectUrl = "https://checkout.payfort.com/FortAPI/paymentPage"
            };
            var actualConfig = configuration.GetEnvironmentConfiguration();
            Assert.Multiple(() =>
            {
                Assert.That(actualConfig.Name, Is.EqualTo(expectedConfig.Name));
                Assert.That(actualConfig.RedirectUrl, Is.EqualTo(expectedConfig.RedirectUrl));
                Assert.That(actualConfig.MaintenanceOperations.BaseUrl,
                    Is.EqualTo(expectedConfig.MaintenanceOperations.BaseUrl));
                Assert.That(actualConfig.MaintenanceOperations.RequestUri,
                    Is.EqualTo(expectedConfig.MaintenanceOperations.RequestUri));

            });
        }
    }
}
