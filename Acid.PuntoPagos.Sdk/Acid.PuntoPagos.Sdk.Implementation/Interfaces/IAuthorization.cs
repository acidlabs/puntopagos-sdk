namespace Acid.PuntoPagos.Sdk.Interfaces
{
    public interface IAuthorization
    {
        string GetAuthorizationHeader(string message);
    }
}