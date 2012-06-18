using System;
using System.Security.Cryptography;
using System.Text;
using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk.Imp
{
    public class AuthorizationHmacsha1 : IAuthorization
    {
        private readonly IClientConfiguration _configuration;
        private readonly ILog _logger;

        public string GetAuthorizationHeader(string message)
        {
            _logger.Debug(string.Format("Start to sign message {0}", message));
            var hmacSha1 = new HMACSHA1(Encoding.Default.GetBytes(_configuration.GetClientSecret()));
            
            var sign = Convert.ToBase64String(hmacSha1.ComputeHash(Encoding.Default.GetBytes(message)));
            
            var authorizationSign = string.Format("PP {0}:{1}", _configuration.GetClientKey(), sign);
            _logger.Debug(string.Format("End sign message. Generate this authorization header: {0}", authorizationSign));

            return authorizationSign;
        }
        
        public AuthorizationHmacsha1(IClientConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
    }
}