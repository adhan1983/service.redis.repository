namespace AOM.Redis.Service.DataAccess
{
    public interface IRedisService<T>
    {
        T Get(string key);

        void Save(string key, T obj);

        void Delete(string key);
    }
}
