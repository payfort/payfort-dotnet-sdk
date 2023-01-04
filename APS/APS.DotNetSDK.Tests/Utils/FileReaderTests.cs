using APS.DotNetSDK.Utils;

namespace APS.DotNetSDK.Tests.Utils
{
    public class FileReaderTests
    {
        [Test]
        public void ReadFromFile_ReturnsTheReadedText()
        {
            //arrange
            const string filePath = @"Utils\TestFileReader.txt";
            string expectedResult = "This is a test.";

            //act
            var actualResult = FileReader.ReadFromFile(filePath);
            
            //assert 
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
