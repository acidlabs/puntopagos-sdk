using System;
using System.Collections.Generic;
using System.Globalization;
using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CreateTransactionResponseDto
    {
        /// <summary>
        /// Unique identifier of the transaction Payment Point
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// Unique identifier of the client's transaction
        /// </summary>
        public string TransactionId { get; private set; }
        /// <summary>
        /// Indicate the cause of transaction was not satisfactory
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// Total value of the transaction
        /// </summary>
        public CurrencyDto Currency { get; private set; }
        /// <summary>
        /// Identifier of the payment method
        /// </summary>
        public PaymentMethod? PaymentMethod { get; private set; }
        /// <summary>
        /// Indicates whether the transaction was not satisfactory
        /// </summary>
        public bool WithError { get; private set; }
        /// <summary>
        /// Get the url to continue the Payment Process
        /// </summary>
        public string ProcessUrl { get; private set; }

        public CreateTransactionResponseDto(IDictionary<string,string> values, IConfiguration configuration)
        {
            if (values.ContainsKey("trx_id"))
                TransactionId = values["trx_id"];
            if (values.ContainsKey("token"))
                Token = values["token"];
            if (values.ContainsKey("respuesta"))
                WithError = values["respuesta"] != "00";
            if (values.ContainsKey("monto"))
                Currency = new CurrencyDto(values["monto"]);
            if (values.ContainsKey("error"))
                ErrorMessage = values["error"];
            if (values.ContainsKey("medio_pago"))
                PaymentMethod = values["medio_pago"] == "999"
                                    ? null
                                    : (PaymentMethod?) Enum.Parse(typeof (PaymentMethod), int.Parse(values["medio_pago"]).ToString(CultureInfo.InvariantCulture));
            ProcessUrl = configuration.GetProcessTransactionUrl() + "/" + Token;
        }
    }
}