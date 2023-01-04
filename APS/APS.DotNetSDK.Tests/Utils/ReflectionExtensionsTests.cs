using APS.DotNetSDK.Tests.Signature.Models;
using APS.DotNetSDK.Utils;
using System.Reflection;

namespace APS.DotNetSDK.Tests.Utils
{
    public class ReflectionExtensionsTests
    {
        [Test]
        public void GetJsonPropertyName_ReturnsTheJsonPropertyName()
        {
            //arrange
            var objectTest = new Payment()
            {
                CardType = "TestCardType"
            };
            var expectedResult = "payment_cardtype";

            //act
            MemberInfo prop = objectTest.GetType().GetProperties().First(x => x.Name == "CardType");
            var actualResult = prop.GetJsonPropertyName();

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetJsonPropertyIgnoreSignatureValidation_ReturnsTrue()
        {
            //arrange
            var objectTest = new CustomAttributesTest()
            {
                TestAttribute = "TestAttibute"
            };

            //act
            MemberInfo prop = objectTest.GetType().GetProperties().Where(x => x.Name == "TestAttribute").First();
            var actualResult = prop.GetJsonPropertyIgnoreSignatureValidation();

            //assert
            Assert.That(actualResult, Is.EqualTo(true));
        }

        [Test]
        public void GetJsonPropertyIgnoreSignatureValidation_ReturnsFalse()
        {
            //arrange
            var objectTest = new CustomAttributesTest()
            {
                TestAttribute2 = "TestAttibute2"
            };

            //act
            MemberInfo prop = objectTest.GetType().GetProperties().Where(x => x.Name == "TestAttribute2").First();
            var actualResult = prop.GetJsonPropertyIgnoreSignatureValidation();

            //assert
            Assert.That(actualResult, Is.EqualTo(false));
        }
    }
}
