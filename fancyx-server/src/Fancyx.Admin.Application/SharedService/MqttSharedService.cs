using Fancyx.Core.Interfaces;
using Fancyx.Shared.Keys;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using StackExchange.Redis;

namespace Fancyx.Admin.Application.SharedService
{
    public class MqttSharedService : ISingletonDependency
    {
        private readonly MqttServer _mqttServer;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _redisDb;
        private readonly string _clientId = "fancyx_admin_";

        public MqttSharedService(MqttServer mqttServer, IConfiguration configuration, IDatabase redisDb)
        {
            _mqttServer = mqttServer;
            _configuration = configuration;
            _redisDb = redisDb;
            _clientId += Guid.NewGuid().ToString("N");
        }

        public async Task ValidatingConnectionAsync(ValidatingConnectionEventArgs e)
        {
            var isValidToken = await _redisDb.KeyExistsAsync(MqttCacheKey.MqttToken(e.UserName)); //此处将userName作为token使用
            var isValidAccount = e.UserName == _configuration["Mqtt:UserName"] && e.Password == _configuration["Mqtt:Password"];
            if (!(isValidToken || isValidAccount))
            {
                e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return;
            }
        }

        /// <summary>
        /// 以指定主题推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<bool> PushAsync<T>(string topic, T? payload = default)
        {
            var payloadString = string.Empty;
            if (payload != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                payloadString = JsonConvert.SerializeObject(payload, settings);
            }
            var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payloadString).Build();

            await _mqttServer.InjectApplicationMessage(
                new InjectedMqttApplicationMessage(message)
                {
                    SenderClientId = _clientId
                });

            return true;
        }
    }
}