using System;
using System.Net;
using Acid.PuntoPagos.Sdk.Dtos;
using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk
{
    public class Transaction
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorization _authorization;
        private readonly IExecutorWeb _webExecute;
        private readonly ILog _logger;

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
        public NotificationTransactionDto NotificationTransaction(WebRequest request)
        {
            _logger.Debug("Start NotificationTransaction");
            if (request.Headers["fecha"] == null)
            {
                var error = new ArgumentNullException("fecha", "The request not contains header 'fecha'");
                _logger.Error("The request not contains header 'fecha'", error);
                throw error;
            }
            if (request.Headers["Autorizacion"] == null)
            {
                var error = new ArgumentNullException("Autorizacion", "The request not contains header 'Autorizacion'");
                _logger.Error("The request not contains header 'Autorizacion'", error);
                throw error;
            }

            var dateTime = request.Headers.Get("Fecha");
            var notificationTransactionDto = new NotificationTransactionDto(_webExecute.GetDataFromRequest(request));
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
            
            notificationTransactionDto.WithError = _authorization.GetAuthorizationHeader(message) != request.Headers.Get("Autorizacion");
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