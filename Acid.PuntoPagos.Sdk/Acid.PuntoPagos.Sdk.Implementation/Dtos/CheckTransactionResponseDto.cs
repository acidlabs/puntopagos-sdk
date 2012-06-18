using System;
using System.Collections.Generic;
using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CheckTransactionResponseDto
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
        /// Total value of the transaction
        /// </summary>
        public CurrencyDto Currency { get; private set; }
        /// <summary>
        /// Identifier of the payment method
        /// </summary>
        public PaymentMethod? PaymentMethod { get; private set; }
        /// <summary>
        /// Date of approval of the transaction
        /// </summary>
        public DateTime? DateTimeAcceptance { get; private set; }
        /// <summary>
        /// Last 4 digits of the credit card
        /// </summary>
        public string CardNumber { get; private set; }
        /// <summary>
        /// Number of shares
        /// </summary>
        public string SharesNumber { get; private set; }
        /// <summary>
        /// Type of Share
        /// </summary>
        public string SharesType { get; private set; }
        /// <summary>
        /// Value of each Share
        /// </summary>
        public CurrencyDto SharesMount { get; private set; }
        /// <summary>
        /// Date of firts expiration
        /// </summary>
        public DateTime? FirtsExpiration { get; private set; }
        /// <summary>
        /// Number of operation at the financial institution
        /// </summary>
        public string OperationNumber { get; private set; }
        /// <summary>
        /// Authorization code of the transaction
        /// </summary>
        public string AuthorizationCode { get; private set; }
        /// <summary>
        /// Indicate the cause of transaction was not satisfactory
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// Indicates whether the transaction was not satisfactory
        /// </summary>
        public bool WithError { get; private set; }

        /// <summary>
        /// Returns if the result of transacction.
        /// <para>
        /// True: if Successful
        /// </para>
        /// <para>False: if Unsuccessful</para>
        /// </summary>
        /// <returns></returns>
        public bool IsTransactionSuccessful()
        {
            return !WithError;
        }

        public CheckTransactionResponseDto(IDictionary<string, string> getDataFromRequest)
        {
            if (getDataFromRequest.ContainsKey("trx_id"))
                TransactionId = getDataFromRequest["trx_id"];
            if (getDataFromRequest.ContainsKey("token"))
                Token = getDataFromRequest["token"];
            if (getDataFromRequest.ContainsKey("monto"))
                Currency = new CurrencyDto(getDataFromRequest["monto"]);
            if (getDataFromRequest.ContainsKey("medio_pago"))
                PaymentMethod = getDataFromRequest["medio_pago"] == "999"
                                    ? null
                                    : (PaymentMethod?)
                                      Enum.Parse(typeof (PaymentMethod),
                                                 int.Parse(getDataFromRequest["medio_pago"]).ToString(
                                                     CultureInfo.InvariantCulture));
            if (getDataFromRequest.ContainsKey("fecha_aprobacion"))
                DateTimeAcceptance = DateTime.ParseExact(getDataFromRequest["fecha_aprobacion"], "yyyy-MM-ddTHH:mm:ss", null);
            if (getDataFromRequest.ContainsKey("CardNumber"))
                CardNumber = getDataFromRequest["CardNumber"];
            if (getDataFromRequest.ContainsKey("num_cuotas"))
                SharesNumber = getDataFromRequest["num_cuotas"];
            if (getDataFromRequest.ContainsKey("tipo_cuotas"))
                SharesType = getDataFromRequest["tipo_cuotas"];
            if (getDataFromRequest.ContainsKey("valor_cuota"))
                SharesMount = new CurrencyDto(getDataFromRequest["valor_cuota"]);
            if (getDataFromRequest.ContainsKey("primer_vencimiento"))
                FirtsExpiration = DateTime.Parse(getDataFromRequest["primer_vencimiento"]);
            if (getDataFromRequest.ContainsKey("numero_operacion"))
                OperationNumber = getDataFromRequest["numero_operacion"];
            if (getDataFromRequest.ContainsKey("codigo_autorizacion"))
                AuthorizationCode = getDataFromRequest["codigo_autorizacion"];
            if (getDataFromRequest.ContainsKey("respuesta"))
                WithError = getDataFromRequest["respuesta"] != "00";
            if (getDataFromRequest.ContainsKey("error"))
                ErrorMessage = getDataFromRequest["error"];
        }
    }
}