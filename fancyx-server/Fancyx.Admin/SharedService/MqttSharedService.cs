using Fancyx.Core.Interfaces;

using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using StackExchange.Redis;

namespace Fancyx.Admin.SharedService
{
    public class MqttSharedService : ISingletonDependency
    {
        private readonly MqttServer mqttServer;
        private readonly IConfiguration configuration;
        private readonly IDatabase redisDb;
        private readonly string clientId = "fancyx_admin_";

        public MqttSharedService(MqttServer mqttServer, IConfiguration configuration, IDatabase redisDb)
        {
            this.mqttServer = mqttServer;
            this.configuration = configuration;
            this.redisDb = redisDb;
            clientId += Guid.NewGuid().ToString("N");
        }

        public async Task ValidatingConnectionAsync(ValidatingConnectionEventArgs e)
        {
            var isValidToken = await redisDb.KeyExistsAsync($"MqttToken:{e.UserName}"); //此处将userName作为token使用
            var isValidAccount = e.UserName == configuration["Mqtt:UserName"] && e.Password == configuration["Mqtt:Password"];
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

            await mqttServer.InjectApplicationMessage(
                new InjectedMqttApplicationMessage(message)
                {
                    SenderClientId = clientId
                });

            return true;
        }
    }
}