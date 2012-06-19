using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CreateTransactionRequestDto
    {
        /// <summary>
        /// Unique identifier of the client's transaction
        /// </summary>
        public ulong TransactionId { get; set; }
        /// <summary>
        /// Total value of the transaction
        /// </summary>
        public CurrencyDto Currency { get; set; }
        /// <summary>
        /// Identifier of the payment method
        /// </summary>
        public PaymentMethod? PaymentMethod { get; set; }

        public string GetJson()
        {
            var json = string.Format("\"monto\":{0},", Currency);
            if (TransactionId != ulong.MinValue)
                json += string.Format("\"trx_id\":{0},", TransactionId);
            if (PaymentMethod.HasValue)
                json += string.Format("\"medio_pago\":\"{0}\"", ((int) PaymentMethod.Value).ToString("000", CultureInfo.InvariantCulture));

            return "{" + json + "}";
        }
        /// <summary>
        /// Create a new object for create process for payment.
        /// </summary>
        /// <param name="amount">Total value of the transaction</param>
        /// <param name="transactionId">Unique identifier of the client's transaction</param>
        public CreateTransactionRequestDto(decimal amount, ulong transactionId)
        {
            TransactionId = transactionId;
            Currency = new CurrencyDto(amount);
        }
    }
}