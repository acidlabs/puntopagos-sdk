using System;
using System.Collections.Generic;
using Acid.PuntoPagos.Sdk.Dtos;
using Acid.PuntoPagos.Sdk.Interfaces;
using Moq;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test
{
    [TestFixture]
    public class TransactionFixture
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

        [Test]
        public void when_call_create_transaction_then_return_create_transaction_response_dto()
        {
            _configuration.Setup(x => x.GetCreateTransactionFunction()).Returns("/create");
            _authorization.Setup(x => x.GetAuthorizationHeader(It.IsAny<string>())).Returns("authorization");
            _webExecute.Setup(
                x =>
                x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                          It.IsAny<DateTime>())).Returns(new Dictionary<string, string>
                                                             {
                                                                 {"respuesta", "00"},
                                                                 {"token", "9XJ08401WN0071839"},
                                                                 {"trx_id", "9787415132"}
                                                             });
            var transactionResponseDto = CreateTransaction().CreateTransaction(new CreateTransactionRequestDto(100, 123121));

            Assert.AreEqual("9XJ08401WN0071839", transactionResponseDto.Token);
            Assert.AreEqual(9787415132, transactionResponseDto.TransactionId);
        }
        
    }
}