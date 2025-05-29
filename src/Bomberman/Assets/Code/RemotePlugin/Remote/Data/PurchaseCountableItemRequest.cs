using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseCountableItemRequest {
        [JsonProperty("PriceID")]
        public string PriceId;
        [JsonProperty("PriceAmount")]
        public int PriceAmount;
        [JsonProperty("ItemID")]
        public string ItemId;
        [JsonProperty("Amount")]
        public int Amount;
    }
}
