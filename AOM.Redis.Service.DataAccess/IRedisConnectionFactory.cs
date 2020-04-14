using StackExchange.Redis;

namespace AOM.Redis.Service.DataAccess
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}
