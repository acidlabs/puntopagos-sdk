using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Acid.PuntoPagos.Sdk.Interfaces;
using Moq;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test
{
    [TestFixture]
    public class NotificationTransactionFixture
    {
        private Mock<IConfiguration> _configuration;
        private Mock<IAuthorization> _authorization;
        private Mock<IExecutorWeb> _webExecute;
        private Mock<ILog> _logger;

        [SetUp]
        public void Setup()
        {
            _authorization = new Mock<IAuthorization>();
            _configuration = new Mock<IConfiguration>();
            _webExecute = new Mock<IExecutorWeb>();
            _logger = new Mock<ILog>();
        }

        private Transaction CreateTransaction()
        {
            return new Transaction(_configuration.Object, _authorization.Object, _webExecute.Object, _logger.Object);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void when_call_notification_transaction_without_fecha_header_then_throw_argument_exception()
        {
            var httpRequest = WebRequest.Create("http://localhost/url");
            CreateTransaction().NotificationTransaction(httpRequest);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void when_call_notification_transaction_without_authorization_header_then_throw_argument_exception()
        {
            var httpRequest = WebRequest.Create("http://localhost/url");
            httpRequest.Headers.Add("fecha", "Thu, 14 Jun 2012 16:56:25 GMT");
            _webExecute.Setup(x => x.GetDataFromRequest(It.IsAny<WebRequest>())).Returns(new Dictionary<string, string>
                                                             {
                                                                 {"respuesta", "00"},
                                                                 {"medio_pago", "999"},
                                                                 {"monto", "1000000.00"},
                                                                 {"fecha_aprobacion", "2009-06-15T20:49:00"},
                                                                 {"numero_operacion", "7897851487"},
                                                                 {"codigo_autorizacion", "34581"},
                                                                 {"token", "9XJ08401WN0071839"},
                                                                 {"trx_id", "9787415132"}
                                                             });
            CreateTransaction().NotificationTransaction(httpRequest);
        }
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void when_call_notification_without_variables_then_throw_argument_exception()
        {
            var httpRequest = WebRequest.Create("http://localhost/url");
            httpRequest.Headers.Add("fecha", "Thu, 14 Jun 2012 16:56:25 GMT");
            httpRequest.Headers.Add("Autorizacion", "PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=");
            _webExecute.Setup(x => x.GetDataFromRequest(It.IsAny<WebRequest>())).Returns(new Dictionary<string, string>());
            
            CreateTransaction().NotificationTransaction(httpRequest);
        }

        [Test]
        public void when_call_notification_whit_correct_variable_and_incorrect_transaction_return_notification_dto_with_error()
        {
            var httpRequest = WebRequest.Create("http://localhost/url");
            httpRequest.Headers.Add("fecha", "Thu, 14 Jun 2012 16:56:25 GMT");
            httpRequest.Headers.Add("Autorizacion", "PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=");
            _authorization.Setup(x => x.GetAuthorizationHeader(It.IsAny<string>())).Returns(
                "PP 0PN5J17HBGZHT7ZZ3X82:AVrD3e9idIqAxRSH+15Yqz7qQkc=");
            _webExecute.Setup(x => x.GetDataFromRequest(It.IsAny<WebRequest>())).Returns(new Dictionary<string, string>
                                                             {
                                                                 {"respuesta", "00"},
                                                                 {"medio_pago", "999"},
                                                                 {"monto", "1000000.00"},
                                                                 {"fecha_aprobacion", "2009-06-15T20:49:00"},
                                                                 {"numero_operacion", "7897851487"},
                                                                 {"codigo_autorizacion", "34581"},
                                                                 {"token", "9XJ08401WN0071839"},
                                                                 {"trx_id", "9787415132"}
                                                             });
            var notificationTransactionDto = CreateTransaction().NotificationTransaction(httpRequest);

            Assert.IsTrue(notificationTransactionDto.WithError);
            Assert.IsFalse(notificationTransactionDto.IsTransactionSuccessful());
            Assert.AreEqual("{\"respuesta\":\"99\",\"token\":\"9XJ08401WN0071839\"}", notificationTransactionDto.GenerateResponse());
        }

        [Test]
        public void when_call_notification_whit_correct_variable_and_correct_transaction_return_notification_dto_without_error()
        {
            var httpRequest = WebRequest.Create("http://localhost/url");
            httpRequest.Headers.Add("fecha", "Thu, 14 Jun 2012 16:56:25 GMT");
            httpRequest.Headers.Add("Autorizacion", "PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=");
            _authorization.Setup(x => x.GetAuthorizationHeader(It.IsAny<string>())).Returns(
                "PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=");
            _webExecute.Setup(x => x.GetDataFromRequest(It.IsAny<WebRequest>())).Returns(new Dictionary<string, string>
                                                             {
                                                                 {"respuesta", "00"},
                                                                 {"medio_pago", "999"},
                                                                 {"monto", "1000000.00"},
                                                                 {"fecha_aprobacion", "2009-06-15T20:49:00"},
                                                                 {"numero_operacion", "7897851487"},
                                                                 {"codigo_autorizacion", "34581"},
                                                                 {"token", "9XJ08401WN0071839"},
                                                                 {"trx_id", "9787415132"}
                                                             });
            var notificationTransactionDto = CreateTransaction().NotificationTransaction(httpRequest);

            Assert.IsFalse(notificationTransactionDto.WithError);
            Assert.IsTrue(notificationTransactionDto.IsTransactionSuccessful());
            Assert.AreEqual("{\"respuesta\":\"00\",\"token\":\"9XJ08401WN0071839\"}", notificationTransactionDto.GenerateResponse());
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void when_call_notification_with_null_headers_then_throw_argument_exception()
        {
            CreateTransaction().NotificationTransaction(null, new NameValueCollection());
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void when_call_notification_with_null_params_then_throw_argument_exception()
        {
            CreateTransaction().NotificationTransaction(new NameValueCollection(), null);
        }

        [Test]
        public void given_correct_variable_when_call_notification_transaction_then_return_dto()
        {
            _authorization.Setup(x => x.GetAuthorizationHeader(It.IsAny<string>())).Returns("PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=");
            var header = new NameValueCollection
                             {
                                 {"fecha", "Thu, 14 Jun 2012 16:56:25 GMT"},
                                 {"Autorizacion", "PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg="}
                             };
            var @params = new NameValueCollection
                              {
                                  {"respuesta", "00"},
                                  {"medio_pago", "999"},
                                  {"monto", "1000000.00"},
                                  {"fecha_aprobacion", "2009-06-15T20:49:00"},
                                  {"numero_operacion", "7897851487"},
                                  {"codigo_autorizacion", "34581"},
                                  {"token", "9XJ08401WN0071839"},
                                  {"trx_id", "9787415132"}
                              };

            var notificationTransactionDto = CreateTransaction().NotificationTransaction(header, @params);

            Assert.IsFalse(notificationTransactionDto.WithError);
            Assert.IsTrue(notificationTransactionDto.IsTransactionSuccessful());
            Assert.AreEqual("{\"respuesta\":\"00\",\"token\":\"9XJ08401WN0071839\"}", notificationTransactionDto.GenerateResponse());
        }
    }
}