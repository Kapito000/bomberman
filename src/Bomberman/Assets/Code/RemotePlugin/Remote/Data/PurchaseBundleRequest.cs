using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseBundleRequest {
        [JsonProperty("Commons")]
        public PurchaseCountableItemRequest[] CommonItems;
        [JsonProperty("Uniques")]
        public PurchaseUniqueItemRequest[] UniqueItems;
    }
}
