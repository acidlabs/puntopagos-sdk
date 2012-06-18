using System;
using System.Configuration;
using Acid.PuntoPagos.Sdk.Imp;
using Acid.PuntoPagos.Sdk.Interfaces;
using Configuration = Acid.PuntoPagos.Sdk.Imp.Configuration;

namespace Acid.PuntoPagos.Sdk
{
    /// <summary>
    /// Entry point to begin the payment process
    /// </summary>
    public class PuntoPago
    {
        private readonly Configuration _configuration;
        private ILog _logger;

        public PuntoPago()
        {
            _configuration = new Configuration();
        }

        public PuntoPago SetLog(ILog logger)
        {
            _logger = logger;
            return this;
        }

        /// <summary>
        /// Set the Client Key of Punto Pagos
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public PuntoPago SetKey(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "The key can not be null or empty ");
            _configuration.ClientKey = key;
            return this;
        }
        /// <summary>
        /// Set the Client Secret Code of Punto Pagos
        /// </summary>
        /// <param name="secretCode"></param>
        /// <returns></returns>
        public PuntoPago SetSecretCode(string secretCode)
        {
            if (string.IsNullOrEmpty(secretCode))
                throw new ArgumentNullException("secretCode", "The secretCode can not be null or empty ");
            _configuration.ClientSecret = secretCode;
            return this;
        }
        /// <summary>
        /// Set the Environment of Punto Pagos.
        /// <para>The possibilities are:</para>
        /// <para>Sandbox: For the testing</para>
        /// <para>Production: For the production</para>
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public PuntoPago SetEnvironment(EnvironmentForPuntoPago environment)
        {
            _configuration.Environment = environment.ToString();
            return this;
        }

        /// <summary>
        /// Create a new instance to generate a transaction to Punto Pagos
        /// </summary>
        /// <returns></returns>
        public Transaction CreateTransaction()
        {
            if (_logger == null)
            {
                _logger = Log4NetLoggerProxy.GetLogger("PuntoPagos-sdk");
                _logger.Info("Logger for log4net Start");
            }

            if(string.IsNullOrEmpty(_configuration.ClientKey) || string.IsNullOrEmpty(_configuration.ClientSecret))
            {
                if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["PuntoPago-Secret"]))
                    throw new ArgumentNullException("PuntoPago-Secret", "The PuntoPago-Secret in AppSettings can not be null or empty");

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PuntoPago-Key"]))
                    throw new ArgumentNullException("PuntoPago-Key", "The PuntoPago-Key in AppSettings can not be null or empty");

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PuntoPago-Environment"]))
                    throw new ArgumentNullException("PuntoPago-Environment", "The PuntoPago-Environment in AppSettings can not be null or empty ");

                _configuration.ClientSecret = ConfigurationManager.AppSettings["PuntoPago-Secret"];
                _configuration.ClientKey = ConfigurationManager.AppSettings["PuntoPago-Key"];
                _configuration.Environment = ConfigurationManager.AppSettings["PuntoPago-Environment"];

                _logger.Debug("End configurate ClientSecret, ClientKey and Environment from AppSettings");
            }
            else
            {
                _logger.Debug("End configurate ClientSecret, ClientKey and Environment from Code");
            }

            return new Transaction(_configuration, PuntoPagoFactory.CreateAuthorization(_configuration, _logger),
                                   PuntoPagoFactory.CreateExecutorWeb(_logger), _logger);
        }
        
    }
}