using System;
using System.Collections.Specialized;
using System.Net;
using Acid.PuntoPagos.Sdk.Dtos;
using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk
{
    /// <summary>
    /// Class defining communications puntopagos
    /// </summary>
    public class Transaction
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorization _authorization;
        private readonly IExecutorWeb _webExecute;
        private readonly ILog _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="authorization"></param>
        /// <param name="webExecute"></param>
        /// <param name="logger"></param>
        public Transaction(IConfiguration configuration, IAuthorization authorization, IExecutorWeb webExecute, ILog logger)
        {
            _configuration = configuration;
            _authorization = authorization;
            _webExecute = webExecute;
            _logger = logger;
        }
        /// <summary>
        /// Create the transaction in puntopago
        /// </summary>
        /// <param name="transactionDto">Data for the token that begins the payment process.</param>
        /// <returns>CreateTransactionResponseDto with contains the Token and Url for the payment process</returns>
        public CreateTransactionResponseDto CreateTransaction(CreateTransactionRequestDto transactionDto)
        {
            _logger.Debug(string.Format("Call to Create Transaction for TransactionId: {0}, and amount: {1}", transactionDto.TransactionId, transactionDto.Currency));
            var dateNow = DateTime.UtcNow;
            var message = string.Format("{0}{1}{2}{1}{3}{1}{4}", _configuration.GetCreateTransactionFunction(), "\n",
                                        transactionDto.TransactionId, transactionDto.Currency, dateNow.ToString("r"));
            _logger.Debug(string.Format("Generate message {0}, for PuntoPago", message));
            
            var authorization = _authorization.GetAuthorizationHeader(message);
            
            var response = _webExecute.Execute(_configuration.GetCreateTransactionUrl(), "POST", transactionDto.GetJson(), authorization, dateNow);
            _logger.Debug("End create transaction, start CreateTransactionResponseDto");
            
            var createTransactionResponseDto = new CreateTransactionResponseDto(response, _configuration);
            _logger.Info(string.Format("Create new Transaction for TransactionId: {0} with Token: {1}",
                                       createTransactionResponseDto.TransactionId, createTransactionResponseDto.Token));
            return createTransactionResponseDto;
        }
        /// <summary>
        /// Verify if the result of payment process was successful or not.
        /// </summary>
        /// <param name="request">The request from Punto Pagos to the end point previously established for the notification</param>
        /// <returns>NotificationTransactionDto with contains result of the process and data of the payment</returns>
        /// <exception cref="ArgumentNullException">Can throw the exception if there headers "fecha" or "Autorizacion", or if the request does not contain the variables "token", "trx_id" or "monto"</exception>
        [Obsolete("Use new version with NameValueCollection for headers and parameters")]
        public NotificationTransactionDto NotificationTransaction(WebRequest request)
        {
            var @params = new NameValueCollection();
            var requestParameters = _webExecute.GetDataFromRequest(request);
            if (requestParameters == null)
            {
                var error = new ArgumentNullException("request", "The request not contains data");
                _logger.Error("The request not contains header 'fecha'", error);
                throw error;
            }
            
            foreach (var data in requestParameters)
            {
                @params.Add(data.Key, data.Value);
            }
            return NotificationTransaction(request.Headers, @params);
        }

        /// <summary>
        /// Verify if the result of payment process was successful or not.
        /// </summary>
        /// <param name="headers">Headers of Request</param>
        /// <param name="params">Parameters of Request</param>
        /// <returns>NotificationTransactionDto with contains result of the process and data of the payment</returns>
        /// <exception cref="ArgumentNullException">Can throw the exception if there headers "fecha" or "Autorizacion", or if the request does not contain the variables "token", "trx_id" or "monto"</exception>
        public NotificationTransactionDto NotificationTransaction(NameValueCollection headers, NameValueCollection @params)
        {
            _logger.Debug("Start NotificationTransaction");
            if (headers == null)
            {
                var error = new ArgumentNullException("headers", "The headers are null");
                _logger.Error("The headers are null", error);
                throw error;
            }
            if(@params == null)
            {
                var error = new ArgumentNullException("params", "The @params is null");
                _logger.Error("The @params are null", error);
                throw error;
            }
            if (headers["fecha"] == null)
            {
                var error = new ArgumentNullException("fecha", "The request not contains header 'fecha'");
                _logger.Error("The request not contains header 'fecha'", error);
                throw error;
            }
            if (headers["Autorizacion"] == null)
            {
                var error = new ArgumentNullException("Autorizacion", "The request not contains header 'Autorizacion'");
                _logger.Error("The request not contains header 'Autorizacion'", error);
                throw error;
            }

            var dateTime = headers.Get("Fecha");
            var notificationTransactionDto = new NotificationTransactionDto(@params);
            _logger.Debug(string.Format("End read data from Request, for Token {0} and TransactionId {1}",
                                        notificationTransactionDto.Token, notificationTransactionDto.TransactionId));

            if (string.IsNullOrEmpty(notificationTransactionDto.Token) || notificationTransactionDto.TransactionId == ulong.MinValue || notificationTransactionDto.Currency == null)
            {
                var error = new ArgumentNullException("token, transactionId, amount", "Some of the following variables do not exist or is empty");
                _logger.Error("Some of the following variables (token, transactionId, amount) do not exist or is empty", error);
                throw error;
            }

            var message = string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}", _configuration.GetNotificationTransactionFunction(), "\n",
                                        notificationTransactionDto.Token, notificationTransactionDto.TransactionId, notificationTransactionDto.Currency, dateTime);
            _logger.Debug(string.Format("Generate message for verificate notification transaction: {0}", message));

            notificationTransactionDto.WithError = _authorization.GetAuthorizationHeader(message) != headers.Get("Autorizacion");
            _logger.Info(string.Format("The transaction with token {0} and TransactionId {1}, was {2}",
                                       notificationTransactionDto.Token, notificationTransactionDto.TransactionId,
                                       notificationTransactionDto.IsTransactionSuccessful()
                                           ? "Successful"
                                           : "Unsuccessful"));

            _logger.Debug("End NotificationTransaction");
            return notificationTransactionDto;
        }

        /// <summary>
        /// Verify if the result of payment process was successful or not in any time.
        /// </summary>
        /// <param name="checkTransaction">To query data, necessitating the token, transaction client id and amount.</param>
        /// <returns>CheckTransactionResponseDto with contains result of the process and data of the payment</returns>
        public CheckTransactionResponseDto CheckStatusTransaction(CheckTransactionRequestDto checkTransaction)
        {
            _logger.Debug("Start CheckStatusTransaction");
            var dateNow = DateTime.UtcNow;
            var message = string.Format("{0}{1}{2}{1}{3}{1}{4}{5}", _configuration.GetCheckTransactionFunction(), "\n",
                                        checkTransaction.Token, checkTransaction.TransactionId, checkTransaction.Currency,
                                        dateNow.ToString("r"));
            _logger.Debug(string.Format("Generate message for check notification transaction: {0}", message));
        

            var authorization = _authorization.GetAuthorizationHeader(message);

            var response = _webExecute.Execute(_configuration.GetCheckTransactionUrl(), "GET", null, authorization,
                                               dateNow);
            _logger.Debug("End execute check transaction, now create CheckTransactionResponseDto");

            var checkResponseDto = new CheckTransactionResponseDto(response);
            _logger.Info(string.Format("The result for check transaction with Token: {0} and TransactionId {1} is {2}",
                                       checkResponseDto.Token, checkResponseDto.TransactionId,
                                       checkResponseDto.IsTransactionSuccessful()
                                           ? "Successful"
                                           : "Unsuccessful"));

            _logger.Debug("End CheckStatusTransaction");
            return checkResponseDto;
        }
    }
}