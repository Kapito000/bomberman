using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseBundleResponse {
        [JsonProperty("Commons")]
        public PurchaseCountableResponse[] CommonItems;
        [JsonProperty("Uniques")]
        public PurchaseUniqueItemResponse[] UniqueItems;
    }
}
