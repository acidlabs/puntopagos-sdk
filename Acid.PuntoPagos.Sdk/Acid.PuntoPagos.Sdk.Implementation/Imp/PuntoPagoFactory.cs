using Acid.PuntoPagos.Sdk.Interfaces;

namespace Acid.PuntoPagos.Sdk.Imp
{
    public static class PuntoPagoFactory
    {
         public static IAuthorization CreateAuthorization(IConfiguration configuration, ILog logger)
         {
             return new AuthorizationHmacsha1(configuration, logger);
         }

        public static IExecutorWeb CreateExecutorWeb(ILog logger)
        {
            return new ExecutorWeb(logger);
        }
    }
}