using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseCountableResponse {
        [JsonProperty("ResultCode")]
        public int ResultCode;
        [JsonProperty("Payload")]
        public PurchaseCountableData Payload;
    }
}
