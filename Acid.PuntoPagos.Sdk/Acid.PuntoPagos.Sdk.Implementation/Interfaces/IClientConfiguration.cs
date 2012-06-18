namespace Acid.PuntoPagos.Sdk.Interfaces
{
    public interface IClientConfiguration
    {
        string GetEnvironment();
        string GetClientKey();
        string GetClientSecret();
    }
}