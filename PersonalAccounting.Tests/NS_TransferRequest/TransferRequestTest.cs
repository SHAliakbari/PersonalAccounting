
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.Tests.NS_TransferRequest
{
    public class TransferRequestTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void When_Calculate_Destination_Amount_Then_Only_Destination_Amount_Changed()
        {
            var obj = new TransferRequest()
            {
            };

            obj.SourceAmount = 100;
            obj.SourceCurrencyName = "CAD";
            obj.DestinationCurrencyName = "CAD";
            obj.ExchangeRate = 20;
            obj.FeeCurrencyName = "CAD";
            obj.Fee = 5;

            obj.CalculateDestinationAmount();

            Assert.That(obj.SourceAmount, Is.EqualTo(100));
            Assert.That(obj.SourceCurrencyName, Is.EqualTo("CAD"));
            Assert.That(obj.DestinationCurrencyName, Is.EqualTo("CAD"));
            Assert.That(obj.ExchangeRate, Is.EqualTo(20));
            Assert.That(obj.FeeCurrencyName, Is.EqualTo("CAD"));
            Assert.That(obj.Fee, Is.EqualTo(5));
        }

        [Test]
        public void When_Calculate_Destination_Amount_With_Same_Currency_For_Fee_Then_Calculate_Destination_Amount()
        {
            var obj = new TransferRequest()
            {
            };

            obj.SourceAmount = 100;
            obj.SourceCurrencyName = "CAD";
            obj.DestinationCurrencyName = "USD";
            obj.ExchangeRate = 20;
            obj.FeeCurrencyName = "CAD";
            obj.Fee = 5;

            obj.CalculateDestinationAmount();

            Assert.That(obj.DestinationAmount, Is.EqualTo((100-5)*20));

        }

        [Test]
        public void When_Calculate_Destination_Amount_Without_Same_Currency_For_Fee_Then_Calculate_Destination_Amount()
        {
            var obj = new TransferRequest()
            {
            };

            obj.SourceAmount = 100;
            obj.SourceCurrencyName = "CAD";
            obj.DestinationCurrencyName = "USD";
            obj.ExchangeRate = 20;
            obj.FeeCurrencyName = "USD";
            obj.Fee = 5;

            obj.CalculateDestinationAmount();

            Assert.That(obj.DestinationAmount, Is.EqualTo(100*20-5));

        }



        [Test]
        public void When_Calculate_Source_Amount_With_Same_Currency_For_Fee_Then_Calculate_Source_Amount()
        {
            var obj = new TransferRequest()
            {
            };

            obj.DestinationAmount = 100;
            obj.SourceCurrencyName = "CAD";
            obj.DestinationCurrencyName = "USD";
            obj.ExchangeRate = 20;
            obj.FeeCurrencyName = "CAD";
            obj.Fee = 5;

            obj.CalculateSourceAmount();

            Assert.That(obj.SourceAmount, Is.EqualTo((100/20)+5));

        }

        [Test]
        public void When_Calculate_Source_Amount_Without_Same_Currency_For_Fee_Then_Calculate_Source_Amount()
        {
            var obj = new TransferRequest()
            {
            };

            obj.DestinationAmount = 100;
            obj.SourceCurrencyName = "CAD";
            obj.DestinationCurrencyName = "USD";
            obj.ExchangeRate = 20;
            obj.FeeCurrencyName = "USD";
            obj.Fee = 5;

            obj.CalculateSourceAmount();

            Assert.That(obj.SourceAmount, Is.EqualTo((100m+5m)/20m));

        }
    }
}