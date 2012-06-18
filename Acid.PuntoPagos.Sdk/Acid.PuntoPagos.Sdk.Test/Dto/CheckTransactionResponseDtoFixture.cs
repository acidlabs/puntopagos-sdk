using System;
using Acid.PuntoPagos.Sdk.Dtos;
using Acid.PuntoPagos.Sdk.Services;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test.Dto
{
    [TestFixture]
    public class CheckTransactionResponseDtoFixture
    {
        [Test]
        public void given_default_json_response_from_punto_pago_when_call_json_serializer_then_return_check_transaction_response_dto()
        {
            const string json =
                "{\"respuesta\":\"00\",\"token\":\"9XJ08401WN0071839\",\"trx_id\":9787415132,\"medio_pago\":\"999\",\"monto\":1000000.00,\"fecha_aprobacion\":\"2009-06-15T20:49:00\",\"numero_operacion\":\"7897851487\",\"codigo_autorizacion\":\"34581\"}";

            var checkTransactionDto = new CheckTransactionResponseDto(JsonSerializerService.DeserializeFromString(json));

            Assert.IsFalse(checkTransactionDto.WithError, "WithError");
            Assert.AreEqual(checkTransactionDto.Token, "9XJ08401WN0071839", "Token");
            Assert.AreEqual(checkTransactionDto.TransactionId, "9787415132", "TransactionId");
            Assert.IsNull(checkTransactionDto.PaymentMethod, "PaymentMethod");
            Assert.AreEqual(checkTransactionDto.Currency.Mount, 1000000, "Mount");
            Assert.AreEqual(checkTransactionDto.DateTimeAcceptance, new DateTime(2009, 6, 15, 20, 49, 00), "DateTimeAcceptance");
            Assert.AreEqual(checkTransactionDto.OperationNumber, "7897851487", "OperationNumber");
            Assert.AreEqual(checkTransactionDto.AuthorizationCode, "34581", "AuthorizationCode");
        }
        [Test]
        public void given_error_json_response_from_punto_pago_when_call_json_serializer_then_return_check_transaction_response_dto()
        {
            const string json = "{\"respuesta\":\"99\",\"token\":\"9XJ08401WN0071839\",\"error\":\"Pago Rechazado\"}";

            var checkTransactionDto = new CheckTransactionResponseDto(JsonSerializerService.DeserializeFromString(json));

            Assert.IsTrue(checkTransactionDto.WithError, "WithError");
            Assert.AreEqual(checkTransactionDto.Token, "9XJ08401WN0071839", "Token");
            Assert.AreEqual(checkTransactionDto.ErrorMessage, "Pago Rechazado", "ErrorMessage");
        }
    }
}