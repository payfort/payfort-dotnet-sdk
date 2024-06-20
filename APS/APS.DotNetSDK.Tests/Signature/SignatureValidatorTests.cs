using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Requests.Airlines;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Tests.Commands;
using APS.Signature;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Signature
{
    public class SignatureValidatorTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private const string Phrase = "PASS";
        private SdkConfigurationDto _sdkConfigurationDto;
        private readonly Mock<ILoggerFactory> _loggerMock = new Mock<ILoggerFactory>();

        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                _loggerMock.Object);
            _sdkConfigurationDto = SdkConfiguration.GetAccount("MainAccount");
        }

        [Test]
        [TestCase(true, "6e35631fc0a493adfa062eb173cf6e37751f6e12dfd3eea9b9eec656559920d2", ShaType.Sha256)]
        [TestCase(true,
            "743f3ea5d8d4c67f07e936512ed4fadfd7236fe39e4d8ec0cf779736155d2edf97e769536c9a074609919b3b62330b43c5a41540b4e48809bdced50904d9fa0a",
            ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfProperties_ReturnsCorrectSignature(bool expectedResult,
            string signature, ShaType shaType)
        {
            //arrange
            var objectTest = new TestRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            var serviceSignatureProvider = new SignatureProvider();
            var service = new SignatureValidator(serviceSignatureProvider);

            //act
            var actualResult = service.ValidateSignature(objectTest, Phrase, shaType, signature);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(true, "2b00a5c944e8619b35c92d2f4026df8468eaf1abc559d28af7308f72113ad4c5", ShaType.Sha256)]
        [TestCase(true,
            "39373f8504f90ea91afc228e6867702c1e5b826c45d1337faa50c5e0519004413044d3295a1b295910fb64015ba636311b622d761d669e67445266b0b24066fd",
            ShaType.Sha512)]
        public void GetSignature_ForAirlines(bool expectedResult,
            string signature, ShaType shaType)
        {
            //arrange
            var objectTest = GetArilineData();
            var serviceSignatureProvider = new SignatureProvider();
            var service = new SignatureValidator(serviceSignatureProvider);

            //act
            var actualResult = service.ValidateSignature(objectTest, Phrase, shaType, signature);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        private RequestCommand GetArilineData()
        {
            var airlineData = new PurchaseRequestCommand()
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Amount = 100,
                AirlineData = new AirlineData()
                {
                    DocumentType = "ysfk",
                    BookingReference = "dsfasd",
                    Passengers = new List<Passenger>()
                    {
                        new Passenger()
                        {
                            FirstName = "Ahmad",
                            LastName = "Khalel",
                            Title = "test",
                            MiddleName = "sdfsdf",
                            SpecificInformation = "sdfsd",
                            FrequentFlyerNumber = "dfdsdf",
                        },
                        new Passenger()
                        {
                            FirstName = "Abdulla",
                            LastName = "Ahmad",
                            Title = "woo",
                            MiddleName = "lsdf",
                            SpecificInformation = "Test",
                            FrequentFlyerNumber = "TestNumber",
                        }
                    },
                    Itinerary = new Itinerary()
                    {
                        Legs = new List<Leg>()
                        {
                            new Leg()
                            {
                                FlightNumber = "400",
                                ExchangeTicketNumber = "345",
                                Fare = "sfsdf",
                                Fees = "4",
                                FareBasis = "df",
                                StopoverPermitted = "fsf",
                                CarrierCode = "sdf",
                                CouponNumber = "4534",
                                ConjunctionTicketNumber = "dfsdf",
                                DestinationAirport = "dsfsdf",
                                Taxes = "33",
                                EndorsementsRestrictions = "dsfsf",
                                DestinationArrivalDate = "4433",
                                DestinationArrivalTime = "44",
                                TravelClass = "4545",
                                DepartureDate = "3434",
                                DepartureTax = "sfsd",
                                DepartureTime = "sfsd",
                                DepartureAirport = "sdfd"
                            }
                        },
                        NumberInParty = "44",
                        OriginCountry = "fd",
                    },

                    Ticket = new Ticket()
                    {
                        TotalFees = "100",
                        Restricted = "sadfads",
                        ConjunctionTicketIndicator = "dfdfsf",
                        ETicket = "sfsdf",
                        ExchangedTicketNumber = "345345",
                        TotalFare = "fadsf",
                        Issue = new TicketIssue()
                        {
                            Address = "testAddress",
                            City = "Test City",
                            Date = "1/2/2001",
                            TravelAgentName = "Test",
                            CarrierCode = "20034",
                            CarrierName = "testName",
                            TravelAgentCode = "009848",
                            Country = "Test Country"
                        },
                        TicketNumber = "24243",
                        TotalTaxes = "4335",
                        TaxOrFee = new List<TaxOrFee>()
                        {
                            new TaxOrFee()
                            {
                                Amount = "1000",
                                TaxOrFeeType = "idkkj"
                            },
                            new TaxOrFee()
                            {
                                Amount = "2000",
                                TaxOrFeeType = "idkddkj"
                            }
                        }
                    },
                    PlanNumber = 44993.99,
                    TransactionType = "asdfasd"
                }
            };
            return airlineData;
        }
    }
}