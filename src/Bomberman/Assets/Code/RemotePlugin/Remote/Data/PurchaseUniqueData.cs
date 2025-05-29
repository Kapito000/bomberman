using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseUniqueData {
        [JsonProperty("ItemID")]
        public string Id;
        [JsonProperty("PriceID")]
        public string PriceId;
        [JsonProperty("PriceItemAmount")]
        public int PriceAmount;

        // NOTE: for now should be only used in mass purchase response
        public int ResultCode;
    }
}
