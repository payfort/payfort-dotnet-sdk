using APS.DotNetSDK.AcquirerResponseMessage;
using APS.DotNetSDK.Commands.Responses;

namespace APS.DotNetSDK.Tests.AcquirerResponseMessage
{
    public class AcquirerResponseMappingTest
    {
        [Test]
        public void GetAcquirerResponseDescription_AcquirerResponseCodeProvided_ReturnCorrectValue()
        {
            //arrange
            var expectedDescription = "Success";
            var acquirerResponseMapping = new AcquirerResponseMapping();

            //act
            var actualDescription = acquirerResponseMapping.GetAcquirerResponseDescription("00");
            //assert
            Assert.That(actualDescription, Is.EqualTo(expectedDescription));
        }

        [Test]
        public void GetAcquirerResponseDescription_AcquirerResponseCodeIsNull_ReturnNullValue()
        {
            //arrange
            var acquirerResponseMapping = new AcquirerResponseMapping();

            //act
            var actualDescription = acquirerResponseMapping.GetAcquirerResponseDescription(null);

            //assert
            Assert.That(actualDescription, Is.Null);
        }

        [Test]
        public void PurchaseResponse_AcquirerResponseCodeIsNull_ReturnNullValue()
        {
            //arrange
            var purchaseResponseCommand = new PurchaseResponseCommand()
            {
                AcquirerResponseCode = null
            };

            //act
            //assert
            Assert.That(purchaseResponseCommand.AcquirerResponseDescription, Is.Null);
        }

        [Test]
        public void PurchaseResponse_AcquirerResponseCodeIsNotSent_ReturnNullValue()
        {
            //arrange
            var purchaseResponseCommand = new PurchaseResponseCommand();

            //act
            //assert
            Assert.That(purchaseResponseCommand.AcquirerResponseDescription, Is.Null);
        }

        [Test]
        public void PurchaseResponse_AcquirerResponseCodeIsSent_ReturnValue()
        {
            //arrange
            var expectedDescription = "Success";
            var purchaseResponseCommand = new PurchaseResponseCommand()
            {
                AcquirerResponseCode = "00",
            };

            //act
            //assert
            Assert.That(purchaseResponseCommand.AcquirerResponseDescription, Is.EqualTo(expectedDescription));
        }

        [Test]
        public void PurchaseResponse_AcquirerResponseCodeIsNotPresentInJson_ReturnErrorDescriptionValue()
        {
            var expectedDescription = "Unrecognized acquirer response code";
            //arrange
            var purchaseResponseCommand = new PurchaseResponseCommand()
            {
                AcquirerResponseCode = "0000",
            };

            //act
            //assert
            Assert.That(purchaseResponseCommand.AcquirerResponseDescription, !Is.EqualTo(expectedDescription));
        }
    }
}
