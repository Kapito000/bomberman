using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class AuthRequest {
        [JsonProperty("TelegramID")]
        public Int64 UserId;
        [JsonProperty("GameID")]
        public string GameId;

        public string InitData;
    }
}
