using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CreateTransactionRequestDto
    {
        /// <summary>
        /// Unique identifier of the client's transaction
        /// </summary>
        public string Id { get; set; }
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
            if (!string.IsNullOrEmpty(Id))
                json += string.Format("\"trx_id\":{0},", Id);
            if (PaymentMethod.HasValue)
                json += string.Format("\"medio_pago\":\"{0}\"", ((int) PaymentMethod.Value).ToString("000", CultureInfo.InvariantCulture));

            return "{" + json + "}";
        }
        /// <summary>
        /// Create a new object for create process for payment.
        /// </summary>
        /// <param name="mount">Total value of the transaction</param>
        /// <param name="transactionId">Unique identifier of the client's transaction</param>
        public CreateTransactionRequestDto(decimal mount, string transactionId)
        {
            Id = transactionId;
            Currency = new CurrencyDto(mount);
        }
    }
}