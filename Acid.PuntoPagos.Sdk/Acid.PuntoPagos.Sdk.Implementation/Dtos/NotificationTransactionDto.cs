using System;
using System.Collections.Generic;
using System.Globalization;

namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class NotificationTransactionDto
    {
        /// <summary>
        /// Unique identifier of the transaction Payment Point
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// Unique identifier of the client's transaction
        /// </summary>
        public ulong TransactionId { get; private set; }
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
        public DateTime DateTimeAcceptance { get; private set; }
        /// <summary>
        /// Last 4 digits of the credit card
        /// </summary>
        public string CardNumber { get; private set; }
        /// <summary>
        /// Number of Instalment
        /// </summary>
        public string InstalmentNumber { get; private set; }
        /// <summary>
        /// Type of Instalment
        /// </summary>
        public string InstalmentType { get; private set; }
        /// <summary>
        /// Value of each Instalment
        /// </summary>
        public CurrencyDto InstalmentAmount { get; private set; }
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
        /// Indicates whether the transaction was not satisfactory
        /// </summary>
        public bool WithError { get; internal set; }

        /// <summary>
        /// Get the json response for send to PuntoPagos.
        /// </summary>
        /// <returns>Json for response to PuntoPagos</returns>
        public string GenerateResponse()
        {
            return "{" + string.Format("\"respuesta\":\"{0}\",\"token\":\"{1}\"", WithError ? "99" : "00", Token) + "}";
        }

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

        public NotificationTransactionDto(IDictionary<string, string> getDataFromRequest)
        {
            if (getDataFromRequest.ContainsKey("trx_id"))
                TransactionId = Convert.ToUInt64(getDataFromRequest["trx_id"]);
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
                DateTimeAcceptance = DateTime.Parse(getDataFromRequest["fecha_aprobacion"]);
            if (getDataFromRequest.ContainsKey("CardNumber"))
                CardNumber = getDataFromRequest["CardNumber"];
            if (getDataFromRequest.ContainsKey("num_cuotas"))
                InstalmentNumber = getDataFromRequest["num_cuotas"];
            if (getDataFromRequest.ContainsKey("tipo_cuotas"))
                InstalmentType = getDataFromRequest["tipo_cuotas"];
            if (getDataFromRequest.ContainsKey("valor_cuota"))
                InstalmentAmount = new CurrencyDto(getDataFromRequest["valor_cuota"]);
            if (getDataFromRequest.ContainsKey("primer_vencimiento"))
                FirtsExpiration = DateTime.Parse(getDataFromRequest["primer_vencimiento"]);
            if (getDataFromRequest.ContainsKey("numero_operacion"))
                OperationNumber = getDataFromRequest["numero_operacion"];
            if (getDataFromRequest.ContainsKey("codigo_autorizacion"))
                AuthorizationCode = getDataFromRequest["codigo_autorizacion"];
            if (getDataFromRequest.ContainsKey("respuesta"))
                WithError = getDataFromRequest["respuesta"] != "00";
        }
    }
}