using Fancyx.Core.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StackExchange.Redis;

namespace Fancyx.Admin.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MqttController : ControllerBase
    {
        private readonly IDatabase redisDb;

        public MqttController(IDatabase redisDb)
        {
            this.redisDb = redisDb;
        }

        [HttpPost]
        public async Task<AppResponse<MqttToken>> GetMqttTokenAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            var codeKey = $"MqttTokenCode:{code}";

            if (await redisDb.KeyExistsAsync(codeKey))
            {
                var ttl = await redisDb.KeyTimeToLiveAsync(codeKey);
                var ttlValue = ttl.HasValue ? ttl.Value.Seconds : 0;
                if (ttlValue > 60)
                {
                    return Result.Data(new MqttToken { Token = redisDb.StringGet(codeKey), Expired = ttlValue });
                }
            }

            var token = Guid.NewGuid().ToString();
            var expired = TimeHelper.Instance.GetCurrentTimestamp() + 3600;
            await redisDb.StringSetAsync($"MqttToken:{token}", expired, TimeSpan.FromHours(1));
            await redisDb.StringSetAsync(codeKey, token, TimeSpan.FromHours(1));
            return Result.Data(new MqttToken { Expired = expired, Token = token });
        }
    }

    public class MqttToken
    {
        public long Expired { get; set; }
        public string? Token { get; set; }
    }
}