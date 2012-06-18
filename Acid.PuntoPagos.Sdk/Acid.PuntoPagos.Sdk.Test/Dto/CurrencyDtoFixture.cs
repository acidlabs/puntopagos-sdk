using Acid.PuntoPagos.Sdk.Dtos;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test.Dto
{
    [TestFixture]
    public class CurrencyDtoFixture
    {
        [Test]
         public void given_string_when_create_currency_then_return_correct_currency()
        {
            const string value = "123.23";
            var currency = new CurrencyDto(value);

            Assert.AreEqual("123.23", currency.ToString());
        }
        [Test]
        public void given_currency_when_call_to_string_get_mount_with_two_decimal()
        {
            var currency = new CurrencyDto(10000.23256m);

            Assert.AreEqual("10000.23", currency.ToString());
        }
    }
}