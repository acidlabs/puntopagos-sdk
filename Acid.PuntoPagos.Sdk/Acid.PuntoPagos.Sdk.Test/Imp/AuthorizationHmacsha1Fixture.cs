using Acid.PuntoPagos.Sdk.Imp;
using Acid.PuntoPagos.Sdk.Interfaces;
using Moq;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test.Imp
{
    [TestFixture]
    public class AuthorizationHmacsha1Fixture
    {
        private Mock<IClientConfiguration> _clientConfiguration;
        private Mock<ILog> _logger;

        [SetUp]
        public void Setup()
        {
            _clientConfiguration = new Mock<IClientConfiguration>();
            _logger = new Mock<ILog>();
        }

        private AuthorizationHmacsha1 GenerateAuthorization()
        {
            return new AuthorizationHmacsha1(_clientConfiguration.Object, _logger.Object);
        }

        [Test]
        public void when_call_get_authorization_header_with_parameters_then_return_authorization_string()
        {
            const string message = "transaccion/crear\n9787415132\n1000000.00\nThu, 14 Jun 2012 16:56:25 GMT";
            _clientConfiguration.Setup(x => x.GetClientKey()).Returns("0PN5J17HBGZHT7ZZ3X82");
            _clientConfiguration.Setup(x => x.GetClientSecret()).Returns("uV3F4YluFJax1cKnvbcGwgjvx4QpvB+leU8dUj2o");

            var authorization = GenerateAuthorization().GetAuthorizationHeader(message);

            Assert.AreEqual("PP 0PN5J17HBGZHT7ZZ3X82:xMO3VqXtzkwbnkEm09NLeKePcwg=", authorization);
        }

        [Test]
        public void when_call_get_authorization_header_with_parameters_then_return_authorization_string_other_other()
        {
            const string message = "transaccion/notificacion\n9XJ08401WN0071839\n9787415132\n1000000.00\nMon, 15 Jun 2009 20:48:30 GMT";
            _clientConfiguration.Setup(x => x.GetClientKey()).Returns("0PN5J17HBGZHT7ZZ3X82");
            _clientConfiguration.Setup(x => x.GetClientSecret()).Returns("uV3F4YluFJax1cKnvbcGwgjvx4QpvB+leU8dUj2o");

            var authorization = GenerateAuthorization().GetAuthorizationHeader(message);

            Assert.AreEqual("PP 0PN5J17HBGZHT7ZZ3X82:fU6+JLYWzOSGuo76XJzT/Z596Qg=", authorization);
        }
    }
}