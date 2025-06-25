namespace TMW2Play.Domain.Interfaces.Services
{
    public interface IHttpService
    {
        Task<TResponse?> PostAsync<TResponse>(string url, object body, CancellationToken cancellationToken);
        Task<TResponse?> GetAsync<TResponse>(string url, CancellationToken cancellationToken);
    }
}
