using APS.DotNetSDK.Utils;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Utils
{
    public class DictionaryExtensionsTests
    {
        [Test]
        public void DictionaryExtensions_InputIsNotNull_ReturnsCorrectOutput()
        {
            //arrange
            var dictionary = new Dictionary<string, string>()
            {
                { "test_key1", "test_value1" },
                { "test_key2", "test_value2" }
            };

            //act
            string joinedDictionary = dictionary.JoinElements(",");

            //assert
            var expectedValue = "test_key1=test_value1,test_key2=test_value2";
            Assert.That(joinedDictionary, Is.EqualTo(expectedValue));
        }

        [Test]
        public void DictionaryExtensions_InputIsNull_ReturnsEmptyValue()
        {
            //arrange
            IDictionary<string, string>? dictionary = null;

            //act
            string joinedDictionary = dictionary.JoinElements(",");

            //assert
            Assert.IsEmpty(joinedDictionary);
        }

        [Test]
        public void CreateObject_InputIsValid_ReturnsCorrectOutput()
        {
            //arrange
            var dictionary = new Dictionary<string, string>()
            {
                { "animal_type", "dog" },
                { "animal_name", "Max" }
            };

            //act
            var actualAnimal = dictionary.CreateObject<Animal>();

            //assert
            Assert.That(actualAnimal.Type, Is.EqualTo(dictionary["animal_type"]));
            Assert.That(actualAnimal.Name, Is.EqualTo(dictionary["animal_name"]));
        }

        [Test]
        public void CreateObject_SomeInputCannotBeMapped_ReturnsNullProperty()
        {
            //arrange
            var dictionary = new Dictionary<string, string>()
            {
                { "animal_location", "sea" },
                { "animal_name", "Max" }
            };

            //act
            var actualAnimal = dictionary.CreateObject<Animal>();

            //assert
            Assert.IsNull(actualAnimal.Type);
            Assert.That(actualAnimal.Name, Is.EqualTo(dictionary["animal_name"]));
        }

    }

        internal class Animal
        {
            [JsonPropertyName("animal_type")]
            public string? Type { get; set; }


            [JsonPropertyName("animal_name")]
            public string? Name { get; set; }
        }
}