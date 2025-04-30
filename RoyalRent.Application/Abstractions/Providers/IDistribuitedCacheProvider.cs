namespace RoyalRent.Application.Abstractions.Providers;

public interface IDistribuitedCacheProvider
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T data);
}
