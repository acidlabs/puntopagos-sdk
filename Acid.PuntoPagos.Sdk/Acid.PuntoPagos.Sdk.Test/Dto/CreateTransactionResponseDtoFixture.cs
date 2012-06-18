using Acid.PuntoPagos.Sdk.Dtos;
using Acid.PuntoPagos.Sdk.Interfaces;
using Acid.PuntoPagos.Sdk.Services;
using Moq;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test.Dto
{
    [TestFixture]
    public class CreateTransactionResponseDtoFixture
    {
        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
        }

        [Test]
        public void given_default_json_response_from_punto_pago_when_call_json_serializer_then_return_transaction_response_dto()
        {
            const string json = "{\"respuesta\":\"00\",\"token\":\"9XJ08401WN0071839\",\"trx_id\":\"9787415132\",\"medio_pago\":\"999\",\"monto\":\"1000000.00\"}";
            _configuration.Setup(x => x.GetProcessTransactionUrl()).Returns("/url");
            var transactionResponseDto = new CreateTransactionResponseDto(JsonSerializerService.DeserializeFromString(json), _configuration.Object);

            Assert.AreEqual("9XJ08401WN0071839", transactionResponseDto.Token);
            Assert.AreEqual("9787415132", transactionResponseDto.TransactionId);
            Assert.IsFalse(transactionResponseDto.WithError);
            Assert.IsNull(transactionResponseDto.PaymentMethod);
            Assert.AreEqual(1000000, transactionResponseDto.Currency.Mount);
            Assert.IsNullOrEmpty(transactionResponseDto.ErrorMessage);
            Assert.AreEqual("/url/9XJ08401WN0071839", transactionResponseDto.ProcessUrl);
        }

        [Test]
        public void given_json_response_with_error_from_punto_pago_when_call_json_serializer_then_return_transaction_response_dto_with_error()
        {
            const string json = "{\"respuesta\":\"99\",\"token\":\"9XJ08401WN0071839\",\"trx_id\":\"9787415132\"}";
            _configuration.Setup(x => x.GetProcessTransactionUrl()).Returns("/url");
            var transactionResponseDto = new CreateTransactionResponseDto(JsonSerializerService.DeserializeFromString(json), _configuration.Object);

            Assert.AreEqual("9XJ08401WN0071839", transactionResponseDto.Token);
            Assert.AreEqual("9787415132", transactionResponseDto.TransactionId);
            Assert.IsTrue(transactionResponseDto.WithError);
            Assert.IsNull(transactionResponseDto.PaymentMethod);
            Assert.IsNull(transactionResponseDto.Currency);
            Assert.IsNullOrEmpty(transactionResponseDto.ErrorMessage);
            Assert.AreEqual("/url/9XJ08401WN0071839", transactionResponseDto.ProcessUrl);
        }
    }
}