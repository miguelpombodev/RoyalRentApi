namespace RoyalRent.Application.Abstractions.Providers;

public interface IDistribuitedCacheService
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T data);
}
