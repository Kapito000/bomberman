using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class FinishRoundRequest {
        [JsonProperty("RunID")]
        public int RoundId;
        [JsonProperty("Score")]
        public int Score;
        [JsonProperty("UsedItems")]
        public Dictionary<string, int> UsedItems;
    }
}
