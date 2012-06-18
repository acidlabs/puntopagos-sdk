using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk.Imp
{
    internal class Configuration : IConfiguration
    {
        internal const string SandboxUrl = "https://sandbox.puntopagos.com";
        internal const string ProductionUrl = "https://www.puntopagos.com";
        internal const string CreateTransactionUrl = "/transaccion/crear";
        internal const string ProcessTransactionUrl = "/transaccion/procesar";
        internal const string NotificationTransactionUrl = "/transaccion/notificacion";
        internal const string CheckTransactionUrl = "/transaccion/traer";

        internal string Environment { get; set; }
        internal string ClientKey { get; set; }
        internal string ClientSecret { get; set; }

        public string GetEnvironment()
        {
            return Environment;
        }

        public string GetClientKey()
        {
            return ClientKey;
        }

        public string GetClientSecret()
        {
            return ClientSecret;
        }

        public string GetCreateTransactionUrl()
        {
            return GetEnvironmentUrl() + CreateTransactionUrl;
        }

        public string GetCreateTransactionFunction()
        {
            return CreateTransactionUrl.Substring(1);
        }

        public string GetProcessTransactionUrl()
        {
            return GetEnvironmentUrl() + ProcessTransactionUrl;
        }

        public string GetProcessTransactionFunction()
        {
            return ProcessTransactionUrl.Substring(1);
        }
        public string GetNotificationTransactionFunction()
        {
            return NotificationTransactionUrl.Substring(1);
        }

        public string GetCheckTransactionUrl()
        {
            return GetEnvironmentUrl() + CheckTransactionUrl;
        }

        public string GetCheckTransactionFunction()
        {
            return CheckTransactionUrl.Substring(1);
        }

        public string GetEnvironmentUrl()
        {
            return GetEnvironment() == "Producction" ? ProductionUrl : SandboxUrl;
        }
    }
}