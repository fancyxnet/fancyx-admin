namespace Fancyx.Shared.Keys
{
    public class MqttCacheKey
    {
        /// <summary>
        /// Mqtt访问token的key
        /// </summary>
        /// <param name="code">浏览器指纹</param>
        /// <returns></returns>
        public static string MqttTokenCode(string code) => $"mqtt_token_code:{code}";

        /// <summary>
        /// Mqtt实际token的key
        /// </summary>
        /// <param name="token">访问token</param>
        /// <returns></returns>
        public static string MqttToken(string token) => $"mqtt_token:{token}";
    }
}