namespace Acid.PuntoPagos.Sdk.Interfaces
{
    public interface IConfiguration : IClientConfiguration
    {
        string GetCreateTransactionFunction();
        string GetCreateTransactionUrl();
        string GetEnvironmentUrl();
        string GetProcessTransactionUrl();
        string GetProcessTransactionFunction();
        string GetNotificationTransactionFunction();
        string GetCheckTransactionUrl();
        string GetCheckTransactionFunction();
    }
}