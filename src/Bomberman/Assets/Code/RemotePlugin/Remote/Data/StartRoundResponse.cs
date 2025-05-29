using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class StartRoundResponse {
        [JsonProperty("RunID")]
        public int RunId;
    }
}
