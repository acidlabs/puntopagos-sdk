using Acid.PuntoPagos.Sdk.Dtos;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test.Dto
{
    [TestFixture]
    public class CreateTransactionRequestDtoFixture
    {
        [Test]
        public void given_transaction_request_dto_with_all_field_when_call_get_json_then_return_json_transaction()
        {
            var transaction = new CreateTransactionRequestDto(123456, "1324567")
                                  {PaymentMethod = PaymentMethod.WebpayTransbank};

            var json = transaction.GetJson();

            Assert.IsTrue(json.Contains("\"trx_id\":1324567"), "Id");
            Assert.IsTrue(json.Contains("\"monto\":123456.00"), "Mount");
            Assert.IsTrue(json.Contains("\"medio_pago\":\"003\""), "PaymentMethod");
            Assert.IsTrue(json.StartsWith("{"), "Start Json");
            Assert.IsTrue(json.EndsWith("}"), "End Json");
        }
        [Test]
        public void given_transaction_request_dto_with_default_field_values_when_call_get_json_then_return_json()
        {
            var transaction = new CreateTransactionRequestDto(123456, "1324567");

            var json = transaction.GetJson();

            Assert.IsTrue(json.Contains("\"trx_id\":1324567"), "Id");
            Assert.IsTrue(json.Contains("\"monto\":123456.00"), "Mount");
            Assert.IsTrue(json.StartsWith("{"), "Start Json");
            Assert.IsTrue(json.EndsWith("}"), "End Json");
        }
    }
}