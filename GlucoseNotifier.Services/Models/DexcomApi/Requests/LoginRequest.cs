using Newtonsoft.Json;

namespace GlucoseNotifier.Services.Models.DexcomApi.Requests
{
    /// <summary>
    /// Authentication model to get session id from dexcom API
    /// </summary>
    public class LoginRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Application id taken from 
        /// https://github.com/nightscout/share2nightscout-bridge
        /// </summary>
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; } = "d89443d2-327c-4a6f-89e5-496bbb0317db";

        [JsonProperty("accountName")]
        public string AccountName { get; set; }
    }
}
