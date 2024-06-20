using APS.DotNetSDK.Commands;
using APS.Signature.Utils;

namespace APS.DotNetSDK.Tests.Utils
{
    public class TypeExtensionMethodsTests
    {
        [Test]
        public void IsSimpleObject_InputIsString_ReturnsTrue()
        {
            //arrange
            const string stringTest = "This is a test.";
            var expectedResult = true;

            //act
            var actualResult = TypeExtensionMethods.IsSimpleObject(stringTest.GetType());

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsSimpleObject_InputIsObject_ReturnsFalse()
        {
            //arrange
            var objectTest = new Command();
            var expectedResult = false;

            //act
            var actualResult = TypeExtensionMethods.IsSimpleObject(objectTest.GetType());

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsListObject_InputIsList_ReturnsTrue()
        {
            //arrange
            var testList = new List<Command>();
            var expectedResult = true;

            //act
            var actualResult = TypeExtensionMethods.IsListObject(testList.GetType());

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
