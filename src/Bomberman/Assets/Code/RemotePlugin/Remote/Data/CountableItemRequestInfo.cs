using System;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class CountableItemRequestInfo {
        [JsonProperty("Amount")]
        public int Amount;
        [JsonProperty("Spent")]
        public int Spent;

        public override string ToString() {
            return $"{nameof(Amount)}: {Amount}{Environment.NewLine}" +
                   $"{nameof(Spent)}: {Spent}{Environment.NewLine}";
        }
    }
}
