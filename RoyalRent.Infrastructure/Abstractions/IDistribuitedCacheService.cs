namespace RoyalRent.Infrastructure.Abstractions;

public interface IDistribuitedCacheService
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T data);
}
