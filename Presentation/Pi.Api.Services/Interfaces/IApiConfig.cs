namespace Pi.Api.Services.Interfaces
{
    public interface IApiConfig
    {
        string PrivateKey { get; }
        string Issuer { get; }
        int TokenExpirationTime { get; }
    }
}
