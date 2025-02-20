using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.Services;
using MimeKit.Cryptography;
using StackExchange.Redis;

namespace app.BLL.Implementations
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private const string queueKey = "user_Id_Queue";
        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task EnqueueUserIdAsync(Guid userId)
        {
            var db = _redis.GetDatabase();//get the instance of database in redis.
            await db.ListLeftPushAsync(queueKey, userId.ToString());
        }
        public async Task<Guid> DequeueUserIdAsync()
        {
            var db= _redis.GetDatabase();
            var _userId= await db.ListRightPopAsync(queueKey);
            return _userId.HasValue ? Guid.Parse(_userId.ToString()) : Guid.Empty;
        }
        
    }
}
