using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class PurchaseCountableData {
        [JsonProperty("ItemID")]
        public string Id;
        [JsonProperty("PurchaseItemAmount")]
        public int ItemAmount;
        [JsonProperty("PriceID")]
        public string PriceId;
        [JsonProperty("PriceItemAmount")]
        public int PriceAmount;

        // NOTE: for now should be only used in mass purchase response
        public int ResultCode;

        public override string ToString() {
            return $"{nameof(ItemAmount)}: {ItemAmount}{Environment.NewLine}" +
                   $"{nameof(PriceAmount)}: {PriceAmount}{Environment.NewLine}";
        }
    }
}
