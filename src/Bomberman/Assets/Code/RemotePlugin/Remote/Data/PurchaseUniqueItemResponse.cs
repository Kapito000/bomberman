using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseUniqueItemResponse {
        [JsonProperty("ResultCode")]
        public int ResultCode;
        [JsonProperty("Payload")]
        public PurchaseUniqueData Payload;
    }
}
